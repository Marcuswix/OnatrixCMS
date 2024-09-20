using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
    public class HelpYouFormSurfaceController : SurfaceController
    {
        private readonly EmailServices _emailServices;
        public HelpYouFormSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, EmailServices emailServices) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _emailServices = emailServices;
        }

        [HttpPost]
        public async Task<IActionResult> HandleSubmit(HelpYouFormModel form)
        {
            if(!ModelState.IsValid)
            {
                TempData["emailHelp"] = form.HelpEmail;

                var emailPattern = @"^[a-zA-Z0-9._%+-]{2,}@[a-zA-Z0-9.-]{2,}\.[a-zA-Z]{2,}$";

                TempData["error_emailHelp"] = string.IsNullOrEmpty(form.HelpEmail);

                if(!string.IsNullOrEmpty(form.HelpEmail))
                {
                    if (!Regex.IsMatch(form.HelpEmail, emailPattern))
                    {
                        TempData["error_email"] = "Invalid Email format";
                    }
                }
                return CurrentUmbracoPage();
            }

            var formToSend = new QuestionFormModel
            {
                Email = form.HelpEmail,
                Name = "Onatrix 24/7 online Support",
                Question = "How can ew Help you?"
            };

            var response = await _emailServices.SendMessageToServiceBusAsync(formToSend);

            if(response is OkResult)
            {
                TempData["SuccessHelpYouForm"] = "Your email was successfully sent!";
                return RedirectToCurrentUmbracoPage();
            }
            else
            {
                TempData["ErrorHelpYouForm"] = "Something went wrong, please try again later.";
                return CurrentUmbracoPage();
            }
        }
    }
}
