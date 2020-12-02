using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParserApp
{
    public partial class Form1 : Form
    {
       public string x;

        public Form1()
        {
            InitializeComponent();
        }

        /* private void Form1_Load(object sender, EventArgs e)
         {

         }*/

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number)&&number!=8)
            {
                e.Handled = true;
            }
           
        }
     

        private void Button1_Click(object sender, EventArgs e)
        {
           
            if (!String.IsNullOrWhiteSpace(textBox2.Text))
            {
                bool f = false;
                 x = textBox2.Text;
               
                var risks = MainWindow.EnumerateSecurityRisk();
                for (int i = 0; i <risks.Count; i++)
                {
                    if (risks[i].Id == x)
                    {
                        System.Windows.MessageBox.Show($"Идентификатор угрозы: УБИ.{risks[i].Id}{Environment.NewLine}Наименование угрозы: {risks[i].Name}{Environment.NewLine}Описание угрозы:{Environment.NewLine} {risks[i].Description}{Environment.NewLine}Источник угрозы: {risks[i].SourceOfThreat}{Environment.NewLine}Объект воздействия угрозы: {risks[i].ObjectOfImpact}{Environment.NewLine}Нарушение конфиденциальности: {risks[i].ConfidentialityViolation}{Environment.NewLine}Нарушение целостности: {risks[i].IntegrityViolation}{Environment.NewLine}Нарушение доступности: {risks[i].AvailabilityViolation}");
                        f = true;
                    }
                  
                }
                if (f==false) System.Windows.MessageBox.Show("Такого ID нет");
            }
            else MessageBox.Show("Введено пустое значение");
           
            
        }

        
    }
}
