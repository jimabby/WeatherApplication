using Microsoft.EntityFrameworkCore;
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

            List<double> dates = new List<double>();
            List<double> temperatures = new List<double>();

            foreach (var data in weatherDatas)
            {
                dates.Add(data.Date.ToOADate());
                temperatures.Add(data.Temperature);
            }

            // User-selected graph type
            if (graphType == "scatter")
            {
                var scatter = plt.Add.Scatter(dates.ToArray(), temperatures.ToArray());
                scatter.Color = Colors.Blue;
                scatter.LineWidth = 2;
            }
            else if (graphType == "line")
            {
                var line = plt.Add.ScatterLine(dates.ToArray(), temperatures.ToArray());
                line.Color = Colors.Green;
                line.LineWidth = 2;
            }
            else if (graphType == "bar")
            {
                var bar = plt.Add.Bars(temperatures.ToArray());
                bar.Color = Colors.Red;

                plt.Axes.Bottom.SetTicks(dates.ToArray(), dates.Select(d => DateTime.FromOADate(d).ToShortDateString()).ToArray());
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
