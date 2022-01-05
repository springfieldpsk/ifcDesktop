using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace ifcDesktop
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public List<string> nameList;
        public List<string> textList;
        private subWinLib subWinStorage;
        public WinMes.delegateMes delegateMes;

        public Window1()
        {
            InitializeComponent();
        }

        public Window1(List<string>list1,List<string>list2,string str)
        {
            InitializeComponent();
            subWinStorage = new subWinLib();

            nameList = list1;
            textList = list2;
            subWinStorage.TextView = str;
            Debug.WriteLine(str);

        }

        private void WinLoaded(object sender, RoutedEventArgs e)
        {
            Binding Textbinding = new Binding();
            Textbinding.Source = subWinStorage;
            Textbinding.Path = new PropertyPath("TextView");

            BindingOperations.SetBinding(this.TextView, TextBox.TextProperty, Textbinding);

            try
            {
                
                if (nameList != null)
                {
                    Cb1.ItemsSource = nameList;
                    Cb2.ItemsSource = nameList;
                }
            }
            catch
            {
                throw;
            }
        }
        public void Bt_Click_Add(object sender, RoutedEventArgs e)
        {
            if(Cb1.Text != null && Cb1.Text != "") textList.Add(Cb1.Text);
            updateTextView();
            Debug.WriteLine(subWinStorage.TextView);
            try
            {
                if(delegateMes != null)
                {
                    this.delegateMes(subWinStorage.TextView, textList);
                }
            }
            catch
            {
                throw;
            }
        }

        private void Bt_Click_rme(object sender, RoutedEventArgs e)
        {
            if (Cb2.Text != null && Cb2.Text != "")
            {
                if(textList.Contains(Cb2.Text)) textList.Remove(Cb2.Text);
            }
            updateTextView();
            Debug.WriteLine(subWinStorage.TextView);
            try
            {
                if (delegateMes != null)
                {
                    this.delegateMes(subWinStorage.TextView, textList);
                }
            }
            catch
            {
                throw;
            }
        }

        private void updateTextView()
        {
            string str = "";
            foreach(var v in textList)
            {
                if (str == "") str += v;
                else str += "," + v;
            }
            subWinStorage.TextView = str;
        }
    }
}
