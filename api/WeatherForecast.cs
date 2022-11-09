
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace api
{
    public  class WeatherForecast
    {
        public static readonly Random random = new Random();
        public static readonly string[] summaries = new string[] {
            "-Freezing",
            "-Bracing",
            "-Balmy",
            "-Chilly"
        };

        [Function("WeatherForecast")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req)
        {
            var forecasts = new object[5];
            var currentDate = DateTime.Today;
            for (int i = 0; i < 5; i++)
            {
                var futureDate = currentDate.AddDays(i);
                var temperatureC = random.Next(-10, 35);
                forecasts[i] = new {
                    date = futureDate.ToString("yyyy-MM-dd"),
                    temperatureC = temperatureC,
                    summary = summaries[random.Next(0, summaries.Length)]
                };
            }

            var res = req.CreateResponse();
            await res.WriteAsJsonAsync(forecasts);
            return res;
        }
    }
}
