using System;
using NIRSCore.ErrorManager;

namespace NIRSCore.Syncronization
{
    /// <summary>
    /// Специальный контейнер-обертка для обмена данными с сервером
    /// </summary>
    public sealed class ErrorsData
    {
        public NirsError[] NirsErrors { get; set; }

        public ErrorsData(NirsError[] nirsErrors) => NirsErrors = nirsErrors;

        public ErrorsData() => NirsErrors = null;
    }
}
