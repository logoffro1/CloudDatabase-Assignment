using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System;
using ShowerShow.Repository.Interface;
using ShowerShow.DTO;
using ShowerShow.Model;

namespace ShowerShow.Controllers
{
    public class ProductController
    {
        private readonly ILogger<ProductController> _logger;
        private IProductService productService;

        public ProductController(ILogger<ProductController> log, IProductService productService)
        {
            _logger = log;
            this.productService = productService;
        }

        [Function("CreateProduct")]
        [OpenApiOperation(operationId: "CreateProduct", tags: new[] { "Products " }, Summary = "Create a new product", Description = "This endpoint creates a new product")]
        [OpenApiRequestBody("application/json", typeof(ProductDTO), Description = "The product data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(CreateOrderDTO), Description = "The OK response with the new product.")]
        public async Task<HttpResponseData> CreateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products/create")] HttpRequestData req)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
                await productService.CreateProduct(productDTO);
                responseData.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("GetProductById")]
        [OpenApiOperation(operationId: "GetProductById", tags: new[] { "Products " }, Summary = "Get product by id", Description = "Get product by id")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product), Description = "The OK response with the new product.")]
        public async Task<HttpResponseData> GetProductById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{productId:Guid}")] HttpRequestData req,Guid productId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

               Product product = await productService.GetProductById(productId);
                await responseData.WriteAsJsonAsync(product);
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("RemoveProduct")]
        [OpenApiOperation(operationId: "RemoveProduct", tags: new[] { "Products " }, Summary = "Remove product by id", Description = "Remove product by id")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product), Description = "The OK response with the deleted product")]
        public async Task<HttpResponseData> RemoveProduct([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "products/{productId:Guid}")] HttpRequestData req, Guid productId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                await productService.RemoveProduct(productId);
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("UpdateProduct")]
        [OpenApiOperation(operationId: "UpdateProduct", tags: new[] { "Products " }, Summary = "Update product by id", Description = "Update product by id")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
        [OpenApiRequestBody("application/json", typeof(ProductDTO), Description = "The product data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product), Description = "The OK response with the updated product")]
        public async Task<HttpResponseData> UpdateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products/{productId:Guid}")] HttpRequestData req, Guid productId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);

                await productService.UpdateProduct(productId, productDTO);
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
    }
}

