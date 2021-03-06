﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApp.Models
{
    [Table("Course")]
    public class CourseModel
    {
        [Key]
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public string Name { get; set; }
        public string CourseCode { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }       
        public UserModel Author { get; set; }
        public string URL { get; set; }
        public bool PaymentRequired { get; set; }
        public Decimal? Price { get; set; }
        public string BannerURL { get; set; }
        public ICollection<SectionModel> Sections { get; set; }
        public ICollection<UserCourseModel> UserCourses { get; set; }
    }
}
