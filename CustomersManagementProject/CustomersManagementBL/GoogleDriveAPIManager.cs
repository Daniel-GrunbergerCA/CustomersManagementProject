using CustomersManagementBL;
using CustomersManagementDP;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using ZXing;

namespace CustomersManagementBL
{
    public class GoogleDriveAPIManager
    {

        public GoogleDriveAPIManager(IBL bl)
        {
            this.ibl = bl;
        }

        IBL ibl;
        // permission level: read, edit and delete the files in the Google drive:
        static string[] Scopes = { @"https://www.googleapis.com/auth/drive" };
        public void QuickStart()
        {
            var service = GetService();

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 100;
            listRequest.Fields = "*";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            string id = "";
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Name == "items")
                    {
                        id = file.Id;
                    }
                }
                foreach (var file in files)
                {
                    if (file.Parents != null)
                    {
                        if (file.Parents.Contains(id))
                        {
                            DownloadFile(service, file);
                        }
                    }
                }
                foreach (var file in files)
                {
                    if (file.Parents != null)
                    {
                        if (file.Parents.Contains(id))
                        {
                            DeleteFile(service, file.Id);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            Console.WriteLine("Process complete");
        }


        //create Drive API service.
        public DriveService GetService()
        {
            //get Credentials from client_secret.json file 
            UserCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            //create Drive API service.
            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleDriveRestAPI-v3",
            });
            return service;
        }

        // Download file from Google drive
        private void DownloadFile(Google.Apis.Drive.v3.DriveService service, Google.Apis.Drive.v3.Data.File file)//, string saveTo)
        {

            var request = service.Files.Get(file.Id);
            var stream = new System.IO.MemoryStream();

            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            QRscan(stream , file);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream);
        }

        //Delete file from the Google drive
        public void DeleteFile(DriveService service, String fileId)
        {
            try
            {
                service.Files.Delete(fileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }

        private void QRscan(System.IO.MemoryStream stream, Google.Apis.Drive.v3.Data.File file)
        {
            BarcodeReader reader = new BarcodeReader();
            reader.AutoRotate = true;
            reader.Options.TryHarder = true;
            reader.Options.PureBarcode = false;
            reader.Options.PossibleFormats = new List<BarcodeFormat>();
            reader.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);

            try
            {
                var res = Image.FromStream(stream);
                var result = reader.Decode((Bitmap)res);
                if (result != null)
                {
                    ibl.AddItem(CreateItem(result.ToString(), file.CreatedTime));
                }
                else
                    Console.WriteLine("failed to scan");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Item CreateItem(string json, DateTime? createdTime)
        {
            Item item = JsonConvert.DeserializeObject<Item>(json);
            /* Probably should be later..*/
            //if (createdTime != null)
            //    item.Date_of_purchase = (DateTime)createdTime;
            return item;
        }

    }
}
