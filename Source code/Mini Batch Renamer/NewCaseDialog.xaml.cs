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
    /// Interaction logic for NewCaseDialog.xaml
    /// </summary>
    public partial class NewCaseDialog : Window
    {
        public delegate void AddMethodDelegate(string methodName, string[] parameters);
        public event AddMethodDelegate AddMethodEvent = null;

        public NewCaseDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            //không check thì không thực hiện
            if (!CheckBox1.IsChecked.Value && !CheckBox2.IsChecked.Value && !CheckBox3.IsChecked.Value)
            {
                MessageBox.Show("Chưa chọn yêu cầu");
                this.DialogResult = true;
                this.Close();
                return;
            }
            if (AddMethodEvent != null)
            {
                string[] parameters = { CheckBox1.IsChecked.ToString(), CheckBox2.IsChecked.ToString(), CheckBox3.IsChecked.ToString() };
                AddMethodEvent.Invoke("NewCase", parameters);
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
