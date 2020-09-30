﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CourseApp.ViewModels
{
    public class CourseEditVM

    {

        [Required]
        public Int64 Id { get; set; }
        public Guid CloudId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Subject { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
    }
}