using BookStore.DataAcess.Models;
using BookStore.DataAcess.Repository;
using BookStore.DataAcess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookStore_MVC_Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.iProductRepository.GetAll(includeProperties: "Category");
            return View(productList);
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart cartItem = new ShoppingCart
            {
                Product = _unitOfWork.iProductRepository.Get(u => u.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId,
            };
            return View(cartItem);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
