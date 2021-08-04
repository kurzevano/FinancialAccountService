using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialAccountService.Database
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

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
        /// Ссылка на баланс пользователя
        /// </summary>
        public int BalanceId { get; set; }

        /// <summary>
        /// Текущий баланс пользователя
        /// </summary>
        public Balance CurrentBalance { get; set; }
    }
}