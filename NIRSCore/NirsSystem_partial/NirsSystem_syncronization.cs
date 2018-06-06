using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using NIRSCore.Syncronization;
using System.Collections.Generic;
using NIRSCore.HelpfulEnumsStructs;

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
        /// Асинхронный запрос на сервер, с целью получения необходимого файла
        /// </summary>
        /// <param name="fileExt">Тип файла</param>
        /// <param name="name">Название файла для сохранения</param>
        public static async void GetFileFromServerAsync(FileToUpload fileExt, string name) => await Task.Run(async () =>
        {
            using (var client = new HttpClient())
            {
                //Формирование строки запроса
                string query = ProgramSettings.AdressServer + "Server/GetFile?";
                query += "Login=" + _login + "&";
                query += "File=";

                string dir = string.Empty;

                switch (fileExt)
                {
                    case FileToUpload.Backup:
                        dir = "Backup";
                        break;
                    case FileToUpload.Database:
                        dir = "Database";
                        break;
                    case FileToUpload.Document:
                        dir = "Document";
                        break;
                    case FileToUpload.Photo:
                        dir = "Photo";
                        break;
                    case FileToUpload.Settings:
                        dir = "Setting";
                        break;
                }

                query += dir + "&Name=" + name;

                //Выполнение запроса
                HttpResponseMessage message = client.GetAsync(query).Result;

                if (!message.IsSuccessStatusCode)
                    return;

                //Сохранение файла
                string path = Environment.CurrentDirectory + "\\data\\" + _login + "\\" + dir + "s\\" + name;

                using (FileStream file = new FileStream(path, FileMode.Create))
                {
                    await message.Content.CopyToAsync(file);
                }
            }
        });

        /// <summary>
        /// Отправка файла на сервер
        /// </summary>
        /// <param name="name">Название файла</param>
        public static async void PostFileFromServerAsync(string name) => await Task.Run(() =>
        {
            using (var client = new HttpClient())
            {
                byte[] file = File.ReadAllBytes(Environment.CurrentDirectory + "\\data\\" + _login + "\\" + name);
                string sfile = Convert.ToBase64String(file);
                FileData fileData = new FileData(_login + "\\" + name, sfile);
                HttpResponseMessage message = client.PostAsJsonAsync(ProgramSettings.AdressServer + "Server/PostFile", fileData).Result;
            }
        });

        /// <summary>
        /// Синхронизация с сервером
        /// </summary>
        /// <param name="exec">Обязательная синхронизация</param>
        public static int Synchronization(bool exec = false)
        {
            if (!User.IsConnectToServer && !exec)
                return 1;
            if (!IsServer)
                return 2;
            //Проверка существования на сервере связки логина и пароля
            using (var client = new HttpClient())
            {
                HttpResponseMessage message = client.PostAsJsonAsync(ProgramSettings.AdressServer + "Server/RegistrationUser", 
                    new RegistrationData { Login = _login, Md5 = _md5 }).Result;
                string resultString = message.Content.ReadAsStringAsync().Result;
                bool result = Convert.ToBoolean(resultString);
                if (!result)
                    return 3;
            }

            //Формирование локального списка файлов для синхронизации
            List<FileInfoData> fileInfos = new List<FileInfoData>();

            if(User.IsSynchronizeSettingsWithServer)
            {
                //Файл настроек
                FileInfoData fileSettings = new FileInfoData("Settings.bin", "", true, User.DateLastEditSettings, FileToUpload.Settings, false, false);
                fileInfos.Add(fileSettings);
            }

            if(User.IsSynchronizeDatabaseWithServer)
            {
                //Файлы БД
                if (User.DBMSName == "SQLite")
                {
                    FileInfoData fileDatabase = new FileInfoData("database.db", "", true, User.DateLastEditDatabase, FileToUpload.Database, false, false);
                    fileInfos.Add(fileDatabase);
                }
                else
                {
                    FileInfoData fileDatabase = new FileInfoData("database.mdf", "", true, User.DateLastEditDatabase, FileToUpload.Database, false, false);
                    fileInfos.Add(fileDatabase);
                    FileInfoData fileDatabaseLog = new FileInfoData("database_log.mdf", "", true, User.DateLastEditDatabase, FileToUpload.Database,
                        false, false);
                    fileInfos.Add(fileDatabaseLog);
                }
            }

            //Файлы резервных копий
            string[] backupFileNames = Directory.GetFiles(Environment.CurrentDirectory + "\\data\\" + _login + "\\Backups");
            foreach(string elem in backupFileNames)
            {
                FileInfo infFile = new FileInfo(elem);
                FileInfoData file = new FileInfoData(infFile.Name, "", false, DateTime.Now, FileToUpload.Backup, false, false);
                fileInfos.Add(file);
            }

            //Файлы фото
            string[] photoFileNames = Directory.GetFiles(Environment.CurrentDirectory + "\\data\\" + _login + "\\Photos");
            foreach(string elem in photoFileNames)
            {
                FileInfo infFile = new FileInfo(elem);
                FileInfoData file = new FileInfoData(infFile.Name, "", false, DateTime.Now, FileToUpload.Photo, false, false);
                fileInfos.Add(file);
            }

            //Файлы документов
            string[] documentFileNames = Directory.GetFiles(Environment.CurrentDirectory + "\\data\\" + _login + "\\Documents");
            foreach(string elem in documentFileNames)
            {
                FileInfo infFile = new FileInfo(elem);
                FileInfoData file = new FileInfoData(infFile.Name, "", false, DateTime.Now, FileToUpload.Document, false, false);
                fileInfos.Add(file);
            }

            //Отправка списка файлов и получение списка сервера
            ListFileInfoData list = new ListFileInfoData(fileInfos.ToArray());
            list.Login = _login;
            using (var client = new HttpClient())
            {
                HttpResponseMessage message = client.PostAsJsonAsync(ProgramSettings.AdressServer + "Server/GetListFilesInfo", list).Result;
                string resp = message.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<ListFileInfoData>(resp);
            }

            //Начало отправки/загрузки файлов
            foreach(var elem in list.FilesInfo)
            {
                if(elem.IsUpload)
                {
                    string prevPath = "";
                    switch(elem.FileType)
                    {
                        case FileToUpload.Backup:
                            prevPath = "Backups\\";
                            break;
                        case FileToUpload.Document:
                            prevPath = "Documents\\";
                            break;
                        case FileToUpload.Photo:
                            prevPath = "Photos\\";
                            break;
                    }
                    PostFileFromServerAsync(prevPath + elem.NameFile);
                }
                if(elem.IsDownload)
                {
                    GetFileFromServerAsync(elem.FileType, elem.NameFile);
                }
            }

            //Проверка дополнительных условий и удаление лишних файлов
            if(User.IsSynchronizeBackupWithServer)
                foreach (var elem in backupFileNames)
                    File.Delete(elem);
            if(User.IsSynchronizeDocumentsWithServer)
                foreach (var elem in backupFileNames)
                    File.Delete(elem);

            return 0;
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
