using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Reflection;

namespace ReflectionApiTipalti
{
    internal class Program
    {
        public static void PrintObject(object obj)
        {
            PrintObject(obj, 0);
        }
        public static void PrintObject(object obj, int indent)
        {

            if (obj == null) return;
            string indentString = new string(' ', indent);
            Type objType = obj.GetType();
            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object propValue = property.GetValue(obj, null);
                
                //აქ უბრალოდ ვამოწმებთ ლისტი ხომ არაა, არაის ამბავში;
                var elems = propValue as IList;
                if (elems != null)
                {
                    foreach (var item in elems)
                    {
                        PrintObject(item, indent + 3);
                    }
                }
                else
                {
                    // ესაა მთავარი, თუკი ჩვენი გაკეთებული კლასია, იმავე ასამბლიში უნდა იყოს რაშიც ველი რომელსაც გადის ციკლი
                    //თუკი არაა მაშინ სხვა ასამბლიდან იქნება
                    //objType.Assembly ეს ველი რეკურსიის მიუხედავად ყოველთვის ერთი და იგივე იქნება, ამ ჩვენი ასამბლის სახელი ერქმევა
                    //თუ ჩვენი ასამბლიში შექმნილი კლასია მაშინ მისი ველები ცალკეა გამოსაკვლევი და რეკურსიულად უნდა გავხნათ
                    //თუკი არაა ჩვენი ასამბლის ნაწილი მაშინ ე.ი უკვე დასულებივართ კლასის ველის დონეზე და შეგვიძლია ეგრევე გამოვიტანოთ კონსოლში ველიუ;
                    if (property.PropertyType.Assembly == objType.Assembly)
                    {
                        Console.WriteLine("{0}{1}:", indentString, property.Name);

                        PrintObject(propValue, indent + 2);
                    }
                    else
                    {
                        Console.WriteLine("{0}{1}: {2}", indentString, property.Name, propValue);
                    }
                }
            }

            //JObject json = JObject.FromObject(some_object);
            //foreach (JProperty property in json.Properties())
            //{
            //    Console.WriteLine("Object of Class '{0}'", property.Name);
            //    Console.WriteLine("---------------------");

            //    Console.WriteLine("           {0} ({1})", property.Name, property.Value);
            //}
            //Console.ReadLine();

        }

        static void Main(string[] args)
        {
            List<string> strings = new List<string>() { "Elene" };

            var name1 = new Name { FirstName = "George", LastName = "Darchiashvili" };
            var prof = new Proffession { Name = ".Net Developer", Experience = 2 };
            var Person1 = new Person { Age = 32, Saxeli = name1, Job = prof };


            PrintObject(Person1);


            Console.ReadKey();
        }

        class Name
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
        }

        class Proffession
        {
            public string? Name { get; set; }
            public int Experience { get; set; }
        }

        class Person
        {
            public int Age { get; set; }
            public Name? Saxeli { get; set; }
            public Proffession? Job { get; set; }
            // public List<string>? ChildrenNames { get; set; }
        }
    }
}