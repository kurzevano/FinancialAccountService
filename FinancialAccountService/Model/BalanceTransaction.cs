namespace FinancialAccountService.Model
{
    /// <summary>
    /// Транзакция денежных стредств на счёте пользователя (пополнение или списание)
    /// </summary>
    public class BalanceTransaction
    {
        /// <summary>
        /// Идентификатор транзакции
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Тип операции (1 - пополнение, 0 - снятие)
        /// </summary>
        public bool OperationTtype { get; set; }

        /// <summary>
        /// Сумма операции
        /// </summary>
        public decimal Summ { get; set; }

        /// <summary>
        /// id баланса пользователя
        /// </summary>
        public int? BalanceId { get; set; }
    }
}