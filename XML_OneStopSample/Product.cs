using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_OneStopSample
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public bool IsFourStroke { get; set; }
        public string Description { get; set; }
        public double CC { get; set; }
        public List<string> Colors { get; set; }
    }

    public enum Company
    {
        Honda,
        Hero,
        TVS
    }
}
