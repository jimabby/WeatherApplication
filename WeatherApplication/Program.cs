using ScottPlot;
using WeatherApplication.Models;
using WeatherApplication.Services;
using WeatherApplication.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using WeatherApplication.Interfaces;

class Program
{
    static void Main()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)  
            .AddSingleton<IWeatherService, WeatherService>()  
            .AddSingleton<IChartService, ChartService>() 
            .BuildServiceProvider();

        // Resolve services
        var weatherService = serviceProvider.GetService<IWeatherService>();
        var chartService = serviceProvider.GetService<IChartService>();

        string apiKey = configuration["ApiKey"];

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
            var weatherDatas = weatherService.GetWeatherDatas(city);

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
