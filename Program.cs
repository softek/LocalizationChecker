using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace LocalizationChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                PrintUsage();
                return;
            }

            var filePath = args[0];
            ModifyResxFile(filePath);
        }

        private static void ModifyResxFile(string filePath)
        {
            XNamespace ns = @"http://www.w3.org/XML/1998/namespace";
            var xml = File.ReadAllText(filePath);
            var doc = XDocument.Parse(xml);
            var textualDataElements = doc.Descendants("data")
                .Where(d => d.Attributes(ns + "space").Any());
            foreach (var dataElement in textualDataElements)
            {
                var newValue = BuildNewValue(dataElement);
                dataElement.Element("value").SetValue(newValue);
            }
            File.WriteAllText(filePath, doc.ToString());
        }

        private static string BuildNewValue(XElement dataElement)
        {
            return $"[✓{dataElement.Element("value").Value}]";
        }

        private static void PrintUsage()
        {
            Console.WriteLine("usage: LocalizationChecker.exe <file>");
            Console.WriteLine();
        }
    }
}
