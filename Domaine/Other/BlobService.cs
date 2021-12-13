using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine.Other
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient) {
            _blobServiceClient = blobServiceClient;
        
        }
        public async Task UploadFileBlobAsync(string filePath, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("imagecontainer");
            var blobClient = containerClient.GetBlobClient(fileName);
            var rep = await blobClient.UploadAsync(filePath, new BlobHttpHeaders { ContentType = filePath.GetContentType() });
            
        }



    }
}
