
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
        private readonly ILogger<WeatherForecast> log;

        public WeatherForecast(ILogger<WeatherForecast> log)
        {
            this.log = log;
        }

        [Function("WeatherForecast")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req)
        {
            log.LogInformation("** PROXY: method: {method} uri:{uri}", req.Method, req.Url);
            var body = req.ReadAsString();
            log.LogInformation("** PROXY: body: {body}", body);

            if(!string.IsNullOrEmpty(body))
            {
                var res2 = req.CreateResponse();
                res2.WriteString("Echo: "+body);
                return res2;
            }

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
