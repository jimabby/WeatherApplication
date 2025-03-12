using ScottPlot;
using WeatherApplication.Models;
using WeatherApplication.Services;
using WeatherApplication.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        var builder = new ConfigurationBuilder()
            .AddEnvironmentVariables(); // This adds environment variables to the configuration.

        var config = builder.Build();

        string apiKey = config["ApiKey"];

        Console.Write("Enter the city name: ");
        string city = Console.ReadLine();

        if (string.IsNullOrEmpty(city))
        {
            Console.WriteLine("City is required!");
            return;
        }

        try
        {
            Console.WriteLine("Geting weather data: ");
            var weatherService = new WeatherService(apiKey);
            List<WeatherData> weatherDatas = weatherService.GetWeatherDatas(city);

            if (weatherDatas.Count > 0)
            {
                Console.WriteLine("Getting weather data successfully.");

                // Display weather data
                foreach (var weatherData in weatherDatas)
                {
                    Console.WriteLine($"Date: {weatherData.Date}");
                    Console.WriteLine($"Temperature: {weatherData.Temperature} °C");
                    Console.WriteLine("--------------------------------------");
                }

                var chartService = new ChartService();

                chartService.SaveChart(weatherDatas, city);
                Console.WriteLine("Save the chart successfully.");
            }
            else
            {
                Console.WriteLine("No weather data available.");
            }
        }
        catch (Exception e)
        {
            Logger.Log("There exists an error: " + e.Message);
        }
    }
}
