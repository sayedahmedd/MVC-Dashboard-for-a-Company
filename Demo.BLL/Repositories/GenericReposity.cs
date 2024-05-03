using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericReposity<T> : IGenericRepository<T> where T : BaseModel
    {
        private protected readonly ApplicationDBContext _dBContext;
        public GenericReposity(ApplicationDBContext dBContext)  // ClR will send object
        {
            // _dBContext = new ApplicationDBContext(); // Bad way to do it 
            _dBContext = dBContext;
        }

        public void Add(T entity)
            => _dBContext.Add(entity);
         

        public void Update(T entity)
        =>_dBContext.Update(entity);
        
        public void Delete(T entity)
        => _dBContext.Remove(entity);
        

        public T Get(int id)
        {
            return _dBContext.Find<T>(id); 
        }

        public IEnumerable<T> GetAll()
        {
            return _dBContext.Set<T>().AsNoTracking().ToList();
        }

    }
}
