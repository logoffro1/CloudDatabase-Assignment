using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Threading.Tasks;
using System.IO;

namespace ExtraFunction.Repository.Interface
{
    public interface IBlobStorageService
    {
        public Task UploadProductPicture(Stream requestBody, Guid productId);
        public Task DeleteProductPicture(Guid productId);
        public Task<HttpResponseData> GetProductPicture(HttpResponseData response, Guid productId);

    }
}
