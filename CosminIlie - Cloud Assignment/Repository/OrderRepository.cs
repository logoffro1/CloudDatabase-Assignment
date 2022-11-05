using AutoMapper;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private DatabaseContext dbContext;
        private IProductRepository productRepository;


        public OrderRepository(DatabaseContext dbContext, IProductRepository productRepository)
        {
            this.dbContext = dbContext;
            this.productRepository = productRepository;
        }

        public async Task AddProductToOrder(Guid orderId, Guid productId)
        {
            Order order = null;
            Product product = null;

            //this is to give priority to tasks
            Task getId = Task.Run(async () =>
            {
                order = GetOrderById(orderId).Result;

                product = productRepository.GetProductById(productId).Result;
                product.ItemStock -= 1;
            });
            await getId.ContinueWith(prev =>
            {
                if (order.IsShipped) throw new Exception("Order already shipped");
                dbContext.Products.Update(product);
                if (order.orderItems.ContainsKey(productId.ToString()))
                {
                    foreach (KeyValuePair<string, int> kvp in order.orderItems)
                    {
                        if (kvp.Key == productId.ToString())
                        {
                            order.orderItems[kvp.Key] = kvp.Value + 1;
                            if (order.orderItems[kvp.Key] > product.ItemStock)
                                throw new Exception("Not enough stock");
                            break;
                        }
                    }
                }
                else
                {
                    order.orderItems.Add(productId.ToString(), 1);
                }
                dbContext.Orders.Update(order);
            });
            await getId.ContinueWith(prev =>
            {
                dbContext.SaveChangesAsync();
            });
        }

        public async Task CreateOrder(CreateOrderDTO orderDTO, Guid userId)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateOrderDTO, Order>()));
            Order order = mapper.Map<Order>(orderDTO);
            order.ShippedDate = null;
            order.UserId = userId;
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DoesOrderExist(Guid orderId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.Orders.FirstOrDefault(c => c.Id == orderId) != null;
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            await dbContext.SaveChangesAsync();

            return dbContext.Orders.FirstOrDefault(c => c.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrderByUser(Guid userId)
        {
            await dbContext.SaveChangesAsync();

            return dbContext.Orders.Where(c => c.UserId == userId).ToList();
        }

        public async Task RemoveProductFromOrder(Guid orderId, Guid productId)
        {
            Order order = null;
            Product product = null;

            //this is to give priority to tasks
            Task getId = Task.Run(async () =>
            {
                order = GetOrderById(orderId).Result;
                product = productRepository.GetProductById(productId).Result;
                product.ItemStock += 1;
            });
            await getId.ContinueWith(prev =>
            {
                if (order.IsShipped) throw new Exception("Order already shipped");
                dbContext.Products.Update(product);
                if (order.orderItems.ContainsKey(productId.ToString()))
                {
                    foreach (KeyValuePair<string, int> kvp in order.orderItems)
                    {
                        if (kvp.Key == productId.ToString())
                        {
                            order.orderItems[kvp.Key] = kvp.Value - 1;
                            if (order.orderItems[kvp.Key] < 0)
                                throw new Exception("Cannot have less than 0 products");
                            break;
                        }
                    }
                }
                else
                {
                    throw new Exception("Cannot remove product");
                }
                dbContext.Orders.Update(order);
            });
            await getId.ContinueWith(prev =>
            {
                dbContext.SaveChangesAsync();
            });
        }

        public async Task ShipOrder(Guid orderId)
        {
            Order order = null;

            //this is to give priority to tasks
            Task getId = Task.Run(async () =>
            {
                order = GetOrderById(orderId).Result;
            });
            await getId.ContinueWith(prev =>
            {
                if (order.IsShipped) throw new Exception("Order already shipped");

                order.IsShipped = true;
                order.ShippedDate = DateTime.Now;
                dbContext.Orders.Update(order);
            });
            await dbContext.SaveChangesAsync();
        }
    }
}
