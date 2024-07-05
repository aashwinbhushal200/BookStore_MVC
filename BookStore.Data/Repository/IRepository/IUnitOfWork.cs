using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAcess.Repository.IRepository
{
    public interface IUnitOfWork
    {

        ICategoryRepository iCategoryRepository { get; }
        IProductRepository iProductRepository { get; }
        ICompanyRepository iCompanyRepository { get; }
        IShoppingCartRepository iShoppingCartRepository { get; }
        void Save();
        
    }
}
