using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public interface IDependentRepository
    {
        IEnumerable<Dependent> GetAllDependents();
        List<Dependent> GetDependentsByEmployeeId(int Id);
        Dependent GetDependentById(int id);
        Dependent AddDependent(Dependent dependent);
        Dependent UpdateDependent(Dependent dependentChanges);
        Dependent DeleteDependent(int id);
    }
}
