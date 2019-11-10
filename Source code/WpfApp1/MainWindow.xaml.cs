using System.Collections.Generic;
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
        private HashSet<string> pathSet = null;

        public MainWindow()
        {
            InitializeComponent();
            listObject = new ObservableCollection<FileInfomation>();
            datagrid.ItemsSource = listObject;
            pathSet = new HashSet<string>();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void FileMode_OnClick(object sender, RoutedEventArgs e)
        {
            if (listObject.Count == 0)
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
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ModeButton.IsEnabled = false;
                string[] pathFiles;
                if (isFileMode)
                {
                    pathFiles = Directory.GetFiles(folderBrowserDialog.SelectedPath);
                }
                else
                {
                    pathFiles = Directory.GetDirectories(folderBrowserDialog.SelectedPath);
                }
                MessageBoxResult msgResult = MessageBoxResult.No;
                foreach (var dir in pathFiles)
                {
                    if (pathSet.Contains(dir))
                    {
                        if (msgResult == MessageBoxResult.No)
                        {
                            var Result = MessageBox.Show(dir + " đã tồn tại trong danh sách\n\nChọn No để bỏ qua file này\nChọn Yes để bỏ qua tất cả những file trùng nhau, chỉ thêm vào những file khác nhau", "MessageBox", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                            if (Result == MessageBoxResult.Cancel)
                            {
                                msgResult = MessageBoxResult.Cancel;
                                break;
                            }
                            else if (Result == MessageBoxResult.No)
                            {
                                msgResult = MessageBoxResult.No;
                            }
                            else
                            {
                                msgResult = MessageBoxResult.Yes;
                            }
                        }
                    }
                    else
                    {
                        pathSet.Add(dir);
                        listObject.Add(new FileInfomation(Path.GetFileName(dir), "", dir, ""));
                    }
                }
                datagrid.SelectedIndex = listObject.Count - 1;
                datagrid.ScrollIntoView(datagrid.SelectedItem);
                datagrid.SelectedIndex = -1;
            }

        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            int n = datagrid.SelectedIndex;
            if (n > -1)
            {
                pathSet.Remove(listObject[n].Path);
                listObject.RemoveAt(n);

                if (listObject.Count == 0)
                {
                    ModeButton.IsEnabled = true;
                }
            }
        }

        private void ClearAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            listObject.Clear();
            pathSet.Clear();
            ModeButton.IsEnabled = true;
        }

        private void MoveDownButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (listObject.Count != 0)
            {
                if (datagrid.SelectedIndex == -1)
                {
                    datagrid.SelectedIndex = 0;
                    datagrid.Focus();
                    datagrid.ScrollIntoView(datagrid.SelectedItem);
                }
                else if (datagrid.SelectedIndex == listObject.Count - 1)
                {
                    DataGridRow row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(datagrid.SelectedIndex);
                    row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                else
                {
                    datagrid.SelectedIndex++;
                    DataGridRow row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(datagrid.SelectedIndex);
                    row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
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
                else if (datagrid.SelectedIndex == -1) // trường hợp chưa chọn dòng nào
                {
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

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("1712907 - Phùng Quốc Việt\n\n1712912 - Nguyễn Hoàng Vinh\n\n1712914 - Phan Nhật Vinh", "About Simple Batch Renamer");
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

