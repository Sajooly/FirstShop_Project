using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Application.Contracts.Slide;
using System.Collections.Generic;

namespace ServiceHost.Areas.Administration.Pages.Shop.Slides
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }

        public List<SlideViewModel> Slides;

        private readonly ISlideApplication _slideApplication;

        public IndexModel(ISlideApplication slideApplication)
        {
            _slideApplication = slideApplication;
        }

        public void OnGet()
        {
            Slides = _slideApplication.GetList();
        }
        public IActionResult OnGetCreate()
        {
            return Partial("Create", new CreateSlide());
        }
        public JsonResult OnPostCreate(CreateSlide command)
        {
            var result = _slideApplication.Create(command);
            return new JsonResult(result);
        }
        public IActionResult OnGetEdit(int id)
        {
            var product = _slideApplication.GetDetails(id);
            return Partial("Edit", product);
        }
        public JsonResult OnPostEdit(EditSlide command)
        {
            var result = _slideApplication.Edit(command);
            return new JsonResult(result);
        }
        public IActionResult OnGetRemove(int id)
        {
            var result = _slideApplication.RemoveSlide(id);
            if (result.IsSuccedded)
                return RedirectToPage("Index");

            Message = result.Message;
            return RedirectToPage("Index");
        }
        public IActionResult OnGetRestore(int id)
        {
            var result = _slideApplication.RestoreSlide(id);
            if (result.IsSuccedded)
                return RedirectToPage("Index");

            Message = result.Message;
            return RedirectToPage("Index");
        }

    }
}
