using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;
using System;
using ShowerShow.Repository.Interface;

namespace ShowerShow.Controllers
{
    public class BlobStorageController
    {
        private readonly ILogger<BlobStorageController> _logger;
        private IBlobStorageService blobStorageService;

        public BlobStorageController(ILogger<BlobStorageController> log, IBlobStorageService blobStorageService)
        {
            _logger = log;
            this.blobStorageService = blobStorageService;
        }


        [Function("UploadProductPicture")] // USE POSTMAN TO TEST
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The productId parameter")]
        [OpenApiOperation(operationId: "UploadProductPicture", tags: new[] { "Blob Storage" })]
        public async Task<HttpResponseData> UploadProductPicture([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products/{productId:Guid}/pictures/uploadpic")] HttpRequestData req, Guid productId)
        {
            _logger.LogInformation("Uploading to blob.");

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                await blobStorageService.UploadProductPicture(req.Body, productId);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("GetProductPicture")] // USE POSTMAN TO TEST
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id  parameter")]
        [OpenApiOperation(operationId: "GetProductPicture", tags: new[] { "Blob Storage" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/jpeg", bodyType: typeof(byte[]), Description = "The OK response with the product picture.")]
        public async Task<HttpResponseData> GetProductPicture([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{productId:Guid}/pictures/getpic")] HttpRequestData req, Guid productId)
        {
            _logger.LogInformation("Uploading to blob.");

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                responseData = await blobStorageService.GetProductPicture(responseData, productId);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("DeleteProductPicture")]
        [OpenApiOperation(operationId: "DeleteProductPicture", tags: new[] { "Blob Storage" })]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The productId parameter")]
        public async Task<HttpResponseData> DeleteProductPicture([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "products/{productId:Guid}/pictures/delete")] HttpRequestData req, Guid productId)
        {
            _logger.LogInformation("Deleting user profile picture.");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                await blobStorageService.DeleteProductPicture(productId);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
    }

}