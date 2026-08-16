namespace myshop.BLL.DTOs.General;

public class PagingDTO<T> 
{
    public int RecordsFiltered { get; set; }
    public int RecordsTotal { get; set; }
    public IEnumerable<T> Data { get; set; }

}
