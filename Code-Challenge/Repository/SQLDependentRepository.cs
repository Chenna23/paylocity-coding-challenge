using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class SQLDependentRepository : IDependentRepository
    {
        private readonly AppDbContext context;
        public SQLDependentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Dependent> GetAllDependents()
        {
            return context?.Dependents;
        }

        public Dependent GetDependentById(int Id)
        {
            return context?.Dependents?.Find(Id);
        }

        public List<Dependent> GetDependentsByEmployeeId(int Id)
        {
            return context?.Dependents?.Where(x => x.EmployeeId == Id)?.ToList();
        }

        public Dependent AddDependent(Dependent dependent)
        {
            context?.Dependents?.Add(dependent);
            context.SaveChanges();
            return dependent;
        }

        public Dependent DeleteDependent(int id)
        {
            Dependent dependent = context?.Dependents?.Find(id);
            if (dependent != null)
            {
                context.Dependents.Remove(dependent);
                context.SaveChanges();
            }
            return dependent;
        }

        public Dependent UpdateDependent(Dependent dependentChanges)
        {
            var dependent = context?.Dependents?.FirstOrDefault(e => e.DependentId == dependentChanges.DependentId);

            if (dependent != null)
            {
                dependent.FirstName = dependentChanges.FirstName;
                dependent.LastName = dependentChanges.LastName;
                dependent.RelationToEmployee = dependentChanges.RelationToEmployee;
                context.SaveChanges();
                return dependent;
            }
            return null;
        }
    }
}
