﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Models;

namespace WeatherApplication.Interfaces
{
    interface IWeatherService
    {
        void CachedWeatherData(string city, List<WeatherData> weatherDatas);
        Task<List<WeatherData>> GetWeatherDatasAsync(string city);
        bool TryGetCachedWeatherData(string city, out List<WeatherData> weatherDatas);
    }
}
