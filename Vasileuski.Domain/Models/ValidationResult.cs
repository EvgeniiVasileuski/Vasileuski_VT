using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;

namespace Vasileuski.Domain.Models
{
    /// <summary>
    /// Результат валидации
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Список ошибок валидации
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Признак успешной валидации
        /// </summary>
        public bool IsValid => !Errors.Any();

        /// <summary>
        /// Добавить ошибку
        /// </summary>
        public void AddError(string error)
        {
            Errors.Add(error);
        }

        /// <summary>
        /// Добавить несколько ошибок
        /// </summary>
        public void AddErrors(IEnumerable<string> errors)
        {
            Errors.AddRange(errors);
        }

        /// <summary>
        /// Успешный результат валидации
        /// </summary>
        public static ValidationResult Success()
        {
            return new ValidationResult();
        }

        /// <summary>
        /// Неуспешный результат валидации
        /// </summary>
        public static ValidationResult Fail(string error)
        {
            var result = new ValidationResult();
            result.AddError(error);
            return result;
        }

        /// <summary>
        /// Неуспешный результат валидации с несколькими ошибками
        /// </summary>
        public static ValidationResult Fail(IEnumerable<string> errors)
        {
            var result = new ValidationResult();
            result.AddErrors(errors);
            return result;
        }
    }
}