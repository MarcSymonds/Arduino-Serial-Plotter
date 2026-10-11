namespace AuduinoSerialPlotter {
	public partial class Plotter : Form {
		public void hGraphScroll_ValueChanged(object sender, EventArgs e) {
			if (Tag == null || (bool)Tag != true) {
				DrawGraph();
				Tag = false;
			}
		}

		private void btnAutoScroll_Click(object sender, EventArgs e) {
			SetAutoScrollState(!_autoScroll);
		}

		private void SetHorizontalScroll() {
			int pointCount = _plotData.DataPointCount - _maxVisibleXDataPoints;

			try {
				if (pointCount > 0) {
					hGraphScroll.Invoke((MethodInvoker)delegate {
						hGraphScroll.Maximum = pointCount - 1;
						hGraphScroll.Enabled = true;

						if (_autoScroll) {
							hGraphScroll.Tag = true;
							hGraphScroll.Value = pointCount - 1;
						}
					});
				}
				else {
					hGraphScroll.Invoke((MethodInvoker)delegate {
						hGraphScroll.Tag = true;
						hGraphScroll.Minimum = 0;
						hGraphScroll.Maximum = 0;
						hGraphScroll.Value = 0;
						hGraphScroll.Enabled = false;
					});
				}
			}
			catch {
				// Ignore. Can error when closing the app.
			}
		}

		private void SetAutoScrollState(bool scrolling) {
			_autoScroll = scrolling;

			btnAutoScroll.BackColor = _autoScroll ? Color.LightGreen : Color.Transparent;

			if (hGraphScroll.Enabled)
				hGraphScroll.Value = hGraphScroll.Maximum;
		}
	}
}
