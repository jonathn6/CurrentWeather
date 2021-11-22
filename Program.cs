using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace CurrentWeather
{
    class Program
    {

        public static string ReturnElement(string passedString)
        {
            var tempArray = passedString.Split(":");
            var tempField = tempArray[1];
            tempField = tempField.Replace("\"", " ").Trim();
            return tempField;
        }
        public static int ReturnElementInt(string passedString)
        {
            var tempArray = passedString.Split(":");
            var tempField = tempArray[1];
            tempField = tempField.Replace("\"", " ").Trim();
            int returnValue = 0;
            bool validInt = int.TryParse(tempField, out returnValue);
            if (validInt == false)
            {
                returnValue = 0;
            }
            return returnValue;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Current_Weather!");

            // API key 5c0d5e4b857084e5350f04481249b55c

            var zipCodeString = "";
            var zipCodeInt = 0;

            do
            {
                Console.Write("What is the zip code of the area you want a weather upate for? ");
                zipCodeString = Console.ReadLine();
                if (zipCodeString.Length != 5)
                {
                    Console.WriteLine("That is not a valid zipcode.  Please try again.");
                } else
                {
                    bool validZipCode = int.TryParse(zipCodeString, out zipCodeInt);
                    if (validZipCode == false)
                    {
                        Console.WriteLine("That is not a valid zipcode.  Please try again.");
                        zipCodeInt = 0;
                    }
                }
            } while (zipCodeInt == 0);

            Console.WriteLine($"Please be patient while I retrieve the weather for zip code {zipCodeInt}");

            var weatherClient = new HttpClient();
            var weatherURL = "https://api.openweathermap.org/data/2.5/weather?zip=" + zipCodeInt + "&appid=5c0d5e4b857084e5350f04481249b55c&units=imperial";
            var weatherResponse = weatherClient.GetStringAsync(weatherURL).Result;

            var weatherReport = new APIReturn();
            var wMain = new Main();
            var WWeather = new Weather();
            weatherReport.Main = wMain;
            weatherReport.Weather = WWeather;


            string weatherJson = JObject.Parse(weatherResponse).GetValue("weather").ToString();
            string mainJson = JObject.Parse(weatherResponse).GetValue("main").ToString();

            var weatherArray = weatherJson.Split(",");
            weatherReport.Weather.main = ReturnElement(weatherArray[1]);
            weatherReport.Weather.description = ReturnElement(weatherArray[2]);

            var mainArray = mainJson.Split(",");

            weatherReport.Main.temp = double.Parse(ReturnElement(mainArray[0]));
            weatherReport.Main.feels_like = double.Parse(ReturnElement(mainArray[1]));
            weatherReport.Main.temp_min = double.Parse(ReturnElement(mainArray[2]));
            weatherReport.Main.temp_max = double.Parse(ReturnElement(mainArray[3]));
            weatherReport.Main.pressure = ReturnElementInt(mainArray[4]);
            weatherReport.Main.humidity = ReturnElementInt(mainArray[5]);

            Console.WriteLine($"Here is the weather for zip code {zipCodeInt}");

            Console.WriteLine($"Today will be {weatherReport.Weather.main} with {weatherReport.Weather.description}.");
            Console.WriteLine($"It is currently {weatherReport.Main.temp} degrees but it feels like {weatherReport.Main.feels_like}.");
            Console.WriteLine($"Todays high will be {weatherReport.Main.temp_max} with a low of {weatherReport.Main.temp_min}.");
            Console.WriteLine($"The barameter is at {weatherReport.Main.pressure} and the humidity is {weatherReport.Main.humidity}%");            


        }
    }
}
