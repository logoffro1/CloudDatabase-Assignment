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
    public class ProductReviewRepository : IProductReviewRepository
    {
        private DatabaseContext dbContext;


        public ProductReviewRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddReview(CreateReviewDTO createReviewDTO, Guid productId)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateReviewDTO, ProductReview>()));
            ProductReview productReview = mapper.Map<ProductReview>(createReviewDTO);
            productReview.ProductId = productId;
            await dbContext.ProductReviews.AddAsync(productReview);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteReview(Guid id)
        {
            ProductReview review = await GetReviewById(id);
            dbContext.ProductReviews.Remove(review);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DoesReviewExist(Guid reviewId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.ProductReviews.FirstOrDefault(c => c.Id == reviewId) != null;
        }

        public async Task<ProductReview> GetReviewById(Guid reviewId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.ProductReviews.FirstOrDefault(c => c.Id == reviewId);
        }

        public async Task<IEnumerable<ProductReview>> GetReviewsForProduct(Guid productId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.ProductReviews.Where(c => c.ProductId == productId).ToList();
        }
    }
}
