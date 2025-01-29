using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
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

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobContainerClient.Name,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMonths(1)
            };

            var uriFile = blobClient.GenerateSasUri(sasBuilder);

            return uriFile.ToString();
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

                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = blobContainerClient.Name,
                    BlobName = blobItem.Name,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.AddMonths(1)
                };
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                urls.Add(blobClient.GenerateSasUri(sasBuilder).ToString());
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
