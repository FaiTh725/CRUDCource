using Application.Contracts.Response;
using Product.Domain.Contracts.Models;

namespace Product.Services.Interfaces
{
    public interface IBlobService
    {
        Task UploadBlob(string nameFile, Stream stream, string contentType, CancellationToken cancellationToken = default);

        Task<string> DownLoadBlob(string name, CancellationToken cancellationToken = default);

        Task DeleteBlob(string name, CancellationToken cancellationToken = default);

        Task<List<string>> DownLoadBlobFolder(string folderName, CancellationToken cancellationToken = default);

        Task DeleteBlobFolder(string name, CancellationToken cancellationToken = default);

        Task<List<string>> UploadBlob(string nameFolder, IFormFileCollection files, CancellationToken cancellationToken = default);
    }
}
