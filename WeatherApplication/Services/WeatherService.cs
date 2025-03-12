using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using WeatherApplication.Interfaces;
using WeatherApplication.Models;
using WeatherApplication.Utils;

namespace WeatherApplication.Services
{
    class WeatherService: IWeatherService
    {
        private string apiKey;
        private string baseUrl = "https://api.openweathermap.org/data/2.5/forecast";
        private readonly IMemoryCache cache;

        public WeatherService(IConfiguration configuration, IMemoryCache cache)
        {
            this.apiKey = configuration["ApiKey"];
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public List<WeatherData> GetWeatherDatas(string city)
        {
            string cacheKey = $"Weather_{city}";

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

        public void CachedWeatherData(string city, List<WeatherData> weatherDatas)
        {
            this.cache.Set(city, weatherDatas, TimeSpan.FromMinutes(10));
        }

        public bool TryGetCachedWeatherData(string city, out List<WeatherData> weatherDatas)
        {
            return this.cache.TryGetValue(city, out weatherDatas);
        }
    }
}
