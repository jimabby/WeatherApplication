using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.Models
{
    public class WeatherRecord
    {
        [Key]
        public long Id { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double Pressure { get; set; }
        public double WindSpeed { get; set; }
        public string Description { get; set; }
    }
}
