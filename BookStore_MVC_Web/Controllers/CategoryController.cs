
using BookStore_MVC_Web.Data;
using BookStore_MVC_Web.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore_MVC_Web.Controllers
{
    
    public class CategoryController : Controller
    {


        public IActionResult Index()
        {
            //  List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }

            /*if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }*/
            return View();

        }

    }
}