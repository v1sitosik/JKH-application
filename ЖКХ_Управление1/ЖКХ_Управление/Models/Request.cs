using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЖКХ_Управление.Models
{
    public class Request
    {
        public int запрос_id { get; set; }
        public string имя { get; set; }
        public string фамилия { get; set; }
        public string адрес { get; set; }
        public string текст { get; set; }
        public string категория { get; set; }
        public string этап { get; set; }
        public string мастер { get; set; }
    }

}
