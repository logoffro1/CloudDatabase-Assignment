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
using System.Collections.Generic;
using ShowerShow.Model;

namespace ShowerShow.Controllers
{
    public class OrderController
    {
        private readonly ILogger<OrderController> _logger;
        private IOrderService orderService;

        public OrderController(ILogger<OrderController> log, IOrderService orderService)
        {
            _logger = log;
            this.orderService = orderService;
        }

        [Function("CreateOrder")]
        [OpenApiOperation(operationId: "CreateOrder", tags: new[] { "Orders " }, Summary = "Create a new order", Description = "This endpoint creates a new order")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiRequestBody("application/json", typeof(CreateOrderDTO), Description = "The order data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(CreateOrderDTO), Description = "The OK response with the new order.")]
        public async Task<HttpResponseData> CreateOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders/{userId:Guid}/create")] HttpRequestData req, Guid userId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                CreateOrderDTO orderDTO = JsonConvert.DeserializeObject<CreateOrderDTO>(requestBody);
                await orderService.CreateOrder(orderDTO, userId);
                responseData.StatusCode = HttpStatusCode.Created;
                responseData.Headers.Add("Result", "Order has been created");
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("AddProductToOrder")]
        [OpenApiOperation(operationId: "AddProductToOrder", tags: new[] { "Orders " }, Summary = "Add a product to order")]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The order id  parameter")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "The OK response with the added product")]
        public async Task<HttpResponseData> AddProductToOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders/{orderId:Guid}/add/{productId:Guid}")] HttpRequestData req, Guid orderId, Guid productId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                await orderService.AddProductToOrder(orderId,productId);
                responseData.StatusCode = HttpStatusCode.OK;
                responseData.Headers.Add("Result", "Order has been created");
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("RemoveProductFromOrder")]
        [OpenApiOperation(operationId: "RemoveProductFromOrder", tags: new[] { "Orders " }, Summary = "Remove product from order", Description = "Remove product from order")]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The order id  parameter")]
        [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "The OK response with the removed product")]
        public async Task<HttpResponseData> RemoveProductFromOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders/{orderId:Guid}/remove/{productId:Guid}")] HttpRequestData req, Guid orderId, Guid productId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {

                await orderService.RemoveProductFromOrder(orderId, productId);
                responseData.StatusCode = HttpStatusCode.OK;
                responseData.Headers.Add("Result", "Order has been created");
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("GetOrderById")]
        [OpenApiOperation(operationId: "GetOrderById", tags: new[] { "Orders " }, Summary = "Get order by id", Description = "Get order by id")]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The order id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "The OK response with the new order.")]
        public async Task<HttpResponseData> GetOrderById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders/{orderId:Guid}")] HttpRequestData req, Guid orderId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                Order order = await orderService.GetOrderById(orderId);
                await responseData.WriteAsJsonAsync(order);
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("GetOrderByUser")]
        [OpenApiOperation(operationId: "GetOrderByUser", tags: new[] { "Orders " }, Summary = "Get order by user id", Description = "Get order by user id")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "The OK response with the order.")]
        public async Task<HttpResponseData> GetOrderByUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders/{userId:Guid}/all")] HttpRequestData req, Guid userId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                List<Order> orders = (List<Order>)await orderService.GetOrderByUser(userId);
                await responseData.WriteAsJsonAsync(orders);
                responseData.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function("ShipOrder")]
        [OpenApiOperation(operationId: "ShipOrder", tags: new[] { "Orders " }, Summary = "ShipOrder", Description = "ShipOrder")]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The order id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "The OK response with the new order.")]
        public async Task<HttpResponseData> ShipOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders/{orderId:Guid}/ship")] HttpRequestData req, Guid orderId)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                await orderService.ShipOrder(orderId);
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

