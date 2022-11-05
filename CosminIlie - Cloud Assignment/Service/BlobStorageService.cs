using Microsoft.Azure.Functions.Worker.Http;
using ShowerShow.Repository.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class BlobStorageService : IBlobStorageService
    {
        private IBlobStorageRepository blobStorageRepository;
        private IProductRepository productRepository;

        public BlobStorageService(IBlobStorageRepository blobStorageRepository, IProductRepository productRepository)
        {
            this.blobStorageRepository = blobStorageRepository;
            this.productRepository = productRepository;
        }

        public async Task DeleteProductPicture(Guid productId)
        {
            if (!await productRepository.DoesProductExist(productId))
                throw new Exception("Product does not exist");

            await blobStorageRepository.DeleteProductPicture(productId);
        }

        public async Task<HttpResponseData> GetProductPicture(HttpResponseData response, Guid productId)
        {
            if (!await productRepository.DoesProductExist(productId))
                throw new Exception("Product does not exist");

            return await blobStorageRepository.GetProductPicture(response, productId);
        }

     

        public async Task UploadProductPicture(Stream requestBody, Guid productId)
        {
            if (!await productRepository.DoesProductExist(productId))
                throw new Exception("Product does not exist");

            await blobStorageRepository.UploadProductPicture(requestBody,productId);
        }

    }
}
