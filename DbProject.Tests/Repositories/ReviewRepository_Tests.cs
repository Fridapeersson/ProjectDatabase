using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Repositories;

public class ReviewRepository_Tests
{
    private readonly ProductCatalogContext _productContext = new(new DbContextOptionsBuilder<ProductCatalogContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void Create_Should_SaveReviewTodatabase()
    {
        //Arrange
        var reviewRepository = new ReviewRepository(_productContext);
        var reviewEntity = new Review { ReviewText = "Test" };

        //Act
        var result = reviewRepository.Create(reviewEntity);

        //Assert
        Assert.Equal(1, reviewEntity.Id);
        Assert.NotNull(result);
    }

    [Fact]
    public void Create_Should_Not_SaveReviewTodatabase_ReturnNull()
    {
        //Arrange
        var reviewRepository = new ReviewRepository(_productContext);
        var reviewEntity = new Review();

        //Act
        var result = reviewRepository.Create(reviewEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_Should_GetAllReviews_ReturnIEnumerableOfTypeReview()
    {
        //Arrange
        var reviewRepository = new ReviewRepository(_productContext);
        var reviewEntity = new Review { ReviewText = "Test" };
        reviewRepository.Create(reviewEntity);

        //Act
        var result = reviewRepository.GetAll();

        //Assert
        Assert.Equal(1, reviewEntity.Id);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_GetOneReviewFromDatabase_ReturnOneReview()
    {
        //Arrange
        var productRepository = new ProductRepository(_productContext);
        var reviewRepository = new ReviewRepository(_productContext);

        var productEntity = new Product { ProductName = "Test", ProductPrice = 10 };
        productRepository.Create(productEntity);
        var reviewEntity = new Review { ReviewText = "Test", ProductId = productEntity.Id };
        reviewRepository.Create(reviewEntity);

        //Act
        var result = reviewRepository.GetOne(x => x.Id == reviewEntity.Id);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_Not_GetOneReviewFromDatabase_ReturnNull()
    {
        //Arrange
        var productRepository = new ProductRepository(_productContext);
        var reviewRepository = new ReviewRepository(_productContext);

        var productEntity = new Product { ProductName = "Test", ProductPrice = 10 };
        //productRepository.Create(productEntity);
        var reviewEntity = new Review { ReviewText = "Test", ProductId = productEntity.Id };
        reviewRepository.Create(reviewEntity);

        //Act
        var result = reviewRepository.GetOne(x => x.Id == reviewEntity.Id);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_Should_UpdateExistingReview_ReturnUpdatedReview()
    {
        //Arrange
        //var productRepository = new ProductRepository(_productContext);
        var reviewRepository = new ReviewRepository(_productContext);

        //var productEntity = new Product { ProductName = "Test", ProductPrice = 10 };
        //productRepository.Create(productEntity);
        var reviewEntity = new Review { ReviewText = "Test" };
        reviewRepository.Create(reviewEntity);

        //Act
        reviewEntity.ReviewText = "NewText";
        var result = reviewRepository.Update(x => x.Id == reviewEntity.Id, reviewEntity);

        //Assert
        Assert.Equal("NewText", reviewEntity.ReviewText);
        Assert.NotNull(result);
    }

    [Fact]
    public void Delete_should_DeleteReview_ReturnTrue()
    {
        //Arrange
        var reviewRepository = new ReviewRepository(_productContext);

        var reviewEntity = new Review { ReviewText = "Test" };
        reviewRepository.Create(reviewEntity);

        //Act
        var result = reviewRepository.Delete(x => x.Id == reviewEntity.Id);

        //Assert
        Assert.True(result);
    }
    [Fact]
    public void Delete_should_Not_DeleteReview_ReturnFalse()
    {
        //Arrange
        var reviewRepository = new ReviewRepository(_productContext);

        var reviewEntity = new Review { ReviewText = "Test" };
        //reviewRepository.Create(reviewEntity);

        //Act
        var result = reviewRepository.Delete(x => x.Id == reviewEntity.Id);

        //Assert
        Assert.False(result);
    }

}
