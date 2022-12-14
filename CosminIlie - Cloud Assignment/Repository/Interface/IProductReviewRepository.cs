using ShowerShow.DTO;
using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IProductReviewRepository
    {
        public Task AddReview(CreateReviewDTO createReviewDTO, Guid productId);
        public Task DeleteReview(Guid id);
        public Task<ProductReview> GetReviewById(Guid reviewId);
        public Task<IEnumerable<ProductReview>> GetReviewsForProduct(Guid productId);
        public Task<bool> DoesReviewExist(Guid reviewId);



    }
}
