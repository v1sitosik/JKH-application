using System;

namespace ЖКХ_Управление.Models
{
    public class MasterAssignmentViewModel
    {
        public int НазначениеId { get; set; }
        public int ЗапросId { get; set; }
        public string Клиент { get; set; }
        public string Адрес { get; set; }
        public string Текст { get; set; }
        public string Категория { get; set; }
        public DateTime ДатаНазначения { get; set; }
        public string СтатусНазначения { get; set; }
    }
}
