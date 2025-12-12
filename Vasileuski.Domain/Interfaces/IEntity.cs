using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vasileuski.Domain.Interfaces
{
    /// <summary>
    /// Базовый интерфейс для всех сущностей
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        int Id { get; set; }
    }
}
