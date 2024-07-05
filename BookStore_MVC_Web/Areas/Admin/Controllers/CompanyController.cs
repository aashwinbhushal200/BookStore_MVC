
using BookStore.DataAcess.Data;
using BookStore.DataAcess.Models;
using BookStore.DataAcess.Repository;
using BookStore.DataAcess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.DataAcess.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        /*  replace by UnitOfWork
          private readonly ICategoryRepository _categoryRepo;*/
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ////include properties
            List<Company> objCompanyList = _unitOfWork.iCompanyRepository.GetAll().ToList();
            //unitOfWork implementation:
            //List<Product> products = _unitOfWork.iProductRepository.GetAll().ToList();
            /*  repo pattern implementation
              List<Category> categories = _categoryRepo.GetAll().ToList();*/
            /*   ApplicationDbContext implementation
                  List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();*/
            //using projects to get listOfCategory
            return View(objCompanyList);
        }
        //Convert to upsert
        /*  public IActionResult Create()
          {
              ProductVM productVM = new()
              {
                  Products = new Product(),
                 categoryList = _unitOfWork.iCategoryRepository.GetAll().Select(
                  u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() })
              };
              return View(productVM);
          }*/
        //
        public IActionResult Upsert(int? id)
        { 
            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
               Company company = _unitOfWork.iCompanyRepository.Get(u => u.Id == id);
                return View(company);
            }

        }
        [HttpPost]
        //after data is filled
        public IActionResult Upsert(Company company, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitOfWork.iCompanyRepository.Add(company);
                }
                else
                {
                    _unitOfWork.iCompanyRepository.Update(company);
                }
                _unitOfWork.Save();
                TempData["success"] = "company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(company);

            }
        }



        //converted to Upsert
        /*public IActionResult Create(ProductVM productVM, IFormFile? file)
        {

            *//* _db.Categories.Add(obj);
             _db.SaveChanges();*/
        /* convert into Repo pattern
         _categoryRepo.Add(obj);*//*

        //unitOfWork

        if (ModelState.IsValid)
        {
            _unitOfWork.iProductRepository.Add(productVM.Products);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        else
        {
            productVM.categoryList = _unitOfWork.iCategoryRepository.GetAll().Select(
                    u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
            return View(productVM);

        }

        //save is only of UnitOfWork not icategory

    }*/
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Company? companyFromDb = _unitOfWork.iCompanyRepository.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();

            if (companyFromDb == null)
            {
                return NotFound();
            }
            return View(companyFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Company obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.iCompanyRepository.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Company updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }


        #region api call
        [HttpGet]
        public IActionResult GetAll(int id)
        {
            List<Company> companies = _unitOfWork.iCompanyRepository.GetAll().ToList();

            return Json(new { Data = companies });
        }
        //added after ajax call made withs sweetalert
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.iCompanyRepository.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

           


            _unitOfWork.iCompanyRepository.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion

    }
}