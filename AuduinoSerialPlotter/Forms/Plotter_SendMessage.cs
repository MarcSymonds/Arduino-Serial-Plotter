
namespace AuduinoSerialPlotter {
	public partial class Plotter : Form {
		/* In dropdown: CR/LF, CR, LF, None */
		private string[] _messageTerminators = ["\r\n", "\r", "\n", ""];

		public void InitializeSendMessage() {
			cbMessageTerminator.SelectedIndex = 3;
		}

		public void SetSendMessageState(bool enabled) {
			if (tbMessage.InvokeRequired) {
				tbMessage.Invoke((MethodInvoker)delegate {
					SetSendMessageState(enabled);
				});

				return;
			}

			tbMessage.Enabled = enabled;
			btnSendMessage.Enabled = enabled;
		}

		private void btnSendMessage_Click(object sender, EventArgs e) {
			string message = tbMessage.Text + _messageTerminators[cbMessageTerminator.SelectedIndex];

			if (_comReader != null)
				_comReader.SendMessage(message);

			tbMessage.Text = string.Empty;
		}
	}
}
