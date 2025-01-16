using System.Linq;

namespace PromoCodeFactory.DataAccess.Data
{
    public class EfDbInitializer
        : IDbInitializer
    {
        private readonly DataContext _dataContext;

        public EfDbInitializer(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void InitializeDb()
        {
            if (_dataContext.Roles.Any())
                return;

            foreach (var item in FakeDataFactory.Roles)
            {
                _dataContext.Add(item);
            }
            _dataContext.SaveChanges();

            foreach (var item in FakeDataFactory.Employees)
            {
                item.Role = _dataContext.Roles.Find(item.Role.Id);
                _dataContext.Add(item);
            }

            foreach (var item in FakeDataFactory.Preferences)
            {
                _dataContext.Add(item);
            }
            _dataContext.SaveChanges();
        }
    }
}