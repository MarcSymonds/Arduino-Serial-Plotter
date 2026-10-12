namespace AuduinoSerialPlotter {
	/// <summary>
	/// Methods to deal with exporting the plot data.
	/// </summary>
	public partial class Plotter : Form {
		private void btnExport_Click(object sender, EventArgs e) {
			SetStopResumeState(false);

			sfExportData.ShowDialog(this);
		}

		private void sfExportData_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
			string fileName = sfExportData.FileName;

			if (string.IsNullOrEmpty(fileName)) {
				MessageBox.Show(this, "Please enter the name of the file to export to!", "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// Lock the plot data so it doesn't change while we are outputting it.
			//
			lock (_plotData) {
				try {
					using (FileStream fs = File.Create(fileName)) {
						using (StreamWriter sw = new StreamWriter(fs)) {
							List<string> values = new List<string>();

							values.Add("ID");

							foreach (var plotLine in _plotData.PlotLines.Values) {
								if (plotLine.Visible) {
									string value = plotLine.Label;

									if (value.Contains(','))
										values.Add(string.Format("\"{0}\"", value));
									else
										values.Add(value);
								}
							}

							sw.WriteLine(string.Join(",", values));

							for (int i = 0; i < _plotData.DataPointCount; i++) {
								values.Clear();

								values.Add(i.ToString());

								foreach (var plotLine in _plotData.PlotLines.Values) {
									if (plotLine.Visible) {
										string value = plotLine.DataPoints[i].HasValue ? plotLine.DataPoints[i].Value.ToString() : string.Empty;

										values.Add(value);
									}
								}

								sw.WriteLine(string.Join(",", values));
							}

							sw.Close();
						}

						fs.Close();
					}

					MessageBox.Show(this, string.Format("Exported {0} data point{1}.", _plotData.DataPointCount, _plotData.DataPointCount == 1 ? string.Empty : "s"), "Export complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception ex) {
					MessageBox.Show(this, string.Format("Failed to create export: {0}", ex.Message), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}
