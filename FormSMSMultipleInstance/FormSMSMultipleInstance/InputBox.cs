using System.Drawing;
using System.Windows.Forms;

namespace FormSMSMultipleInstance
{
    class InputBox
    {
        public static DialogResult ShowInputDialog(ref string input)
        {
            Size size = new Size(200, 70);
            Form inputBox = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ClientSize = size,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                Text = "Enter New Number"
            };

            TextBox textBox = new TextBox
            {
                Size = new Size(size.Width - 10, 23),
                Location = new Point(5, 5),
                Text = input
            };
            inputBox.Controls.Add(textBox);

            Button okButton = new Button
            {
                DialogResult = DialogResult.OK,
                Name = "okButton",
                Size = new Size(75, 23),
                Text = "&OK",
                Location = new Point(size.Width - 80, 39)
            };
            inputBox.Controls.Add(okButton);

            inputBox.AcceptButton = okButton;

            DialogResult result = inputBox.ShowDialog();

            if (result == DialogResult.OK)
            {
                input = textBox.Text;
            }

            return result;
        }

    }
}
