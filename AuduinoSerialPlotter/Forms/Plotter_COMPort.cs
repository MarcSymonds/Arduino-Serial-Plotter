using System.IO.Ports;

namespace AuduinoSerialPlotter {
	/// <summary>
	/// Methods to handle changing the COM port or baud rate.
	/// </summary>
	public partial class Plotter : Form {
		public void InitialiseCOMPort(string comPort, string baudRate) {
			ListCOMPorts(comPort);

			if (cbPort.SelectedIndex < 0 && cbPort.Items.Count > 0)
				cbPort.SelectedIndex = 0;

			for (int i = 0; i < cbBaud.Items.Count; i++) {
				if (cbBaud.Items[i].ToString().Equals(baudRate)) {
					cbBaud.SelectedIndex = i;
					break;
				}
			}

			if (cbBaud.SelectedIndex < 0)
				cbBaud.SelectedIndex = 12; // Default to the 115200 baud rate.

			cbPort.SelectedIndexChanged += cbPort_SelectedIndexChanged;
			cbBaud.SelectedIndexChanged += cbBaud_SelectedIndexChanged;
		}

		private bool ListCOMPorts(string? defaultCOMPort = null) {
			string? current = cbPort.SelectedItem?.ToString();// cbPointsToDisplay.SelectedIndex < 0 ? string.Empty : cbPort.Items[cbPort.SelectedIndex].ToString();
			bool stillHasCurrent = false;

			cbPort.Items.Clear();
			cbPort.Items.AddRange(SerialPort.GetPortNames());

			if (defaultCOMPort != null)
				current = defaultCOMPort;

			if (current != null) {
				for (int i = 0; i < cbPort.Items.Count; i++) {
					if (cbPort.Items[i].Equals(current)) {
						cbPort.SelectedIndex = i;
						stillHasCurrent = true;
						break;
					}
				}
			}

			if (cbPort.SelectedIndex < 0 && cbPort.Items.Count > 0)
				cbPort.SelectedIndex = 0;

			return stillHasCurrent;
		}

		private void cbBaud_SelectedIndexChanged(object? sender, EventArgs e) {
			ChangeDevice();
		}

		private void cbPort_SelectedIndexChanged(object? sender, EventArgs e) {
			ChangeDevice();
		}

		public void ChangeDevice(bool startReading = true) {
			if (_comReader != null) {
				_comReader.DataReceived -= ComReader_DataReceived;
				_comReader.COMException -= ComReader_COMException;
				_comReader.Dispose();
				_comReader = null;
			}

			ClearGraph();

			if (cbPort.SelectedIndex >= 0 && cbBaud.SelectedIndex >= 0) {
				string comPort = cbPort.Items[cbPort.SelectedIndex].ToString();
				string baudRate = cbBaud.Items[cbBaud.SelectedIndex].ToString();

				this.Text = string.Format("{0} / {1}", comPort, baudRate);

				_comReader = new COMReader(comPort, baudRate);
				_comReader.DataReceived += ComReader_DataReceived;
				_comReader.COMException += ComReader_COMException;

				if (startReading) {
					SetConnectedState(true);
					SetSendMessageState(true);
					_comReader.StartReading();
				}
			}
			else
				this.Text = "Not connected";
		}
	}
}
