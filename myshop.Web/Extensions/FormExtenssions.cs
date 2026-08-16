

using myshop.BLL.DTOs.General;

namespace myshop.Web.Extensions;

public static class FormExtenssions
{
    extension(IFormCollection form)
    {
        public FormDto GetRequestForm()
        {
            return new FormDto(
                PageSize: int.Parse(form["length"]!),
                Start: int.Parse(form["start"]!),
                SortingCol: form[$"columns[{form["order[0][column]"]}][data]"]!,
                SortingDir: form["order[0][dir]"]!,
                Search: form["search[value]"]);
        }
    }
}