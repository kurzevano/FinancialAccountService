using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAccountService.Dto
{
    /// <summary>
    /// Dto для класса User
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Имя
        /// </summary>
        [Required(ErrorMessage = "Не указано имя")]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required(ErrorMessage = "Не указана фамилия")]
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? DateBirth { get; set; }
    }
}
