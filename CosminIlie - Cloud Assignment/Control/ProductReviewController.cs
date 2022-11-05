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
using AutoMapper;
using ShowerShow.Repository.Interface;
using ShowerShow.DTO;
using ShowerShow.Models;
using System.Collections.Generic;
using ShowerShow.Model;

namespace ShowerShow.Controllers
{
    public class ProductReviewController
    {
        private readonly ILogger<ProductReviewController> _logger;
        private IProductReviewService productReviewService;

        public ProductReviewController(ILogger<ProductReviewController> log, IProductReviewService productReviewService)
        {
            _logger = log;
            this.productReviewService = productReviewService;
        }

        [Function("AddReview")]
        [OpenApiOperation(operationId: "AddReview", tags: new[] { "Reviews " }, Summary = "Create a new review", Description = "This endpoint creates a new review")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
        [OpenApiRequestBody("application/json", typeof(CreateReviewDTO), Description = "The review data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CreateReviewDTO), Description = "The OK response with the new review.")]
        public async Task<HttpResponseData> AddReview([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reviews/{productId:Guid}/add")] HttpRequestData req,Guid productId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                CreateReviewDTO reviewDTO = JsonConvert.DeserializeObject<CreateReviewDTO>(requestBody);

                await productReviewService.AddReview(reviewDTO,productId);
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("DeleteReview")]
        [OpenApiOperation(operationId: "DeleteReview", tags: new[] { "Reviews " }, Summary = "Delete review", Description = "Delete review")]
        [OpenApiParameter(name: "reviewId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CreateReviewDTO), Description = "The OK response with the new review.")]
        public async Task<HttpResponseData> DeleteReview([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "reviews/{reviewId:Guid}")] HttpRequestData req, Guid reviewId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {

                await productReviewService.DeleteReview(reviewId);
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("GetReviewById")]
        [OpenApiOperation(operationId: "GetReviewById", tags: new[] { "Reviews " }, Summary = "Get review by id", Description = "Get review by id")]
        [OpenApiParameter(name: "reviewId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The review id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ProductReview), Description = "The OK response with the review.")]
        public async Task<HttpResponseData> GetReviewById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "reviews/{reviewId:Guid}")] HttpRequestData req,Guid reviewId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

               ProductReview productReview = await productReviewService.GetReviewById(reviewId);
                await responseData.WriteAsJsonAsync(productReview);
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("GetReviewsForProduct")]
        [OpenApiOperation(operationId: "GetReviewsForProduct", tags: new[] { "Reviews " }, Summary = "Get reviews for product", Description = "Get reviews for product")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ProductReview), Description = "The OK response with the review.")]
        public async Task<HttpResponseData> GetReviewsForProduct([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "reviews/{productId:Guid}/all")] HttpRequestData req, Guid productId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {

                List<ProductReview> reviews = (List<ProductReview>)await productReviewService.GetReviewsForProduct(productId);
                await responseData.WriteAsJsonAsync(reviews);
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

