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
			bool stillHasCurrent = false;

			cbPort.SelectedIndexChanged -= cbPort_SelectedIndexChanged;

			string[] portNames = SerialPort.GetPortNames();

			foreach (string portName in portNames) {
				bool exists = false;

				for (int i = 0; i < cbPort.Items.Count; i++) {
					if (portName.Equals(cbPort.Items[i].ToString())) {
						exists = true;
						break;
					}
				}

				if (!exists)
					cbPort.Items.Add(portName);
			}

			int defaultIdx = -1;

			for (int i = cbPort.Items.Count - 1; i >= 0; i--) {
				if (!portNames.Contains(cbPort.Items[i].ToString())) {
					if (i == cbPort.SelectedIndex)
						cbPort.SelectedIndex = -1;

					cbPort.Items.RemoveAt(i);

					if (i < defaultIdx)
						--defaultIdx;
				}
				else if (defaultCOMPort != null && defaultCOMPort.Equals(cbPort.Items[i].ToString()))
					defaultIdx = i;
			}

			cbPort.SelectedIndexChanged += cbPort_SelectedIndexChanged;

			if (defaultIdx >= 0 && defaultIdx != cbPort.SelectedIndex)
				cbPort.SelectedIndex = defaultIdx;
			else if (cbPort.SelectedIndex < 0 && cbPort.Items.Count > 0)
				cbPort.SelectedIndex = 0;
			else
				stillHasCurrent = true;

			return stillHasCurrent;
		}

		private void cbBaud_SelectedIndexChanged(object? sender, EventArgs e) {
			ChangeDevice();
		}

		private void cbPort_SelectedIndexChanged(object? sender, EventArgs e) {
			if (cbPort.Tag == null || (bool)cbPort.Tag == false) {
				ChangeDevice();
			}
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
