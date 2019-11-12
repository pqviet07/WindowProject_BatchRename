using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        private ObservableCollection<MethodInformation> listMethod = new ObservableCollection<MethodInformation>();
        private ObservableCollection<string> listDescriptMethod = null;
        private ObservableCollection<Index> listIndexMethod = null;
        private bool? isChangeExistName = null;
        private bool isRefreshLisstObject = false;

        public MainWindow()
        {
            InitializeComponent();
            listObject = new ObservableCollection<FileInfomation>();
            datagrid.ItemsSource = listObject;
            pathSet = new HashSet<string>();
            listDescriptMethod = new ObservableCollection<string>();
            listIndexMethod = new ObservableCollection<Index>();
            listview.ItemsSource = listDescriptMethod;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "MyFavourite.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                string data = "";
                foreach (var method in listDescriptMethod)
                {
                    data = data + method + "\n";
                }
                data = data + "********************\n";
                data = data + listMethod.Count.ToString();
                foreach (var method in listMethod)
                {
                    data = data + "\n" + method.ToString();
                }
                File.WriteAllText(saveFileDialog.FileName, data);
            }
        }

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                listMethod.Clear();
                listDescriptMethod.Clear();
                listIndexMethod.Clear();
                var lines = File.ReadAllLines(openFileDialog.FileName);
                int dem = 0;
                while (lines[dem][0] != '*')
                {
                    dem++;
                }
                int start = dem + 1;
                var count = int.Parse(lines[start]);
                if (count > 0)
                {
                    char[] seperator = {'|'};
                    for (int i = 1; i <= count; i++)
                    {
                        var tokens = lines[start + i].Split(seperator);
                        string methodName = tokens[0];
                        string[] methodParameters = new string[tokens.Length - 1]; 
                        for (int j = 1; j < tokens.Length; j++)
			            {
                            methodParameters[j - 1] = tokens[j];
			            }
                        AddMethodFunction(methodName, methodParameters);
                    }
                }
            }
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

                if (pathFiles.Length != 0)
                {
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
                else
                {
                    ModeButton.IsEnabled = true;
                }
                isRefreshLisstObject = false;
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

        private void ReplaceButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceDialog replaceDialog = new ReplaceDialog();
            replaceDialog.AddMethodEvent += AddMethodFunction;
            replaceDialog.ShowDialog();
        }

        private void NewCaseButton_Click(object sender, RoutedEventArgs e)
        {
            NewCaseDialog newCaseDialog = new NewCaseDialog();
            newCaseDialog.AddMethodEvent += AddMethodFunction;
            newCaseDialog.ShowDialog();
        }

        private void FullnameNormalizeButton_Click(object sender, RoutedEventArgs e)
        {
            FullnameNormalizeDialog fullnameNormalizeDialog = new FullnameNormalizeDialog();
            fullnameNormalizeDialog.AddMethodEvent += AddMethodFunction;
            fullnameNormalizeDialog.ShowDialog();
        }

        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            MoveDialog moveDialog = new MoveDialog();
            moveDialog.AddMethodEvent += AddMethodFunction;
            moveDialog.ShowDialog();
        }

        private void UniqueNameButton_Click(object sender, RoutedEventArgs e)
        {
            string[] tmp = { " " };
            AddMethodFunction("Unique", tmp);
        }

        private void AddMethodFunction(string methodName, string[] methodParameters)
        {
            listMethod.Add(new MethodInformation(methodName, methodParameters));
            if (methodName == "Replace")
            {
                listDescriptMethod.Add("- Thay thế chuỗi \"" + methodParameters[0] + "\"" + " thành chuỗi \"" + methodParameters[1] + "\"");
                listIndexMethod.Add(new Index(listMethod.Count - 1, 0));
            }
            else if (methodName == "NewCase")
            {
                if (methodParameters[0] == "True")
                {
                    listDescriptMethod.Add("- Toàn bộ chữ hoa");
                    listIndexMethod.Add(new Index(listMethod.Count - 1, 1));
                }
                if (methodParameters[1] == "True")
                {
                    listDescriptMethod.Add("- Toàn bộ chữ thường");
                    listIndexMethod.Add(new Index(listMethod.Count - 1, 2));
                }
                if (methodParameters[2] == "True")
                {
                    listDescriptMethod.Add("- Kí tự đầu tiên mỗi từ viết hoa, còn lại viết thường");
                    listIndexMethod.Add(new Index(listMethod.Count - 1, 3));
                }
            }
            else if (methodName == "FullnameNormalize")
            {
                if (methodParameters[0] == "True")
                {
                    listDescriptMethod.Add("- Bắt đầu và kết thúc không có khoảng trắng");
                    listIndexMethod.Add(new Index(listMethod.Count - 1, 4));
                }
                if (methodParameters[1] == "True")
                {
                    listDescriptMethod.Add("- Kí tự đầu tiên mỗi từ viết hoa, còn lại viết thường");
                    listIndexMethod.Add(new Index(listMethod.Count - 1, 5));
                }
                if (methodParameters[2] == "True")
                {
                    listDescriptMethod.Add("- Không có quá 1 khoảng trắng giữa các từ");
                    listIndexMethod.Add(new Index(listMethod.Count - 1, 6));
                }
            }
            else if (methodName == "Move")
            {
                if (methodParameters[1] == "True")
                {
                    listDescriptMethod.Add("- Di chuyển " + methodParameters[0] + " kí tự ISBN ra phía trước");
                    listIndexMethod.Add(new Index(listMethod.Count - 1, 7));
                }
                if (methodParameters[2] == "True")
                {
                    listDescriptMethod.Add("- Di chuyển " + methodParameters[0] + " kí tự ISBN ra phía sau");
                    listIndexMethod.Add(new Index(listMethod.Count - 1, 8));
                }
            }
            else if (methodName == "Unique")
            {
                listDescriptMethod.Add("- Tạo 1 tên ngẫu nhiên");
                listIndexMethod.Add(new Index(listMethod.Count - 1, 9));
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (listObject.Count == 0)
            {
                MessageBox.Show("Chưa chọn tập tin hoặc thư mục");
                return;
            }
            if (listMethod.Count == 0)
            {
                MessageBox.Show("Bạn chưa chọn thao tác");
                return;
            }
            List<string> listExistItem = new List<string>();
            bool existName = false;
            foreach (var item in listObject)
            {
                listExistItem.Add(item.CurrName);
            }

            foreach (var item in listObject)
            {
                listExistItem.RemoveAt(0);
                string tmpItemName, extension = "";
                //nếu là file thì không thao tác với đuôi file
                if (isFileMode)
                {
                    int indexOfExtension = item.CurrName.LastIndexOf('.');
                    if (indexOfExtension >= 0)
                    {
                        extension = item.CurrName.Substring(indexOfExtension);
                        tmpItemName = item.CurrName.Substring(0, indexOfExtension);
                    }
                    else
                    {
                        tmpItemName = item.CurrName;
                    }
                }
                else
                {
                    tmpItemName = item.CurrName;
                }
                foreach (var method in listMethod)
                {
                    // chỉ có replace là có thao tác với đuôi file
                    if (method.MethodName == "Replace")
                    {
                        string currString = method.MethodParameters[0];
                        string newString = method.MethodParameters[1];
                        tmpItemName = XuLiChuoi.Replace(tmpItemName + extension, currString, newString);
                        if (isFileMode)
                        {
                            int indexOfExtension = tmpItemName.LastIndexOf('.');
                            if (indexOfExtension >= 0)
                            {
                                extension = tmpItemName.Substring(indexOfExtension);
                                tmpItemName = tmpItemName.Substring(0, indexOfExtension);
                            }
                        }

                    }
                    else if (method.MethodName == "NewCase")
                    {
                        if (method.MethodParameters[0] == "True")
                        {
                            tmpItemName = XuLiChuoi.ToUpper(tmpItemName);
                        }
                        if (method.MethodParameters[1] == "True")
                        {
                            tmpItemName = XuLiChuoi.ToLower(tmpItemName);
                        }
                        if (method.MethodParameters[2] == "True")
                        {
                            tmpItemName = XuLiChuoi.ConvertFirstLetterOfEachWordToUpper(tmpItemName);
                        }
                    }
                    else if (method.MethodName == "FullnameNormalize")
                    {
                        if (method.MethodParameters[0] == "True")
                        {
                            tmpItemName = XuLiChuoi.Trim(tmpItemName);
                        }
                        if (method.MethodParameters[1] == "True")
                        {
                            tmpItemName = XuLiChuoi.ConvertFirstLetterOfEachWordToUpper(tmpItemName);
                        }
                        if (method.MethodParameters[2] == "True")
                        {
                            tmpItemName = XuLiChuoi.AllowOnlyOneSpaceBetweenWords(tmpItemName);
                        }
                    }
                    else if (method.MethodName == "Move")
                    {
                        if (method.MethodParameters[1] == "True")
                        {
                            try
                            {
                                tmpItemName = XuLiChuoi.MoveISBNFromEndToFrontOfString(tmpItemName, int.Parse(method.MethodParameters[0]));
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                item.Error = "không thể di chuyển isbn nên thao tác move không được thực hiện";
                            }
                        }
                        else
                        {
                            try
                            {
                                tmpItemName = XuLiChuoi.MoveISBNFromFrontToEndOfString(tmpItemName, int.Parse(method.MethodParameters[0]));
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                item.Error = "không thể di chuyển isbn nên thao tác move không được thực hiện";
                            }
                        }
                    }
                    else if (method.MethodName == "Unique")
                    {
                        tmpItemName = XuLiChuoi.CreateUniqueString();
                    }
                }

                //chưa cấu hình
                item.NewName = tmpItemName + extension;
                if (listExistItem.Contains(item.NewName))
                {
                    item.Error = "Item bị trùng tên";
                    existName = true;
                }
                else
                {
                    string newPath = item.Path.Substring(0, item.Path.LastIndexOf('\\')) + '\\' + item.NewName;
                    if (isFileMode)
                    {
                        try
                        {
                            File.Move(item.Path, newPath);
                        }
                        catch (Exception)
                        {
                            item.Error = "File không còn tồn tại ở thời điểm này";
                        }
                    }
                    else
                    {
                        try
                        {
                            var dir = new DirectoryInfo(item.Path);
                            dir.MoveTo(item.Path + XuLiChuoi.CreateUniqueString()); // di chuyển đến thư mục tạm thời
                            dir.MoveTo(newPath);
                        }
                        catch (Exception)
                        {
                            item.Error = "Thư mục không còn tồn tại ở thời điểm này";
                        }
                    }
                }
            }
            if (isChangeExistName == null && existName)
            {
                MessageBoxResult existNameMessageBox = MessageBox.Show("Có 1 số item bị trùng tên sau khi đổi tên, bạn có muốn giữ nguyên tên ban đầu hay thêm hậu tố là số?\nẤn Cancel để giữ nguyên tên ban đầu\nẤn OK để thêm hậu tố là số", "Bị trùng tên!!", MessageBoxButton.OKCancel);
                if (existNameMessageBox == MessageBoxResult.OK)
                {
                    isChangeExistName = true;
                }
                else
                {
                    isChangeExistName = false;
                }
            }
            if (isChangeExistName == true)
            {
                foreach (var item in listObject)
                {
                    listExistItem.Add(item.CurrName);
                }
                foreach (var item in listObject)
                {
                    listExistItem.RemoveAt(0);
                    if (item.Error == "Item bị trùng tên")
                    {
                        int count = 1;
                        string itemName, ext = "";
                        if (isFileMode)
                        {
                            itemName = item.NewName.Substring(0, item.NewName.LastIndexOf('.'));
                            ext = item.NewName.Substring(item.NewName.LastIndexOf('.'));
                        }
                        else
                        {
                            itemName = item.NewName;
                        }
                        while (listExistItem.Contains(itemName + count.ToString() + ext))
                        {
                            count++;
                        }
                        if (isFileMode)
                        {
                            item.NewName = itemName + count.ToString() + ext;
                            string newPath = item.Path.Substring(0, item.Path.LastIndexOf('\\')) + '\\' + item.NewName;
                            try
                            {
                                File.Move(item.Path, newPath);
                                item.Error = "";
                            }
                            catch (Exception)
                            {
                                item.Error = "File không còn tồn tại ở thời điểm này";
                            }
                        }
                        else
                        {
                            item.NewName = itemName + count.ToString();
                            string newPath = item.Path.Substring(0, item.Path.LastIndexOf('\\')) + '\\' + item.NewName;
                            try
                            {
                                var dir = new DirectoryInfo(item.Path);
                                dir.MoveTo(item.Path + XuLiChuoi.CreateUniqueString()); // di chuyển đến thư mục tạm thời
                                dir.MoveTo(newPath);
                                item.Error = "";
                            }
                            catch (Exception)
                            {
                                item.Error = "Thư mục không còn tồn tại ở thời điểm này";
                            }
                        }
                    }
                }
            }
            else if (isChangeExistName == false)
            {
                foreach (var item in listObject)
                {
                    if (item.Error == "Item bị trùng tên")
                    {
                        item.NewName = item.CurrName;
                        item.Error = "";
                    }
                }
            }
            isChangeExistName = null;
            MessageBox.Show("Đổi tên thành công");
            isRefreshLisstObject = true;
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (listObject.Count == 0)
            {
                MessageBox.Show("Chưa chọn tập tin hoặc thư mục");
                return;
            }
            if (listMethod.Count == 0)
            {
                MessageBox.Show("Bạn chưa chọn thao tác");
                return;
            }
            List<string> listExistItem = new List<string>();
            bool existName = false;
            foreach (var item in listObject)
            {
                listExistItem.Add(item.CurrName);
            }

            foreach (var item in listObject)
            {
                listExistItem.RemoveAt(0);
                string tmpItemName, extension = "";
                //nếu là file thì không thao tác với đuôi file
                if (isFileMode)
                {
                    int indexOfExtension = item.CurrName.LastIndexOf('.');
                    if (indexOfExtension >= 0)
                    {
                        extension = item.CurrName.Substring(indexOfExtension);
                        tmpItemName = item.CurrName.Substring(0, indexOfExtension);
                    }
                    else
                    {
                        tmpItemName = item.CurrName;
                    }
                }
                else
                {
                    tmpItemName = item.CurrName;
                }
                foreach (var method in listMethod)
                {
                    // chỉ có replace là có thao tác với đuôi file
                    if (method.MethodName == "Replace")
                    {
                        string currString = method.MethodParameters[0];
                        string newString = method.MethodParameters[1];
                        tmpItemName = XuLiChuoi.Replace(tmpItemName + extension, currString, newString);
                        if (isFileMode)
                        {
                            int indexOfExtension = tmpItemName.LastIndexOf('.');
                            if (indexOfExtension >= 0)
                            {
                                extension = tmpItemName.Substring(indexOfExtension);
                                tmpItemName = tmpItemName.Substring(0, indexOfExtension);
                            }
                        }
                    }
                    else if (method.MethodName == "NewCase")
                    {
                        if (method.MethodParameters[0] == "True")
                        {
                            tmpItemName = XuLiChuoi.ToUpper(tmpItemName);
                        }
                        if (method.MethodParameters[1] == "True")
                        {
                            tmpItemName = XuLiChuoi.ToLower(tmpItemName);
                        }
                        if (method.MethodParameters[2] == "True")
                        {
                            tmpItemName = XuLiChuoi.ConvertFirstLetterOfEachWordToUpper(tmpItemName);
                        }
                    }
                    else if (method.MethodName == "FullnameNormalize")
                    {
                        if (method.MethodParameters[0] == "True")
                        {
                            tmpItemName = XuLiChuoi.Trim(tmpItemName);
                        }
                        if (method.MethodParameters[1] == "True")
                        {
                            tmpItemName = XuLiChuoi.ConvertFirstLetterOfEachWordToUpper(tmpItemName);
                        }
                        if (method.MethodParameters[2] == "True")
                        {
                            tmpItemName = XuLiChuoi.AllowOnlyOneSpaceBetweenWords(tmpItemName);
                        }
                    }
                    else if (method.MethodName == "Move")
                    {
                        if (method.MethodParameters[1] == "True")
                        {
                            try
                            {
                                tmpItemName = XuLiChuoi.MoveISBNFromEndToFrontOfString(tmpItemName, int.Parse(method.MethodParameters[0]));
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                item.Error = "Không thể di chuyển isbn nên thao tác move không được thực hiện";
                            }
                        }
                        else
                        {
                            try
                            {
                                tmpItemName = XuLiChuoi.MoveISBNFromFrontToEndOfString(tmpItemName, int.Parse(method.MethodParameters[0]));
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                item.Error = "Không thể di chuyển isbn nên thao tác move không được thực hiện";
                            }
                        }
                    }
                    else if (method.MethodName == "Unique")
                    {
                        tmpItemName = XuLiChuoi.CreateUniqueString();
                    }
                }
                item.NewName = tmpItemName + extension;
                if (listExistItem.Contains(item.NewName))
                {
                    item.Error = "Item bị trùng tên";
                    existName = true;
                }
            }
            if (existName)
            {
                MessageBoxResult existNameMessageBox = MessageBox.Show("Có 1 số item bị trùng tên sau khi đổi tên, bạn có muốn giữ nguyên tên ban đầu hay thêm hậu tố là số?\nẤn Cancel để giữ nguyên tên ban đầu\nẤn OK để thêm hậu tố là số", "Bị trùng tên!!", MessageBoxButton.OKCancel);
                if (existNameMessageBox == MessageBoxResult.OK)
                {
                    isChangeExistName = true;
                }
                else
                {
                    isChangeExistName = false;
                }
            }
            else
            {
                MessageBox.Show("Tên các item sau khi đổi hoàn toàn hợp lệ");
            }
        }

        private void listview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult deleteItemMessageBox = MessageBox.Show("Bạn có muốn xóa hành động này?", "Xóa hành động", MessageBoxButton.YesNo);
            if (deleteItemMessageBox == MessageBoxResult.Yes)
            {
                int index1 = listIndexMethod[listview.SelectedIndex].index1;
                int index2 = listIndexMethod[listview.SelectedIndex].index2;
                bool flag = false; // cập nhật lại listIndexMethod
                switch (index2)
                {
                    case 0: case 7: case 8: case 9:
                        listMethod.RemoveAt(index1);
                        flag = true;
                        break;
                    case 1 : case 4:
                        listMethod[index1].MethodParameters[0] = "False";
                        if (listMethod[index1].MethodParameters[0] == "False" && listMethod[index1].MethodParameters[1] == "False" && listMethod[index1].MethodParameters[2] == "False")
                        {
                            listMethod.RemoveAt(index1);
                            flag = true;
                        }
                        break;
                    case 2 : case 5:
                        listMethod[index1].MethodParameters[1] = "False";
                        if (listMethod[index1].MethodParameters[0] == "False" && listMethod[index1].MethodParameters[1] == "False" && listMethod[index1].MethodParameters[2] == "False")
                        {
                            listMethod.RemoveAt(index1);
                            flag = true;
                        }
                        break;
                    case 3 : case 6:
                        listMethod[index1].MethodParameters[2] = "False";
                        if (listMethod[index1].MethodParameters[0] == "False" && listMethod[index1].MethodParameters[1] == "False" && listMethod[index1].MethodParameters[2] == "False")
                        {
                            listMethod.RemoveAt(index1);
                            flag = true;
                        }
                        break;
                }
                if (flag == true)
                {
                    for (int i = listview.SelectedIndex + 1; i < listIndexMethod.Count; i++)
                    {
                        listIndexMethod[i].index1--;
                    }
                }
                listIndexMethod.Remove(listIndexMethod[listview.SelectedIndex]);
                listDescriptMethod.Remove(listDescriptMethod[listview.SelectedIndex]);       
            }
            MessageBox.Show("Xóa hành động thành công");
        }

        private void RefreshListObjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isRefreshLisstObject)
            {
                MessageBox.Show("Không thể refresh danh sách do bạn chưa confirm hoặc bạn vừa thêm item mới vào danh sách\nVui lòng confirm để có thể refresh");
                return;
            }
            foreach (var item in listObject)
            {
                item.CurrName = item.NewName;
                item.Path = item.Path.Substring(0, item.Path.LastIndexOf('\\')) + '\\' + item.NewName;
                item.NewName = "";
                item.Error = "";
            }
        }

        private void RefreshListMethodButton_Click(object sender, RoutedEventArgs e)
        {
            listMethod.Clear();
            listIndexMethod.Clear();
            listDescriptMethod.Clear();
        }
    }
}


public class FileInfomation : INotifyPropertyChanged
{
    private string currName;
    public string CurrName
    {
        get { return this.currName; }
        set
        {
            if (this.currName != value)
            {
                this.currName = value;
                this.NotifyPropertyChanged("CurrName");
            }
        }
    }
    private string newName;
    public string NewName
    {
        get { return this.newName; }
        set
        {
            if (this.newName != value)
            {
                this.newName = value;
                this.NotifyPropertyChanged("NewName");
            }
        }
    }
    public string Path { get; set; }
    private string error;
    public string Error
    {
        get { return this.error; }
        set
        {
            if (this.error != value)
            {
                this.error = value;
                this.NotifyPropertyChanged("Error");
            }
        }
    }

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
    public event PropertyChangedEventHandler PropertyChanged;

    public void NotifyPropertyChanged(string propName)
    {
        if (this.PropertyChanged != null)
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
}

public class MethodInformation
{
    public string MethodName { get; set; }
    public string[] MethodParameters { get; set; }
    public MethodInformation(string methodName, string[] methodParameters)
    {
        this.MethodName = methodName;
        this.MethodParameters = (string[])methodParameters.Clone();
    }
    public override string ToString()
    {
        string result = this.MethodName;
        foreach (var item in this.MethodParameters)
        {
            result = result + "|" + item;
        }
        return result;
    }
}

public class Index
{
    public int index1;
    public int index2;
    public Index(int a1, int a2)
    {
        this.index1 = a1;
        this.index2 = a2;
    }
}