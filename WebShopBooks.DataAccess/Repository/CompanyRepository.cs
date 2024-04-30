using WebShopBooks.DataAccess.Data;
using WebShopBooks.DataAccess.Repository.IRepository;
using WebShopBooks.Models.Models;

namespace WebShopBooks.DataAccess.Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Company company)
    {
        var companyInDb = _context.Companies.FirstOrDefault(p => p.Id == company.Id);

        if (companyInDb != null)
        {
            companyInDb.Name = company.Name;
            companyInDb.City = company.City;
            companyInDb.State = company.State;
            companyInDb.PhoneNumber = company.PhoneNumber;
            companyInDb.PostalCode = company.PostalCode;
            companyInDb.StreetAddress = company.StreetAddress;

            // _context.Products.Update(productInDb);
        }
    }
}
