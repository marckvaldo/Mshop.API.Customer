using Microsoft.EntityFrameworkCore;
using MShop.Core.Test.Domain.Entity.Address;
using MShop.Core.Test.Domain.Entity.Customer;
using MShop.E2ETest.Base;
using MShop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.E2ETest.Common
{
    public class BaseFixture : BaseWebApplication
    {
        protected CustomerFaker _customerFaker;
        protected AddressFaker _addressFaker;
        public BaseFixture() :base()
        {
            _customerFaker = new CustomerFaker();
            _addressFaker = new AddressFaker();
        }

        protected async Task AddMigration(MshopDbContext dbContext)
        {
            if (dbContext != null)
                await dbContext.Database.MigrateAsync();
        }

        protected async Task DeleteDataBase(MshopDbContext dbContext)
        {
            if (dbContext != null)
                await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
