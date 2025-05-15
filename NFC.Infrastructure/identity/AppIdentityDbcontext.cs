using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NFC.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Infrastructure.identity
{
	public class AppIdentityDbcontext:IdentityDbContext<ApplicationUser>
	{
        public AppIdentityDbcontext(DbContextOptions<AppIdentityDbcontext> options):base (options)
		{
           

		}
    }
}
