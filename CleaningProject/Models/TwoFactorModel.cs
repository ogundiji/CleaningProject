﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Models
{
    public class TwoFactorModel
    {
        [Required]
        public string Token { get; set; }
    }
}
