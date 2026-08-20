namespace myshop.BLL.ViewModels.Customer.Products;

public class ProductReviewFormVM
{
    public int ReviewId { get; set; }
    public int ProductId { get; set; }

    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    [Range(1, 5, ErrorMessage = "Please choose a rating from 1 to 5.")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "Please write your review.")]
    [StringLength(4000, MinimumLength = 3, ErrorMessage = "Your review must be between 3 and 4000 characters.")]
    public string Comment { get; set; } = string.Empty;
}
