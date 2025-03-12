using Newtonsoft.Json.Linq;
using RestSharp;
using WeatherApplication.Models;
using WeatherApplication.Utils;

namespace WeatherApplication.Services
{
    class WeatherService
    {
        private string apiKey;
        private string baseUrl = "https://api.openweathermap.org/data/2.5/forecast";

        public WeatherService(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public List<WeatherData> GetWeatherDatas(string city)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddParameter("q", city);
            request.AddParameter("appid", this.apiKey);
            request.AddParameter("units", "metric");

            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK || string.IsNullOrEmpty(response.Content))
            {
                Logger.Log("Failed to get weather data: " + response.Content);
                return new List<WeatherData>();
            }

            List<WeatherData> weatherDatas = new List<WeatherData>();
            var json = JObject.Parse(response.Content);

            //Console.WriteLine("API Response: " + response.Content);

            if (json["list"] is JArray list)
            {
                foreach (var item in list)
                {
                    DateTime date = DateTime.Parse(item["dt_txt"].ToString());
                    double temp = double.Parse(item["main"]["temp"].ToString());

                    if (date >= DateTime.UtcNow && date <= DateTime.UtcNow.AddDays(5))
                    {
                        weatherDatas.Add(new WeatherData
                        {
                            Date = date,
                            Temperature = temp
                        });
                    }
                }
            }

            return weatherDatas;
        }
    }
}
