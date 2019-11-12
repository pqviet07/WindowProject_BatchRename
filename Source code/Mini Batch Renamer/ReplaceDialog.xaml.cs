using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ReplaceDialog.xaml
    /// </summary>
    public partial class ReplaceDialog : Window
    {
        public delegate void AddMethodDelegate(string methodName, string[] parameters);
        public event AddMethodDelegate AddMethodEvent = null;

        public ReplaceDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (char item in "\\/:*?\"<>|")
            {
                if (NewString.Text.Contains(item))
                {
                    MessageBox.Show("Từ thay thế có chứa kí tự không hợp lệ");
                    this.DialogResult = true;
                    this.Close();
                    return;
                }
            }
            if (OldString.Text == "")
            {
                MessageBox.Show("Từ thay thế chưa được điền");
                this.DialogResult = true;
                this.Close();
                return;
            }
            if (AddMethodEvent != null)
            {
                string[] parameters = { OldString.Text, NewString.Text };
                AddMethodEvent.Invoke("Replace", parameters);
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
