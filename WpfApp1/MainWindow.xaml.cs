using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool isFileMode = true;
        private ObservableCollection<FileInfomation> listObject = null;
        private static int c = 1;
        public MainWindow()
        {
            InitializeComponent();
            listObject = new ObservableCollection<FileInfomation>();
            datagrid.ItemsSource = listObject;

        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void FileMode_OnClick(object sender, RoutedEventArgs e)
        {

            if (isFileMode == true)
            {
                isFileMode = false;
            }
            else
            {
                isFileMode = true;
            }
        }


        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (isFileMode)
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    string[] pathFiles = Directory.GetFiles(folderBrowserDialog.SelectedPath);
                    foreach (var dir in pathFiles)
                    {
                        listObject.Add(new FileInfomation(Path.GetFileName(dir), "", dir, ""));
                    }
                }
            }
            else
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    string[] pathFiles = Directory.GetDirectories(folderBrowserDialog.SelectedPath);
                    foreach (var dir in pathFiles)
                    {
                        listObject.Add(new FileInfomation(Path.GetFileName(dir), "", dir, ""));
                    }
                }
            }
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            int n = datagrid.SelectedIndex;
            if (n > -1)
            {
                listObject.RemoveAt(n);
            }
        }

        private void ClearAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            listObject.Clear();
        }

        private void MoveDownButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (listObject.Count != 0)
            {
                if (datagrid.SelectedIndex + 1 != listObject.Count)
                {
                    datagrid.SelectedIndex++;
                }
                DataGridRow row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(datagrid.SelectedIndex);
                row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }

        }

        private void MoveTopButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (listObject.Count != 0)
            {
                datagrid.SelectedIndex = 0;
                datagrid.Focus();
                datagrid.ScrollIntoView(datagrid.SelectedItem);
            }
        }
        private void MoveBottomButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (listObject.Count != 0)
            {
                if (listObject.Count != 0)
                {
                    datagrid.SelectedIndex = listObject.Count - 1;
                    datagrid.Focus();
                    datagrid.ScrollIntoView(datagrid.SelectedItem);
                }
            }
        }
        private void MoveUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (listObject.Count != 0)
            {
                if (datagrid.SelectedIndex == 0)
                {
                    DataGridRow row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(datagrid.SelectedIndex);
                    row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                else if (datagrid.SelectedIndex == -1)
                {
                    MessageBox.Show(datagrid.SelectedIndex.ToString());
                    datagrid.SelectedIndex = listObject.Count - 1;
                    datagrid.Focus();
                }
                else
                {
                    datagrid.SelectedIndex--;
                    DataGridRow row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(datagrid.SelectedIndex);
                    row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }


    }
}


public class FileInfomation
{
    public string CurrName { get; set; }
    public string NewName { get; set; }
    public string Path { get; set; }
    public string Error { get; set; }

    public FileInfomation(string a1, string a2, string a3, string a4)
    {
        this.CurrName = a1;
        this.NewName = a2;
        this.Path = a3;
        this.Error = a4;
    }
    public override string ToString()
    {
        return this.CurrName + " " + this.NewName + " " + this.Path + " " + this.Error;
    }
}

