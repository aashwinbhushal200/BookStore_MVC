using BookStore.DataAcess.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAcess.Repository.IRepository
{
    public interface ICategoryRepository :IRepository<Category>
    { 
        //T - Category
        void  Update(Category category);
        void Save();
        
    }   
}
