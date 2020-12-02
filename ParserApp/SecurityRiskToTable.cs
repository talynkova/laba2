using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp
{
    public class SecurityRiskToTable
    {
        private string id;
        public string ID
        {
            get => "УБИ." + id;
            set => id = value;
        }
        public string Name { get; set; }

    }
}
