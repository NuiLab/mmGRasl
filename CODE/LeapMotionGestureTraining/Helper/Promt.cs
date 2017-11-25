using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeapMotionGestureTraining.Helper
{
    class Promt
    {

        /// <summary>
        /// Show a dialog that containa an input textbox
        /// </summary>
        /// <param name="text">Textbox description </param>
        /// <param name="dialogTitle">Title of dialog</param>
        /// <returns>Return entered text </returns>
        public static string ShowDialog(string text, string dialogTitle)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = dialogTitle,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        /// <summary>
        /// Show a dialog that contain checked list box
        /// </summary>
        /// <param name="signFolders">Gesture folder list </param>
        /// <param name="dialogTitle">Title of dialog</param>
        /// <returns>Return checked gesture folder list</returns>
        public static List<string> ShowCheckedListBox(string[] signFolders, string dialogTitle)
        {

            Form prompt = new Form()
            {
                Width = 500,
                Height = 500,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = dialogTitle,
                StartPosition = FormStartPosition.CenterScreen
            };

            CheckedListBox clbSigns = new CheckedListBox() { Top = 60, Left = 10, Width = 460, Height = 340};
            foreach (string sign in signFolders)
            {
                clbSigns.Items.Add(sign);
            }

            Button btnSelectAll = new Button() { Text = "Select All", Top = 10, Left = 10, Width = 150 };
            btnSelectAll.Click += (sender, e) =>
            {
                for (int i = 0; i < clbSigns.Items.Count; i++)
                {
                    clbSigns.SetItemChecked(i, true);
                }
            };

            Button btnUnSelectAll = new Button() { Text = "Unselect All", Top = 10, Left = 200, Width = 150 };
            btnUnSelectAll.Click += (sender, e) =>
            {
                for (int i = 0; i < clbSigns.Items.Count; i++)
                {
                    clbSigns.SetItemChecked(i, false);
                }
            };

     
            Button confirmation = new Button() { Text = "Ok", Left = 100,Width = 100, Top = 410, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => 
            {
                List<string> selectedItems = clbSigns.CheckedItems.OfType<string>().ToList();
                prompt.Close();
            };
            confirmation.Left = prompt.Width / 2 - 50;
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(btnSelectAll);
            prompt.Controls.Add(btnUnSelectAll);
            prompt.Controls.Add(clbSigns);

            return prompt.ShowDialog() == DialogResult.OK ? clbSigns.CheckedItems.OfType<string>().ToList() : null;
        }
    }
}
