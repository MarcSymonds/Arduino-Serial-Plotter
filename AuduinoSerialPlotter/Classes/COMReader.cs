using System.IO.Ports;
using System.Text.RegularExpressions;

namespace AuduinoSerialPlotter {
	public class COMReaderExceptionEventArgs : EventArgs {
		public string Action { get; set; }
		public Exception Exception { get; set; }

	}

	public class COMReader {
		public EventHandler<string>? DataReceived;
		public EventHandler<object, COMReaderExceptionEventArgs>? COMException;

		public bool CaptureData { get; set; }

		private SerialPort? _serialPort = null;
		private bool _continueReading;

		private Thread? _readThread = null;


		public bool IsPortOpen {
			get {
				return _serialPort != null && _serialPort.IsOpen;
			}
		}

		public COMReader(string port = "COM9", int baudRate = 115200) {
			SetSerialPort(port, baudRate);
		}

		public COMReader(string port, string baudRate) {
			Regex getRate = new Regex(@"\d+");

			Match match = getRate.Match(baudRate);

			if (match.Success)
				SetSerialPort(port, int.Parse(match.Value));
			else
				throw new ArgumentException("Could not parse baud rate from [" + baudRate + "]", nameof(baudRate));
		}

		private void SetSerialPort(string port, int baudRate) {
			_serialPort = new SerialPort(port, baudRate) {
				Parity = Parity.None,
				DataBits = 8,
				StopBits = StopBits.One,
				Handshake = Handshake.None,

				ReadTimeout = 500,
				WriteTimeout = 500,
			};

			/*try {
				_serialPort.Open();
			}
			catch (Exception ex) {
				if (COMException != null)
					COMException.Invoke(this, new COMReaderExceptionEventArgs() {
						Action = "Open",
						Exception = ex
					});
			}*/
		}

		/// <summary>
		/// Opens the serial port, and starts the reading thread.
		/// </summary>
		/// <returns></returns>
		public bool StartReading() {
			if (_serialPort != null) {
				_readThread = new Thread(Read);

				try {
					if (!_serialPort.IsOpen)
						_serialPort.Open();

					CaptureData = true;
					_continueReading = true;
					_readThread.Start();

					return true;
				}
				catch (Exception ex) {
					if (COMException != null)
						COMException.Invoke(this, new COMReaderExceptionEventArgs() {
							Action = "Open",
							Exception = ex
						});
				}
			}

			return false;
		}

		/// <summary>
		/// Closes the serial port, and terminates the reading thread.
		/// </summary>
		public void StopReading() {
			CaptureData = false;
			_continueReading = false;

			if (_readThread != null) {
				if (_readThread.IsAlive)
					_readThread.Join(3000);

				_readThread = null;
			}

			if (_serialPort != null)
				_serialPort.Close();
		}

		/// <summary>
		/// Stops the read thread, closes the port and disposes it.
		/// </summary>
		public void Dispose() {
			StopReading();

			if (_serialPort != null) {
				if (_serialPort.IsOpen)
					_serialPort.Close();

				_serialPort.Dispose();
				_serialPort = null;
			}
		}

		private void Read() {
			while (_continueReading && _serialPort != null && _serialPort.IsOpen) {
				try {
					string message = _serialPort.ReadLine();

					if (CaptureData && _continueReading && DataReceived != null)
						DataReceived.Invoke(this, message);
				}
				catch (TimeoutException) {
					// Ignore timeouts since the device might not be sending anything.
				}
				catch (Exception ex) {
					if (COMException != null)
						COMException.Invoke(this, new COMReaderExceptionEventArgs() {
							Action = "Read",
							Exception = ex
						});
				}
			}
		}

		/// <summary>
		/// Send a message to the device. Will open the serial port, if it's not open.
		/// </summary>
		/// <param name="message"></param>
		public void SendMessage(string message) {
			if (_serialPort != null) {
				try {
					if (!_serialPort.IsOpen)
						_serialPort.Open();

					_serialPort.Write(message);
				}
				catch (Exception ex) {
					if (COMException != null) {
						COMException.Invoke(this, new COMReaderExceptionEventArgs() {
							Action = "Write",
							Exception = ex
						});
					}
				}
			}
		}
	}
}
