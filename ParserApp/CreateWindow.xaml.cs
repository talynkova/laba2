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
using System.Net;
using System.IO;
using System.Management;
using System.ComponentModel;

namespace ParserApp
{
    /// <summary>
    /// Логика взаимодействия для CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        
        public CreateWindow()
        {
            InitializeComponent();
        }
        private void Yes_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (File.Exists(MainWindow.DataBase) && new FileInfo(MainWindow.DataBase).Length == 0)
                {
                    File.Delete(MainWindow.DataBase);
                }
                Yes.IsEnabled = false;
                No.IsEnabled = false;
                WebClient wc = new WebClient();
                wc.DownloadFile(new Uri("https://bdu.fstec.ru/files/documents/thrlist.xlsx"), MainWindow.DataBase);
                Close();
                MessageBox.Show("Файл загружен");
            }
            catch (WebException)
            {
                MessageBox.Show("Пожалуйста, проверьте соединение с сетью Интернет");
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка: " + exception.Message);
                Close();
            }

        }
        private void No_Click(object sender, RoutedEventArgs e)
        {
            Close();
           
        }

    }
}
