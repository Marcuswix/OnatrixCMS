using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using System.ComponentModel.DataAnnotations;
using OnatrixCMS.Model;


namespace OnatrixCMS.Controller
{
	public class SearchPageSurfaceController : SurfaceController
	{
        public SearchPageSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search()
		{
            var searchQuery = Request.Form["Search"];

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                string stringSearch = searchQuery.ToString();

                return Redirect($"/search/?searchWord={stringSearch}");
            }
            return CurrentUmbracoPage();
        }
	}
}
