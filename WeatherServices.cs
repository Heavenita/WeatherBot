using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Discord;

namespace WheaterBot
{
    public class WeatherServices
    {
        public static async Task<string> ObterClimaAsync(string city)
        {
            Env.Load("C:\\Users\\Pedro Rossi\\source\\repos\\WheaterBot\\.env");

            string apiKey = Env.GetString("WEATHER_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                return "Chave da API não encontrada!";
            }

            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=pt_br";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return "Cidade não encontrada ou erro na API.";
                }

                string content = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(content);

                string description = json["weather"][0]["description"]?.ToString();
                double temperature = Math.Round(double.Parse(json["main"]["temp"].ToString()));

                return $"🌦️ **Previsão para `{city.ToUpper()}`**\n" +
                        "```ini\n" +
                        $"[Condição]:   {description}\n" +
                        $"[Temperatura]: {temperature}°C\n" +
                        "```";
            }


        }
    }
}
