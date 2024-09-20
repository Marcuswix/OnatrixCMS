using Microsoft.AspNetCore.Mvc;
using OnatrixCMS.Model;
using OnatrixCMS.Services;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace OnatrixCMS.Controller
{
    public class ContactSurfaceController : SurfaceController
    {
        private readonly EmailServices _emailServices;
		public ContactSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, EmailServices emailServices) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
			_emailServices = emailServices;
		}

		public async Task<IActionResult> HandleSubmit(ContactFormModel form)
        {
            if(!ModelState.IsValid)
            {
                TempData["Name"] = form.Name;
                TempData["Email"] = form.Email;
                TempData["Message"] = form.Message;
                TempData["Phone"] = form.Phone;

                var emailPattern = @"^[a-zA-Z0-9._%+-]{2,}@[a-zA-Z0-9.-]{2,}\.[a-zA-Z]{2,}$";

                TempData["error_name"] = string.IsNullOrEmpty(form.Name);
                TempData["error_email"] = string.IsNullOrEmpty(form.Email);
                TempData["error_message"] = string.IsNullOrEmpty(form.Message);
                TempData["error_phone"] = string.IsNullOrEmpty(form.Phone);
                
                if(!string.IsNullOrEmpty(form.Email))
                {
                    if (!Regex.IsMatch(form.Email, emailPattern))
                    {
                        TempData["error_email"] = "Invalid Email format";
                    }
                }
                return CurrentUmbracoPage();
            }

            //Denna del gör så att du kan hämta hem rätt node... och så att du kan skapa ett callbackItem...
            var contentService = Services.ContentService;
            var nodeGuid = new Guid("405211fe-3f89-4e2a-af88-01fe3d974a3a");
            var nodeId = contentService?.GetById(nodeGuid);
            var callbackItem = contentService?.Create(Guid.NewGuid().ToString(), nodeId, "requestACallBackItem");

            callbackItem?.SetValue("callbackName", form.Name);
            callbackItem?.SetValue("callbackEmail", form.Email);
            callbackItem?.SetValue("callbackMessage", form.Message);
            callbackItem?.SetValue("callbackPhone", form.Phone);

            var saveResult = contentService?.Save(callbackItem);
            if(saveResult.Success)
            {
                var publishResult = contentService.Publish(callbackItem, []);
                if(publishResult.Success)
                {
                    var formToSend = new QuestionFormModel
                    {
                        Email = form.Email,
                        Name = form.Name,
                        Question = form.Message
                    };

                    var response = await _emailServices.SendMessageToServiceBusAsync(formToSend);

                    if (response is OkResult)
                    {
                        TempData["Success"] = "Your contact request was successfully sent!";
                        return RedirectToCurrentUmbracoPage();
                    }

                    TempData["ContactErrorMessage"] = "Your contact request was saved and published, but no confirmation email was sent.";
                }

                TempData["ContactErrorMessage"] = "Your contact request was saved, but not published";
            }
            
            TempData["ContactErrorMessage"] = "Your contact request recived. Something went wrong!";
            return CurrentUmbracoPage();
        }

        public async Task<IActionResult> HandleQuestionSubmit(QuestionFormModel form)
        {
            if(!ModelState.IsValid)
            {
                TempData["NameQuestion"] = form.Name;
                TempData["EmailQuestion"] = form.Email;
                TempData["QuestionQuestion"] = form.Question;

                var emailPattern = @"^[a-zA-Z0-9._%+-]{2,}@[a-zA-Z0-9.-]{2,}\.[a-zA-Z]{2,}$";

                if (string.IsNullOrEmpty(form.Name))
                TempData["error_nameQuestion"] = "You must enter a name";

                if (string.IsNullOrEmpty(form.Email))
                TempData["error_emailQuestion"] = "You must enter an Email";

                if (string.IsNullOrEmpty(form.Question))
                TempData["error_questionQuestion"] = "You must enter a question";

                if (!string.IsNullOrEmpty(form.Email))
                {
                    if (!Regex.IsMatch(form.Email, emailPattern))
                    {
                        TempData["error_emailQuestion"] = "Invalid Email format";
                    }
                }
                return CurrentUmbracoPage();
            }

            var response = await _emailServices.SendMessageToServiceBusAsync(form);
            
            if(response is OkResult)
            {
				TempData["SuccessQuestion"] = "Your question was successfully sent!";
				return RedirectToCurrentUmbracoPage();
			}

            TempData["ErrorQuestion"] = "Something went wrong, please try again later!";
            return CurrentUmbracoPage();
		}
    }
}
