using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
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

        public IEnumerable<Dependent> GetAllDependents()
        {
            return _dependentList;
        }

        public Dependent GetDependentById(int Id)
        {
            return _dependentList?.FirstOrDefault(e => e.DependentId == Id);
        }

        public List<Dependent> GetDependentsByEmployeeId(int Id)
        {
            return _dependentList?.Where(e => e.EmployeeId == Id)?.ToList();
        }

        public Dependent AddDependent(Dependent dependent)
        {
            dependent.DependentId = _dependentList.Max(dependent => dependent.DependentId) + 1;
            _dependentList.Add(dependent);
            return dependent;
        }

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
