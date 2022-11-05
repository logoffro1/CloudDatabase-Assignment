using AutoMapper;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class OrderService : IOrderService
    {
        private IOrderRepository orderRepository;
        private IProductRepository productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
        }

        public async Task AddProductToOrder(Guid orderId, Guid productId)
        {
            if (!await DoesOrderExist(orderId))
                throw new Exception("Order does not exist.");
            if (!await productRepository.DoesProductExist(productId))
                throw new Exception("Product does not exist.");

            await orderRepository.AddProductToOrder(orderId, productId);
        }

        public async Task CreateOrder(CreateOrderDTO orderDTO, Guid userId)
        {
            await orderRepository.CreateOrder(orderDTO, userId);
        }

        public async Task<bool> DoesOrderExist(Guid orderId)
        {
           return await orderRepository.DoesOrderExist(orderId);
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            if (!await DoesOrderExist(orderId))
                throw new Exception("Order does not exist.");

            return await orderRepository.GetOrderById(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrderByUser(Guid userId)
        {
            return await orderRepository.GetOrderByUser(userId);
        }

        public async Task RemoveProductFromOrder(Guid orderId, Guid productId)
        {
            if (!await DoesOrderExist(orderId))
                throw new Exception("Order does not exist.");
            if (!await productRepository.DoesProductExist(productId))
                throw new Exception("Product does not exist.");

            await orderRepository.RemoveProductFromOrder(orderId, productId);
        }

        public async Task ShipOrder(Guid orderId)
        {
            if (!await DoesOrderExist(orderId))
                throw new Exception("Order does not exist.");
            await orderRepository.ShipOrder(orderId);
        }
    }
}
