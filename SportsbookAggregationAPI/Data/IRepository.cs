﻿using System.Linq;

 namespace SportsbookAggregationAPI.Data
{
    public interface IRepository<T>
    {
        IQueryable<T> Read();
        void Create(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}