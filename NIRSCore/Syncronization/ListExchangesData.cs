namespace NIRSCore.Syncronization
{
    /// <summary>
    /// Объект представляет список всех обменов, предложенных пользователю или пользователем
    /// </summary>
    public sealed class ListExchangesData
    {
        /// <summary>
        /// Идентификатор обмена
        /// </summary>
        public int ExchangeId { get; set; }

        /// <summary>
        /// Логин получателя или отправителя
        /// </summary>
        public string LoginCreatorOrSender { get; set; }

        /// <summary>
        /// Является ли текущий пользователь создателем обмена
        /// </summary>
        public bool IsIAmCreator { get; set; }

        /// <summary>
        /// Односторонний ли обмен
        /// </summary>
        public bool IsOneWay { get; set; }

        /// <summary>
        /// Принял ли получатель обмен
        /// </summary>
        public bool IsSenderAccept { get; set; }

        /// <summary>
        /// Выполнил ли создатель обмен
        /// </summary>
        public bool IsCreatorDone { get; set; }

        /// <summary>
        /// Выполнил ли получатель обмен
        /// </summary>
        public bool IsSenderDone { get; set; }
    }
}
