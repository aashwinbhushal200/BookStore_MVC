﻿using BookStore.DataAccess.Models;
using BookStore.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    { 
        //T - Category
        void  Update(ShoppingCart shoppingCart);
        
        
    }   
}
