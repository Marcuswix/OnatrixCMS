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

		public async Task<IActionResult> SendMessageToServiceBusAsync(QuestionFormModel model)
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
	}
}
