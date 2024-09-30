using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
    public class RecaptchaResponse
    {
        public bool Success { get; set; }
        public string? ChallengeTs { get; set; }
        public string? Hostname { get; set; }
    }

    public class HelpYouFormSurfaceController : SurfaceController
    {
        private readonly EmailServices _emailServices;
        private readonly IConfiguration _configuration;
        public HelpYouFormSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, EmailServices emailServices, IConfiguration configuration) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _emailServices = emailServices;
            _configuration = configuration;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            var formToSend = new HelpYouFormToSendModel
            {
                HelpEmail = form.HelpEmail,
                Date = form.Date,
            };

            string RecaptchaSecret = _configuration["Values:SecretKey"] ?? "";

            var recaptchaResponse = Request.Form["g-recaptcha-response"];

            var isValidRecaptcha = await ValidateRecaptcha(recaptchaResponse);

            if (string.IsNullOrEmpty(recaptchaResponse))
            {
                TempData["ErrorHelpYouForm"] = "reCAPTCHA response is empty...";
                return CurrentUmbracoPage();
            }
            if(isValidRecaptcha == false)
            {
                TempData["ErrorHelpYouForm"] = "reCAPTCHA validation failed. Please try again!";
                return CurrentUmbracoPage();
            }

            var contentServices = Services.ContentService;
            var parentGuid = new Guid("e0860029-1818-4bb3-b01a-64e637c4ebdd");
            var helpItem = contentServices?.Create(Guid.NewGuid().ToString(), parentGuid, "helpYouFormItem");

            helpItem?.SetValue("helpEmail", form.HelpEmail ?? "");
            helpItem?.SetValue("helpDate", form.Date ?? "");

            if(helpItem != null)
            {
                var result = contentServices?.Save(helpItem);

                if(result != null && result.Success)
                {
                    var success = contentServices?.Publish(helpItem, []);
                    var response = await _emailServices.SendEmailMessageAsync(formToSend);

                    if (success!.Success && response is OkResult)
                    {
                        TempData["SuccessHelpYouForm"] = "Your help request was successfully sent!";
                        return RedirectToCurrentUmbracoPage();
                    }
                    if (success!.Success)
                    {
                        TempData["ErrorHelpYouForm"] = "Your help request was successfully sent, but no confirmation Email was sent. Shure you got the right address: " + form.HelpEmail + "?";
                        return RedirectToCurrentUmbracoPage();
                    }
                }
            }
            TempData["ErrorHelpYouForm"] = "Something went wrong, please try again later.";
            return CurrentUmbracoPage();
        }

        private async Task<bool> ValidateRecaptcha(string recaptchaResponse)
        {
            string recaptchaSecret = _configuration["Values:SecretKey"] ?? "";
            var httpClient = new HttpClient();
            var googleApiUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={recaptchaSecret}&response={recaptchaResponse}";

            var response = await httpClient.GetStringAsync(googleApiUrl);
            var recaptchaResult = JsonConvert.DeserializeObject<RecaptchaResponse>(response);

            return recaptchaResult != null && recaptchaResult.Success;
        }
    }
}
