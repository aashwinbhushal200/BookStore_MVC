
using BookStore_MVC_Web.Data;
using BookStore_MVC_Web.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore_MVC_Web.Controllers
{
    
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
                _db=db;
        }
        public IActionResult Index()
        {
            List<Category> categories = _db.Categories.ToList();
            //  List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        //after data is filled
        public IActionResult Create(Category obj)
        {
            //custom validation
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }
            _db.Categories.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
            
            /* if (ModelState.IsValid)
             {
                 _unitOfWork.Category.Add(obj);
                 _unitOfWork.Save();
                 TempData["success"] = "Category created successfully";
                 return RedirectToAction("Index");
             }*/
          

        }

    }
}