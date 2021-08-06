using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAccountService.Dto
{
    /// <summary>
    /// Dto для изменения баланса
    /// </summary>
    public class ChangeBalanceDto
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        [Required(ErrorMessage = "Пользователь не найден")]
        public int UserId { get; set; }

        /// <summary>
        /// Сумма пополнения или снятия
        /// </summary>
        [Required(ErrorMessage = "Не указана сумма")]
        public decimal Summ { get; set; }
    }
}