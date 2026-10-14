using System.Text.RegularExpressions;

namespace AuduinoSerialPlotter {
	public class PlotData {
		private static List<Color> _colourList = [
			// Colours from the Arduino Serial Plotter.
			//
			Color.FromArgb(0x00, 0x72, 0xb2),
			Color.FromArgb(0xd5, 0x5e, 0x00),
			Color.FromArgb(0x00, 0x9e, 0x73),
			Color.FromArgb(0xe6, 0x9f, 0x00),
			Color.FromArgb(0xcc, 0x79, 0xa7),
			Color.FromArgb(0x56, 0xb4, 0xe9),
			Color.FromArgb(0xae, 0xa1, 0x0e),
			Color.FromArgb(0x95, 0xa5, 0xa6),

			// Others.
			//
			Color.DarkBlue,
			Color.DarkOliveGreen,
			Color.IndianRed,
			Color.Tomato,
			Color.DarkViolet,
			Color.DarkOrange,
			Color.SandyBrown,
			Color.Cyan,
			Color.HotPink,
			Color.Goldenrod,
			Color.SeaGreen,
			Color.DarkGray,
			Color.Red,
			Color.Teal,
			Color.SaddleBrown,
			Color.Lime,
			Color.Firebrick,
			Color.DarkOrchid
		];

		private int _colourIndex = 0;

		/// <summary>
		/// Regex to extract label-value pairs from the received data. The expected format is "label:value",
		/// where the label consists of letters, numbers, underscores, or hyphens, and the value is a
		/// number (integer or decimal). A comma is used to separate multiple label-value pairs in the 
		/// received data.
		/// </summary>
		private Regex extract = new Regex(@"(?<label>[-_a-z][^:]*)\s*:\s*(?<value>-?\d+(\.\d+)?)", RegexOptions.IgnoreCase);


		private List<string> RemovedParameters = new List<string>();

		public float? MinValue { get; private set; } = null;
		public float? MaxValue { get; private set; } = null;

		public int DataPointCount { get; private set; } = 0;

		public Dictionary<string, PlotLine> PlotLines { get; private set; } = new Dictionary<string, PlotLine>();

		public List<PlotLine> AddDataPoints(string receivedData) {
			List<PlotLine> newPlotLines = new List<PlotLine>();
			PlotLine plotLine;

			Match parameter = extract.Match(receivedData);

			if (parameter.Success) {
				while (parameter.Success) {
					string label = parameter.Groups["label"].Value;
					float value = float.Parse(parameter.Groups["value"].Value);

					if (!RemovedParameters.Contains(label)) {

						if (!PlotLines.ContainsKey(label)) {
							plotLine = new PlotLine(label) {
								Color = _colourList[_colourIndex]
							};

							PlotLines[label] = plotLine;

							newPlotLines.Add(plotLine);

							_colourIndex = (_colourIndex + 1) % _colourList.Count;
						}
						else
							plotLine = PlotLines[label];

						plotLine.Add(value);

						if (!MinValue.HasValue || value < MinValue)
							MinValue = value;

						if (!MaxValue.HasValue || value > MaxValue)
							MaxValue = value;
					}

					parameter = parameter.NextMatch();
				}

				DataPointCount++;

				// Make sure all of the plot lines have the same number of data points by adding null
				// values to the end of any plot line that has fewer data points than the others.
				// Makes processing them easier later, since we don't have to keep checking the length
				// of each plot line's data points.
				foreach (var pl in PlotLines.Values) {
					if (pl.DataPoints != null) {
						while (pl.DataPoints.Count < DataPointCount)
							pl.Add(null);
					}
				}
			}

			return newPlotLines;
		}

		public void RemovePlotLine(string label) {
			if (PlotLines.ContainsKey(label)) {
				PlotLine pl = PlotLines[label];

				pl.Clear();

				PlotLines.Remove(label);

				if (!RemovedParameters.Contains(label))
					RemovedParameters.Add(label);
			}
		}

		public void GetVisiblePlotLinesMinMax(out float? min, out float? max) {
			min = null;
			max = null;

			foreach (var plotLine in PlotLines.Values) {
				if (plotLine.Visible && plotLine.DataPoints != null && plotLine.DataPoints.Count > 0) {
					if (!min.HasValue || (plotLine.MinValue.HasValue && plotLine.MinValue < min))
						min = plotLine.MinValue;

					if (!max.HasValue || (plotLine.MaxValue.HasValue && plotLine.MaxValue > max))
						max = plotLine.MaxValue;
				}
			}
		}

		public void Reset() {
			PlotLines.Clear();
			RemovedParameters.Clear();
			MinValue = null;
			MaxValue = null;
			_colourIndex = 0;
			DataPointCount = 0;
		}
	}
}
