using Newtonsoft.Json;
using System.Collections.Generic;

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
