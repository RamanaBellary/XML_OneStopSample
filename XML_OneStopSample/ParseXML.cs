using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XML_OneStopSample
{
    public class ParseXML
    {
        public static void Do()
        {
            //********PARSING XML********
            //ParseUsingXmlDocument(@"D:\Mine\Samples\XML\XML_OneStopSample\XML_OneStopSample\XMLFile1.xml");
            //ParseUsingXDocument(@"D:\Mine\Samples\XML\XML_OneStopSample\XML_OneStopSample\XMLFile1.xml");

            List<Product> lst = new List<Product>()
            {
                new Product
                {
                    Name = "Shine",
                    Id = 1,
                    CC = 110,
                    Company = "Honda",
                    Description = "Honda Shine",
                    Type="Bike",
                    Colors = new List<string>() {"Black","Grey","RedBlack" }
                }
            };
            //********CREATING XML********
            //CreateXmlUsingXmlDocument(lst, @"D:\Mine\Samples\XML\XML_OneStopSample\XML_OneStopSample\XMLFile2.xml");
            //CreateXmlUsingXDocument(lst, @"D:\Mine\Samples\XML\XML_OneStopSample\XML_OneStopSample\XMLFile2.xml");

            //********Validation of XML********
            //ValidatingXmlUsingXsd(@"D:\Mine\Samples\XML\XML_OneStopSample\XML_OneStopSample\XMLFile1.xml",
            //    @"D:\Mine\Samples\XML\XML_OneStopSample\XML_OneStopSample\XMLFile1.xsd");

            /*Difference between
            XMLDocument, XDocument
            Node, Element
            DescendantNodes(), Nodes()
            */
        }

        /// <summary>
        /// Parse XML Using XmlDocument
        /// </summary>
        /// <param name="xmlFileFullPath"></param>
        private static void ParseUsingXmlDocument(string xmlFileFullPath)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileFullPath);
            var result = xmlDoc.SelectNodes("/Products/Product[@Type='Scooter']/colors[color='Black']");
            foreach (XmlNode r in result)
            {
                var p = r.ParentNode;
                //var n = p.Attributes("");
            }
            xmlDoc = null;
        }

        /// <summary>
        /// Parse XML Using XDocument
        /// </summary>
        /// <param name="xmlFileFullPath"></param>
        private static void ParseUsingXDocument(string xmlFileFullPath)
        {
            var xDoc = XDocument.Load(xmlFileFullPath);
            var products = xDoc.Descendants("Product").Where(
                (ele) =>
                {
                    var colors = ele.Descendants("color").Where(c => c.Value == "Black");
                    return ele.Attribute("Type").Value.Equals("Bike") && colors.Any() ? true : false;
                }).
                    Select(e =>
                    new
                    {
                        Name = e.Attribute("Name").Value,
                        Company = e.Attribute("Company").Value
                    });
            foreach (var n in products)
            {

            }
            xDoc = null;
        }

        private static void CreateXmlUsingXmlDocument(List<Product> lstProducts, string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("Products");
            foreach (var p in lstProducts)
            {
                var prod = xmlDoc.CreateElement("Product");

                var idAtt = xmlDoc.CreateAttribute("Id");
                idAtt.Value = p.Id.ToString();
                prod.Attributes.Append(idAtt);

                var nameAtt = xmlDoc.CreateAttribute("Name");
                nameAtt.Value = p.Name;
                prod.Attributes.Append(nameAtt);

                var cc = xmlDoc.CreateElement("CC");
                cc.InnerText = p.CC.ToString();
                prod.AppendChild(cc);

                var colors = xmlDoc.CreateElement("colors");
                foreach (var c in p.Colors)
                {
                    var color = xmlDoc.CreateElement("color");
                    color.InnerText = c;
                    colors.AppendChild(color);
                }
                prod.AppendChild(colors);

                root.AppendChild(prod);
            }
            xmlDoc.AppendChild(root);
            xmlDoc.Save(fileName);
        }

        private static void CreateXmlUsingXDocument(List<Product> lstProducts, string fileName)
        {
            //var element = new XElement("");
            //element.Add(new XElement("properties", new XElement("property",
            //                                            new XAttribute("name", "name"),
            //                                            new XAttribute("value", "desirevalue"))));
            var xDoc = new XDocument();
            var xEles = new XElement("Products",
                from p in lstProducts
                select new XElement("Product", new XAttribute("Id", p.Id),
                                               new XAttribute("Name", p.Name),
                                               new XAttribute("Type", p.Type),
                                               new XAttribute("Company", p.Company),
                                               new XAttribute("IsFourStroke", p.IsFourStroke),
                new XElement("CC", p.CC),
                new XElement("Description", p.Description),
                new XElement("colors", p.Colors.Select(c => new XElement("color", c)))//TODO: null check if list is empty
                ));
            xDoc.Add(xEles);
            xDoc.Save(fileName);
        }

        private static void ValidatingXmlUsingXsd(string xmlFile, string xsdFile)
        {
            var xDoc = XDocument.Load(xmlFile);
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add("", xsdFile);
            xDoc.Validate(schemaSet, new ValidationEventHandler(validationEventHandler_Handle));
        }

        private static void validationEventHandler_Handle(object sender, ValidationEventArgs args)
        {
            
        }
    }

    [Serializable]
    public class Customer
    {
        public int Id;
        public string Name;
        [XmlIgnore]
        public double Salary;
        [OptionalField(VersionAdded=2)]
        public string Company;
    }
}
