﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSServer.Database
{
    /// <summary>
    /// Класс для представления строки таблицы обмена базами данных
    /// </summary>
    [Table("DatabaseExchangeTable")]
    public sealed class DatabaseExchangeTable
    {
        /// <summary>
        /// Идентификатор обмена
        /// </summary>
        [Key]
        public int DatabaseExchangeId { get; set; }

        /// <summary>
        /// Идентификатор инициатора обмена (внешний ключ)
        /// </summary>
        public int UserCreatorId { get; set; }

        /// <summary>
        /// Идентификатор получателя обмена (внешний ключ)
        /// </summary>
        public int UserSenderId { get; set; }

        /// <summary>
        /// Принял ли получатель обмен
        /// </summary>
        public bool? IsSenderAccept { get; set; }

        /// <summary>
        /// Выполнил ли инициатор обмен
        /// </summary>
        public bool IsCreatorDone { get; set; }

        /// <summary>
        /// Выполнил ли получатель обмен
        /// </summary>
        public bool IsSenderDone { get; set; }
    }
}