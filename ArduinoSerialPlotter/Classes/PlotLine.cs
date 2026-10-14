namespace AuduinoSerialPlotter {
	public class PlotLine {
		public string? Label { get; set; }
		public List<float?>? DataPoints { get; set; } = null;

		public Color Color { get; set; } = Color.FromArgb(255, 0, 0);
		public bool Visible { get; set; } = true;

		public float? MinValue { get; set; } = null;
		public float? MaxValue { get; set; } = null;

		public PlotLine() {

		}

		public PlotLine(string label) {
			Label = label;
			DataPoints = new List<float?>();
		}

		public void Add(float? value) {
			DataPoints?.Add(value);

			if (!MinValue.HasValue || (value.HasValue && value < MinValue))
				MinValue = value;

			if (!MaxValue.HasValue || (value.HasValue && value > MaxValue))
				MaxValue = value;
		}

		public void Clear() {
			if (DataPoints != null)
				DataPoints.Clear();
		}
	}
}
