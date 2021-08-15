using FinancialAccountService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FinancialAccount.Tests
{
    internal static class TestUtils
    {
        /// <summary>
        /// Количество тестовых полььзователей
        /// </summary>
        internal const int FakeUsersCount = 50;

        /// <summary>
        /// Создаёт список пользователей для теста
        /// </summary>
        /// <returns></returns>
        internal static List<User> GetFakeUsers(decimal initialBalance = 0)
        {
            var users = new List<User>();
            for (int i = 0; i < FakeUsersCount; i++)
            {
                var user = new User
                {
                    Id = i + 1,
                    FirstName = $"Name{i}",
                    LastName = $"Lastaname{i}",
                    MiddleName = $"Lastaname{i}"
                };

                user.CurrentBalance = new Balance() { Id = user.Id, Summ = initialBalance };
                user.BalanceId = user.CurrentBalance.Id;

                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// Создаёт новую БД в памяти и наполняет тестовыми данными
        /// </summary>
        /// <param name="fakeUsers">Список тестовых пользователей</param>
        /// <param name="initialBalance">Начальный баланс каждого пользователя</param>
        /// <returns></returns>
        internal static string CreateTestDatabase(out List<User> fakeUsers, decimal initialBalance = 0)
        {
            fakeUsers = new List<User>();
            var dbName = Guid.NewGuid().ToString();
            using var dbContext = CreateDbContext(dbName);

            if (!dbContext.Set<User>().Any())
            {
                fakeUsers = TestUtils.GetFakeUsers(initialBalance);

                dbContext.User.AddRange(fakeUsers.AsQueryable());
                dbContext.SaveChanges();
            }

            return dbName;
        }

        /// <summary>
        /// Удаляет файл с базой данных
        /// </summary>
        /// <param name="databaseName"></param>
        internal static void DeleteTestDatabase(string databaseName)
        {
            File.Delete(databaseName);
        }

        /// <summary>
        /// Создаёт новый dbContext для базы данных с указанным именем
        /// </summary>
        /// <param name="databaseName">Имя базы данных</param>
        /// <returns></returns>
        internal static FinancialAccountDbContext CreateDbContext(string databaseName)
        {
            var context = new FinancialAccountDbContext(new DbContextOptionsBuilder<FinancialAccountDbContext>()
                    .UseSqlite($"Filename={databaseName}")
                    .Options);
            context.Database.Migrate();
            return context;
        }
    }
}
