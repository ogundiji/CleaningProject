using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CleaningProject.ViewModels
{
    public class ConfigureEditModel
    {
        [Required]
        public int TeamId { get; set; }

        public SelectList team { get; set; }

        [Required]
        public string StaffId { get; set; }

        public SelectList Staff { get; set; }
    }
}
