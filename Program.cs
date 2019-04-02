using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace olotest
{
    public class Pizza
    {
        public ICollection<string> Toppings { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("http://files.olo.com/pizzas.json");
                var p = JsonConvert.DeserializeObject<ICollection<Pizza>>(json);

                var ordered = p.Select(x => string.Join(",", x.Toppings.OrderBy(y => y)));
                var groups = ordered.GroupBy(x => x);

                var toppingCounts = new Dictionary<string, int>();
                foreach (var g in groups)
                {
                    toppingCounts.Add(g.Key, g.Count());
                }

                var orderedGroupedDesc = toppingCounts.OrderByDescending(x => x.Value);

                var top20 = orderedGroupedDesc.Take(20);
                var i = 20;
                var maxChars = top20.Max(x => x.Key.Length);
                string format = "{0,2}. {1,-" + (maxChars++).ToString() + "}: {2,4}";
                foreach (var t in top20)
                {
                    Console.WriteLine(format,i--,string.Join(",", t.Key) , t.Value);
                }   
            }
        }
    }
}
