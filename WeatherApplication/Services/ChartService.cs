using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Interfaces;
using WeatherApplication.Models;

namespace WeatherApplication.Services
{
    class ChartService: IChartService
    {
        public void SaveChart(List<WeatherData> weatherDatas, string city)
        {
            var plt = new ScottPlot.Plot();

            List<double> dates = new List<double>();
            List<double> temperatures = new List<double>();

            foreach (var data in weatherDatas)
            {
                dates.Add(data.Date.ToOADate());
                temperatures.Add(data.Temperature);
            }

            var scatter = plt.Add.Scatter(dates.ToArray(), temperatures.ToArray());
            scatter.Color = Colors.Blue;
            scatter.LineWidth = 2;


            plt.Axes.DateTimeTicksBottom();
            plt.Axes.Title.Label.Text = $"Weather Forecast for {city} for the next 5 Days";
            plt.Axes.Bottom.Label.Text = "Date";
            plt.Axes.Left.Label.Text = "Temperature (°C)";

            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "weather_chart.png");
            plt.SavePng(savePath, 800, 600);

            Console.WriteLine($"Chart saved to: {savePath}");
        }
    }
}
