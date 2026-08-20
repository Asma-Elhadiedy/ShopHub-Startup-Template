namespace myshop.BLL.ViewModels.Customer.Products;

public class ProductDetailsVM
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public IReadOnlyList<ProductReviewVM> Reviews { get; set; } = [];
    public int ReviewCount => Reviews.Count;
    public double AverageRating => Reviews.Count == 0 ? 0 : Math.Round(Reviews.Average(review => review.Rating), 1);
    public ProductReviewVM? CurrentUserReview => Reviews.FirstOrDefault(review => review.IsMine);
    public ProductReviewFormVM ProductReviewFormVM => new()
    {
        ProductId = Id,
        ReviewId = CurrentUserReview?.Id ?? 0,
        Rating = CurrentUserReview?.Rating ?? 0,
        Comment = CurrentUserReview?.Comment ?? string.Empty
    };
}
