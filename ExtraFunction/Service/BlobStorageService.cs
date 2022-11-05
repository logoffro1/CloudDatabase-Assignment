using Microsoft.Azure.Functions.Worker.Http;
using ExtraFunction.Repository.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ExtraFunction.Service
{
    public class BlobStorageService : IBlobStorageService
    {
        private IBlobStorageRepository blobStorageRepository;

        public BlobStorageService(IBlobStorageRepository blobStorageRepository)
        {
            this.blobStorageRepository = blobStorageRepository;
        }

        public async Task DeleteProductPicture(Guid productId)
        {
            await blobStorageRepository.DeleteProductPicture(productId);
        }

        public async Task<HttpResponseData> GetProductPicture(HttpResponseData response, Guid productId)
        {
            return await blobStorageRepository.GetProductPicture(response, productId);
        }

     

        public async Task UploadProductPicture(Stream requestBody, Guid productId)
        {

            await blobStorageRepository.UploadProductPicture(requestBody,productId);
        }

    }
}
