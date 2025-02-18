using System;
using Newtonsoft.Json;// include this dll

namespace HelloWorldApplication
{
    class HelloWorld
    {
        static void Main(string[] args)
        {
            /* my first program in C# */
            Console.WriteLine("Hello World");
            YR yr = new YR();
            yr.appname = "CCCCC";
            /* 
             fill all item in the object
             */
      string jsonresult=      JsonConvert.SerializeObject(yr);  //------> call this method ,pass you class object as parameter
            Console.ReadKey();
        }
    }

    public class YR
    {
        public string appname { get; set; }
        public string title { get; set; }
        public string transstatus { get; set; }
        public string lastmodifieddate { get; set; }
        public string notification { get; set; }
        public Approver2 approver { get; set; }
        public Displaydetails2 displaydetails { get; set; }
        public List<Layout2> layout { get; set; }
    }
}

