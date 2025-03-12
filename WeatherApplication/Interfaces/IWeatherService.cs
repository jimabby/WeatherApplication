using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Models;

namespace WeatherApplication.Interfaces
{
    interface IWeatherService
    {
        List<WeatherData> GetWeatherDatas(string city);
    }
}
