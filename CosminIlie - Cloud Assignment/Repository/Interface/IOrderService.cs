﻿using ShowerShow.DTO;
using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IOrderService
    {
        public Task CreateOrder(CreateOrderDTO orderDTO, Guid userId);
        public Task AddProductToOrder(Guid orderId, Guid productId);
        public Task RemoveProductFromOrder(Guid orderId, Guid productId);
        public Task ShipOrder(Guid orderId);
        public Task<Order> GetOrderById(Guid orderId);
        public Task<IEnumerable<Order>> GetOrderByUser(Guid userId);
        public Task<bool> DoesOrderExist(Guid orderId);


    }
}