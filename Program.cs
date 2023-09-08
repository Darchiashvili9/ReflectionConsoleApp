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
                    // This will not cut-off System.Collections because of the first check
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
            var Person1 = new Person { Age = 32, Name = name1 };


            PrintObject(Person1);


            Console.ReadKey();
        }

        class Name
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
        }

        class Person
        {
            public int Age { get; set; }
            public Name? Name { get; set; }
            // public List<string>? ChildrenNames { get; set; }
        }
    }
}