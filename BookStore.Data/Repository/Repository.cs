﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookStore.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _dbContext = db;

          /* for eg dbset will be set to categories when constructor gets called.
            now it becomes as _db.Categories == dbSet*/
            this.dbSet = _dbContext.Set<T>();
            // _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
           // _dbContext.Products.Include(u => u.Category);
            //      include can have multiple properties:
            _dbContext.Products.Include(u => u.Category).Include(u => u.CategoryId);



        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            //same as _db.Categories.Add(entity)
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            //bool tracked = false for EF core not update itself eg shopping cart. 
            IQueryable<T> query;
            if (tracked) {
                 query= dbSet;
                
            }
            else {
                 query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);
            //include properties
            if (!string.IsNullOrEmpty(includeProperties)) {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();

        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null) {
                query = query.Where(filter);
            }
            ////include properties
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
