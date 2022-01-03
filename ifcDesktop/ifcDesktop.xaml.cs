using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace ifcDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DataLib DataStorage;
        List<string> ThrItems = new List<string>();
        List<string> ColItems = new List<string>();
        List<string> CjlItems = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            BindData();
        }

        private void BindData()
        {
            DataStorage = new DataLib();
            Binding IFCbinding = new Binding();
            IFCbinding.Source = DataStorage;
            IFCbinding.Path = new PropertyPath("IFCFilePath");

            BindingOperations.SetBinding(this.IFCFile, TextBox.TextProperty, IFCbinding);

            Binding csvFbinding = new Binding();
            csvFbinding.Source = DataStorage;
            csvFbinding.Path = new PropertyPath("csvFilePath");

            BindingOperations.SetBinding(this.OutputFolder, TextBox.TextProperty, csvFbinding);

            Binding csvbinding = new Binding();
            csvbinding.Source = DataStorage;
            csvbinding.Path = new PropertyPath("csvFileName");

            BindingOperations.SetBinding(this.OutputName, TextBox.TextProperty, csvbinding);

            Binding THWbinding = new Binding();
            THWbinding.Source = DataStorage;
            THWbinding.Path = new PropertyPath("ThresholdWidth");

            BindingOperations.SetBinding(this.THW, TextBox.TextProperty, THWbinding);

            Binding THHbinding = new Binding();
            THHbinding.Source = DataStorage;
            THHbinding.Path = new PropertyPath("ThresholdHeight");

            BindingOperations.SetBinding(this.THH, TextBox.TextProperty, THHbinding);

            Binding ThrListbinding = new Binding();
            ThrListbinding.Source = DataStorage;
            ThrListbinding.Path = new PropertyPath("ThrList");

            BindingOperations.SetBinding(this.THR, TextBox.TextProperty, ThrListbinding);

            Binding ColListBinding = new Binding();
            ColListBinding.Source = DataStorage;
            ColListBinding.Path = new PropertyPath("ColList");

            BindingOperations.SetBinding(this.COL, TextBox.TextProperty, ColListBinding);

            Binding CjlListbinding = new Binding();
            CjlListbinding.Source = DataStorage;
            CjlListbinding.Path = new PropertyPath("CjlList");

            BindingOperations.SetBinding(this.CJL, TextBox.TextProperty, CjlListbinding);
        }

        private void Button_Click_LoadinCFG(object sender, RoutedEventArgs e)
        {
            string FileName = IfcFileDialog.OpenFile("CFG配置|*.cfg","打开CFG文件");

            if (FileName == null) return;

            try
            {
                StreamReader reader = new StreamReader(FileName);

                int flag = 0;
                string str = reader.ReadLine();
                str = reader.ReadLine().Trim();

                DataStorage.csvFilePath = System.IO.Path.GetDirectoryName(str);
                DataStorage.csvFileName = System.IO.Path.GetFileNameWithoutExtension(str);
                DataStorage.ThresholdWidth = reader.ReadLine().Trim();
                DataStorage.ThresholdHeight = reader.ReadLine().Trim();

                string ThrList = "";
                string CjlList = "";
                string ColList = "";

                ThrItems.Clear();
                CjlItems.Clear();
                ColItems.Clear();

                while (true)
                {
                    str = reader.ReadLine();
                    if(str != null) str = str.Trim();
                    Debug.WriteLine((str == "%THR").ToString());
                    if (str == "%THR") flag = 1;
                    else if(str == "%COL") flag = 2;
                    else if(str == "%CJL") flag = 3;
                    else if (str == null) break;
                    else
                    {
                        if(flag == 1)
                        {
                            if (ThrList == "") ThrList = str;
                            else ThrList += ","+str;
                            ThrItems.Add(str);
                        }
                        else if(flag == 2)
                        {
                            if (ColList == "") ColList = str;
                            else ColList += "," + str;
                            ColItems.Add(str);
                        }
                        else if(flag == 3)
                        {
                            if (CjlList == "") CjlList = str;
                            else CjlList += "," + str;
                            CjlItems.Add(str);
                        }
                    }
                    Debug.WriteLine(str + " " + flag.ToString());
                }

                DataStorage.ThrList = ThrList;
                DataStorage.CjlList = CjlList;
                DataStorage.ColList = ColList;

                Debug.WriteLine(DataStorage.ThrList);
                Debug.WriteLine(ThrList);
                reader.Close();
            }
            catch(Exception ex)
            {
                IfcFileDialog.throwEx(ex);
            }
        }

        private void SaveCFG(string FileName)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(FileName);

                streamWriter.WriteLine(".\\tmp\\res.obj");


                streamWriter.WriteLine(DataStorage.csvFilePath + "\\" + DataStorage.csvFileName + ".csv");
                streamWriter.WriteLine(DataStorage.ThresholdWidth);
                streamWriter.WriteLine(DataStorage.ThresholdHeight);
                streamWriter.WriteLine("%THR");

                foreach (var item in ThrItems)
                {
                    streamWriter.WriteLine(item);
                }

                streamWriter.WriteLine("%COL");

                foreach (var item in ColItems)
                {
                    streamWriter.WriteLine(item);
                }

                streamWriter.WriteLine("%CJL");

                foreach (var item in CjlItems)
                {
                    streamWriter.WriteLine(item);
                }

                streamWriter.Close();
            }
            catch (Exception ex)
            {
                IfcFileDialog.throwEx(ex);
            }

        }

        private void Button_Click_SaveCFG(object sender, RoutedEventArgs e)
        {
            if (!CheckDataStorage()) return;
            string FileName = IfcFileDialog.SaveFile(".cfg");
            Debug.WriteLine(FileName);
            if (FileName == null) return;

            SaveCFG(FileName);

            IfcFileDialog.showDia("保存完成");
        }

        private void Button_Click_RunCal(object sender, RoutedEventArgs e)
        {
            if (!CheckDataStorage()) return;
            SaveCFG(@".\tmp\tmp.cfg");

            try
            {

                Process process = new Process();
                process.StartInfo.FileName = System.Environment.CurrentDirectory + "\\IfcObjDeal.exe";
                process.StartInfo.Arguments = "--CFG .\\tmp\\tmp.cfg";
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();//启动程序

                process.WaitForExit();
                process.Close();
            }
            catch (Exception ex)
            {
                IfcFileDialog.throwEx(ex);
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_LoadIfc(object sender, RoutedEventArgs e)
        {
            string FileName = IfcFileDialog.OpenFile("IFC文件|*.ifc", "打开IFC文件");

            if (FileName != null)
            {
                DataStorage.IFCFilePath = FileName;

                try
                {
                    //string cmd = @".\convert.exe " + DataStorage.IFCFilePath;
                    //Debug.WriteLine(cmd);
                    //Process process = new Process();

                    //process.StartInfo.FileName = @"cmd.exe";
                    //process.StartInfo.UseShellExecute = false;
                    //process.StartInfo.RedirectStandardInput = true;
                    //process.StartInfo.RedirectStandardOutput = true;
                    //process.StartInfo.RedirectStandardError = true;
                    //process.StartInfo.CreateNoWindow = true;
                    //process.Start();//启动程序
                    //process.StandardInput.WriteLine(cmd); //向cmd窗口写入命令
                    //process.StandardInput.AutoFlush = true;

                    Process process = new Process();
                    process.StartInfo.FileName = System.Environment.CurrentDirectory + "\\convert.exe";
                    process.StartInfo.Arguments = DataStorage.IFCFilePath;
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();//启动程序

                    process.WaitForExit();
                    process.Close();
                }
                catch (Exception ex)
                {
                    IfcFileDialog.throwEx(ex);
                }

                OutputName.IsEnabled = true;
                THH.IsEnabled = true;
                THW.IsEnabled = true;
                BeginCal.IsEnabled = true;
                getFile1.IsEnabled = true;
                getList1.IsEnabled = true;
                getList2.IsEnabled = true;
                getList3.IsEnabled = true;
                
            }
        }

        private void Button_Click_LoadcsvF(object sender, RoutedEventArgs e)
        {
            string FolderName = IfcFileDialog.OpenFolder();

            if (FolderName != null)
            {
                DataStorage.csvFilePath = FolderName;
            }
        }

        private void limitnumber(object sender, TextCompositionEventArgs e)
        {
            string pattern = "[^\\d.]";
            Regex re = new Regex(pattern);
            e.Handled = re.IsMatch(e.Text);
        }
        private bool CheckDataStorage()
        {
            if (DataStorage.csvFileName == null || DataStorage.csvFileName == "") IfcFileDialog.showDia("请输入输出文件名");
            else if (DataStorage.csvFilePath == null || DataStorage.csvFilePath == "") IfcFileDialog.showDia("请输入输出文件路径");
            else if (DataStorage.ThresholdHeight == null || DataStorage.ThresholdHeight == "") IfcFileDialog.showDia("请输入长度阈值");
            else if (DataStorage.ThresholdWidth == null || DataStorage.ThresholdWidth == "") IfcFileDialog.showDia("请输入宽度阈值");
            else return true;
            return false;
        }
    }
    
    
}
