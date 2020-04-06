using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.ViewModels
{
    public class ViewTeamModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public string Staff { get; set; }
        public DateTime Created { get; set; }
    }
}
