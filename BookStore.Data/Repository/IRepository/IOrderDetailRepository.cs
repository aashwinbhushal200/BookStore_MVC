﻿using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;


namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}
