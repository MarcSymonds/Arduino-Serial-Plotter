using System.Drawing.Drawing2D;

namespace AuduinoSerialPlotter {
	public partial class Plotter : Form {
		private const int WM_DEVICECHANGE = 0x0219;

		private const int DBT_DEVNODES_CHANGED = 0x0007;
		private const int DBT_DEVICEARRIVAL = 0x8000;       // System detected a new device
		private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;



		private Padding PhysicalSpacePadding = new Padding(60, 20, 100, 80);

		public PlotData _plotData { get; private set; } = new PlotData();

		private COMReader? _comReader = null;

		private bool _drawCurvyLines = false;

		private int _maxVisibleXDataPoints = 50;

		private bool _autoScroll = true;

		private int _lastDrawnPlotValues = -1;

		/// <summary>
		/// Is the app closing.
		/// </summary>
		private bool _isClosing = false;

		/// <summary>
		/// Queue to store messages in to as they are read by the COMReader.
		/// </summary>
		private Queue<string> _messageQueue = new Queue<string>();

		/// <summary>
		/// Thread that will process messages from the queue, and add them to the graph.
		/// </summary>
		private Thread _queueProcess;

		private bool _mouseOverGraph = false;

		public Plotter() {
			InitializeComponent();

			// Enable double buffering on the panel used to draw the graph on, to reduce flickering.
			//
			typeof(Panel).InvokeMember("DoubleBuffered",
						System.Reflection.BindingFlags.SetProperty |
						System.Reflection.BindingFlags.Instance |
						System.Reflection.BindingFlags.NonPublic,
						null,
						pnlGraph,
						new object[] { true });

			typeof(Panel).InvokeMember("DoubleBuffered",
						System.Reflection.BindingFlags.SetProperty |
						System.Reflection.BindingFlags.Instance |
						System.Reflection.BindingFlags.NonPublic,
						null,
						pnlGraphLabels,
						new object[] { true });

			typeof(Panel).InvokeMember("DoubleBuffered",
						System.Reflection.BindingFlags.SetProperty |
						System.Reflection.BindingFlags.Instance |
						System.Reflection.BindingFlags.NonPublic,
						null,
						pnlGraphLabelHighlights,
						new object[] { true });

			// TODO: Save these somewhere (registry) to restore next time.
			//
			string comPort = "COM9";
			string baudRate = "115200 baud";

			InitialiseCOMPort(comPort, baudRate);

			ChangeDevice(false);


			SetAutoScrollState(true);
			SetStopResumeState(true);

			InitializeSendMessage();
			InitialisePointsToDisplay();

			if (_comReader != null) {
				SetConnectedState(true);
				SetSendMessageState(true);
				SetSendMessageState(_comReader.StartReading());
			}
			else {
				SetConnectedState(false);
				SetSendMessageState(false);
			}


			_queueProcess = new Thread(ProcessQueue);

			_queueProcess.Start();
		}

		protected override void WndProc(ref Message m) {
			// Intercept the device change message
			if (m.Msg == WM_DEVICECHANGE) {
				// If a USB device is just "unplugged", then we may not get the DEVICEREMOVECOMPLETE message,
				// so we are just looking at "any" device change.
				//
				switch (m.WParam.ToInt32()) {
					case DBT_DEVNODES_CHANGED:
						if (!ListCOMPorts()) {
							if (_comReader != null) {
								SetConnectedState(false);
								SetSendMessageState(false);

								_comReader.DataReceived -= ComReader_DataReceived;
								_comReader.COMException -= ComReader_COMException;
								_comReader.Dispose();
								_comReader = null;
							}
						}
						break;

						/*case DBT_DEVICEARRIVAL:
						case DBT_DEVICEREMOVECOMPLETE:
							ListCOMPorts();
							break;*/
				}
			}

			// Always pass messages down to the base handler
			base.WndProc(ref m);
		}


		public void Plotter_FormClosing(object sender, FormClosingEventArgs e) {
			_isClosing = true;

			if (_comReader != null) {
				_comReader.DataReceived -= ComReader_DataReceived;
				_comReader.COMException -= ComReader_COMException;

				_comReader.Dispose();
			}
		}

		/// <summary>
		/// When a message is received by the COMReader, the message is simply added to the message queue.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message"></param>
		private void ComReader_DataReceived(object? sender, string message) {
			if (_isClosing)
				return;

			lock (_messageQueue) {
				_messageQueue.Enqueue(message);
			}
		}

		private void ComReader_COMException(object? sender, COMReaderExceptionEventArgs args) {
			if (_comReader != null && !args.Action.Equals("Write")) {
				_comReader.DataReceived -= ComReader_DataReceived;
				_comReader.COMException -= ComReader_COMException;
				_comReader.Dispose();
				_comReader = null;

				SetConnectedState(false);
				SetSendMessageState(false);
			}

			if (this.InvokeRequired) {
				this.Invoke((MethodInvoker)delegate {
					ShowCOMException(sender, args);
				});
			}
			else
				ShowCOMException(sender, args);
		}

		private void ShowCOMException(object? sender, COMReaderExceptionEventArgs args) {
			MessageBox.Show(this, "Exception: " + args.Action + " - " + args.Exception.ToString(), "Communication Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		/// <summary>
		/// This is run on its own thread and will read messages from the message queue, add values to the plot
		/// data, and then redraw the graph.
		/// </summary>
		private void ProcessQueue() {
			// While the app is running ...
			//
			while (!_isClosing) {
				// Any messages in the queue?
				//
				if (_messageQueue.Count > 0) {
					string message;

					// Get the first message from the queue.
					//
					lock (_messageQueue) {
						message = _messageQueue.Dequeue();
					}

					List<PlotLine> newPlotLines = new List<PlotLine>();

					// Limit the number of messages we read and add to the plot data at a time.
					// This is so we don't leave the graph not being updated for a long time.
					//
					int limit = 10;

					while (true && --limit > 0) {
						lock (_plotData) {
							newPlotLines.AddRange(_plotData.AddDataPoints(message));
						}

						// Get the next message from the queue.
						//
						lock (_messageQueue) {
							if (_messageQueue.Count == 0)
								break;

							message = _messageQueue.Dequeue();
						}
					}

					int pointCount = _plotData.DataPointCount - _maxVisibleXDataPoints;

					SetHorizontalScroll();

					// For any new parameters passed, add checkboxes for each of them.
					//
					foreach (var plotLine in newPlotLines) {
						CheckBox cb = new CheckBox() {
							Text = plotLine.Label,
							Tag = plotLine.Label,
							Checked = plotLine.Visible,
							Width = 150,
							AutoSize = true,
							ForeColor = plotLine.Color,
							FlatStyle = FlatStyle.Flat,
							ContextMenuStrip = cmParameter
						};

						cb.CheckedChanged += (s, e) => {
							plotLine.Visible = cb.Checked;
							DrawGraph();
						};

						pnlParameters.Invoke((MethodInvoker)delegate {
							pnlParameters.Controls.Add(cb);
						});
					}

					// Finally, redraw the graph.
					//
					try {
						pnlGraph.Invoke((MethodInvoker)delegate {
							DrawGraph();
						});
					}
					catch {
						// Ignore. Can get exceptions if the app is terminated while in the middle of this.
					}
				}
			}
		}

		// Used to draw the graph to, before it is copied to the panel on the form.
		// Note that double-buffering is enabled on the panel to make the updates 
		// look smoother.
		//
		private Bitmap? graphBuffer = null;

		public void pnlGraph_Resize(object sender, EventArgs e) {
			DrawGraph();
		}

		public void pnlGraph_Paint(object sender, PaintEventArgs e) {
			if (graphBuffer != null)
				e.Graphics.DrawImage(graphBuffer, 0, 0);
		}

		private bool _isDrawing = false;

		public void DrawGraph() {
			if (_isClosing)
				return;

			lock (_plotData) {
				_isDrawing = true;
				DrawGraph2();
				_isDrawing = false;
			}
		}

		public struct GraphData {
			public float? minValue;
			public float? maxValue;

			public float yRange;
			public float yBottom;
			public float yTop;

			public float scaleX;
			public float scaleY;
		}

		public GraphData _graphData = new GraphData();

		public void DrawGraph2() {

			// Create a new buffer if the size of the panel has changed.
			//)
			if (graphBuffer == null || graphBuffer.Width != pnlGraph.Width || graphBuffer.Height != pnlGraph.Height) {
				graphBuffer?.Dispose();
				graphBuffer = new Bitmap(pnlGraph.Width, pnlGraph.Height);
			}

			using (Graphics g = Graphics.FromImage(graphBuffer)) {
				// Define a physical drawing area. Allows space for axis labels and margins.
				//
				Rectangle physicalSpace = new Rectangle(PhysicalSpacePadding.Left, PhysicalSpacePadding.Top, pnlGraph.Width - PhysicalSpacePadding.Right, pnlGraph.Height - PhysicalSpacePadding.Bottom);

				// Clear the buffer before drawing.
				//
				g.Clear(Color.White);

				// Get the min and max values of the visible plot lines.
				//
				//float? minValue;
				//float? maxValue;

				_plotData.GetVisiblePlotLinesMinMax(out _graphData.minValue, out _graphData.maxValue);

				if (_graphData.minValue.HasValue && _graphData.maxValue.HasValue) {
					// Set smoothing mode for better quality lines.
					//
					g.SmoothingMode = SmoothingMode.AntiAlias;

					_graphData.yRange = _graphData.maxValue.Value - _graphData.minValue.Value;
					_graphData.yBottom = _graphData.minValue.Value;
					_graphData.yTop = _graphData.maxValue.Value;

					int yStep = 0;

					if (_graphData.yRange <= 10)
						yStep = 1;
					else if (_graphData.yRange < 50)
						yStep = 5;
					else if (_graphData.yRange < 100)
						yStep = 10;
					else if (_graphData.yRange < 500)
						yStep = 50;
					else if (_graphData.yRange < 1000)
						yStep = 100;
					else
						yStep = 500;

					float rem = _graphData.maxValue.Value % yStep;

					if (_graphData.maxValue.Value < 0f)
						_graphData.yTop = _graphData.maxValue.Value - rem - yStep;
					else
						_graphData.yTop = _graphData.maxValue.Value - rem + yStep;


					rem = _graphData.minValue.Value % yStep;

					if (_graphData.minValue.Value < 0f)
						_graphData.yBottom = _graphData.minValue.Value - rem - yStep;
					else
						_graphData.yBottom = _graphData.minValue.Value - rem;

					Rectangle virtualSpace = new Rectangle(0, (int)_graphData.yTop, _plotData.DataPointCount - 1, (int)(_graphData.yTop - _graphData.yBottom));

					int visibleDataPoints = _plotData.DataPointCount > _maxVisibleXDataPoints ? _maxVisibleXDataPoints : _plotData.DataPointCount;

					_graphData.scaleY = (float)physicalSpace.Height / virtualSpace.Height;
					_graphData.scaleX = (float)physicalSpace.Width / _maxVisibleXDataPoints;


					// Draw Y axis lines and labels.
					//
					for (int y = (int)_graphData.yBottom; y <= (int)_graphData.yTop; y += yStep) {
						float yPos = physicalSpace.Bottom - ((y - _graphData.yBottom) * _graphData.scaleY);

						g.DrawLine(Pens.LightGray, physicalSpace.Left, yPos, physicalSpace.Right, yPos);
						g.DrawString(y.ToString(), this.Font, Brushes.Black, 5, yPos - 10);
					}

					int diff = hGraphScroll.Value % 5;
					int point = hGraphScroll.Value - diff;
					float xPos = physicalSpace.Left - (diff * _graphData.scaleX);

					for (int i = 0; i < _maxVisibleXDataPoints; i++, point += 5, xPos += (_graphData.scaleX * 5)) {
						g.DrawLine(Pens.LightGray, xPos, physicalSpace.Top, xPos, physicalSpace.Bottom);
						g.DrawString(point.ToString(), this.Font, Brushes.Black, xPos - 10, physicalSpace.Bottom + 5);
					}

					PointF?[] points = new PointF?[_maxVisibleXDataPoints];

					// Go through each plot line ...
					//
					foreach (var plotLine in _plotData.PlotLines.Values) {
						if (plotLine.Visible && plotLine.DataPoints != null && plotLine.DataPoints.Count > 0) {
							int dataPoint = hGraphScroll.Value;

							// Convert the points in to a list of PointF objects for drawing.
							//
							for (int i = 0; i < _maxVisibleXDataPoints && dataPoint < plotLine.DataPoints.Count; i++, dataPoint++) {
								if (plotLine.DataPoints[dataPoint].HasValue) {
									float x = (float)(physicalSpace.X + (i * _graphData.scaleX));
									float y = (float)(physicalSpace.Bottom - ((plotLine.DataPoints[dataPoint].Value - _graphData.yBottom) * _graphData.scaleY));

									points[i] = new PointF(x, y);
								}
								else
									points[i] = null; // No data for this point.
							}

							Pen pen = new Pen(plotLine.Color, 2);

							// In case there are null points, we need to break the line into segments and draw each segment separately.
							//
							int startPoint = 0;
							while (startPoint < points.Length) {
								while (startPoint < points.Length && points[startPoint] == null)
									++startPoint;

								if (startPoint >= _plotData.DataPointCount)
									break;

								int endPoint = startPoint + 1;
								while (endPoint < points.Length && points[endPoint] != null)
									++endPoint;

								if (endPoint - startPoint > 1) {
									if (_drawCurvyLines)
										g.DrawCurve(pen, points[startPoint..endPoint].Where(p => p.HasValue).Select(p => p.Value).ToArray());
									else
										g.DrawLines(pen, points[startPoint..endPoint].Where(p => p.HasValue).Select(p => p.Value).ToArray());
								}

								// Draw circles at each point.
								//
								for (int i = startPoint; i < points.Length && i < endPoint; i++) {
									if (points[i].HasValue)
										g.FillEllipse(new SolidBrush(plotLine.Color), points[i].Value.X - 5, points[i].Value.Y - 5, 10, 10);
								}

								startPoint = endPoint + 1;
							}
						}
					}

					/*Point mousePos = pnlGraph.PointToClient(Control.MousePosition);

					if (mousePos.X >= physicalSpace.Left && mousePos.X <= physicalSpace.Right && mousePos.Y >= physicalSpace.Top && mousePos.Y <= physicalSpace.Bottom) {
						point = (int)((mousePos.X - physicalSpace.Left + (scaleX / 2)) / scaleX) + hGraphScroll.Value;

						xPos = physicalSpace.Left + (scaleX * (point - hGraphScroll.Value));

						if (point < _plotData.DataPointCount) {
							foreach (var plotLine in _plotData.PlotLines.Values) {
								if (point < plotLine.DataPoints.Count() && plotLine.DataPoints[point].HasValue) {
									float yPos = (float)(physicalSpace.Bottom - ((plotLine.DataPoints[point].Value - yBottom) * scaleY));

									Panel pnlLabel = new Panel() {
										Location = new Point((int)xPos - 100, (int)yPos),
										Size = new Size(100, 50),
										BackColor = Color.Black,
										BorderStyle = BorderStyle.FixedSingle,

									};

									pnlGraph.Controls.Add(pnlLabel);
								}
							}

							//lblDebug.Text = string.Format("Point: {0}", point);

							_lastDrawnPlotValues = point;
						}
					}
					else
						_lastDrawnPlotValues = -1;*/
				}
			}

			pnlGraph.Invalidate();
		}

		public void btnStopStart_Click(object sender, EventArgs e) {
			if (_comReader != null)
				SetStopResumeState(!_comReader.CaptureData);
		}

		private void SetStopResumeState(bool running) {
			if (_comReader != null)
				_comReader.CaptureData = running;// !_comReader.CaptureData;

			if (running) {
				btnStopStart.Text = "Pause";
				btnStopStart.BackColor = Color.Firebrick;
			}
			else {
				btnStopStart.Text = "Resume";
				btnStopStart.BackColor = Color.ForestGreen;
			}
		}


		private void btnSizeParameters_Click(object sender, EventArgs e) {
			splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
			btnSizeParameters.ImageIndex = splitContainer1.Panel2Collapsed ? 0 : 1;
		}

		private void cmParameter_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
			cmParameter.Tag = sender;
		}

		private void cmOptRemoveParameter_Click(object sender, EventArgs e) {
			ToolStripMenuItem me = (ToolStripMenuItem)sender;

			if (me.Owner != null) {
				CheckBox? cbPlotLine = (CheckBox)(((ContextMenuStrip)me.Owner).SourceControl);

				lock (_plotData) {
					_plotData.RemovePlotLine(cbPlotLine.Tag.ToString());
				}

				cbPlotLine.Parent.Controls.Remove(cbPlotLine);

				DrawGraph();
			}
		}

		private void btnGraphStyle_Click(object sender, EventArgs e) {
			_drawCurvyLines = !_drawCurvyLines;

			btnGraphStyle.ImageIndex = _drawCurvyLines ? 0 : 1;

			DrawGraph();
		}

		private void pnlGraph_MouseEnter(object sender, EventArgs e) {
		}

		private void pnlGraph_MouseLeave(object sender, EventArgs e) {
		}

		private System.Threading.Timer? graphMouseMoveTimer = null;

		private void pnlGraph_MouseMove(object sender, MouseEventArgs e) {
		}

		private void pnlGraphLabels_MouseEnter(object sender, EventArgs e) {

		}

		private void pnlGraphLabels_MouseLeave(object sender, EventArgs e) {
			if (graphMouseMoveTimer != null) {
				graphMouseMoveTimer.Dispose();
				graphMouseMoveTimer = null;
			}

			ClearGraphLabels();
		}

		private void pnlGraphLabels_MouseMove(object sender, MouseEventArgs e) {
			if (graphMouseMoveTimer != null)
				graphMouseMoveTimer.Change(10, System.Threading.Timeout.Infinite);
			else
				graphMouseMoveTimer = new System.Threading.Timer(CheckForMouseOverGraph, null, 10, System.Threading.Timeout.Infinite);

			lblDebug.Text = string.Format("L {0}", e.Location);
		}

		private void pnlGraphLabelHighlights_MouseMove(object sender, MouseEventArgs e) {
			if (graphMouseMoveTimer != null)
				graphMouseMoveTimer.Change(10, System.Threading.Timeout.Infinite);
			else
				graphMouseMoveTimer = new System.Threading.Timer(CheckForMouseOverGraph, null, 10, System.Threading.Timeout.Infinite);
		}

		private void CheckForMouseOverGraph(object stateInfo) {
			if (_isClosing)
				return;

			if (pnlGraphLabels.InvokeRequired) {
				pnlGraphLabels.Invoke((MethodInvoker)delegate {
					CheckForMouseOverGraph(stateInfo);
				});

				return;
			}

			Rectangle physicalSpace = new Rectangle(PhysicalSpacePadding.Left, PhysicalSpacePadding.Top, pnlGraph.Width - PhysicalSpacePadding.Right, pnlGraph.Height - PhysicalSpacePadding.Bottom);

			var mousePos = pnlGraphLabels.PointToClient(Control.MousePosition);

			if (mousePos.X >= physicalSpace.Left && mousePos.X <= physicalSpace.Right) {
				int point = (int)((mousePos.X - physicalSpace.Left + (_graphData.scaleX / 2)) / _graphData.scaleX) + hGraphScroll.Value;

				if (point != _lastDrawnPlotValues) {
					ClearGraphLabels();

					if (point >= 0 && point < _plotData.DataPointCount)
						DrawGraphLabels(mousePos, physicalSpace, point);

					_lastDrawnPlotValues = point;
				}
			}
			else
				ClearGraphLabels();
		}

		private void ClearGraphLabels() {
			if (pnlGraphLabels.InvokeRequired) {
				pnlGraphLabels.Invoke((MethodInvoker)delegate {
					ClearGraphLabels();
				});

				return;
			}

			pnlGraphLabels.Controls.Clear();

			if (labelHighlightsBuffer != null) {
				using (Graphics g = Graphics.FromImage(labelHighlightsBuffer)) {
					g.Clear(Color.Transparent);
				}

				pnlGraphLabelHighlights.Invalidate();
			}

			_lastDrawnPlotValues = -1;
		}

		private Bitmap? labelHighlightsBuffer = null;


		private void DrawGraphLabels(Point mousePos, Rectangle physicalSpace, int point) {
			float xPos = physicalSpace.Left + (_graphData.scaleX * (point - hGraphScroll.Value));

			if (point < _plotData.DataPointCount) {
				if (labelHighlightsBuffer == null || labelHighlightsBuffer.Width != pnlGraphLabelHighlights.Width || labelHighlightsBuffer.Height != pnlGraphLabelHighlights.Height) {
					labelHighlightsBuffer?.Dispose();
					labelHighlightsBuffer = new Bitmap(pnlGraphLabelHighlights.Width, pnlGraphLabelHighlights.Height);
				}

				using (Graphics g = Graphics.FromImage(labelHighlightsBuffer)) {
					g.Clear(Color.Transparent);

					// Create a list of the labels that needs to be drawn.
					//
					List<LabelSorting> labelSorting = new List<LabelSorting>();

					foreach (var plotLine in _plotData.PlotLines.Values) {
						if (plotLine.Visible && point >= 0 && point < plotLine.DataPoints.Count() && plotLine.DataPoints[point].HasValue)
							labelSorting.Add(new LabelSorting() { PlotLine = plotLine, Value = plotLine.DataPoints[point].Value });
					}

					// Sort them in value ascending.
					// This is so that if values are close together, we can easily adjust the positions
					// of the labels so they don't overlap.
					//
					labelSorting.Sort((a, b) => a.Value.CompareTo(b.Value));

					List<Control> pointLabels = new List<Control>();

					float offset = 24f;

					Size size = new Size(100, 50);

					float labelXPos = (int)(xPos - size.Width - offset);

					if (labelXPos < physicalSpace.Left)
						labelXPos = (int)xPos + offset;

					foreach (var sortedLabel in labelSorting) {
						var plotLine = sortedLabel.PlotLine;

						float yPos = (float)(physicalSpace.Bottom - ((sortedLabel.Value - _graphData.yBottom) * _graphData.scaleY));

						// Initial position for the label.
						//
						Point location = new Point((int)labelXPos, (int)yPos - (size.Height / 2));

						// If this label will overlap another label, then adjust it's position.
						//
						foreach (var pl in pointLabels) {
							if (location.Y + size.Height >= pl.Location.Y && location.Y <= pl.Location.Y + pl.Size.Height)
								location = new Point(location.X, pl.Location.Y - size.Height - 2);
						}

						Panel pnlLabel = new Panel() {
							Location = location,
							Size = size,
							BackColor = Color.White,
							BorderStyle = BorderStyle.FixedSingle,
						};

						Label pointLabel = new Label() {
							Location = new Point(5, 5),
							Size = new Size(size.Width, 26),
							BackColor = Color.Transparent,
							ForeColor = plotLine.Color,
							Font = pnlGraph.Font,
							Text = plotLine.Label,
							AutoSize = true,
							AutoEllipsis = true,
							MaximumSize = new Size(size.Width - 10, 20)
						};

						pnlLabel.Controls.Add(pointLabel);

						Panel pointIndicator = new Panel() {
							Location = new Point(5, 24),
							Size = new Size(14, 15),
							BackColor = plotLine.Color,
							BorderStyle = BorderStyle.FixedSingle,
						};

						pnlLabel.Controls.Add(pointIndicator);

						Label pointValue = new Label() {
							Location = new Point(24, 24),
							ForeColor = Color.Black,
							Font = pnlGraph.Font,
							Text = sortedLabel.Value.ToString(),
							AutoSize = true,
							AutoEllipsis = true,
							MaximumSize = new Size(size.Width - 34, 20)
						};

						pnlLabel.Controls.Add(pointValue);


						pointLabels.Add(pnlLabel);

						pnlGraphLabels.Controls.Add(pnlLabel);

						g.DrawEllipse(new Pen(plotLine.Color, 2), xPos - 15, yPos - 15, 30, 30);
					}
				}

				_lastDrawnPlotValues = point;

				pnlGraphLabelHighlights.Invalidate();
			}
		}

		public class LabelSorting {
			public PlotLine? PlotLine { get; set; }
			public float Value { get; set; }
		}

		private void pnlGraphLabelHighlights_Paint(object sender, PaintEventArgs e) {
			if (labelHighlightsBuffer != null)
				e.Graphics.DrawImage(labelHighlightsBuffer, 0, 0);
		}

		private void btnConnect_Click(object sender, EventArgs e) {
			if (_comReader != null) {
				if (_comReader.IsPortOpen) {
					_comReader.StopReading();
					SetConnectedState(false);
					SetSendMessageState(false);
				}
				else {
					SetConnectedState(true);
					SetSendMessageState(true);
					_comReader.StartReading();
				}
			}
			else {
				ChangeDevice(true);
			}
		}

		private void SetConnectedState(bool connected) {
			btnConnect.BackColor = connected ? Color.LightGreen : Color.Salmon;
		}
	}
}
