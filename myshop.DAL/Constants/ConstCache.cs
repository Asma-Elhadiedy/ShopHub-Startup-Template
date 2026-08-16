namespace myshop.DAL.Constants;

public static class ConstCacheCategories
{
    public const string Tag = "tagCategories";

    private const string Prefix = "categories";
    public const string All = $"{Prefix}:all";
    public const string SelectedList = $"{Prefix}:selected";
    public static string ById(int id) => $"{Prefix}:id:{id}";

}

public static class ConstCacheProducts
{
    public const string Tag = "tagProducts";

    private const string Prefix = "products";
    public const string All = $"{Prefix}:all";
    public static string ById(int id) => $"{Prefix}:id:{id}";
    public static string ByCategoryId(int id) => $"{Prefix}:categoryId:{id}";

}
