using System;
using System.Drawing;
using System.Windows.Forms;

namespace FormSMSMultipleInstance
{
    internal class InputBox
    {
        private static TextBox _textBox = new TextBox();

        public static DialogResult ShowInputDialog(ref string input)
        {
            var size = new Size(200, 70);
            var inputBox = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ClientSize = size,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                TopMost = true,
            Text = "Enter New Number"
            };

            _textBox = new TextBox
            {
                Size = new Size(size.Width - 10, 23),
                Location = new Point(5, 5),
                Text = input
            };
            inputBox.Controls.Add(_textBox);

            _textBox.TextChanged += textBox_TextChanged;

            var okButton = new Button
            {
                DialogResult = DialogResult.OK,
                Name = "okButton",
                Size = new Size(75, 23),
                Text = "&OK",
                Location = new Point(size.Width - 80, 39)
            };
            inputBox.Controls.Add(okButton);

            inputBox.AcceptButton = okButton;

            var result = inputBox.ShowDialog();

            if (result == DialogResult.OK)
            {
                input = _textBox.Text;
            }

            return result;
        }

        private static void textBox_TextChanged(object sender, EventArgs e)
        {
            var originalText = _textBox.Text.ToCharArray();
            foreach (var c in originalText)
            {
                if (!(char.IsNumber(c)))
                {
                    _textBox.Text = _textBox.Text.Remove(_textBox.Text.IndexOf(c));
                }
            }
        }
    }
}
