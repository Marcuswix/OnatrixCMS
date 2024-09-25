using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnatrixCMS.Model;
using System.Text;

namespace OnatrixCMS.Services
{
	public class EmailServices
	{
		private readonly IConfiguration _configuration;

		public EmailServices(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IActionResult> SendMessageToServiceBusAsync(ContactFormToSendModel model)
		{
			if(model != null)
			{
				var jsonToSend = JsonConvert.SerializeObject(model);

				var connectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");

				if(connectionString == null)
				{
					connectionString = _configuration["Values:ServiceBusConnectionString"];

					if(connectionString == null)
					{
						connectionString = _configuration.GetConnectionString("ServiceBusConnectionString");
					}
				}

				await using var client = new ServiceBusClient(connectionString);

				ServiceBusSender sender = client.CreateSender("email_request");

				ServiceBusMessage message = new ServiceBusMessage(jsonToSend);

				try
				{
					await sender.SendMessageAsync(message);
					return new OkResult();
				}
				catch (Exception ex) {
					Console.WriteLine(ex.Message);
					return new StatusCodeResult(StatusCodes.Status500InternalServerError);
				}	
			}

			return new BadRequestResult();
		}

		public async Task<IActionResult> SendEmailMessageAsync(HelpYouFormToSendModel model)
		{
			if(model != null)
			{
                var objectToSend = JsonConvert.SerializeObject(model);
                var stringifyContent = new StringContent(objectToSend, Encoding.UTF8, "application/json");

                try
                {
                    using HttpClient _http = new HttpClient();
					var apiKey = _configuration["Values:ApiKey"];
                    _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(apiKey ?? "");
                    var response = await _http.PostAsync("https://onatrix-webapi-gmdqeucxdycqfvcr.westeurope-01.azurewebsites.net/api/Message", stringifyContent); 
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        return new OkResult();
                    }
					else if(response.StatusCode.Equals(500))
					{
						return new StatusCodeResult(500);
                    }
					else
					{
						return new StatusCodeResult(400);
                    }
                }
				catch(Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.WriteLine($"Error: {ex.Message}, StackTrace: {ex.StackTrace}");
				}
            }
			return new BadRequestResult();
		}
	}
}
