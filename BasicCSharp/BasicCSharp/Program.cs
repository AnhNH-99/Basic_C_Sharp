using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BasicCSharp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.Unicode;
            string filePath = @"C:\Users\hoang\OneDrive\Máy tính\Hybrid-Technologies\Basic_C_Sharp\BasicCSharp\BasicCSharp\API_Test.txt";
            string[] lines = ReadFile(filePath);
            List<Question> questions = GetListQuestion(lines);
            while (true)
            {
                Console.WriteLine("******************************");
                Console.WriteLine("*          API TEST          *");
                Console.WriteLine("*     1. Start               *");
                Console.WriteLine("*     2. List Question       *");
                Console.WriteLine("*     3. Question and Answer *");
                Console.WriteLine("*     4. Choice Question     *");
                Console.WriteLine("******************************");
                Console.Write("Enter: ");
                int enter = Convert.ToInt32(Console.ReadLine());
                switch (enter)
                {
                    case 1:
                        foreach (Question item in questions)
                        {
                            string correctAnswer = null;
                            item.GetQuestion();
                            do
                            {
                                Console.Write("Correct Answer: ");
                                correctAnswer = Console.ReadLine();
                            } while (!Regex.IsMatch(correctAnswer, "[a-d]"));
                            bool result = CheckAnsewr(correctAnswer, item);
                            Console.WriteLine(result);
                            Console.WriteLine();
                        }
                        break;
                    case 2:
                        foreach (var item in questions)
                        {
                            item.GetQuestion();
                            Console.WriteLine();
                        }
                        break;
                    case 3:
                        foreach (var item in questions)
                        {
                            Console.WriteLine("Correct Answer: " + item.number);
                            item.GetQuestion();
                            Console.WriteLine("Correct Answer: " + item.answer);
                            Console.WriteLine();
                        }
                        break;
                    case 4:
                        while (true)
                        {
                            Console.Write("Choice Question: ");
                            int choice = Convert.ToInt32(Console.ReadLine());
                            Question question = SearchByNumber(choice, questions);
                            if (question == null)
                                break;
                            else
                            {
                                question.GetQuestion();
                                Console.WriteLine("Correct Answer: " + question.answer);
                                Console.WriteLine();
                            }
                        };
                        break;
                    default:
                        break;
                }
            }
        }
        public static string[] ReadFile(string filePath)
        {
            string[] lines;
            if (System.IO.File.Exists(filePath))
            {
                lines = System.IO.File.ReadAllLines(filePath);
            }
            else
            {
                lines = null;
            }
            return lines;
        }
        public static List<Question> GetListQuestion(string[] lines)
        {
            List<Question> listQuestions = new List<Question>();
            List<string> questions = new List<string>();
            List<string> answers = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (!(lines[i].Contains("Answer")))
                    questions.Add(lines[i]);
                else
                    answers.Add(lines[i]);
            }
            List<string> questionstr = new List<string>();
            for (int i = 0; i < questions.Count; i++)
            {
                if (questions[i].ToString() != "")
                    questionstr.Add(questions[i].ToString());
                if (questions[i].ToString() == "")
                {
                    if (questionstr.Count > 0)
                    {
                        int number = GetNumber(questionstr[0]);
                        Question question = new Question();
                        question.InputQuestion(number, questionstr);
                        listQuestions.Add(question);
                        questionstr = new List<string>();
                    }

                }
            }
            for (int i = 1; i < answers.Count; i++)
            {
                string[] arrAnswers = answers[i].Split(':');
                for (int j = 0; j < listQuestions[i - 1].question.Count; j++)
                {
                    if (listQuestions[i - 1].question[j].Contains(arrAnswers[1].Trim()))
                    {
                        listQuestions[i - 1].InputAnswer(listQuestions[i - 1].question[j].ToString());
                    }
                }

            }
            return listQuestions;
        }
        public static Question SearchByNumber(int search, List<Question> listQuestions)
        {
            return listQuestions.SingleOrDefault(x => x.number == search);
        }
        public static bool CheckAnsewr(string ansewr, Question question)
        {
            bool result = false;
            string[] arrAnsewr = question.answer.Split('.');
            if (String.Compare(ansewr, arrAnsewr[0], true) == 0)
                result = true;
            return result;
        }
        public static int GetNumber(string str)
        {
            string[] questionNumber = str.Split('.');
            string[] number = questionNumber[0].Split(' ');
            int result = Convert.ToInt32(number[1]);
            return result;
        }
        public class Question
        {
            public int number { get; set; }
            public List<string> question { get; set; }
            public string answer { get; set; }
            public Question()
            {
            }
            public void InputQuestion(int number, List<string> question)
            {
                this.number = number;
                this.question = question;
            }
            public void InputAnswer(string answer)
            {
                this.answer = answer;
            }
            public void GetQuestion()
            {
                foreach (var item in question)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine();
            }
        }
    }
}
