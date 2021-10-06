using System.Collections.Generic;

namespace EmployeeManagement.Models
{
    //Dependent Repository
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
