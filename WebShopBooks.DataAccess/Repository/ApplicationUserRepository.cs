using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebShopBooks.DataAccess.Data;
using WebShopBooks.DataAccess.Repository.IRepository;
using WebShopBooks.Models.Models;

namespace WebShopBooks.DataAccess.Repository;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
    private ApplicationDbContext _context;

    public ApplicationUserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(ApplicationUser applicationUser)
    {
        _context.Update(applicationUser);
    }
}
