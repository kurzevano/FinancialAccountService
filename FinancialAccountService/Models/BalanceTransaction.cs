namespace FinancialAccountService.Models
{
    /// <summary>
    /// Транзакция денежных стредств на счёте пользователя (пополнение или списание)
    /// </summary>
    public class BalanceTransaction
    {
        /// <summary>
        /// Идентификатор транзакции
        /// </summary>
        private int TransactionId { get; set; }

        /// <summary>
        /// Тип операции (1 - пополнение, 0 - снятие)
        /// </summary>
        private bool OperationTtype { get; set; }

        /// <summary>
        /// Сумма операции
        /// </summary>
        public decimal Summ { get; set; }

    }
}