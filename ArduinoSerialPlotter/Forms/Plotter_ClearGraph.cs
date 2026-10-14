namespace AuduinoSerialPlotter {
	public partial class Plotter : Form {
		public void btnClear_Click(object sender, EventArgs e) {
			bool currState = false;

			if (_comReader != null) {
				currState = _comReader.CaptureData;
				_comReader.CaptureData = false;
			}

			ClearGraph();

			if (_comReader != null)
				_comReader.CaptureData = currState;
		}

		private void ClearGraph() {
			lock (_plotData) {
				_plotData.Reset();

				if (this.IsHandleCreated) {
					pnlParameters.Invoke((MethodInvoker)delegate {
						pnlParameters.Controls.Clear();
					});
				}
			}

			SetHorizontalScroll();
			DrawGraph();
		}
	}
}
