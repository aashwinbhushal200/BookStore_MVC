using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore_MVC_Web.Areas.Customer.Controllers
{

	[Area("customer")]
	[Authorize]
	public class CartController : Controller
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailSender _emailSender;
		[BindProperty]
		public ShoppingCartVM shoppingCartVM { get; set; }
		public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
		{
			_unitOfWork = unitOfWork;
			_emailSender = emailSender;
		}


		public IActionResult Index()
		{

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			shoppingCartVM = new()
			{
				ShoppingCartList = _unitOfWork.iShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId,
				includeProperties: "Product"),
				OrderHeader= new()
			};

			//IEnumerable<ProductImage> productImages = _unitOfWork.ProductImage.GetAll();

			foreach (var cart in shoppingCartVM.ShoppingCartList)
			{
				// cart.Product.ProductImages = productImages.Where(u => u.ProductId == cart.Product.Id).ToList();
				cart.Price = GetPriceBasedOnQuantity(cart);
				//ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

			return View(shoppingCartVM);
		}

		public IActionResult Summary()
		{

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			shoppingCartVM = new()
			{
				ShoppingCartList = _unitOfWork.iShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId,
				includeProperties: "Product"),
				OrderHeader = new()
			};

			shoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.iApplicationUserRepository.Get(u => u.Id == userId);

			shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.Name;
			shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
			shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
			shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
			shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.ApplicationUser.State;
			shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.ApplicationUser.PostalCode;



			foreach (var cart in shoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			return View(shoppingCartVM);
		}

		[HttpPost]
		[ActionName("Summary")]
		public IActionResult SummaryPOST()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			shoppingCartVM.ShoppingCartList = _unitOfWork.iShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId,
				includeProperties: "Product");

			shoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
			shoppingCartVM.OrderHeader.ApplicationUserId = userId;

			ApplicationUser applicationUser = _unitOfWork.iApplicationUserRepository.Get(u => u.Id == userId);


			foreach (var cart in shoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

			if (applicationUser.CompanyId.GetValueOrDefault() == 0)
			{
				//it is a regular customer 
				shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
				shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
			}
			else
			{
				//it is a company user
				shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
				shoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
			}
			_unitOfWork.iOrderHeaderRepository.Add(shoppingCartVM.OrderHeader);
			_unitOfWork.Save();
			foreach (var cart in shoppingCartVM.ShoppingCartList)
			{
				OrderDetail orderDetail = new()
				{
					ProductId = cart.ProductId,
					OrderHeaderId = shoppingCartVM.OrderHeader.Id,
					Price = cart.Price,
					Count = cart.Count
				};
				_unitOfWork.iOrderDetailRepository.Add(orderDetail);
				_unitOfWork.Save();
			}
			//strip logic 
			/*if (applicationUser.CompanyId.GetValueOrDefault() == 0)
			{
				//it is a regular customer account and we need to capture payment
				//stripe logic
				var domain = Request.Scheme + "://" + Request.Host.Value + "/";
				var options = new SessionCreateOptions
				{
					SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
					CancelUrl = domain + "customer/cart/index",
					LineItems = new List<SessionLineItemOptions>(),
					Mode = "payment",
				};

				foreach (var item in shoppingCartVM.ShoppingCartList)
				{
					var sessionLineItem = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
							Currency = "usd",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Product.Title
							}
						},
						Quantity = item.Count
					};
					options.LineItems.Add(sessionLineItem);
				}


				var service = new SessionService();
				Session session = service.Create(options);
				_unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
				_unitOfWork.Save();
				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);

			}*/


			return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartVM.OrderHeader.Id });
        }


          public IActionResult OrderConfirmation(int id) 
		{

			OrderHeader orderHeader = _unitOfWork.iOrderHeaderRepository.Get(u => u.Id == id, includeProperties: "ApplicationUser");
			/*strip logic
			if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
			{
				//this is an order by customer

				var service = new SessionService();
				Session session = service.Get(orderHeader.SessionId);

				if (session.PaymentStatus.ToLower() == "paid")
				{
					_unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
					_unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
					_unitOfWork.Save();
				}
				HttpContext.Session.Clear();

			}
*/
			_emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Bulky Book",
				$"<p>New Order Created - {orderHeader.Id}</p>");

			List<ShoppingCart> shoppingCarts = _unitOfWork.iShoppingCartRepository
				.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

			_unitOfWork.iShoppingCartRepository.RemoveRange(shoppingCarts);
			_unitOfWork.Save();

			return View(id);
		  }


        public IActionResult Plus(int cartId)
		{
			var cartFromDb = _unitOfWork.iShoppingCartRepository.Get(u => u.Id == cartId);
			cartFromDb.Count += 1;
			_unitOfWork.iShoppingCartRepository.Update(cartFromDb);
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Minus(int cartId)
		{
			var cartFromDb = _unitOfWork.iShoppingCartRepository.Get(u => u.Id == cartId);
			if (cartFromDb.Count <= 1)
			{
				//remove that from cart

				_unitOfWork.iShoppingCartRepository.Remove(cartFromDb);
				HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.iShoppingCartRepository
					.GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
			}
			else
			{
				cartFromDb.Count -= 1;
				_unitOfWork.iShoppingCartRepository.Update(cartFromDb);
			}

			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Remove(int cartId)
		{
			var cartFromDb = _unitOfWork.iShoppingCartRepository.Get(u => u.Id == cartId);

			_unitOfWork.iShoppingCartRepository.Remove(cartFromDb);
			//remove from cart 
			HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.iShoppingCartRepository
			  .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}



		private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
		{
			if (shoppingCart.Count <= 50)
			{
				return shoppingCart.Product.Price;
			}
			else
			{
				if (shoppingCart.Count <= 100)
				{
					return shoppingCart.Product.Price50;
				}
				else
				{
					return shoppingCart.Product.Price100;

				}
			}
		}
	}
}
