using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebAPI8.Models
{
    public class AccountContext:DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options)
            :base(options)
        {
            
        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<PhoneNumber> PhoneNumbers { get; set; } = null!;

    }
}
