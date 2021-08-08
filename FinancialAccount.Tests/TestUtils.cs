using FinancialAccountService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    }
}
