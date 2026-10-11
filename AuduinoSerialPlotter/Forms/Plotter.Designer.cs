namespace AuduinoSerialPlotter {
	partial class Plotter {
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Plotter));
			pnlGraphHeader = new Panel();
			btnGraphStyle = new Button();
			ilLineStyle = new ImageList(components);
			cbPointsToDisplay = new ComboBox();
			btnExport = new Button();
			btnAutoScroll = new Button();
			btnStopStart = new Button();
			btnClear = new Button();
			pnlParameters = new FlowLayoutPanel();
			pnlParametersContainer = new Panel();
			btnConnect = new Button();
			lblDebug = new Label();
			cbMessageTerminator = new ComboBox();
			panel3 = new Panel();
			tbMessage = new TextBox();
			btnSendMessage = new Button();
			cbBaud = new ComboBox();
			cbPort = new ComboBox();
			cmParameter = new ContextMenuStrip(components);
			cmOptRemoveParameter = new ToolStripMenuItem();
			tooltip = new ToolTip(components);
			btnSizeParameters = new Button();
			ilSizeParameters = new ImageList(components);
			pnlGraph = new Panel();
			pnlGraphLabelHighlights = new Panel();
			pnlGraphLabels = new Panel();
			hGraphScroll = new HScrollBar();
			sfExportData = new SaveFileDialog();
			splitContainer1 = new SplitContainer();
			pnlParametersHeader = new Panel();
			label1 = new Label();
			pnlGraphHeader.SuspendLayout();
			pnlParametersContainer.SuspendLayout();
			panel3.SuspendLayout();
			cmParameter.SuspendLayout();
			pnlGraph.SuspendLayout();
			pnlGraphLabelHighlights.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			pnlParametersHeader.SuspendLayout();
			SuspendLayout();
			// 
			// pnlGraphHeader
			// 
			pnlGraphHeader.BackColor = Color.WhiteSmoke;
			pnlGraphHeader.Controls.Add(btnGraphStyle);
			pnlGraphHeader.Controls.Add(cbPointsToDisplay);
			pnlGraphHeader.Controls.Add(btnExport);
			pnlGraphHeader.Controls.Add(btnAutoScroll);
			pnlGraphHeader.Controls.Add(btnStopStart);
			pnlGraphHeader.Controls.Add(btnClear);
			pnlGraphHeader.Dock = DockStyle.Top;
			pnlGraphHeader.Location = new Point(0, 0);
			pnlGraphHeader.Margin = new Padding(2);
			pnlGraphHeader.Name = "pnlGraphHeader";
			pnlGraphHeader.Size = new Size(1329, 48);
			pnlGraphHeader.TabIndex = 0;
			// 
			// btnGraphStyle
			// 
			btnGraphStyle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnGraphStyle.FlatAppearance.BorderSize = 0;
			btnGraphStyle.FlatStyle = FlatStyle.Flat;
			btnGraphStyle.ImageIndex = 1;
			btnGraphStyle.ImageList = ilLineStyle;
			btnGraphStyle.Location = new Point(1193, 9);
			btnGraphStyle.Margin = new Padding(2);
			btnGraphStyle.Name = "btnGraphStyle";
			btnGraphStyle.Size = new Size(32, 32);
			btnGraphStyle.TabIndex = 7;
			tooltip.SetToolTip(btnGraphStyle, "Line style");
			btnGraphStyle.UseVisualStyleBackColor = true;
			btnGraphStyle.Click += btnGraphStyle_Click;
			// 
			// ilLineStyle
			// 
			ilLineStyle.ColorDepth = ColorDepth.Depth32Bit;
			ilLineStyle.ImageStream = (ImageListStreamer)resources.GetObject("ilLineStyle.ImageStream");
			ilLineStyle.TransparentColor = Color.Transparent;
			ilLineStyle.Images.SetKeyName(0, "curvy-lines.png");
			ilLineStyle.Images.SetKeyName(1, "straight-lines.png");
			// 
			// cbPointsToDisplay
			// 
			cbPointsToDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			cbPointsToDisplay.DropDownStyle = ComboBoxStyle.DropDownList;
			cbPointsToDisplay.FormattingEnabled = true;
			cbPointsToDisplay.Items.AddRange(new object[] { "25", "50", "75", "100", "125", "150", "175", "200", "250", "300" });
			cbPointsToDisplay.Location = new Point(985, 14);
			cbPointsToDisplay.Margin = new Padding(2);
			cbPointsToDisplay.Name = "cbPointsToDisplay";
			cbPointsToDisplay.Size = new Size(66, 23);
			cbPointsToDisplay.TabIndex = 6;
			tooltip.SetToolTip(cbPointsToDisplay, "Number of points to show horizontally");
			// 
			// btnExport
			// 
			btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnExport.BackgroundImage = ArduinoSerialPlotter.Properties.Resources.download;
			btnExport.BackgroundImageLayout = ImageLayout.Center;
			btnExport.FlatAppearance.BorderSize = 0;
			btnExport.FlatStyle = FlatStyle.Flat;
			btnExport.Location = new Point(1238, 9);
			btnExport.Margin = new Padding(2);
			btnExport.Name = "btnExport";
			btnExport.Size = new Size(32, 32);
			btnExport.TabIndex = 5;
			tooltip.SetToolTip(btnExport, "Export graph data");
			btnExport.UseVisualStyleBackColor = true;
			btnExport.Click += btnExport_Click;
			// 
			// btnAutoScroll
			// 
			btnAutoScroll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnAutoScroll.BackColor = Color.PaleGreen;
			btnAutoScroll.BackgroundImage = (Image)resources.GetObject("btnAutoScroll.BackgroundImage");
			btnAutoScroll.BackgroundImageLayout = ImageLayout.Center;
			btnAutoScroll.Cursor = Cursors.Hand;
			btnAutoScroll.FlatAppearance.BorderSize = 0;
			btnAutoScroll.FlatStyle = FlatStyle.Flat;
			btnAutoScroll.Location = new Point(1155, 9);
			btnAutoScroll.Margin = new Padding(2);
			btnAutoScroll.Name = "btnAutoScroll";
			btnAutoScroll.Size = new Size(32, 32);
			btnAutoScroll.TabIndex = 4;
			tooltip.SetToolTip(btnAutoScroll, "Autoscroll");
			btnAutoScroll.UseVisualStyleBackColor = false;
			btnAutoScroll.Click += btnAutoScroll_Click;
			// 
			// btnStopStart
			// 
			btnStopStart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnStopStart.BackColor = Color.FromArgb(218, 91, 74);
			btnStopStart.Cursor = Cursors.Hand;
			btnStopStart.FlatAppearance.BorderSize = 0;
			btnStopStart.FlatStyle = FlatStyle.Flat;
			btnStopStart.Font = new Font("Segoe UI Variable Text Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
			btnStopStart.ForeColor = Color.White;
			btnStopStart.Location = new Point(1065, 11);
			btnStopStart.Margin = new Padding(2);
			btnStopStart.Name = "btnStopStart";
			btnStopStart.Size = new Size(84, 29);
			btnStopStart.TabIndex = 1;
			btnStopStart.Text = "Pause";
			btnStopStart.UseVisualStyleBackColor = false;
			btnStopStart.Click += btnStopStart_Click;
			// 
			// btnClear
			// 
			btnClear.Anchor = AnchorStyles.Right;
			btnClear.BackColor = Color.Transparent;
			btnClear.BackgroundImage = ArduinoSerialPlotter.Properties.Resources.clear;
			btnClear.BackgroundImageLayout = ImageLayout.Center;
			btnClear.Cursor = Cursors.Hand;
			btnClear.FlatAppearance.BorderColor = Color.White;
			btnClear.FlatAppearance.BorderSize = 0;
			btnClear.FlatStyle = FlatStyle.Flat;
			btnClear.Location = new Point(1284, 10);
			btnClear.Margin = new Padding(2);
			btnClear.Name = "btnClear";
			btnClear.Size = new Size(32, 32);
			btnClear.TabIndex = 0;
			tooltip.SetToolTip(btnClear, "Clear graph");
			btnClear.UseVisualStyleBackColor = false;
			btnClear.Click += btnClear_Click;
			// 
			// pnlParameters
			// 
			pnlParameters.AutoScroll = true;
			pnlParameters.BackColor = Color.White;
			pnlParameters.Dock = DockStyle.Fill;
			pnlParameters.FlowDirection = FlowDirection.TopDown;
			pnlParameters.Location = new Point(0, 48);
			pnlParameters.Margin = new Padding(0);
			pnlParameters.Name = "pnlParameters";
			pnlParameters.Padding = new Padding(10, 9, 10, 9);
			pnlParameters.Size = new Size(214, 583);
			pnlParameters.TabIndex = 2;
			pnlParameters.WrapContents = false;
			// 
			// pnlParametersContainer
			// 
			pnlParametersContainer.BackColor = Color.FromArgb(236, 241, 241);
			pnlParametersContainer.BorderStyle = BorderStyle.FixedSingle;
			pnlParametersContainer.Controls.Add(btnConnect);
			pnlParametersContainer.Controls.Add(lblDebug);
			pnlParametersContainer.Controls.Add(cbMessageTerminator);
			pnlParametersContainer.Controls.Add(panel3);
			pnlParametersContainer.Controls.Add(btnSendMessage);
			pnlParametersContainer.Controls.Add(cbBaud);
			pnlParametersContainer.Controls.Add(cbPort);
			pnlParametersContainer.Dock = DockStyle.Bottom;
			pnlParametersContainer.Location = new Point(0, 631);
			pnlParametersContainer.Margin = new Padding(2);
			pnlParametersContainer.Name = "pnlParametersContainer";
			pnlParametersContainer.Size = new Size(1549, 48);
			pnlParametersContainer.TabIndex = 1;
			// 
			// btnConnect
			// 
			btnConnect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnConnect.BackgroundImage = ArduinoSerialPlotter.Properties.Resources.connect;
			btnConnect.BackgroundImageLayout = ImageLayout.Center;
			btnConnect.FlatAppearance.BorderSize = 0;
			btnConnect.FlatStyle = FlatStyle.Flat;
			btnConnect.Location = new Point(1503, 7);
			btnConnect.Margin = new Padding(2);
			btnConnect.Name = "btnConnect";
			btnConnect.Size = new Size(32, 32);
			btnConnect.TabIndex = 7;
			tooltip.SetToolTip(btnConnect, "Connect/Disconnect");
			btnConnect.UseVisualStyleBackColor = true;
			btnConnect.Click += btnConnect_Click;
			// 
			// lblDebug
			// 
			lblDebug.Location = new Point(346, 10);
			lblDebug.Margin = new Padding(2, 0, 2, 0);
			lblDebug.Name = "lblDebug";
			lblDebug.Size = new Size(818, 23);
			lblDebug.TabIndex = 6;
			lblDebug.Text = "Debug";
			lblDebug.Visible = false;
			// 
			// cbMessageTerminator
			// 
			cbMessageTerminator.DropDownStyle = ComboBoxStyle.DropDownList;
			cbMessageTerminator.FormattingEnabled = true;
			cbMessageTerminator.Items.AddRange(new object[] { "CR/LF", "CR", "LF", "None" });
			cbMessageTerminator.Location = new Point(268, 12);
			cbMessageTerminator.Margin = new Padding(2);
			cbMessageTerminator.Name = "cbMessageTerminator";
			cbMessageTerminator.Size = new Size(62, 23);
			cbMessageTerminator.TabIndex = 5;
			// 
			// panel3
			// 
			panel3.BackColor = Color.White;
			panel3.BorderStyle = BorderStyle.FixedSingle;
			panel3.Controls.Add(tbMessage);
			panel3.Location = new Point(11, 10);
			panel3.Margin = new Padding(2);
			panel3.Name = "panel3";
			panel3.Size = new Size(180, 27);
			panel3.TabIndex = 4;
			// 
			// tbMessage
			// 
			tbMessage.BorderStyle = BorderStyle.None;
			tbMessage.Enabled = false;
			tbMessage.Font = new Font("Segoe UI Variable Text", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			tbMessage.HideSelection = false;
			tbMessage.Location = new Point(6, 4);
			tbMessage.Margin = new Padding(2);
			tbMessage.Name = "tbMessage";
			tbMessage.PlaceholderText = "Message";
			tbMessage.Size = new Size(168, 18);
			tbMessage.TabIndex = 2;
			tbMessage.WordWrap = false;
			// 
			// btnSendMessage
			// 
			btnSendMessage.BackColor = Color.FromArgb(118, 185, 186);
			btnSendMessage.Enabled = false;
			btnSendMessage.FlatAppearance.BorderSize = 0;
			btnSendMessage.FlatStyle = FlatStyle.Flat;
			btnSendMessage.Font = new Font("Segoe UI Variable Small Semibol", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
			btnSendMessage.ForeColor = Color.White;
			btnSendMessage.Location = new Point(195, 9);
			btnSendMessage.Margin = new Padding(2);
			btnSendMessage.Name = "btnSendMessage";
			btnSendMessage.Size = new Size(63, 28);
			btnSendMessage.TabIndex = 3;
			btnSendMessage.Text = "Send";
			btnSendMessage.UseVisualStyleBackColor = false;
			btnSendMessage.Click += btnSendMessage_Click;
			// 
			// cbBaud
			// 
			cbBaud.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			cbBaud.DropDownStyle = ComboBoxStyle.DropDownList;
			cbBaud.FormattingEnabled = true;
			cbBaud.Items.AddRange(new object[] { "300 baud", "600 baud", "750 baud", "1200 baud", "2400 baud", "4800 baud", "9600 baud", "19200 baud", "31250 baud", "38400 baud", "57600 baud", "74880 baud", "115200 baud", "230400 baud", "250000 baud", "480800 baud", "500000 baud", "921600 baud", "1000000 baud", "2000000 baud" });
			cbBaud.Location = new Point(1366, 12);
			cbBaud.Margin = new Padding(2);
			cbBaud.Name = "cbBaud";
			cbBaud.Size = new Size(129, 23);
			cbBaud.TabIndex = 1;
			tooltip.SetToolTip(cbBaud, "Select baud rate");
			// 
			// cbPort
			// 
			cbPort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			cbPort.DropDownStyle = ComboBoxStyle.DropDownList;
			cbPort.FormattingEnabled = true;
			cbPort.Location = new Point(1288, 12);
			cbPort.Margin = new Padding(2);
			cbPort.Name = "cbPort";
			cbPort.Size = new Size(75, 23);
			cbPort.TabIndex = 0;
			tooltip.SetToolTip(cbPort, "Select COM port");
			// 
			// cmParameter
			// 
			cmParameter.BackColor = SystemColors.ControlLightLight;
			cmParameter.ImageScalingSize = new Size(24, 24);
			cmParameter.Items.AddRange(new ToolStripItem[] { cmOptRemoveParameter });
			cmParameter.Name = "contextMenuStrip1";
			cmParameter.Size = new Size(183, 34);
			cmParameter.Opening += cmParameter_Opening;
			// 
			// cmOptRemoveParameter
			// 
			cmOptRemoveParameter.Image = ArduinoSerialPlotter.Properties.Resources.delete;
			cmOptRemoveParameter.Name = "cmOptRemoveParameter";
			cmOptRemoveParameter.Size = new Size(182, 30);
			cmOptRemoveParameter.Text = "Remove parameter";
			cmOptRemoveParameter.Click += cmOptRemoveParameter_Click;
			// 
			// btnSizeParameters
			// 
			btnSizeParameters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnSizeParameters.BackColor = Color.LightSteelBlue;
			btnSizeParameters.BackgroundImageLayout = ImageLayout.Zoom;
			btnSizeParameters.FlatAppearance.BorderSize = 0;
			btnSizeParameters.FlatStyle = FlatStyle.Flat;
			btnSizeParameters.ImageIndex = 1;
			btnSizeParameters.ImageList = ilSizeParameters;
			btnSizeParameters.Location = new Point(1535, 16);
			btnSizeParameters.Margin = new Padding(2);
			btnSizeParameters.Name = "btnSizeParameters";
			btnSizeParameters.Size = new Size(14, 20);
			btnSizeParameters.TabIndex = 1;
			tooltip.SetToolTip(btnSizeParameters, "Show/Hide Parameters");
			btnSizeParameters.UseVisualStyleBackColor = false;
			btnSizeParameters.Click += btnSizeParameters_Click;
			// 
			// ilSizeParameters
			// 
			ilSizeParameters.ColorDepth = ColorDepth.Depth32Bit;
			ilSizeParameters.ImageStream = (ImageListStreamer)resources.GetObject("ilSizeParameters.ImageStream");
			ilSizeParameters.TransparentColor = Color.Transparent;
			ilSizeParameters.Images.SetKeyName(0, "chevron-left.png");
			ilSizeParameters.Images.SetKeyName(1, "chevron-right.png");
			// 
			// pnlGraph
			// 
			pnlGraph.BackColor = Color.White;
			pnlGraph.Controls.Add(pnlGraphLabelHighlights);
			pnlGraph.Dock = DockStyle.Fill;
			pnlGraph.Location = new Point(0, 48);
			pnlGraph.Margin = new Padding(2);
			pnlGraph.Name = "pnlGraph";
			pnlGraph.Size = new Size(1329, 544);
			pnlGraph.TabIndex = 2;
			pnlGraph.Paint += pnlGraph_Paint;
			pnlGraph.MouseEnter += pnlGraph_MouseEnter;
			pnlGraph.MouseLeave += pnlGraph_MouseLeave;
			pnlGraph.MouseMove += pnlGraph_MouseMove;
			pnlGraph.Resize += pnlGraph_Resize;
			// 
			// pnlGraphLabelHighlights
			// 
			pnlGraphLabelHighlights.BackColor = Color.Transparent;
			pnlGraphLabelHighlights.Controls.Add(pnlGraphLabels);
			pnlGraphLabelHighlights.Dock = DockStyle.Fill;
			pnlGraphLabelHighlights.Location = new Point(0, 0);
			pnlGraphLabelHighlights.Margin = new Padding(2);
			pnlGraphLabelHighlights.Name = "pnlGraphLabelHighlights";
			pnlGraphLabelHighlights.Size = new Size(1329, 544);
			pnlGraphLabelHighlights.TabIndex = 1;
			pnlGraphLabelHighlights.Paint += pnlGraphLabelHighlights_Paint;
			// 
			// pnlGraphLabels
			// 
			pnlGraphLabels.BackColor = Color.Transparent;
			pnlGraphLabels.BackgroundImageLayout = ImageLayout.None;
			pnlGraphLabels.Dock = DockStyle.Fill;
			pnlGraphLabels.Font = new Font("Segoe UI Variable Text Semibold", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
			pnlGraphLabels.Location = new Point(0, 0);
			pnlGraphLabels.Margin = new Padding(2);
			pnlGraphLabels.Name = "pnlGraphLabels";
			pnlGraphLabels.Size = new Size(1329, 544);
			pnlGraphLabels.TabIndex = 0;
			pnlGraphLabels.MouseEnter += pnlGraphLabels_MouseEnter;
			pnlGraphLabels.MouseLeave += pnlGraphLabels_MouseLeave;
			pnlGraphLabels.MouseMove += pnlGraphLabels_MouseMove;
			// 
			// hGraphScroll
			// 
			hGraphScroll.Dock = DockStyle.Bottom;
			hGraphScroll.Enabled = false;
			hGraphScroll.LargeChange = 1;
			hGraphScroll.Location = new Point(0, 592);
			hGraphScroll.Maximum = 0;
			hGraphScroll.Name = "hGraphScroll";
			hGraphScroll.Size = new Size(1329, 39);
			hGraphScroll.TabIndex = 3;
			hGraphScroll.ValueChanged += hGraphScroll_ValueChanged;
			// 
			// sfExportData
			// 
			sfExportData.Filter = "CSV Files|*.csv";
			sfExportData.FileOk += sfExportData_FileOk;
			// 
			// splitContainer1
			// 
			splitContainer1.BackColor = Color.Black;
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.Location = new Point(0, 0);
			splitContainer1.Margin = new Padding(2);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(pnlGraph);
			splitContainer1.Panel1.Controls.Add(pnlGraphHeader);
			splitContainer1.Panel1.Controls.Add(hGraphScroll);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.BackColor = Color.LightCyan;
			splitContainer1.Panel2.Controls.Add(pnlParameters);
			splitContainer1.Panel2.Controls.Add(pnlParametersHeader);
			splitContainer1.Size = new Size(1549, 631);
			splitContainer1.SplitterDistance = 1329;
			splitContainer1.SplitterWidth = 6;
			splitContainer1.TabIndex = 4;
			// 
			// pnlParametersHeader
			// 
			pnlParametersHeader.BackColor = Color.WhiteSmoke;
			pnlParametersHeader.Controls.Add(label1);
			pnlParametersHeader.Dock = DockStyle.Top;
			pnlParametersHeader.Location = new Point(0, 0);
			pnlParametersHeader.Margin = new Padding(2);
			pnlParametersHeader.Name = "pnlParametersHeader";
			pnlParametersHeader.Size = new Size(214, 48);
			pnlParametersHeader.TabIndex = 0;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("Segoe UI Variable Text Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
			label1.Location = new Point(12, 16);
			label1.Margin = new Padding(0);
			label1.Name = "label1";
			label1.Size = new Size(78, 19);
			label1.TabIndex = 0;
			label1.Text = "Parameters";
			// 
			// Plotter
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(1549, 679);
			Controls.Add(btnSizeParameters);
			Controls.Add(splitContainer1);
			Controls.Add(pnlParametersContainer);
			Margin = new Padding(2);
			Name = "Plotter";
			Text = "COM9";
			FormClosing += Plotter_FormClosing;
			pnlGraphHeader.ResumeLayout(false);
			pnlParametersContainer.ResumeLayout(false);
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			cmParameter.ResumeLayout(false);
			pnlGraph.ResumeLayout(false);
			pnlGraphLabelHighlights.ResumeLayout(false);
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			pnlParametersHeader.ResumeLayout(false);
			pnlParametersHeader.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private Panel pnlGraphHeader;
		private Panel pnlParametersContainer;
		private Button btnClear;
		private ToolTip tooltip;
		private Button btnStopStart;
		private FlowLayoutPanel pnlParameters;
		private Panel pnlGraph;
		private HScrollBar hGraphScroll;
		private Button btnAutoScroll;
		private Button btnExport;
		private ComboBox cbBaud;
		private ComboBox cbPort;
		private ComboBox cbPointsToDisplay;
		private SaveFileDialog sfExportData;
		private Button btnSendMessage;
		private TextBox tbMessage;
		private Panel panel3;
		private SplitContainer splitContainer1;
		private ComboBox cbMessageTerminator;
		private Panel pnlParametersHeader;
		private Button btnSizeParameters;
		private Label label1;
		private ImageList ilSizeParameters;
		private ContextMenuStrip cmParameter;
		private ToolStripMenuItem cmOptRemoveParameter;
		private Button btnGraphStyle;
		private ImageList ilLineStyle;
		private Label lblDebug;
		private Panel pnlGraphLabels;
		private Panel pnlGraphLabelHighlights;
		private Button btnConnect;
	}
}
