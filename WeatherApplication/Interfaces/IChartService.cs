using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Models;

namespace WeatherApplication.Interfaces
{
    interface IChartService
    {
        void SaveChart(List<WeatherData> weatherDatas, string city);
    }
}
