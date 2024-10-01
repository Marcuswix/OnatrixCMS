using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using OnatrixCMS.Model;
using System.ComponentModel.DataAnnotations;


namespace OnatrixCMS.Controller
{
	public class SearchPageSurfaceController : SurfaceController
	{
        public SearchPageSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        public class SearchModel
        {
            [Required(ErrorMessage = "This input can not be empty")]
            [Display(Name = "Search", Prompt = "Search")]
            public string? Search { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(SearchModel search)
		{
            var searchQuery = Request.Form["Search"];

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                string stringSearch = searchQuery.ToString().ToLower();


                ViewBag.SearchWord = stringSearch;
                TempData["SearchWord"] = stringSearch;
                //var contentServices = Services.ContentService;
                //var rootContent = contentServices!.GetRootContent();
                //if (rootContent != null && rootContent.Any())
                //{
                //    foreach (var content in rootContent)
                //    {
                //        Console.WriteLine(content.Name); // Loggar namnen på rotinnehållet
                //    }
                //}

                //var homePage = rootContent.FirstOrDefault(x => x.Name == "Home");

                //long totalItems; // Detta kommer att hålla det totala antalet resultat
                //int pageIndex = 0; // Första sidan
                //int pageSize = 60; // Antal poster per sida (du kan justera detta beroende på ditt behov)

                //// Hämta alla barn till homePage paginerat
                //var children = contentServices.GetPagedChildren(homePage!.Id, pageIndex, pageSize, out totalItems).ToList();

                //var servicesNode = children.FirstOrDefault(x => x.Name == "Services");

                return Redirect("/searchpage/");
            }

            return CurrentUmbracoPage();
        }
	}
}
