namespace myshop.DAL.Configurations;

public class ReviewConfigurations : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(review => review.Comment)
            .IsRequired()
            .HasMaxLength(4000);
        builder.Property(review => review.Rating)
            .IsRequired();
        builder.HasIndex(review => new { review.ProductId, review.ApplicationUserId })
            .IsUnique();
        builder.HasOne(review => review.Product)
            .WithMany(product => product.Reviews)
            .HasForeignKey(review => review.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(review => review.ApplicationUser)
            .WithMany(user => user.Reviews)
            .HasForeignKey(review => review.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
