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
using static System.Net.WebRequest;
using System.IO;
using System.Net;
namespace ParserApp
{
    /// <summary>
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private readonly List<SecurityRisk> listNew = new List<SecurityRisk>();
        private readonly List<SecurityRisk> listOld = new List<SecurityRisk>();
        private readonly List<ICloneable> list = new List<ICloneable>();
        public int updates=-1;
        public bool f;
        
        public UpdateWindow()
        {

            InitializeComponent();
            ShowDialog();
        }
      
        public void Yes_Click1(object sender, RoutedEventArgs args)
        {
            f = false;
            try
            {
                StringBuilder sb = new StringBuilder();
                Yes1.IsEnabled = false;
                No1.IsEnabled = false;
                if (File.Exists(MainWindow.DataBase) && new FileInfo(MainWindow.DataBase).Length == 0)
                {
                    File.Delete(MainWindow.DataBase);
                }
                
                MainWindow.listOfRisks = MainWindow.EnumerateSecurityRisk();
                foreach (var item in MainWindow.listOfRisks)
                {
                   list.Add((ICloneable)item.Clone());
                }
                
                var httpWebRequest = (HttpWebRequest)Create("https://bdu.fstec.ru/files/documents/thrlist.xlsx");
                using ((HttpWebResponse)httpWebRequest.GetResponse()) { }
                WebClient client = new WebClient();
                client.DownloadFile(new Uri("https://bdu.fstec.ru/files/documents/thrlist.xlsx"),MainWindow.DataBase);
                
                MainWindow.listOfRisksToTable = MainWindow.EnumerateSecurityRiskToTable();
                var riskyRisk = MainWindow.EnumerateSecurityRisk();
                MainWindow.listOfRisks = riskyRisk;
                int c = MainWindow.listOfRisks.Count > list.Count ? list.Count : MainWindow.listOfRisks.Count;
                for (int i = 0; i < c; i++)
                {
                    if (!MainWindow.listOfRisks[i].Equals(list[i]))
                    {
                        listNew.Add(MainWindow.listOfRisks[i]);
                        listOld.Add((SecurityRisk)list[i]);
                        
                    }
                }

                for (int i = c; i < MainWindow.listOfRisks.Count; i++)
                {
                    listNew.Add(MainWindow.listOfRisks[i]);
                }

                for (int i = c; i < list.Count; i++)
                {
                    listOld.Add((SecurityRisk)list[i]);
                }
               updates = listOld.Count > listNew.Count ? listOld.Count : listNew.Count;

                if (updates == 0)
                {
                    System.Windows.MessageBox.Show("База успешно обновлена, изменений нет");
                    Close();
                }
               if (updates>0)
                {
                    System.Windows.MessageBox.Show("База данных успешно обновлена, количество изменений " + updates);
                    RisksDataGridOld.ItemsSource = listOld;
                    RisksDataGridOld.ItemsSource = listNew;
                }
            }
            catch (WebException)
            {
                MessageBox.Show("Пожалуйста, проверьте соединение с сетью Интернет");
                Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка: " + e.Message);
                Close();
            }

        }
        public void No_Click1(object sender, RoutedEventArgs args)
        {
            f = true;
            Close();
        }

        
    }
}
