
namespace myshop.BLL.Services;

public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper, IFileService _fileService) : IProductService
{
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        return await _unitOfWork.Repository<Product>()
                                .GetQueryable(null)
                                .Select(p => new ProductDto()
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Description = p.Description,
                                    Price = p.Price,
                                    CategoryName = p.Category.Name
                                }).ToListAsync();
    }

    public async Task<ProductVM?> PrepareProductModelAsync(int productId)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);

        var productVM = new ProductVM { CategoryList = await GetCategoriesSelectListAsync() };

        return product is not null
            ? _mapper.Map(product, productVM)
            : productVM;
    }

    public async Task<bool> CreateProductAsync(ProductVM model)
    {
        var product = _mapper.Map<Product>(model);
        _unitOfWork.Repository<Product>().Add(product);

        if (model.File != null)
        {
            product.Img = await _fileService.SaveFileAsync(model.File, PathConsts.ProductImagesPath);
        }

        if (await _unitOfWork.CompleteAsync() > 0)
            return true;
        return false;
    }

    public async Task<bool> UpdateProductAsync(ProductVM model)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(model.Id);
        product = _mapper.Map(model, product);


        if (model.File != null)
        {
            var oldimg = Path.Combine(product.Img);
            await _fileService.DeleteFileAsync(oldimg);

            product.Img = await _fileService.SaveFileAsync(model.File, PathConsts.ProductImagesPath);
        }

        if (await _unitOfWork.CompleteAsync() > 0)
            return true;
        return false;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
        if (product is null)
            return false;


        var oldimg = Path.Combine(product.Img);
        await _fileService.DeleteFileAsync(oldimg);


        _unitOfWork.Repository<Product>().Remove(product);
        if (await _unitOfWork.CompleteAsync() > 0)
            return true;
        return false;
    }

    private async Task<IEnumerable<SelectListItem>> GetCategoriesSelectListAsync()
    {
        return await _unitOfWork.Repository<Category>().GetQueryable(null)
            .Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToListAsync();
    }
}
