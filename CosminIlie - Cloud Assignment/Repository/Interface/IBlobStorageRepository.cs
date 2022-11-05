using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Threading.Tasks;
using System.IO;

namespace ShowerShow.Repository.Interface
{
    public interface IBlobStorageRepository
    {
        public Task UploadProductPicture(Stream requestBody, Guid productId);
        public Task DeleteProductPicture(Guid productId);
        public Task<HttpResponseData> GetProductPicture(HttpResponseData response, Guid productId);

    }
}
