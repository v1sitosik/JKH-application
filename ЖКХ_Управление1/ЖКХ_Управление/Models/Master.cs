using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЖКХ_Управление.Models
{
    public class Master
    {
        public int СотрудникId { get; set; }
        public string Имя { get; set; }
        public string Фамилия { get; set; }

        public string ФИО => $"{Имя} {Фамилия}";
    }
}
