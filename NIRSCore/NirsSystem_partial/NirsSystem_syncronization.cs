using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NIRSCore
{
    public static partial class NirsSystem
    {
        /// <summary>
        /// Задержка таймера перед следующей проверкой сервера
        /// </summary>
        private const int MSTOTIMER = 20000;

        /// <summary>
        /// Метод обратного вызова для таймера
        /// </summary>
        private static TimerCallback _timerCallback;

        /// <summary>
        /// Таймер для проверки доступности сервера
        /// </summary>
        private static Timer _timer;

        /// <summary>
        /// Доступен ли сервер
        /// </summary>
        private static bool _isServer;

        /// <summary>
        /// Проверка работоспособности сервера
        /// </summary>
        private async static void AsyncPingToServer(object obj) =>
            IsServer = await TaskPingToServer();

        /// <summary>
        /// Асинхронный запрос на сервер, с целью проверки его доступности
        /// </summary>
        /// <returns></returns>
        private static Task<bool> TaskPingToServer()
        {
            return Task.Run(() =>
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage message = client.GetAsync(ProgramSettings.AdressServer + "Server/Ping").Result;
                        bool result = false;
                        if (message.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string resultString = message.Content.ReadAsStringAsync().Result;
                            result = Convert.ToBoolean(resultString);
                        }
                        return result;
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                }
            });
        }

        /// <summary>
        /// Синхронизация с сервером
        /// </summary>
        private static void Synchronization()
        {

        }

        /// <summary>
        /// Делегат без параметров
        /// </summary>
        public delegate void eventSender();

        /// <summary>
        /// Событие изменения статуса доступности сервера
        /// </summary>
        public static event eventSender ChangeStatusServer;
    }
}
