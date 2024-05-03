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
    public class DepartmentRepository : GenericReposity<Department> , IDepartmentReposirory
    {
        public DepartmentRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
            
        }
    }
}
