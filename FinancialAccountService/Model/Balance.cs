using System.Collections.Generic;

namespace FinancialAccountService.Model
{
    /// <summary>
    /// Текущий баланс пользователя
    /// </summary>
    public class Balance
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Текущий остаток на счёте
        /// </summary>
        public decimal Summ { get; set; }

        /// <summary>
        /// История операций на счёте пользователя
        /// </summary>
        public ICollection<BalanceTransaction> BalanceTransactions { get; set; }
    }
}