using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnatrixCMS.Model;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Controllers;

namespace OnatrixCMS.Controller
{
    [Route("umbraco/api/itemcontroller")]
    [ApiController]
    public class ItemController : UmbracoApiController
    {
        private readonly IContentService _contentService;

        public ItemController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpPost("deletequestion")]
        public IActionResult DeleteQuestion([FromBody] DeleteModel model)
        {
            int contentId = model.ContentId;
            var node = _contentService.GetById(contentId);

            if (node != null)
            {
                _contentService.Delete(node);
                return Ok(new { success = true, message = "Item is successfully deleted" });
            }
            return NotFound(new { success = false, message = "Item is not found" });
        }
    }
}
