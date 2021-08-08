using FinancialAccountService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinancialAccount.Tests
{
    internal static class TestUtils
    {
        internal const int FakeUsersCount = 50;

        /// <summary>
        /// Создаёт список пользователей для теста
        /// </summary>
        /// <returns></returns>
        internal static List<User> GetFakeUsers()
        {
            var users = new List<User>();
            for (int i = 0; i < FakeUsersCount; i++)
            {
                users.Add(new User
                {
                    Id = i + 1,
                    FirstName = $"Name{i}",
                    LastName = $"Lastaname{i}",
                    MiddleName = $"Lastaname{i}"
                });
            }

            return users;
        }

        /// <summary>
        /// Заполняет БД с указанным именем тестовыми данными
        /// </summary>
        internal static string CreateTestDatabase(out List<User> fakeUsers)
        {
            fakeUsers = new List<User>();
            var dbName = Guid.NewGuid().ToString();
            using var dbContext = new FinancialAccountDbContext(new DbContextOptionsBuilder<FinancialAccountDbContext>()
           .UseInMemoryDatabase(databaseName: dbName)
           .Options);

            if (!dbContext.Set<User>().Any())
            {
                fakeUsers = TestUtils.GetFakeUsers();

                dbContext.User.AddRange(fakeUsers.AsQueryable());
                dbContext.SaveChanges();
            }

            return dbName;
        }
    }
}
