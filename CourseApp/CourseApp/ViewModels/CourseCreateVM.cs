﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CourseApp.ViewModels
{
    public class CourseCreateVM
    {
        [Required]
        [StringLength(30, ErrorMessage = "Name length can't be more than 30 characters.")]
        [Display(Name = "Course Name")]
        public string Name { get; set; }
        [Required]
        public string Subject { get; set; }
        [StringLength(8, ErrorMessage = "Name length can't be more than 8 characters.")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }
        public string Description { get; set; }
    }
}
