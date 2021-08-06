using FinancialAccountService.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Moq;
using EntityFrameworkCoreMock;
using System.Collections.Generic;
using FinancialAccountService.Controllers;
using System.Threading.Tasks;

namespace FinancialAccount.Tests
{
    [TestFixture]
    public class BalanceControllerTests
    {

        private FinancialAccountDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var users = new List<User>();

            for (int i = 0; i< 50; i++)
            {
                users.Add(new User
                {
                    Id = i,
                    FirstName = $"Name{i}",
                    LastName = $"Lastaname{i}",
                    MiddleName = $"Lastaname{i}"
                });
            }

            var dbContextMock = new DbContextMock<FinancialAccountDbContext>(new DbContextOptionsBuilder<FinancialAccountDbContext>().Options);
            dbContextMock.CreateDbSetMock(x => x.User, users);

            _dbContext = dbContextMock.Object;
        }

        [Test]
        public async Task Test1()
        {
            var controller = new BalanceController(_dbContext);
            var balance = await controller.GetBalance(1);
            
            Assert.AreEqual(0, balance.Value);
        }
    }
}