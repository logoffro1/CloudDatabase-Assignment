using AutoMapper;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Repository;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class ProductReviewService : IProductReviewService
    {
        private IProductReviewRepository reviewRepository;
        private IProductRepository productRepository;

        public ProductReviewService(IProductReviewRepository reviewRepository, IProductRepository productRepository)
        {
            this.reviewRepository = reviewRepository;
            this.productRepository = productRepository;
        }

        public async Task AddReview(CreateReviewDTO createReviewDTO, Guid productId)
        {
            if (!await productRepository.DoesProductExist(productId))
                throw new Exception("Product does not exist.");

            await reviewRepository.AddReview(createReviewDTO, productId);
        }

        public async Task DeleteReview(Guid id)
        {
            if (!await DoesReviewExist(id))
                throw new Exception("Review does not exist.");

            await reviewRepository.DeleteReview(id);
        }

        public async Task<bool> DoesReviewExist(Guid reviewId)
        {
          return await reviewRepository.DoesReviewExist(reviewId);
        }

        public async Task<ProductReview> GetReviewById(Guid reviewId)
        {
            if (!await DoesReviewExist(reviewId))
                throw new Exception("Review does not exist.");

            return await reviewRepository.GetReviewById(reviewId);
        }

        public async Task<IEnumerable<ProductReview>> GetReviewsForProduct(Guid productId)
        {
            if (!await productRepository.DoesProductExist(productId))
                throw new Exception("Product does not exist.");

            return await reviewRepository.GetReviewsForProduct(productId);
        }
    }
}
