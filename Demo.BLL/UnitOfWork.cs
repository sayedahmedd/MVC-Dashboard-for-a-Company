using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly ApplicationDBContext _dBContext;

        public IEmployeeRepository EmployeeRepository { get; set ; }
        public IDepartmentReposirory DepartmentReposirory { get ; set ; }

        public UnitOfWork(ApplicationDBContext dBContext) // Ask CLR for creating object from D bcontext 
        {
            _dBContext = dBContext;
            EmployeeRepository = new EmployeeRepository(_dBContext);
            DepartmentReposirory = new DepartmentRepository (_dBContext);
        }
        public int Complete()
        {
            return _dBContext.SaveChanges();
        }

        public void Dispose()
        {
            _dBContext.Dispose();
        }
    }
}
