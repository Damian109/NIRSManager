using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http;
using NIRSCore.Syncronization;
using NIRSCore.FileOperations;
using NIRSManagerServer.Models;
using System.Collections.Generic;
using NIRSCore.HelpfulEnumsStructs;
using NIRSCore.BackupManager;

namespace NIRSManagerServer.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за ответы на запросы клиента, касательно служебной информации
    /// </summary>
    [System.Web.Mvc.RoutePrefix("Server")]
    public class ServerController : Controller
    {
        /// <summary>
        /// Проверка работоспособности сервера и его доступности
        /// </summary>
        /// <returns>Всегда истина</returns>
        [System.Web.Mvc.HttpGet]
        public bool Ping() => true;

        /// <summary>
        /// Получение от клиента списка ошибок и сохранение в логе
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public bool ErrorsSet([FromBody]ErrorsData errors)
        {
            if (errors.NirsErrors == null || errors.NirsErrors.Length <= 0)
                return false;
            FileErrors file = new FileErrors(Request.MapPath("..//data//"));
            file.Read();
            List<FileErrorsItem> items;
            if (file.ErrorsItems == null)
                items = new List<FileErrorsItem>();
            else
                items = file.ErrorsItems;
            foreach (var elem in errors.NirsErrors)
                items.Add(new FileErrorsItem()
                {
                    Message = elem.Message,
                    NameSource = elem.NameSource,
                    NameSystem = elem.NameSystem,
                    DateError = elem.DateError
                });
            file.ErrorsItems = items;
            file.Write();
            return true;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="md5">Md5-сумма логина и пароля</param>
        /// <returns>Получилось ли зарегистрировать пользователя</returns>
        [System.Web.Mvc.HttpPost]
        public bool RegistrationUser(RegistrationData data)
        {
            using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
            {
                UserTable user = databaseContext.UserTables.FirstOrDefault(u => u.Login == data.Login && u.Md5 == data.Md5);
                if (user == null)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Получения списка файлов пользователя
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public JsonResult GetListFilesInfo([FromBody]ListFileInfoData list)
        {
            //Формирование полного пути
            string mainPath = Request.MapPath("..//data//" + list.Login);

            //Получение массивов путей к файлам
            string[] masb = Directory.GetFiles(mainPath + "\\Backups");
            string[] masp = Directory.GetFiles(mainPath + "\\Photos");
            string[] masd = Directory.GetFiles(mainPath + "\\Documents");
            string[] masm = Directory.GetFiles(mainPath);

            //Получение списков имен файлов
            List<string> backupFileNames = new List<string>();
            List<string> photoFileNames = new List<string>();
            List<string> documentFileNames = new List<string>();
            List<string> mainFileNames = new List<string>();

            foreach(var elem in masb)
            {
                FileInfo infFile = new FileInfo(elem);
                backupFileNames.Add(infFile.Name);
            }
            foreach (var elem in masp)
            {
                FileInfo infFile = new FileInfo(elem);
                photoFileNames.Add(infFile.Name);
            }
            foreach (var elem in masd)
            {
                FileInfo infFile = new FileInfo(elem);
                documentFileNames.Add(infFile.Name);
            }
            foreach( var elem in masm)
            {
                FileInfo infFile = new FileInfo(elem);
                mainFileNames.Add(infFile.Name);
            }

            if (list.FilesInfo != null)
            {
                foreach (var elem in list.FilesInfo)
                {
                    switch (elem.FileType)
                    {
                        case FileToUpload.Settings:
                            {
                                using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
                                {
                                    UserTable user = databaseContext.UserTables.FirstOrDefault(u => u.Login == list.Login);
                                    if (user.DateEditSetting == null || elem.DateChange - user.DateEditSetting.Value > TimeSpan.FromMinutes(1))
                                    {
                                        user.DateEditSetting = DateTime.Now;
                                        databaseContext.SaveChanges();
                                        elem.IsUpload = true;
                                    }
                                    else if(user.DateEditSetting.Value - elem.DateChange > TimeSpan.FromMinutes(1))
                                    {
                                        if (System.IO.File.Exists(mainPath + "\\" + elem.NameFile))
                                            elem.IsDownload = true;
                                    }
                                    if (mainFileNames.Contains(elem.NameFile))
                                        mainFileNames.Remove(elem.NameFile);
                                }
                                break;
                            }
                        case FileToUpload.Database:
                            {
                                using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
                                {
                                    UserTable user = databaseContext.UserTables.FirstOrDefault(u => u.Login == list.Login);
                                    if (user.DateEditDatabase == null || elem.DateChange - user.DateEditDatabase.Value > TimeSpan.FromMinutes(1))
                                    {
                                        user.DateEditDatabase = DateTime.Now;
                                        databaseContext.SaveChanges();
                                        elem.IsUpload = true;
                                    }
                                    else if(user.DateEditDatabase.Value - elem.DateChange > TimeSpan.FromMinutes(1))
                                    {
                                        if (System.IO.File.Exists(mainPath + "\\" + elem.NameFile))
                                            elem.IsDownload = true;
                                    }
                                    if (mainFileNames.Contains(elem.NameFile))
                                        mainFileNames.Remove(elem.NameFile);
                                }
                                break;
                            }
                        case FileToUpload.Backup:
                            {
                                if (!backupFileNames.Contains(elem.NameFile))
                                {
                                    using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
                                    {
                                        UserTable user = databaseContext.UserTables.FirstOrDefault(u => u.Login == list.Login);
                                        databaseContext.BackupTables.Add(new BackupTable { BackupName = elem.NameFile, DateOfCreate = DateTime.Now,
                                            UserId = user.UserId });
                                        databaseContext.SaveChanges();
                                        elem.IsUpload = true;
                                    }
                                }
                                else
                                    backupFileNames.Remove(elem.NameFile);
                                break;
                            }
                        case FileToUpload.Document:
                            {
                                if (!documentFileNames.Contains(elem.NameFile))
                                    elem.IsUpload = true;
                                else
                                    documentFileNames.Remove(elem.NameFile);
                                break;
                            }
                        case FileToUpload.Photo:
                            {
                                if (!photoFileNames.Contains(elem.NameFile))
                                    elem.IsUpload = true;
                                else
                                    photoFileNames.Remove(elem.NameFile);
                                break;
                            }
                    }
                }
            }

            List<FileInfoData> newList = list.FilesInfo.ToList();

            if (documentFileNames.Count > 0)
                foreach (var elem in documentFileNames)
                {
                    newList.Add(new FileInfoData
                    {
                        DateChange = DateTime.Now,
                        FileType = FileToUpload.Document,
                        IsChanged = false,
                        IsDownload = true,
                        IsUpload = false,
                        NameFile = elem,
                        PathFile = ""
                    });
                }
            if (photoFileNames.Count > 0)
                foreach (var elem in photoFileNames)
                {
                    newList.Add(new FileInfoData
                    {
                        DateChange = DateTime.Now,
                        FileType = FileToUpload.Photo,
                        IsChanged = false,
                        IsDownload = true,
                        IsUpload = false,
                        NameFile = elem,
                        PathFile = ""
                    });
                }

            if(mainFileNames.Count > 0)
                foreach(var elem in mainFileNames)
                {
                    FileToUpload fileTo;
                    if (elem == "Settings.bin")
                        fileTo = FileToUpload.Settings;
                    else
                        fileTo = FileToUpload.Database;
                    newList.Add(new FileInfoData
                    {
                        DateChange = DateTime.Now,
                        FileType = fileTo,
                        IsChanged = false,
                        IsDownload = true,
                        IsUpload = false,
                        NameFile = elem,
                        PathFile = ""
                    });
                }

            list = new ListFileInfoData(newList.ToArray());
            return Json(list);
        }

        /// <summary>
        /// Получение файла с сервера
        /// </summary>
        /// <param name="Login">Логин пользователя</param>
        /// <param name="File">Тип файла</param>
        /// <param name="Name">Имя файла</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public void GetFile(string Login, FileToUpload File, string Name)
        {
            //Формирование полного пути
            string mainPath = Request.MapPath("..//data//" + Login);
            string fullPath = "";
            switch (File)
            {
                case FileToUpload.Backup:
                    fullPath = mainPath + "\\Backups\\" + Name;
                    break;
                case FileToUpload.Database:
                    fullPath = mainPath + "\\" + Name;
                    break;
                case FileToUpload.Document:
                    fullPath = mainPath + "\\Documents\\" + Name;
                    break;
                case FileToUpload.Photo:
                    fullPath = mainPath + "\\Photos\\" + Name;
                    break;
                case FileToUpload.Settings:
                    fullPath = mainPath + "\\" + Name;
                    break;
            }
            Response.WriteFile(fullPath);
        }

        /// <summary>
        /// Загрузка файла на сервер
        /// </summary>
        /// <param name="file">Информация о файле и сам файл</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public bool PostFile([FromBody]FileData file)
        {
            byte[] sfile = Convert.FromBase64String(file.FileBytes);
            System.IO.File.WriteAllBytes(Request.MapPath("..//data//") + file.FileName, sfile);
            return true;
        }

        /// <summary>
        /// Принятие или отказ от обмена
        /// </summary>
        /// <param name="Id">Идентификатор обмена</param>
        /// <param name="Accepting">Принять обмен - 1, Отказаться - 0</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public bool ExchangeAccept(int Id, int Accepting)
        {
            using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
            {
                DatabaseExchangeTable exchange = databaseContext.DatabaseExchangeTables.Where(u => u.DatabaseExchangeId == Id).FirstOrDefault();
                if (Accepting == 0)
                    exchange.IsSenderAccept = false;
                else
                    exchange.IsSenderAccept = true;
                databaseContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// Выполнение обмена
        /// </summary>
        /// <param name="Id">Идентификатор обмена</param>
        /// <param name="Doned">У кого выполнен обмен (получатель - 0, создатель - 1)</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public bool ExchangeDone(int Id, int Doned)
        {
            using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
            {
                DatabaseExchangeTable exchange = databaseContext.DatabaseExchangeTables.Where(u => u.DatabaseExchangeId == Id).FirstOrDefault();
                if (Doned == 0)
                    exchange.IsSenderDone = false;
                else
                    exchange.IsCreatorDone = true;
                databaseContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// Создать новый обмен БД
        /// </summary>
        /// <param name="LoginSender">Логин получателя</param>
        /// <param name="LoginCreator">Логин создателя</param>
        /// <param name="IsOneWay">Односторонний или нет</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public bool ExchangeNew(string LoginSender, string LoginCreator, int IsOneWay)
        {
            using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
            {
                UserTable creator = databaseContext.UserTables.Where(u => u.Login == LoginCreator).FirstOrDefault();
                UserTable sender = databaseContext.UserTables.Where(u => u.Login == LoginSender).FirstOrDefault();
                bool isOne = false;
                if (IsOneWay == 1)
                    isOne = true;
                DatabaseExchangeTable exchange = new DatabaseExchangeTable
                {
                    IsCreatorDone = false,
                    IsOneWay = isOne,
                    IsSenderAccept = null,
                    IsSenderDone = false,
                    UserCreatorId = creator.UserId,
                    UserSenderId = sender.UserId
                };
                if (IsOneWay == 1)
                    exchange.IsSenderDone = true;

                databaseContext.DatabaseExchangeTables.Add(exchange);
                databaseContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// Получить список пользователей, доступных для обмена
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public JsonResult GetLoginsToExchange(string Login)
        {
            List<string> logins = new List<string>();
            using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
            {
                List<UserTable> userTables = databaseContext.UserTables.ToList();
                foreach (var elem in userTables)
                    if (elem.Login != Login)
                        logins.Add(elem.Login);
            }
            ListLoginsData listLoginsData = new ListLoginsData(logins.ToArray());
            return Json(listLoginsData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Получить все входящие и исходящие запросы на обмен
        /// </summary>
        /// <param name="Login">Логин пользователя</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public JsonResult GetExchangesToExchange(string Login)
        {
            List<ListExchangesData> exchangesDatas = new List<ListExchangesData>();
            using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
            {
                UserTable user = databaseContext.UserTables.Where(u => u.Login == Login).FirstOrDefault();
                List<DatabaseExchangeTable> exchanges = databaseContext.DatabaseExchangeTables.ToList();
                if(exchanges != null)
                    foreach(var elem in exchanges)
                        if((elem.UserCreatorId == user.UserId && !elem.IsCreatorDone) || (elem.UserSenderId == user.UserId && 
                            (!elem.IsSenderDone || elem.IsSenderAccept == null)))
                        {
                            ListExchangesData data = new ListExchangesData
                            {
                                ExchangeId = elem.DatabaseExchangeId,
                                IsCreatorDone = elem.IsCreatorDone,
                                IsIAmCreator = false,
                                IsOneWay = elem.IsOneWay,
                                IsSenderAccept = elem.IsSenderAccept,
                                IsSenderDone = elem.IsSenderDone,
                                LoginCreatorOrSender = ""
                            };

                            if (user.UserId == elem.UserCreatorId)
                            {
                                data.IsIAmCreator = true;
                                UserTable userTo = databaseContext.UserTables.First(u => u.UserId == elem.UserSenderId);
                                data.LoginCreatorOrSender = userTo.Login;
                            }
                            else
                            {
                                UserTable userTo = databaseContext.UserTables.First(u => u.UserId == elem.UserCreatorId);
                                data.LoginCreatorOrSender = userTo.Login;
                            }

                            exchangesDatas.Add(data);
                        }
            }
            ExchangesDataArray array = new ExchangesDataArray(exchangesDatas.ToArray());
            return Json(array, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Получить список резервных копий пользователя на сервере
        /// </summary>
        /// <param name="Login">Логин пользователя</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public JsonResult GetListBackups(string Login)
        {
            List<BackupElem> backups = new List<BackupElem>();

            using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
            {
                UserTable user = databaseContext.UserTables.Where(u => u.Login == Login).FirstOrDefault();

                List<BackupTable> backupTables = databaseContext.BackupTables.ToList();
                if (backupTables != null)
                    foreach (var elem in backupTables)
                        if (elem.UserId == user.UserId)
                            backups.Add(new BackupElem
                            {
                                DateOfCreate = (DateTime)elem.DateOfCreate,
                                DBMSName = "",
                                Name = elem.BackupName
                            });
            }
            return Json(backups.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
