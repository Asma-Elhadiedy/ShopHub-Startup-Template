namespace myshop.DAL.Entities;

public class OrderDetail : IDomainModelMarker
{
    public int Id { get; set; }

    public int OrderHeaderId { get; set; }
    [ValidateNever]
    public OrderHeader OrderHeader { get; set; }

    public int ProductId { get; set; }
    [ValidateNever]
    public Product Product { get; set; }

    public decimal Price { get; set; }

    public int Count { get; set; }


}
