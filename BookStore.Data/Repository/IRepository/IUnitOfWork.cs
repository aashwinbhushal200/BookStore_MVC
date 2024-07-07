using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {

        ICategoryRepository iCategoryRepository { get; }
        IProductRepository iProductRepository { get; }
        ICompanyRepository iCompanyRepository { get; }
        IShoppingCartRepository iShoppingCartRepository { get; }
        IApplicationUserRepository iApplicationUserRepository { get; }
        IOrderHeaderRepository iOrderHeaderRepository { get; }
		IOrderDetailRepository iOrderDetailRepository { get; }
        void Save();
        
    }
}
