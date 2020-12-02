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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using ClosedXML.Excel;
using PagedList;
using System.Windows.Forms;





namespace ParserApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string DataBase = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\thrlist.xlsx";
        public static List<SecurityRiskToTable> listOfRisksToTable = new List<SecurityRiskToTable>();
        public static List<SecurityRisk> listOfRisks = new List<SecurityRisk>();
        public static IPagedList<SecurityRiskToTable> listRisk;
        private static int pageNumber = 1;
        private static int countOfPages;
        private const int pageSize = 30;
        
        public bool f = false;
        public MainWindow()
        {
            InitializeComponent();

            NextPage.IsEnabled = false;
            PrevPage.IsEnabled = false;
          
                Update.IsEnabled = false;
                ShowAll.IsEnabled = false;
                ShowAllInformation.IsEnabled = false;
                Save.IsEnabled = false;

            

        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (File.Exists(DataBase) && new FileInfo(DataBase).Length != 0)
            {
                 Loading();
                System.Windows.MessageBox.Show("Файл загружен с локального компьютера");
            }
            else if (!File.Exists(DataBase) || (File.Exists(DataBase) && new FileInfo(DataBase).Length == 0))
            {
                var cr = new CreateWindow();
                cr.ShowDialog();
                if (File.Exists(DataBase) && new FileInfo(DataBase).Length != 0)
                {
                    Create.IsEnabled = false;
                    Update.IsEnabled = true;
                    ShowAll.IsEnabled = true;
                    ShowAllInformation.IsEnabled = true;
                    Save.IsEnabled = true;
                }
                }
            

        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            if (File.Exists(DataBase) && new FileInfo(DataBase).Length == 0)
            {
                File.Delete(DataBase);
            }

            if (!File.Exists(DataBase) || File.Exists(DataBase) && new FileInfo(DataBase).Length == 0)
            {
                Create.IsEnabled = true;
                NextPage.IsEnabled = false;
                PrevPage.IsEnabled = false;
                Update.IsEnabled = false;
                ShowAll.IsEnabled = false;
                ShowAllInformation.IsEnabled = false;
                Save.IsEnabled = false;
                System.Windows.MessageBox.Show("Базы данных нет");
                return;
            }
            var updateWindow = new UpdateWindow();

            if (updateWindow.updates != 0 && updateWindow.updates != -1)
            {
                ShowDialog();
            }
            if (updateWindow.updates == -1&&updateWindow.f==false)
            {

                
                Create.IsEnabled = true;
                Update.IsEnabled = false;
                ShowAll.IsEnabled = false;
                ShowAllInformation.IsEnabled = false;
                Save.IsEnabled = false;
                return;
            }
            if (updateWindow.updates>0)
            {
                NextPage.IsEnabled = false;
                PrevPage.IsEnabled = false;
                
            }
           
            pageNumber = 1;
            if ((NextPage.IsEnabled==true|| PrevPage.IsEnabled == true)&&updateWindow.updates>0) { NextPage.IsEnabled = false;
                if (countOfPages >= 2) PrevPage.IsEnabled = true;
                else PrevPage.IsEnabled = false;
                var risks = EnumerateSecurityRiskToTable().ToList();
                listRisk = risks.ToPagedList(pageNumber, pageSize);
                RisksDataGrid.ItemsSource = listRisk; }
        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            NextPage.IsEnabled = true;
            PrevPage.IsEnabled = true;
            var risks = EnumerateSecurityRiskToTable().ToList();
            listRisk = risks.ToPagedList(pageNumber, pageSize);
            RisksDataGrid.ItemsSource = listRisk;
            countOfPages = listRisk.PageCount;
            LabelCountOfPages.Content = string.Format($"{pageNumber}/{countOfPages}");
        }
        private void Next(object sender, RoutedEventArgs e)
        {
            if (!listRisk.HasNextPage) return;
            pageNumber++;
            listRisk = listOfRisksToTable.ToPagedList(pageNumber, pageSize);
            RisksDataGrid.ItemsSource = listRisk;
            NextPage.IsEnabled = listRisk.HasNextPage;
            PrevPage.IsEnabled = true;
            LabelCountOfPages.Content = string.Format($"{pageNumber}/{countOfPages}");
        }
        private void Prev(object sender, RoutedEventArgs e)
        {
            if (!listRisk.HasPreviousPage) return;
            pageNumber--;
            listRisk = listOfRisksToTable.ToPagedList(pageNumber, pageSize);
            RisksDataGrid.ItemsSource = listRisk;
            PrevPage.IsEnabled = listRisk.HasPreviousPage;
            NextPage.IsEnabled = true;
            LabelCountOfPages.Content = string.Format($"{pageNumber}/{countOfPages}");
        }

        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Далее Вам будет предложено ввести десятичное число-идентификатор УБИ. Продолжить?", "Внимание", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                Form1 form = new Form1();
                form.Show();
            }
        }
            private void Button_Click5(object sender, RoutedEventArgs e)
        {
            List<SecurityRisk> secRisk = new List<SecurityRisk>();
            List<SecurityRiskToTable> secRiskToTable = new List<SecurityRiskToTable>();
            try
            {
                secRisk = EnumerateSecurityRisk();
                secRiskToTable = EnumerateSecurityRiskToTable();
                var book = new XLWorkbook();
                var sheet =book.Worksheets.Add("Sheet1");
                sheet.Cell(1, "A").Value = "Идентификатор угрозы";
                sheet.Cell(1, "B").Value = "Наименование угрозы";
                sheet.Cell(1, "C").Value = "Описание угрозы";
                sheet.Cell(1, "D").Value = "Источник угрозы";
                sheet.Cell(1, "E").Value = "Объект воздействия угрозы";
                sheet.Cell(1, "F").Value = "Нарушение конфиденциальности";
                sheet.Cell(1, "G").Value = "Нарушение целостности";
                sheet.Cell(1, "H").Value = "Нарушение доступности";
                for (int row = 0; row < secRisk.Count; row++)
                {
                    sheet.Cell(row + 2, "A").Value = secRisk[row].Id;
                    sheet.Cell(row + 2, "B").Value = secRisk[row].Name;
                    sheet.Cell(row + 2, "C").Value = secRisk[row].Description;
                    sheet.Cell(row + 2, "D").Value = secRisk[row].SourceOfThreat;
                    sheet.Cell(row + 2, "E").Value = secRisk[row].ObjectOfImpact;
                    sheet.Cell(row + 2, "F").Value = secRisk[row].ConfidentialityViolation;
                    sheet.Cell(row + 2, "G").Value = secRisk[row].IntegrityViolation;
                    sheet.Cell(row + 2, "H").Value = secRisk[row].AvailabilityViolation;
                }
                var table1 = sheet.Range("A1:H" + (secRisk.Count + 1));
                var sheet2 = book.Worksheets.Add("Sheet2");
                sheet2.Cell(1, "A").Value = "Идентификатор угрозы";
                sheet2.Cell(1, "B").Value = "Наименование угрозы";
                for (int row = 0; row < secRiskToTable.Count; row++)
                {
                    sheet2.Cell(row + 2, "A").Value = secRiskToTable[row].ID;
                    sheet2.Cell(row + 2, "B").Value = secRiskToTable[row].Name;
                }
                var table2 = sheet2.Range("A1:b" + (secRiskToTable.Count + 1));
                sheet.Columns().AdjustToContents();
                sheet2.Columns().AdjustToContents();
                var dialog = new Microsoft.Win32.SaveFileDialog()
                {
                    Filter = "Книга Excel (*.xlsx)|*.xlsx",
                    InitialDirectory = @"c:\"
                };
                if (dialog.ShowDialog() == true)
                {
                    book.SaveAs(dialog.FileName);
                }
            }
            catch(Exception ex)
            {
                File.Delete(DataBase);
                Update.IsEnabled = false;
                Create.IsEnabled = true;
                Save.IsEnabled = false;
                ShowAll.IsEnabled = false;
                ShowAllInformation.IsEnabled = false;
                listOfRisks.Clear();
                listOfRisksToTable.Clear();
                System.Windows.MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
        public void Loading()
        {
            try
            {
               listOfRisks = EnumerateSecurityRisk();
                listOfRisksToTable = EnumerateSecurityRiskToTable();
                if (!File.Exists(DataBase))
                {
                    Update.IsEnabled = false;
                    Create.IsEnabled = true;
                    Save.IsEnabled = false;
                    ShowAll.IsEnabled = false;
                    ShowAllInformation.IsEnabled = false;
                }
                else
                {
                    Update.IsEnabled = true;
                   ShowAll.IsEnabled = true;
                    Create.IsEnabled = false;
                    Save.IsEnabled = true;
                    ShowAllInformation.IsEnabled = true;
                }
            }
            catch (FileFormatException ex)
            {
                File.Delete(DataBase);
                Update.IsEnabled = false;
                Create.IsEnabled = true;
                Save.IsEnabled = false;
                ShowAll.IsEnabled = false;
                ShowAll.IsEnabled = false;
                PrevPage.IsEnabled = false;
                NextPage.IsEnabled = false;
                System.Windows.MessageBox.Show("Ошибка: " + ex.Message);
            }
            catch (Exception ex)
            {
                Update.IsEnabled = false;
                Create.IsEnabled = true;
                Save.IsEnabled = false;
                ShowAll.IsEnabled = false;
                ShowAll.IsEnabled = false;
                PrevPage.IsEnabled = false;
                NextPage.IsEnabled = false;
                System.Windows.MessageBox.Show("Ошибка: " + ex.Message);
            }
          
        }
        public static List<SecurityRiskToTable> EnumerateSecurityRiskToTable()
        {
            listOfRisksToTable.Clear();
            using (var book = new XLWorkbook(DataBase))
            {
                var sheet = book.Worksheets.Worksheet(1);
                for (int row = 3; row <= sheet.RowsUsed().Count(); ++row)
                {
                    SecurityRiskToTable securityRiskToTable = new SecurityRiskToTable();
                    {
                        securityRiskToTable.ID = sheet.Cell(row, 1).Value.ToString();
                        securityRiskToTable.Name = sheet.Cell(row, 2).Value.ToString();
                    };
                    listOfRisksToTable.Add(securityRiskToTable);
                }
                listRisk = listOfRisksToTable.ToPagedList(pageNumber, pageSize);
                countOfPages = listRisk.PageCount;
            }
            return listOfRisksToTable;
        }
        public static List<SecurityRisk> EnumerateSecurityRisk()
        {
            listOfRisks.Clear();
            using (var book = new XLWorkbook(DataBase))
            {
                var sheet = book.Worksheets.Worksheet(1);
                for (int row = 3; row <= sheet.RowsUsed().Count(); ++row)
                {
                    SecurityRisk securityRisk = new SecurityRisk();
                    {
                        securityRisk.Id = sheet.Cell(row, 1).Value.ToString();
                        securityRisk.Name = sheet.Cell(row, 2).Value.ToString();
                        securityRisk.Description = sheet.Cell(row, 3).Value.ToString();
                        securityRisk.SourceOfThreat = sheet.Cell(row, 4).Value.ToString();
                        securityRisk.ObjectOfImpact = sheet.Cell(row, 5).Value.ToString();
                        securityRisk.ConfidentialityViolation = sheet.Cell(row, 6).Value.ToString();
                        securityRisk.IntegrityViolation = sheet.Cell(row, 7).Value.ToString();
                        securityRisk.AvailabilityViolation = sheet.Cell(row, 8).Value.ToString();
                    };
                    listOfRisks.Add(securityRisk);
                }
                return listOfRisks;
            }

        }
    }
}


