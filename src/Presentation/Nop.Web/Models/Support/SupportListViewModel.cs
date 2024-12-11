#nullable enable
using Nop.Core.Domain.Support;

namespace Nop.Web.Models.Support;

public class SupportListViewModel
{
    // requests to be displayed
    public List<SupportRequest>? Requests { get; set; }
    
    // pagination
    public bool HasPreviousPage { get; set; }
    
    public bool HasNextPage { get; set; }
    
    public int TotalPages { get; set; }
    
    public int CurrentPage { get; set; }
    
    public int PageSize { get; set; }
}