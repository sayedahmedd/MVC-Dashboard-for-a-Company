using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericReposity<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDBContext dBContext) : base(dBContext)
        {

        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return _dBContext.Employees.Where(E => E.Address.ToLower().Contains( address.ToLower()));
        }

        public IQueryable<Employee> SearchByName(string name)
        => _dBContext.Employees.Where(E => E.Name.ToLower().Contains(name));
    }
}
