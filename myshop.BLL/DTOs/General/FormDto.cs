namespace myshop.BLL.DTOs.General;

public record FormDto(
    int PageSize,
    int Start,
    string SortingCol, 
    string SortingDir, 
    string? Search);
