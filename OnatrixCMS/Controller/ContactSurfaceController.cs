using Json.Patch;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> HandleSubmit(ContactFormModel form)
        {
            var contentServiceFormName = Services.ContentService;


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

            if(form.FormName == "Callback")
            {
                //Denna del gör så att du kan hämta hem rätt node... och så att du kan skapa ett callbackItem...
                var contentService = Services.ContentService;
                var nodeGuid = new Guid("405211fe-3f89-4e2a-af88-01fe3d974a3a");
                var nodeId = contentService?.GetById(nodeGuid);
                var callbackItem = contentService?.Create(Guid.NewGuid().ToString(), nodeId, "requestACallBackItem");

                callbackItem?.SetValue("callbackName", form.Name);
                callbackItem?.SetValue("callbackEmail", form.Email);
                callbackItem?.SetValue("callbackMessage", form.Message);
                callbackItem?.SetValue("callbackPhone", form.Phone);
                callbackItem?.SetValue("callbackDate", form.DateTime);

                if(callbackItem != null)
                {
                    var saveResult = contentService?.Save(callbackItem);

                    if (saveResult!.Success)
                    {
                        var publishResult = contentService?.Publish(callbackItem!, []);

                        if (publishResult!.Success)
                        {
                            var formToSend = new ContactFormToSendModel
                            {
                                FormName = "callback",
                                Email = form.Email,
                                Name = form.Name,
                                Message = form.Message
                            };

                            //var response = await _emailServices.SendMessageToServiceBusAsync(formToSend);

                            var response = await _emailServices.SendEmailConfirmationByApiAsync(formToSend);

                            if (response is OkResult)
                            {
                                TempData["Success"] = "Your contact request was successfully sent!";
                                return RedirectToCurrentUmbracoPage();
                            }
                            else if(response is BadRequestResult)
                            {
                                TempData["ContactErrorMessage"] = "Your contact request was saved and published, but no confirmation email was sent.";
                                return RedirectToCurrentUmbracoPage();
                            }
                            TempData["ContactErrorMessage"] = "Your contact request was saved and published, but no confirmation email was sent.";
                            return RedirectToCurrentUmbracoPage();
                        }

                        TempData["ContactErrorMessage"] = "Your contact request was saved, but not published";
                    }
                }
                TempData["ContactErrorMessage"] = "Your contact request was NOT recived. Something went wrong!";
                return CurrentUmbracoPage();
            }

            if(form.FormName == "Questions")
            {
                var contentService = Services.ContentService;
                var nodeGuid = new Guid("0fad70a6-dc69-47b1-bb76-02d9f803e3c1");
                var parentNode = contentService!.GetById(nodeGuid)!;
                var questionItem = contentService.Create(form.Email, parentNode, "haveAQuestionItem");

                questionItem.SetValue("questionName", form.Name);
                questionItem.SetValue("questionEmail", form.Email);
                questionItem.SetValue("questionMessage", form.Message);
                questionItem.SetValue("questionDate", form.DateTime);

                var saveQuestion = contentService.Save(questionItem);

                if(saveQuestion.Success)
                {
                    var published = contentService.Publish(questionItem, []);

                    if(published.Success)
                    {
                        var formToSend = new ContactFormToSendModel
                        {
                            FormName = "question",
                            Email = form.Email,
                            Name = form.Name,
                            Message = form.Message,
                        };

                        //var response = await _emailServices.SendMessageToServiceBusAsync(formToSend);

                        var response = await _emailServices.SendEmailConfirmationByApiAsync(formToSend);

                        if (response is OkResult)
                        {
                            TempData["Success"] = "Your question was successfully sent!";
                            return RedirectToCurrentUmbracoPage();
                        }

                        TempData["ContactErrorMessage"] = "Your question was sent, but no confirmation email was sent.";
                    }

                    TempData["ContactErrorMessage"] = "Your question was saved, but no published and confirmation email was sent.";
                }

                TempData["ContactErrorMessage"] = "Your question was NOT recived. Something went wrong!";
                return CurrentUmbracoPage();

            }

            TempData["ContactErrorMessage"] = "Something went wrong. Please try again later!";
            return CurrentUmbracoPage();
        }
    }
}
