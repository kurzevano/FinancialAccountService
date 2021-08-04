using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialAccountService.Models
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateBirth { get; set; }

        /// <summary>
        /// Текущий баланс пользователя
        /// </summary>
        public Balance CurrentBalance { get; set; }

        /// <summary>
        /// История операций на счёте пользователя
        /// </summary>
        public List<BalanceTransaction> BalanceTransactions { get; set; }
    }
}