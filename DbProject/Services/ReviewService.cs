using DbProject.Entities;
using DbProject.Repositories;
using System.Linq.Expressions;

namespace DbProject.Services;

public class ReviewService
{
    private readonly ReviewRepository _reviewRepository;

    public ReviewService(ReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    /// <summary>
    ///     Creates a new ReviewEntity in database
    /// </summary>
    /// <param name="entity">The review entity containing data for the new review</param>
    /// <returns>The new ReviewEntity, else null</returns>
    public Review CreateReview(Review entity)
    {
        try
        {
            var reviewEntity = new Review
            {
                ReviewText = entity.ReviewText,
                ReviewDate = DateTime.Now,
                ProductId = entity.ProductId
            };
            var createReview = _reviewRepository.Create(entity);
            if(createReview != null)
            {
                return createReview;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets all Reviews from database
    /// </summary>
    /// <returns>A collection of ReviewEntity objects, else null</returns>
    public IEnumerable<Review> GetAllReviews()
    {
        try
        {
            var reviews = _reviewRepository.GetAll();
            if(reviews != null)
            {
                var reviewList = new HashSet<Review>();
                foreach(var review in reviews)
                {
                    reviewList.Add(review);
                }
                
                return reviewList;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets one Review from database based on provided predicate/expression
    /// </summary>
    /// <param name="expression">The predicate/expression used to filter ReviewEntity objects</param>
    /// <returns>The ReviewEntity that matches the predicate/expression</returns>
    public Review GetOneReview(Expression<Func<Review, bool>> expression)
    {
        try
        {
            var reviewEntity = _reviewRepository.GetOne(expression);
            if(reviewEntity != null)
            {
                return reviewEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Updates an existing review
    /// </summary>
    /// <param name="reviewEntity">The reviewEntity containing the updated information</param>
    /// <returns>The updated ReviewEntity, else null</returns>
    public Review UpdateReview(Review reviewEntity)
    {
        try
        {
            var updatedReviewEntity = _reviewRepository.Update(x => x.Id == reviewEntity.Id, reviewEntity);
            if(updatedReviewEntity != null)
            {
                return updatedReviewEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Deletes a ReviewEntity based on provided predicate/expression
    /// </summary>
    /// <param name="expression">The predicate/expression used to filter ReviewEntity objects</param>
    /// <returns>True if deleted successfully, else false</returns>
    public bool DeleteReview(Expression<Func<Review, bool>> expression)
    {
        try
        {
            var reviewEntity = _reviewRepository.Delete(expression);
            if(reviewEntity)
            {
                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    /// <summary>
    ///     Gets ReviewEntities with same id and groups them by productId
    /// </summary>
    /// <returns>A collection of ReviewEntities grouped by productId</returns>
    public IEnumerable<Review> GetReviewsWithSameId()
    {
        try
        {
            var reviews = _reviewRepository.GetAll();

            // Gruppera orderRows efter OrderId
            var groupedReviews = reviews.GroupBy(i => i.ProductId);

            foreach (var groupedReview in groupedReviews)
            {
                Console.WriteLine($"Reviews for productid {groupedReview.Key}:");
                foreach (var review in groupedReview)
                {
                    Console.WriteLine($"Product: {review.Product.ProductName}, ReviewText: {review.ReviewText}, Date of review: {review.ReviewDate}");
                }
                Console.WriteLine();
            }
            return reviews;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}
