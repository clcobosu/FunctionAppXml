using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionAppXml
{
    public static class Function1
    {
        [FunctionName("Function1")]
        
        /// Funcion por timer ejecutandose cada 30 min este valor puede ser modificado para el intervalo deseado
        /// 
        public static void Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            XmlDocument xDoc = new XmlDocument();
            try
            {
                XmlDocument doc;
                XmlNodeList nodo;               
                doc = new XmlDocument();
                doc.Load("gosocket.xml");
                nodo = doc.GetElementsByTagName("area");
                var xcantnodoArea = nodo.Count;
                decimal salarioTotal = 0;
                string dept = string.Empty;
                var cantnodo = 0;
                var mas3 = 0;

                Console.WriteLine($"Cantidad nodos tipo area {xcantnodoArea}");
                
                foreach (XmlNode n1 in doc.DocumentElement.ChildNodes)
                {   
                    if (n1.HasChildNodes)
                    {
                        var attributeValue = n1.ChildNodes;
                        dept = attributeValue[0].LastChild.Value;

                        foreach (XmlNode n2 in n1.ChildNodes)
                        {
                            if (n2.FirstChild.Name == "employee")
                            {
                                cantnodo = n2.ChildNodes.Count;
                                if (cantnodo > 2)
                                    mas3++;

                                foreach (XmlNode n3 in n2.ChildNodes)
                                {
                                    if (n2.ChildNodes.Count > 2)
                                    {                                        
                                        Console.WriteLine($" Elemento del nodo area con mas de 2 empleados "+n3.OuterXml);
                                    }
                                    else
                                    {
                                        Console.WriteLine($" Elemento del nodo area con menos de 3 empleados  " + n3.OuterXml); 
                                    }

                                    var xdato = n3.Attributes["salary"].Value;
                                    salarioTotal = salarioTotal + Decimal.Parse(xdato, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                                }

                                Console.WriteLine($"Salario Total del Area: {dept} Total : "+ salarioTotal + " $");
                                Console.WriteLine($" Cantidad de Nodos Area con mas de 2 empleados {mas3} ");

                            }                           
                        }
                    }
                }              
            }
            catch (Exception ex)
            {
                log.LogError($"Se produjo una excepcion {ex.Message}, {ex.InnerException}");
                throw;
            }
        }
    }
}
