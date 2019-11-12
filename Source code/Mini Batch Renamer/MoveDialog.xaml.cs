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
    /// Interaction logic for MoveDialog.xaml
    /// </summary>
    public partial class MoveDialog : Window
    {
        public delegate void AddMethodDelegate(string methodName, string[] parameters);
        public event AddMethodDelegate AddMethodEvent = null;

        public MoveDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            //không check thì không thực hiện
            if (!RadioButton1.IsChecked.Value && !RadioButton2.IsChecked.Value)
            {
                MessageBox.Show("Chưa chọn yêu cầu");
                this.DialogResult = true;
                this.Close();
                return;
            }
            int len;
            if (!int.TryParse(LengthISBN.Text.ToString(), out len) || len <= 0)
            {
                MessageBox.Show("Độ dài ISBN không hợp lệ");
                this.DialogResult = true;
                this.Close();
                return;
            }
            if (AddMethodEvent != null)
            {
                string[] parameters = { LengthISBN.Text, RadioButton1.IsChecked.ToString(), RadioButton2.IsChecked.ToString() };
                AddMethodEvent.Invoke("Move", parameters);
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
