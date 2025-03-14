using ScottPlot;
using WeatherApplication.Models;
using WeatherApplication.Services;
using WeatherApplication.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using WeatherApplication.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using WeatherApplication.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

class Program
{
    static void Main()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        string dbPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "weather.db");

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddMemoryCache()
            .AddSingleton<IWeatherService, WeatherService>()
            .AddSingleton<IChartService, ChartService>()
            .AddDbContext<WeatherDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"))//.LogTo(Console.WriteLine, LogLevel.Information))
            .BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();

            // WARNING: This deletes the database and recreates it!
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        // Resolve services
        var weatherService = serviceProvider.GetService<IWeatherService>();
        var chartService = serviceProvider.GetService<IChartService>();

        string apiKey = configuration["ApiKey"];

        while (true)
        {
            Console.Write("Enter the city name (You can enter 'exit' to exit.): ");
            string city = Console.ReadLine();

            if (string.IsNullOrEmpty(city))
            {
                Console.WriteLine("City is required!");
                continue;
            }

            if (city.ToLower() == "exit")
            {
                break;
            }

            Console.Write("Enter the graph type (scatter, line, bar): ");
            string graphType = Console.ReadLine()?.ToLower();

            if (graphType != "scatter" && graphType != "line" && graphType != "bar")
            {
                Console.WriteLine("Invalid graph type! Please enter 'scatter', 'line', or 'bar'.");
                continue;
            }

            try
            {
                Console.WriteLine("Geting weather data: ");
                List<WeatherData> weatherDatas;

                if (!weatherService.TryGetCachedWeatherData(city, out weatherDatas))
                {
                    weatherDatas = weatherService.GetWeatherDatas(city);
                    weatherService.CachedWeatherData(city, weatherDatas);
                }

                if (weatherDatas.Count > 0)
                {
                    Console.WriteLine("Getting weather data successfully.");

                    // Display weather data
                    foreach (var weatherData in weatherDatas)
                    {
                        Console.WriteLine($"Date: {weatherData.Date}");
                        Console.WriteLine($"Temperature: {weatherData.Temperature} °C");
                        Console.WriteLine($"Humidity: {weatherData.Humidity}");
                        Console.WriteLine($"Pressure: {weatherData.Pressure}");
                        Console.WriteLine($"WindSpeed: {weatherData.WindSpeed}");
                        Console.WriteLine($"Description: {weatherData.Description}");
                        Console.WriteLine("--------------------------------------");
                    }

                    chartService.SaveChart(weatherDatas, city, graphType);
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
}
