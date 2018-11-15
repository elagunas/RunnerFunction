using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class RequestModel
    {
        private Dictionary<string, List<string>> CitiesCalendar = new Dictionary<string, List<string>>();

        public RequestModel()
        {
            var cities = LoadCities();
            var calendars = LoadCalendars();

            foreach (var city in cities)
            {
                CitiesCalendar.Add(city, calendars);
            }
        }

        public Dictionary<string,List<string>> GetModel()
        {
            return CitiesCalendar;
        }

        private List<string> LoadCalendars()
        {
            return new List<string>() { "2018-11", "2018-12", "2019-01", "2019-02", "2019-03", "2019-04", "2019-05",
             "2019-06", "2019-07", "2019-08", "2019-10", "2019-11", "2019-12", };

        }

        private List<string> LoadCities()
        {
            return new List<string>() { "cataluna/barcelona", "madrid/madrid" };
        }
    }
}
