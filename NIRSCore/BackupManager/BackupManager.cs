using System;
using System.IO;
using System.IO.Compression;
using NIRSCore.FileOperations;
using System.Collections.Generic;

namespace NIRSCore.BackupManager
{
    public static class BackupManager
    {
        /// <summary>
        /// Получение списка резервных копий
        /// </summary>
        /// <param name="path">Путь к директории пользователя</param>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Список описаний к резервным копиям</returns>
        public static List<BackupElem> GetListOfBackups(string path, string login)
        {
            List<BackupElem> elements = new List<BackupElem>();

            //Получение файлов
            string[] masfiles = Directory.GetFiles(path + "\\Backups\\");

            //Все файлы открываем и вытаскиваем данные
            foreach (var elem in masfiles)
            {
                //Извлечение архива
                ZipFile.ExtractToDirectory(elem, path + "\\temp\\");

                //Получение описания резервной копии
                FileBackupReadme fileBackup = new FileBackupReadme(login);
                fileBackup.Read();
                if (fileBackup.BackupElement != null)
                    elements.Add(fileBackup.BackupElement);

                //Уничтожение файлов
                string[] masTemp = Directory.GetFiles(path + "\\temp\\");
                foreach (var f in masTemp)
                {
                    FileInfo info = new FileInfo(f);
                    info.Delete();
                }
            }
            return elements;
        }

        /// <summary>
        /// Создание новой резервной копии
        /// </summary>
        /// <param name="path">Путь к директории пользователя</param>
        /// <param name="login">Логин пользователя</param>
        /// <param name="dbmsname">Название СУБД</param>
        public static void CreateBackup(string path, string login, string dbmsname)
        {
            //Создание описания
            BackupElem backupElem = new BackupElem
            {
                DateOfCreate = DateTime.Now,
                DBMSName = dbmsname,
                Name = "backup-" + DateTime.Now.Date.ToShortDateString() + ".bak"
            };

            FileBackupReadme fileBackup = new FileBackupReadme(login);
            fileBackup.BackupElement = backupElem;
            fileBackup.Write();

            //Перемещение файлов БД
            if (dbmsname == "SQLite")
                File.Copy(path + "\\database.db", path + "\\temp\\database.db");
            else
            {
                File.Copy(path + "\\database.mdf", path + "\\temp\\database.mdf");
                File.Copy(path + "\\database_log.ldf", path + "\\temp\\database_log.ldf");
            }

            //Создание архива
            ZipFile.CreateFromDirectory(path + "\\temp\\", path + "\\Backups\\backup-" + DateTime.Now.Date.ToShortDateString() + ".bak");

            //Уничтожение файлов
            string[] masTemp = Directory.GetFiles(path + "\\temp\\");
            foreach (var f in masTemp)
            {
                FileInfo info = new FileInfo(f);
                info.Delete();
            }
        }

        /// <summary>
        /// Восстановление базы данных из резервной копии
        /// </summary>
        /// <param name="path">Путь к директории пользователя</param>
        /// <param name="name">Логин пользователя</param>
        /// <param name="dbmsname">Название СУБД</param>
        public static void CreateDatabase(string path, string name, string dbmsname)
        {
            //Извлечение архива
            ZipFile.ExtractToDirectory(path + "\\Backups\\" + name, path + "\\temp\\");

            //Замена базы данных
            if (dbmsname == "SQLite")
            {
                FileInfo file = new FileInfo(path + "\\database.db");
                if (file.Exists)
                    file.Delete();
                File.Copy(path + "\\temp\\database.db", path + "\\database.db");
            }

            else
            {
                FileInfo file = new FileInfo(path + "\\database.mdf");
                FileInfo filelog = new FileInfo(path + "\\database_log.ldf");
                if (file.Exists)
                    file.Delete();
                if (filelog.Exists)
                    filelog.Delete();
                File.Copy(path + "\\temp\\database.mdf", path + "\\database.mdf");
                File.Copy(path + "\\temp\\database_log.ldf", path + "\\database_log.ldf");
            }

            //Уничтожение файлов
            string[] masTemp = Directory.GetFiles(path + "\\temp\\");
            foreach (var f in masTemp)
            {
                FileInfo info = new FileInfo(f);
                info.Delete();
            }
        }
    }
}


//Потом добавить работу с сетью
//Получение списка копий на сервере
//Получение копии с сервера