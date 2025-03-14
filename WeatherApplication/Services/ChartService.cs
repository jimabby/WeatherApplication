using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ScottPlot;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Database;
using WeatherApplication.Interfaces;
using WeatherApplication.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WeatherApplication.Services
{
    class ChartService: IChartService
    {
        private readonly WeatherDbContext dbContext;
        public ChartService(WeatherDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SaveChart(List<WeatherData> weatherDatas, string city, string graphType)
        {
            var plt = new ScottPlot.Plot();

            List<double> dates = weatherDatas.Select(data => data.Date.ToOADate()).ToList();
            Dictionary<string, List<double>> metricsData = new Dictionary<string, List<double>>
            {
                { "Temperature (°C)", weatherDatas.Select(data => data.Temperature).ToList() },
                { "Humidity (%)", weatherDatas.Select(data => (double)data.Humidity).ToList() },
                { "Wind Speed (km/h)", weatherDatas.Select(data => data.WindSpeed).ToList() },
            };

            Dictionary<string, Color> metricColors = new Dictionary<string, Color>
            {
                { "Temperature (°C)", Colors.Blue },
                { "Humidity (%)", Colors.Green },
                { "Wind Speed (km/h)", Colors.Red }
            };

            foreach (var metric in metricsData.Keys)
            {
                // User-selected graph type
                if (graphType == "scatter")
                {
                    var scatter = plt.Add.Scatter(dates.ToArray(), metricsData[metric].ToArray());
                    scatter.Color = metricColors[metric];
                    scatter.LineWidth = 2;
                }
                else if (graphType == "line")
                {
                    var line = plt.Add.ScatterLine(dates.ToArray(), metricsData[metric].ToArray());
                    line.Color = metricColors[metric];
                    line.LineWidth = 2;
                }
                else if (graphType == "bar")
                {
                    var bar = plt.Add.Bars(metricsData[metric].ToArray());
                    bar.Color = metricColors[metric];

                    plt.Axes.Bottom.SetTicks(dates.ToArray(), dates.Select(d => DateTime.FromOADate(d).ToShortDateString()).ToArray());
                }
            }


            plt.Axes.DateTimeTicksBottom();
            plt.Axes.Title.Label.Text = $"Weather Forecast for {city} for the next 5 Days";
            plt.Axes.Bottom.Label.Text = "Date";
            plt.Axes.Left.Label.Text = "Temperature (°C)";

            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "weather_chart.png");
            plt.SavePng(savePath, 800, 600);

            Console.WriteLine($"Chart saved to: {savePath}");

            // Convert chart to byte array
            byte[] imageBytes = plt.GetImageBytes(800, 600, ScottPlot.ImageFormat.Png);

            // Debug: Ensure imageBytes is not null
            if (imageBytes == null || imageBytes.Length == 0)
            {
                Console.WriteLine("Error: Chart imageBytes is null or empty.");
                return;
            }

            Console.WriteLine($"Generated image size: {imageBytes.Length} bytes");


            // Store in database
            try
            {
                // Use direct SQL since we know this works reliably
                string connectionString = dbContext.Database.GetDbConnection().ConnectionString;
                using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        // Clear the entire table
                        command.CommandText = "DELETE FROM WeatherCharts";
                        command.ExecuteNonQuery();

                        // Insert the new record
                        command.Parameters.Clear();
                        command.CommandText = @"INSERT INTO WeatherCharts (City, Date, ChartImage) 
                               VALUES (@city, @date, @image)";
                        command.Parameters.AddWithValue("@city", city);
                        command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@image", imageBytes);
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Database operation affected {rowsAffected} rows");
                    }
                }
                Console.WriteLine($"Chart for {city} saved in database successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}
