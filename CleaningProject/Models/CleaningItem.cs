using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Models
{
    public class CleaningItem:Entity
    {
        [Required]
        public ServiceRequest ServiceRequest { get; set; }

        [Required]
        public Team Team { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Created { get; set;}
        public string Status { get; set; }
       
    }

    public class ConfigureTeam : Entity
    {
        [Required]
        public Team Team { get; set; }

        [Required]
        public CleaningUser Staff { get; set; }
       
    }

    public class Team:Entity
    {
        [Required]
        public string name { get; set; }

        [Required]
        public DateTime TeamDate { get; set; } 
       
    }

   

    public class EquipmentTracking:Entity
    {
        [Required]
        public Equipment Equipment { get; set; }
        [Required]
        public Team Team { get; set; }
        [Required]
        public ServiceRequest ServiceRequest { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime EquipmentDate { get; set; }
        [Required]
        public string Status { get; set; }  /*returned still in use*/
       
    }
}
