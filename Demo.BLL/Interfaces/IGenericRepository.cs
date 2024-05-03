using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    // T Repersent Domin model 
    // Now T is all Every thing 
    // I need to make Constranint over T To Confirme T is Domian Model 
    // What We Should Do ??
    // Think !!
    // Doman Model Base Common Atrribute as id and make All Entity inhert from them

    public interface IGenericRepository<T> where T : BaseModel
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
