namespace myshop.DAL.Entities;

public class OrderStatus : DomainModelBase
{
    public string Name { get; set; } = default!;
}

public class CartStatus : DomainModelBase
{
    public string Name { get; set; } = default!;
}

public class PaymentStatus : DomainModelBase
{
    public string Name { get; set; } = default!;
}
public class PaymentMethod : DomainModelBase
{
    public string Name { get; set; } = default!;
}