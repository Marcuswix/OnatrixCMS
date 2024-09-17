using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
    public class HelpYouFormSurfaceController : SurfaceController
    {
        public HelpYouFormSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        public IActionResult HandleSubmit(HelpYouFormModel form)
        {
            if(!ModelState.IsValid)
            {
                TempData["email"] = form.Email;

                var emailPattern = @"^[a-zA-Z0-9._%+-]{2,}@[a-zA-Z0-9.-]{2,}\.[a-zA-Z]{2,}$";

                TempData["error_email"] = string.IsNullOrEmpty(form.Email);

                if (!Regex.IsMatch(form.Email, emailPattern))
                {
                    TempData["error_email"] = "Invalid Email (xx@xx.xx)";
                }
                return CurrentUmbracoPage();
            }

            TempData["SuccessHelpYouForm"] = "Your email was successfully sent!";
            return RedirectToCurrentUmbracoPage();
        }
    }
}
