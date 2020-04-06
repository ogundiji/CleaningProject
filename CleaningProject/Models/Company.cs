using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CleaningProject.Models
{
    
    public class Company:Entity
    {
        [Required]
        public string name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public byte[] Logo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM ddd d yyyy HH:mm}")]
        public DateTime companyCreated { get; set; }

    }
    public class UserImageRecord:Entity
    {
        public  CleaningUser Staff { get; set; }
        public byte[] Upload { get; set; }
    }

    public class Service : Entity
    {
        [Required]
        public string ServiceName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM ddd d yyyy HH:mm}")]
        public DateTime ServiceDate { get; set; }
       
        public  List<ServiceTypeRecord> serviceType { get; set; }
       
    }

    public class ServiceRecord:Entity
    {
        [Required]
        public string ServiceType { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM ddd d yyyy HH:mm}")]
        public DateTime ServiceTypeDate { get; set; }
       
        public List<ServiceTypeRecord> service { get; set; }
    }

    public class ServiceRequest : Entity
    {
        public CleaningUser Customer { get; set; }

        public string RequestName { get; set; }

        [Required]
        public Service Service { get; set; }

        [Required]
        public ServiceRecord ServiceType { get; set; }

        [Required]
        public string DaysOfWork { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM ddd d yyyy HH:mm}")]
        public DateTime SheduleDate { get; set; }

     
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        public string Status { get; set; }
      
    }
    public class ServiceTypeRecord:Entity
    {
        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }

        [Required]
        public decimal ServiceCost { get; set; }

        [Required]
        public decimal Discount { get; set; }

        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM ddd d yyyy HH:mm}")]
        public DateTime StartDate { get; set; }

        public Service po { get; set; }

        public ServiceRecord qo { get; set; }
        
    }

    public class Equipment:Entity
    {
        [Required]
        public string EquipmentName { get; set; }
        [Required]
        public string EquipmentType { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM ddd d yyyy HH:mm}")]
        public DateTime EquipmentDate { get; set; }
    }
}
