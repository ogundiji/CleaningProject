using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.ViewModels
{
    public class ViewUsedEquipment
    {
        public int Id { get; set; }
        public string Request { get; set; }
        public string Equipment { get; set; }
        public string Team { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
