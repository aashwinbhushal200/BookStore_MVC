using BookStore.DataAccess.Data;
using BookStore.DataAccess.Models;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;

        //when we get this implementation , we want to pass this implementation to all base class
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;   
        }
        /*this save function is not part of model or controller, so we move it to unitOfwork.
        public void Save()
        {
            _db.SaveChanges();
        }*/
        public void Update(ShoppingCart shoppingCart)
        {
            _db.ShoppingCart.Update(shoppingCart);
        }

    }
}
