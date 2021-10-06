using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    //Mock Dependent repository data
    public class MockDependentRepository : IDependentRepository
    {
        private readonly List<Dependent> _dependentList;

        public MockDependentRepository()
        {
            _dependentList = new List<Dependent>()
            {
                new Dependent() {EmployeeId = 1, DependentId = 1, RelationToEmployee = RelationToEmployee.Spouse, FirstName = "Andy", LastName = "Jaffer"},
                new Dependent() {EmployeeId = 1, DependentId = 2, RelationToEmployee = RelationToEmployee.Children, FirstName = "Rose", LastName = "Bird"},
                new Dependent() {EmployeeId = 2, DependentId = 3, RelationToEmployee = RelationToEmployee.Children, FirstName = "Danny", LastName = "Cole"},
                new Dependent() {EmployeeId = 2, DependentId = 4, RelationToEmployee = RelationToEmployee.Children, FirstName = "Anand", LastName = "Tripati" }
            };
        }

        //Get all dependents
        public IEnumerable<Dependent> GetAllDependents()
        {
            return _dependentList;
        }

        //Get dependent by Id
        public Dependent GetDependentById(int Id)
        {
            return _dependentList?.FirstOrDefault(e => e.DependentId == Id);
        }

        //Get All Dependents by employee Id
        public List<Dependent> GetDependentsByEmployeeId(int Id)
        {
            return _dependentList?.Where(e => e.EmployeeId == Id)?.ToList();
        }

        //Add dependenet
        public Dependent AddDependent(Dependent dependent)
        {
            dependent.DependentId = _dependentList.Max(dependent => dependent.DependentId) + 1;
            _dependentList.Add(dependent);
            return dependent;
        }

        //Update Dependent
        public Dependent UpdateDependent(Dependent dependentChanges)
        {
            Dependent dependent = _dependentList.FirstOrDefault(e => e.DependentId == dependentChanges.DependentId);
            if (dependent != null)
            {
                dependent.EmployeeId = dependentChanges.EmployeeId;
                dependent.FirstName = dependentChanges.FirstName;
                dependent.LastName = dependentChanges.LastName;
                dependent.RelationToEmployee = dependentChanges.RelationToEmployee;
            }
            return dependent;
        }

        //Delete Dependent
        public Dependent DeleteDependent(int id)
        {
            Dependent dependent = _dependentList.FirstOrDefault(e => e.DependentId == id);
            if (dependent != null)
            {
                _dependentList.Remove(dependent);
            }
            return dependent;
        }
    }
}
