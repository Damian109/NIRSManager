using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using NIRSCore.Syncronization;

namespace NIRSCore.BackupManager
{
    public sealed class BackupManager
    {/*
        public async void GetFileBackupFromServerAsync(LoginData loginData, string name, string adress) => await Task.Run(async () =>
        {
            using (var client = new HttpClient())
            {
                //Формирование строки запроса
                string query

                ///HttpResponseMessage message = client.GetAsync(adress + "Sync/GetFile?LoginData=" + login).Result;
                string path = Environment.CurrentDirectory + "\\data\\" + NirsSystem.GetLogin() + "\\temp\\";
                using (FileStream file = new FileStream(path + name, FileMode.Create))
                {
                    await message.Content.CopyToAsync(file);
                }
            }
        });*/
    }
}
