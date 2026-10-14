namespace AuduinoSerialPlotter {
	public partial class Plotter : Form {
		/// <summary>
		/// Methods to handle changes to the number of points to display.
		/// </summary>
		public void InitialisePointsToDisplay() {
			string value = _maxVisibleXDataPoints.ToString();

			for (int i = 0; i < cbPointsToDisplay.Items.Count; i++) {
				if (cbPointsToDisplay.Items[i].ToString().Equals(value)) {
					cbPointsToDisplay.SelectedIndex = i;
					break;
				}
			}

			if (cbPointsToDisplay.SelectedIndex < 0)
				cbPointsToDisplay.SelectedIndex = 1;

			cbPointsToDisplay.SelectedIndexChanged += cbPointsToDisplay_SelectedIndexChanged;
		}

		public void cbPointsToDisplay_SelectedIndexChanged(object? sender, EventArgs e) {
			SetPointsToDisplay();
		}

		private void SetPointsToDisplay() {
			_maxVisibleXDataPoints = cbPointsToDisplay.SelectedIndex < 0 ? 50 : int.Parse(cbPointsToDisplay.Items[cbPointsToDisplay.SelectedIndex].ToString());

			SetHorizontalScroll();

			DrawGraph();
		}
	}
}
