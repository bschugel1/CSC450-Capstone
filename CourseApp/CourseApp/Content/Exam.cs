using CourseApp.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.Content
{
    public class Exam
    {

        public long Id { get; set; }
        public LinkedList<Question> questions;

        public Exam()
        {

            questions = new LinkedList<Question>();
        }


        public void BuildExam(List<string> questionsList)
        {
            if (questions.Count > 0)
            {
                questions.Clear();
            }
            LinkedListNode<Question> prevNode = null;
            foreach (string s in questionsList)
            {

                var question = new Question(s);
                if (!(questions.Count < 1))
                {
                    if (prevNode != null)
                    {
                        questions.AddAfter(prevNode, question);
                        prevNode.Value = question;
                    }
                }
                else {
                    if(question != null)
                    questions.AddFirst(question);                        
                     }
            }
        }


        public void printOutExam()
        {
            foreach(Question q in questions)
            {

                Console.WriteLine(q.ToString());

            }

        }


    }
}
