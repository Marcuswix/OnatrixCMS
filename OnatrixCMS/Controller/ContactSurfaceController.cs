using Microsoft.AspNetCore.Mvc;
using OnatrixCMS.Model;
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
        public ContactSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        public IActionResult HandleSubmit(ContactFormModel form)
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

                if (!Regex.IsMatch(form.Email, emailPattern))
                {
                    TempData["error_email"] = "Invalid Email (xx@xx.xx)";
                }

                return CurrentUmbracoPage();

            }

            TempData["Success"] = "Your contact request was successfully sent!";
            return RedirectToCurrentUmbracoPage();
        }
    }
}
