using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Product.Domain.Contracts.Models;
using Product.Services.Interfaces;

namespace Product.Services.Implementations
{
    public class BlobService : IBlobService
    {
        private const string ImageContainer = "images";
        private readonly BlobServiceClient blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public async Task DeleteBlob(string name, CancellationToken cancellationToken = default)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(ImageContainer);
            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient(name);

            await blobClient.DeleteIfExistsAsync();
        }

        public async Task DeleteBlobFolder(string name, CancellationToken cancellationToken = default)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(ImageContainer);

            var blobDirecroty = blobContainerClient.GetBlobClient(name);

            await blobContainerClient.DeleteIfExistsAsync();
        }

        public async Task<string> DownLoadBlob(string name, CancellationToken cancellationToken = default)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(ImageContainer);
            await blobContainerClient.CreateIfNotExistsAsync();
            await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);
            
            var blobClient = blobContainerClient.GetBlobClient(name);
            
            var uriFile = blobClient.Uri.ToString();

            return uriFile;
        }

        public async Task<List<string>> DownLoadBlobFolder(string folderName, CancellationToken cancellationToken = default)
        {
            var urls = new List<string>();

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(ImageContainer);
            await blobContainerClient.CreateIfNotExistsAsync();
            await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            var blobItems = blobContainerClient.GetBlobsAsync(prefix: folderName);

            await foreach (var blobItem in blobItems)
            {
                var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);

                urls.Add(blobClient.Uri.ToString());
            }

            return urls;
        }

        public async Task UploadBlob(string nameFile, Stream stream, string contentType, CancellationToken cancellationToken = default)
        {

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(ImageContainer);
            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient(nameFile);

            await blobClient.UploadAsync(
                stream, 
                new BlobHttpHeaders { ContentType = contentType }, 
                cancellationToken: cancellationToken);
        
            
        }

        public async Task<List<string>> UploadBlob(string nameFolder, IFormFileCollection files, CancellationToken cancellationToken = default)
        {
            var uploadUrls = new List<string>();

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(ImageContainer);

            foreach (var file in files)
            {
                var path = nameFolder + "/" + Guid.NewGuid().ToString() + file.FileName;

                var blobClient = blobContainerClient.GetBlobClient(path);

                using var fileStream = file.OpenReadStream();
                 
                var resultUpload = await blobClient.UploadAsync(
                    fileStream,
                    new BlobHttpHeaders { ContentType = file.ContentType});
                
                uploadUrls.Add(blobClient.Uri.ToString());
            }

            return uploadUrls;
        }
    }
}
