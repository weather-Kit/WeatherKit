﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public class CityListService : ICityListService
    {
        private List<CityOptions> cityOptionsList;
        public CityListService()
        {
            ReadCityOptionsList();
        }
        public List<CityOptions> GetCityOptionsList()
        {
            return cityOptionsList;
        }

        public void ReadCityOptionsList()
        {
            cityOptionsList = new List<CityOptions>();
            List<CityInfo> tempList = new List<CityInfo>();

            using (StreamReader r = new StreamReader("wwwroot/json/city.list.json"))
            {
                string json = r.ReadToEnd();
                tempList = JsonConvert.DeserializeObject<List<CityInfo>>(json);
            }

            foreach (var city in tempList)
            {
                cityOptionsList.Add(new CityOptions(city.name, city.state, city.country));
            }

            return;
        }
    }
}
