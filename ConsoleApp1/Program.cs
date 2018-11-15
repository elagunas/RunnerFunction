using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            //MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        async static Task MainAsync(string[] args)
        {
            var urlsModel =  CreateIntializeModel();
            List<HtmlDocument> lstHtmlDocuments = new List<HtmlDocument>();
            foreach (var url in urlsModel)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                var pageContents = await response.Content.ReadAsStringAsync();
                HtmlDocument pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);
                lstHtmlDocuments.Add(pageDocument);
            }

            foreach (var pageDocument in lstHtmlDocuments)
            {
                List<string> events = new List<string>();
                Dictionary<string, List<string>> dictEvents = new Dictionary<string, List<string>>();
                string currentMonth = string.Empty;
                var rootNode = pageDocument.DocumentNode.SelectNodes("(//div[contains(@id,'cal-container-llistat-curses2')])")[0];

                if (rootNode != null)
                {
                    var childNodes = rootNode.ChildNodes;

                    foreach (var node in childNodes)
                    {
                        var finished = false;
                        if (node.Name == "div")
                        {
                            if (node.Id == "cal-container-llistat-barra-mes" && !finished)
                            {
                                if (currentMonth != string.Empty && events.Count > 0)
                                {
                                    dictEvents.Add(currentMonth, events);
                                    finished = true;
                                    return;
                                }

                                currentMonth = node.InnerText;
                                events = new List<string>();
                            }

                            var rows = node.SelectNodes("(div[contains(@class,'cal-cont-cursa-fila')])");

                            if (rows != null)
                            {
                                var dataRow = rows[0];

                                var date = dataRow.ChildNodes[1].SelectSingleNode("(div[contains(@id,'cal-cont-cursa-fila-item-data')])").InnerText.Trim();
                                var title = dataRow.ChildNodes[3].SelectSingleNode("(div[contains(@class,'cal-cont-cursa-fila-item-title')])").InnerText.Trim();
                                var distance = dataRow.ChildNodes[5].SelectSingleNode("(div[contains(@class,'cal-cont-cursa-fila-item-dist1')])").InnerText.Trim();
                                events.Add($"{date}-{title}-{distance}");
                            }
                        }
                    }
                }
            }


        }

        private static List<string> CreateIntializeModel()
        {
            var requestModel = new RequestModel();
            var citiesCalendar = requestModel.GetModel();
            var urlBase = "https://runedia.mundodeportivo.com/calendario-carreras/espana";
            List<string> urls = new List<string>();

            foreach (var cityCalendar in citiesCalendar)
            {
                var city = cityCalendar.Key;
                var urlBaseCity = $"{urlBase}/{city}/tipo/distancia";
                foreach (var date in cityCalendar.Value)
                {
                    var urlBaseCityCalendar = $"{urlBaseCity}/{date}/0/0/";
                    urls.Add(urlBaseCityCalendar);
                }
            }
            return urls;
        }
    }
}
