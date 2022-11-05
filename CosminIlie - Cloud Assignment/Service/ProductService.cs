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
    public class ProductService : IProductService
    {
        private IProductRepository productRepository;
        private IUserRepository userRepository;

        public ProductService(IProductRepository productRepository, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.productRepository = productRepository; 
        }

        public async Task CreateProduct(ProductDTO productDTO)
        {
            await productRepository.CreateProduct(productDTO);
        }

        public async Task<bool> DoesProductExist(Guid productId)
        {
           return await productRepository.DoesProductExist(productId);
        }

        public async Task<Product> GetProductById(Guid productId)
        {
            if (!await DoesProductExist(productId))
                throw new Exception("Product does not exist.");

            return await productRepository.GetProductById(productId);
        }

        public async Task RemoveProduct(Guid productId)
        {
            if (!await DoesProductExist(productId))
                throw new Exception("Product does not exist.");

            await productRepository.RemoveProduct(productId);
        }

        public async Task UpdateProduct(Guid productId, ProductDTO updateProductDTO)
        {
            if (!await DoesProductExist(productId))
                throw new Exception("Product does not exist.");

            await productRepository.UpdateProduct(productId, updateProductDTO);
        }
    }
}
