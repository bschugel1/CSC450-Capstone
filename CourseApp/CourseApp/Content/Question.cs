using System.Collections.Generic;

namespace CourseApp.Content
{
    public class Question
    {
        private string question { get; }

        public Question(string question)
        {
            this.question = question;
        }
       
        override
        public string ToString()
        {

            return question;

        }
    }
}