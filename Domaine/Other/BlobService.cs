using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine.Other
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient) {
            _blobServiceClient = blobServiceClient;
        
        }
        public async Task UploadFileBlobAsync(string filePath, string fileName, string nameOfContainer)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(nameOfContainer);
            var blobClient = containerClient.GetBlobClient(fileName);
            var rep = await blobClient.UploadAsync(filePath, new BlobHttpHeaders { ContentType = filePath.GetContentType() });
            
        }

        public async Task UploadContentBlobAsync(string content, string fileName, string nameOfContainer) {
            
                var containerClient = _blobServiceClient.GetBlobContainerClient(nameOfContainer);
                var blobClient = containerClient.GetBlobClient(fileName);
                var bytes = Convert.FromBase64String(content);

                await using var memoryStream = new MemoryStream(bytes);
                await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = fileName.GetContentType() });
        }

    }
}
