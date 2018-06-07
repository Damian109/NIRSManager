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
                UserTable user = databaseContext.Users.FirstOrDefault(u => u.Login == data.Login && u.Md5 == data.Md5);
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
                                    UserTable user = databaseContext.Users.FirstOrDefault(u => u.Login == list.Login);
                                    if (user.DateEditSetting == null || user.DateEditSetting < elem.DateChange)
                                    {
                                        user.DateEditSetting = elem.DateChange;
                                        databaseContext.SaveChanges();
                                        elem.IsUpload = true;
                                    }
                                    else
                                    {
                                        if (System.IO.File.Exists(mainPath + "\\" + elem))
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
                                    UserTable user = databaseContext.Users.FirstOrDefault(u => u.Login == list.Login);
                                    if (user.DateEditDatabase == null || user.DateEditDatabase < elem.DateChange)
                                    {
                                        user.DateEditDatabase = elem.DateChange;
                                        databaseContext.SaveChanges();
                                        elem.IsUpload = true;
                                    }
                                    else
                                    {
                                        if (System.IO.File.Exists(mainPath + "\\" + elem))
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
                                        UserTable user = databaseContext.Users.FirstOrDefault(u => u.Login == list.Login);
                                        databaseContext.Backups.Add(new BackupTable { BackupName = elem.NameFile, DateOfCreate = DateTime.Now,
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
        public bool GetFile(string Login, FileToUpload File, string Name)
        {
            //Формирование полного пути
            string mainPath = Request.MapPath("..//data//" + Login);
            string fullPath = "";
            switch (File)
            {
                case FileToUpload.Backup:
                    {
                        fullPath = mainPath + "\\Backups\\" + Name;
                        using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
                        {
                            UserTable user = databaseContext.Users.FirstOrDefault(u => u.Login == Login);
                            databaseContext.Backups.Add(new BackupTable { BackupName = Name, DateOfCreate = DateTime.Now, UserId = user.UserId });
                            databaseContext.SaveChanges();
                        }
                        break;
                    }                    
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
            return true;
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
    }
}
