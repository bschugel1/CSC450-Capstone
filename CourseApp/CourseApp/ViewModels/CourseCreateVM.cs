﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CourseApp.ViewModels
{
    public class CourseCreateVM
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name length can't be more than 50 characters.")]
        [Display(Name = "Course Name")]
        public string Name { get; set; }
        [Required]
        public string Subject { get; set; }
        [StringLength(30, ErrorMessage = "Name length can't be more than 30 characters.")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }
        public string Description { get; set; }
        [Display(Name = "Payment Requirement")]
        public bool PaymentRequired { get; set; }
        [Range(0, 100, ErrorMessage = "Price must be between $1 and $100")]
        public Decimal Price { get; set; }
     }
}
