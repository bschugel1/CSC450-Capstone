using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.ViewModels
{
   
    [JsonObject]
    public class SubjectListVM
    {
   
        public List<Subject> Subjects { get; set; }
    }
    public class Subject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
