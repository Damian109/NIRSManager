using System;
using System.Linq;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using NIRSCore.Syncronization;
using System.Collections.Generic;
using NIRSCore.HelpfulEnumsStructs;
using NIRSCore.DataBaseModels;

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
                    case FileToUpload.Document:
                        dir = "Document";
                        break;
                    case FileToUpload.Photo:
                        dir = "Photo";
                        break;
                    case FileToUpload.Database:
                        dir = "Database";
                        break;
                    case FileToUpload.Settings:
                        dir = "Settings";
                        break;
                }

                query += dir + "&Name=" + name;

                //Выполнение запроса
                HttpResponseMessage message = client.GetAsync(query).Result;

                if (!message.IsSuccessStatusCode)
                    return;


                if (fileExt != FileToUpload.Settings)
                {
                    //Сохранение файла
                    string path = Environment.CurrentDirectory + "\\data\\" + _login;
                    if (fileExt == FileToUpload.Settings || fileExt == FileToUpload.Database)
                        path += "\\" + name;
                    else
                        path += "\\" + dir + "s\\" + name;

                    FileInfo fileE = new FileInfo(path);
                    if (fileE.Exists)
                        fileE.Delete();

                    using (FileStream file = new FileStream(path, FileMode.Create))
                    {
                        await message.Content.CopyToAsync(file);
                    }
                }

                /*
                //Изменение пути к БД
                if(fileExt == FileToUpload.Settings)
                {
                    try
                    {
                        FileSettings fileSettings = new FileSettings(_login, _md5);
                        fileSettings.Read();

                        string newPathDB = Environment.CurrentDirectory + "\\data\\" + NirsSystem.GetLogin() + "\\database";
                        if (fileSettings.User.DBMSName == "MS SQL Express")
                            newPathDB += ".mdf";
                        else
                            newPathDB += ".db";
                        if (fileSettings.User.DBMSName == "MS SQL Express")
                        {
                            fileSettings.User.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='" + newPathDB + "'; Integrated Security = " +
                                fileSettings.User.IntegratedSecurity.ToString() + ";pooling=false;";
                        }
                        if (fileSettings.User.DBMSName == "SQLite")
                        {
                            fileSettings.User.ConnectionString = @"Data Source=" + newPathDB + ";pooling=false;";
                        }
                        fileSettings.Write();
                    }
                    catch(ErrorManager.NirsException e)
                    {
                        ErrorManager.ExecuteException(e);
                    }
                }*/
            }
        });

        /// <summary>
        /// Отправка файла на сервер
        /// </summary>
        /// <param name="name">Название файла</param>
        public static void PostFileFromServerAsync(string name)
        {
            using (var client = new HttpClient())
            {
                byte[] file = File.ReadAllBytes(Environment.CurrentDirectory + "\\data\\" + _login + "\\" + name);
                string sfile = Convert.ToBase64String(file);
                FileData fileData = new FileData(_login + "\\" + name, sfile);
                HttpResponseMessage message = client.PostAsJsonAsync(ProgramSettings.AdressServer + "Server/PostFile", fileData).Result;
                string t = message.ToString();
            }
        }

        /// <summary>
        /// Синхронизация с сервером
        /// </summary>
        /// <param name="exec">Обязательная синхронизация</param>
        public static int Synchronization(bool exec = false)
        {
            if (!User.IsConnectToServer)
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
                    if (File.Exists(Environment.CurrentDirectory + "\\data\\" + _login + "\\database.db"))
                        fileInfos.Add(fileDatabase);
                }
                //Отсутствует поддержка синхронизации файлов SQL Server
                /*else
                {
                    FileInfoData fileDatabase = new FileInfoData("database.mdf", "", true, User.DateLastEditDatabase, FileToUpload.Database, false, false);
                    fileInfos.Add(fileDatabase);
                    FileInfoData fileDatabaseLog = new FileInfoData("database_log.ldf", "", true, User.DateLastEditDatabase, FileToUpload.Database,
                        false, false);
                    fileInfos.Add(fileDatabaseLog);
                }*/
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
                if(elem.IsUpload && exec)
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
                if(elem.IsDownload && !exec)
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

        /// <summary>
        /// Вернуть список доступных резервных копий с сервера
        /// </summary>
        /// <returns></returns>
        public static List<BackupManager.BackupElem> GetBackupsFromServer()
        {
            if (!IsServer || !User.IsConnectToServer)
                return null;

            List<BackupManager.BackupElem> backups = new List<BackupManager.BackupElem>();

            using (var client = new HttpClient())
            {
                //Формирование строки запроса
                string query = ProgramSettings.AdressServer + "Server/GetListBackups?";
                query += "Login=" + _login;

                //Выполнение запроса
                HttpResponseMessage message = client.GetAsync(query).Result;
                string resp = message.Content.ReadAsStringAsync().Result;
                BackupManager.BackupElem[] elems = JsonConvert.DeserializeObject<BackupManager.BackupElem[]>(resp);
                if (elems != null)
                    backups = elems.ToList();
            }
            return backups;
        }


        /// <summary>
        /// Вернуть список доступных обменов с сервера
        /// </summary>
        /// <returns></returns>
        public static ExchangesDataArray GetExchanges()
        {
            if (!IsServer || !User.IsConnectToServer)
                return null;

            ExchangesDataArray array = new ExchangesDataArray();

            using (var client = new HttpClient())
            {
                //Формирование строки запроса
                string query = ProgramSettings.AdressServer + "Server/GetExchangesToExchange?";
                query += "Login=" + _login;

                //Выполнение запроса
                HttpResponseMessage message = client.GetAsync(query).Result;
                string resp = message.Content.ReadAsStringAsync().Result;
                array = JsonConvert.DeserializeObject<ExchangesDataArray>(resp);
            }
            return array;
        }

        /// <summary>
        /// Вернуть список пользователей, доступных для обмена
        /// </summary>
        /// <returns></returns>
        public static ListLoginsData GetUsers()
        {
            if (!IsServer || !User.IsConnectToServer)
                return null;

            ListLoginsData array = new ListLoginsData();

            using (var client = new HttpClient())
            {
                //Формирование строки запроса
                string query = ProgramSettings.AdressServer + "Server/GetLoginsToExchange?";
                query += "Login=" + _login;

                //Выполнение запроса
                HttpResponseMessage message = client.GetAsync(query).Result;
                string resp = message.Content.ReadAsStringAsync().Result;
                array = JsonConvert.DeserializeObject<ListLoginsData>(resp);
            }
            return array;
        }

        /// <summary>
        /// Отправить новый обмен на сервер
        /// </summary>
        /// <param name="loginSender">Логин получателя</param>
        /// <param name="isOneWay">Односторонний ли обмен</param>
        public static void AddExchange(string loginSender, bool isOneWay)
        {
            if (!IsServer || !User.IsConnectToServer)
                return;

            using (var client = new HttpClient())
            {
                //Формирование строки запроса
                string query = ProgramSettings.AdressServer + "Server/ExchangeNew?";
                query += "LoginSender=" + loginSender + "&LoginCreator=" + _login + "&";
                if (isOneWay)
                    query += "IsOneWay=1";
                else
                    query += "IsOneWay=0";

                //Выполнение запроса
                HttpResponseMessage message = client.GetAsync(query).Result;
            }
        }

        /// <summary>
        /// Принятие или отклонение обмена
        /// </summary>
        /// <param name="id">Идентификатор обмена</param>
        /// <param name="status">Принят обмен или отклонен</param>
        public static void TrueFalseExchange(int id, bool status)
        {
            if (!IsServer || !User.IsConnectToServer)
                return;

            using (var client = new HttpClient())
            {
                int t = 0;
                if (status)
                    t = 1;

                //Формирование строки запроса
                string query = ProgramSettings.AdressServer + "Server/ExchangeAccept?";
                query += "Id=" + id + "&Accepting=" + t;

                //Выполнение запроса
                HttpResponseMessage message = client.GetAsync(query).Result;
            }
        }

        /// <summary>
        /// Выполнить получение данных из БД другого пользователя
        /// </summary>
        /// <param name="id">Идентификатор обмена</param>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Успешна операция или нет</returns>
        public static async void DoneExchange(int id, string login, bool iAmCreator)
        {
            //Отправить запрос на получение файла
            string query = ProgramSettings.AdressServer + "Server/GetFile?";
            query += "Login=" + login + "&";
            query += "File=Database&Name=database.db";

            string path = Environment.CurrentDirectory + "\\data\\" + _login + "\\temp\\database.db";

            //Выполнение запроса
            using (var client = new HttpClient())
            {
                HttpResponseMessage message = client.GetAsync(query).Result;

                if (!message.IsSuccessStatusCode)
                    return;

                //Сохранение файла
                using (FileStream file = new FileStream(path, FileMode.Create))
                {
                    await message.Content.CopyToAsync(file);
                }
            }

            //Временно меняем строку подключения
            string connOld = User.ConnectionString;
            User.ConnectionString = @"Data Source=" + path + ";pooling=false;";

            //Получение списков данных
            List<AcademicDegree> academicDegrees = (List<AcademicDegree>)GetListObject<AcademicDegree>();
            List<Author> authors = (List<Author>)GetListObject<Author>();
            List<CoAuthor> coAuthors = (List<CoAuthor>)GetListObject<CoAuthor>();
            List<Conference> conferences = (List<Conference>)GetListObject<Conference>();
            List<Department> departments = (List<Department>)GetListObject<Department>();
            List<Direction> directions = (List<Direction>)GetListObject<Direction>();
            List<DirectionWork> directionWorks = (List<DirectionWork>)GetListObject<DirectionWork>();
            List<Faculty> faculties = (List<Faculty>)GetListObject<Faculty>();
            List<Group> groups = (List<Group>)GetListObject<Group>();
            List<Journal> journals = (List<Journal>)GetListObject<Journal>();
            List<Organization> organizations = (List<Organization>)GetListObject<Organization>();
            List<Position> positions = (List<Position>)GetListObject<Position>();
            List<Reward> rewards = (List<Reward>)GetListObject<Reward>();
            List<RewardWork> rewardWorks = (List<RewardWork>)GetListObject<RewardWork>();
            List<Work> works = (List<Work>)GetListObject<Work>();

            //Возвращаем старую строку подключения
            User.ConnectionString = connOld;

            //Получаем данные из текущей БД
            List<AcademicDegree> academicDegrees2 = (List<AcademicDegree>)GetListObject<AcademicDegree>();
            List<Conference> conferences2 = (List<Conference>)GetListObject<Conference>();
            List<Department> departments2 = (List<Department>)GetListObject<Department>();
            List<Direction> directions2 = (List<Direction>)GetListObject<Direction>();
            List<Faculty> faculties2 = (List<Faculty>)GetListObject<Faculty>();
            List<Group> groups2 = (List<Group>)GetListObject<Group>();
            List<Journal> journals2 = (List<Journal>)GetListObject<Journal>();
            List<Organization> organizations2 = (List<Organization>)GetListObject<Organization>();
            List<Position> positions2 = (List<Position>)GetListObject<Position>();
            List<Reward> rewards2 = (List<Reward>)GetListObject<Reward>();

            List<Author> authors2 = (List<Author>)GetListObject<Author>();
            List<CoAuthor> coAuthors2 = (List<CoAuthor>)GetListObject<CoAuthor>();
            List<DirectionWork> directionWorks2 = (List<DirectionWork>)GetListObject<DirectionWork>();
            List<RewardWork> rewardWorks2 = (List<RewardWork>)GetListObject<RewardWork>();
            List<Work> works2 = (List<Work>)GetListObject<Work>();


            //Начинаем добавлять полученные данные
            foreach (var elem in academicDegrees)
            {
                var varquery = academicDegrees2.FirstOrDefault(u => u.AcademicDegreeName == elem.AcademicDegreeName);
                if(varquery == null)
                    AddObject(elem);
            }
            academicDegrees2 = (List<AcademicDegree>)GetListObject<AcademicDegree>();

            foreach (var elem in conferences)
            {
                var varquery = conferences2.FirstOrDefault(u => u.ConferenceName == elem.ConferenceName);
                if (varquery == null)
                    AddObject(elem);
            }
            conferences2 = (List<Conference>)GetListObject<Conference>();

            foreach (var elem in departments)
            {
                var varquery = departments2.FirstOrDefault(u => u.DepartmentName== elem.DepartmentName);
                if (varquery == null)
                    AddObject(elem);
            }
            departments2 = (List<Department>)GetListObject<Department>();

            foreach (var elem in directions)
            {
                var varquery = directions2.FirstOrDefault(u => u.DirectionName == elem.DirectionName);
                if (varquery == null)
                    AddObject(elem);
            }
            directions2 = (List<Direction>)GetListObject<Direction>();

            foreach (var elem in faculties)
            {
                var varquery = faculties2.FirstOrDefault(u => u.FacultyName == elem.FacultyName);
                if (varquery == null)
                    AddObject(elem);
            }
            faculties2 = (List<Faculty>)GetListObject<Faculty>();

            foreach (var elem in groups)
            {
                var varquery = groups2.FirstOrDefault(u => u.GroupName == elem.GroupName);
                if (varquery == null)
                    AddObject(elem);
            }
            groups2 = (List<Group>)GetListObject<Group>();

            foreach (var elem in journals)
            {
                var varquery = journals2.FirstOrDefault(u => u.JournalName == elem.JournalName);
                if (varquery == null)
                    AddObject(elem);
            }
            journals2 = (List<Journal>)GetListObject<Journal>();

            foreach (var elem in organizations)
            {
                var varquery = organizations2.FirstOrDefault(u => u.OrganizationName == elem.OrganizationName);
                if (varquery == null)
                    AddObject(elem);
            }
            organizations2 = (List<Organization>)GetListObject<Organization>();

            foreach (var elem in positions)
            {
                var varquery = positions2.FirstOrDefault(u => u.PositionName == elem.PositionName);
                if (varquery == null)
                    AddObject(elem);
            }
            positions2 = (List<Position>)GetListObject<Position>();

            foreach (var elem in rewards)
            {
                var varquery = rewards2.FirstOrDefault(u => u.RewardName == elem.RewardName);
                if (varquery == null)
                    AddObject(elem);
            }
            rewards2 = (List<Reward>)GetListObject<Reward>();

            foreach (var elem in authors)
            {
                var varquery = authors2.FirstOrDefault(u => u.AuthorName == elem.AuthorName);
                if(varquery == null)
                {
                    if(elem.AcademicDegreeId != null)
                    {
                        string oldADS = academicDegrees.First(u => u.AcademicDegreeId == elem.AcademicDegreeId).AcademicDegreeName;
                        elem.AcademicDegreeId = academicDegrees2.First(u => u.AcademicDegreeName == oldADS).AcademicDegreeId;
                    }
                    
                    if(elem.DepartmentId != null)
                    {
                        string oldD = departments.First(u => u.DepartmentId == elem.DepartmentId).DepartmentName;
                        elem.DepartmentId = departments2.First(u => u.DepartmentName == oldD).DepartmentId;
                    }

                    if(elem.FacultyId != null)
                    {
                        string oldF = faculties.First(u => u.FacultyId == elem.FacultyId).FacultyName;
                        elem.FacultyId = faculties2.First(u => u.FacultyName == oldF).FacultyId;
                    }
                    
                    if(elem.GroupId != null)
                    {
                        string oldG = groups.First(u => u.GroupId == elem.GroupId).GroupName;
                        elem.GroupId = groups2.First(u => u.GroupName == oldG).GroupId;
                    }
                    
                    if(elem.OrganizationId != null)
                    {
                        string oldO = organizations.First(u => u.OrganizationId == elem.OrganizationId).OrganizationName;
                        elem.OrganizationId = organizations2.First(u => u.OrganizationName == oldO).OrganizationId;
                    }
                    
                    if(elem.PositionId != null)
                    {
                        string oldP = positions.First(u => u.PositionId == elem.PositionId).PositionName;
                        elem.PositionId = positions2.First(u => u.PositionName == oldP).PositionId;
                    }

                    AddObject(elem);
                }
            }
            authors2 = (List<Author>)GetListObject<Author>();

            foreach (var elem in works)
            {
                var varquery = works2.FirstOrDefault(u => u.WorkName == elem.WorkName);
                if(varquery == null)
                {
                    if(elem.ConferenceId != null)
                    {
                        string oldC = conferences.First(u => u.ConferenceId == elem.ConferenceId).ConferenceName;
                        elem.ConferenceId = conferences2.First(u => u.ConferenceName == oldC).ConferenceId;
                    }

                    if(elem.HeadAuthorId != null)
                    {
                        string oldH = authors.First(u => u.AuthorId == elem.HeadAuthorId).AuthorName;
                        elem.HeadAuthorId = authors2.First(u => u.AuthorName == oldH).AuthorId;
                    }

                    if(elem.JournalId != null)
                    {
                        string oldJ = journals.First(u => u.JournalId == elem.JournalId).JournalName;
                        elem.JournalId = journals2.First(u => u.JournalName == oldJ).JournalId;
                    }
                    AddObject(elem);
                }
            }
            works2 = (List<Work>)GetListObject<Work>();

            foreach(var elem in coAuthors)
            {
                var work = works.First(u => u.WorkId == elem.WorkId);
                var auth = authors.First(u => u.AuthorId == elem.AuthorId);

                elem.WorkId = works2.First(u => u.WorkName == work.WorkName).WorkId;
                elem.AuthorId = authors2.First(u => u.AuthorName == auth.AuthorName).AuthorId;

                AddObject(elem);
            }

            foreach (var elem in directionWorks)
            {
                var work = works.First(u => u.WorkId == elem.WorkId);
                var dir = directions.First(u => u.DirectionId == elem.DirectionId);

                elem.WorkId = works2.First(u => u.WorkName == work.WorkName).WorkId;
                elem.DirectionId = directions2.First(u => u.DirectionName == dir.DirectionName).DirectionId;

                AddObject(elem);
            }

            foreach(var elem in rewardWorks)
            {
                var work = works.First(u => u.WorkId == elem.WorkId);
                var rew = rewards.First(u => u.RewardId == elem.RewardId);

                elem.WorkId = works2.First(u => u.WorkName == work.WorkName).WorkId;
                elem.RewardId = rewards2.First(u => u.RewardName == rew.RewardName).RewardId;

                AddObject(elem);
            }

            query = ProgramSettings.AdressServer + "Server/ExchangeDone?";
            query += "Id=" + id + "&Doned=";
            if (iAmCreator)
                query += 1;
            else
                query += 0;

            //Выполнение запроса
            using (var client = new HttpClient())
            {
                HttpResponseMessage message = client.GetAsync(query).Result;
            }
        }
    }
}