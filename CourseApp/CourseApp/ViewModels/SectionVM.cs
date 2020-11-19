﻿using CourseApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.ViewModels
{
    public class SectionVM
    {
        public long Id { get; set; }
        public long CourseId { get; set; }
        public long? ParentSectionId { get; set; }

        public CourseModel Course { get; set; }

        public string Name { get; set; }
        public int DisplayOrder { get; set; }

        public ICollection<MediaItemModel> Items { get; set; }
    }
}