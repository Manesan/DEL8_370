using Azure.Storage.Blobs;
using BlackGoldProperties_API.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace BlackGoldProperties_API.Controllers
{
    public class DocumentController
    {
        //Azure storage connection string
        protected static string AzureStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=blackgoldprojectstorage;AccountKey=l9UAbpyHyLK4NDpHtL0cVTegDlUg/x9wA1yG48vGUgtygCbiBs1EuBNwHy8oIqMLUeurLUVYDtWxwr7dySiB2w==;EndpointSuffix=core.windows.net";

        //Azure storage containers
        public class Containers
        {
            public static string rentalApplicationDocumentsContainer = "rental-application-documents";
            public static string saleAgreementDocumentsContainer = "sale-agreement-documents";
            public static string clientDocumentsContainer = "client-documents";
            public static string propertyDocumentsContainer = "property-documents";
            public static string rentalDocumentsContainer = "rental-documents";
            public static string inspectionDocumentsContainer = "inspection-documents";
            public static string valuationDocumentsContainer = "valuation-documents";
            public static string auditReportDocumentsContainer = "audit-report-documents";
            public static string mandateDocumentsContainer = "mandate-documents";
            public static string listingPicturesContainer = "listing-pictures";
            public static string rentalApplicationDocumentsContainerUri = "https://blackgoldprojectstorage.blob.core.windows.net/rental-application-documents/"; //Used in deleting a file from filestorage, update if Azure storage changes locations
        }
        public class UploadClass
        {
            public string FileBase64 { get; set; }
            public string FileExtension { get; set; }
        }

        /* This commented portion relates to Azure file storage
        //Upload file
        public static dynamic UploadFile(string container, UploadClass file)
        {
            try
            {
                
                var data = Convert.FromBase64String(file.FileBase64);
                //Create a stream
                var memoryStream = new MemoryStream(data);
                //Get a random file name
                var filename = Utilities.RandomString(8);
                filename += $".{file.FileExtension}";
                BlobServiceClient blobServiceClient = new BlobServiceClient(AzureStorageConnectionString);
                var blobContainer = blobServiceClient.GetBlobContainerClient(container);
                var blobClient = blobContainer.GetBlobClient(filename);
                blobClient.Upload(memoryStream);
                memoryStream.Close();
                string fileUri = blobClient.Uri.ToString();
                return (fileUri);
            }
            catch
            {
                return null;
            }
        }

        //Delete file
        public static bool DeleteFile(string container, string fileUri)
        {
            try
            {
                fileUri.Remove(0, Containers.rentalApplicationDocumentsContainerUri.Length);
                BlobServiceClient blobServiceClient = new BlobServiceClient(AzureStorageConnectionString);
                var blobContainer = blobServiceClient.GetBlobContainerClient(container);
                var blobClient = blobContainer.GetBlobClient(fileUri);
                blobClient.DeleteIfExists();
                return (true);
            }
            catch
            {
                return (false);
            }

        }*/

        // This section below relates to local file storage
        //Upload file
        public static dynamic UploadFile(string container, UploadClass file)
        {
            try
            {
                var data = Convert.FromBase64String(file.FileBase64);
                //Create a stream
                var memoryStream = new MemoryStream(data);
                //Get a random file name
                var filename = Utilities.RandomString(8);
                filename += $".{file.FileExtension}";
                var fileStream = File.Create(HttpRuntime.AppDomainAppPath + "/" + container + filename);
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(fileStream);
                fileStream.Close();
                var filePath = HttpRuntime.AppDomainAppPath + "/" + container + filename;
                return (filePath);
            }
            catch
            {
                return null;
            }
        }

        //Delete file
        public static bool DeleteFile(string container, string fileUri)
        {
            try
            {
                File.Delete(fileUri);
                return (true);
            }
            catch
            {
                return (false);
            }

        }


        //DOwnload File
        public static string DownloadFile(string filepath)
        {
                
                Byte[] bytes = File.ReadAllBytes(filepath);
                var newFile = Convert.ToBase64String(bytes);
                return newFile;
        }

    }

}