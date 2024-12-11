#nullable enable
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Support;

namespace Nop.Web.Areas.Admin.Models.Support;

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

    // sort requests by date
    public static List<SelectListItem> SortOptions { get; } = new()
    {
        new SelectListItem() { Text = "Oldest", Value = "date_asc" },
        new SelectListItem() { Text = "Newest", Value = "date_dsc" },
    };
    
    public string? SelectedSortOption { get; set; }
    
    // Term to search in requests
    public string? SearchTerm { get; set; }
    
    // filter requests by status
    public static List<SelectListItem> AvailableStatuses { get; private set; } = GetAvailableStatuses();
    
    public string? FilterByStatus { get; set; }
    
    private static List<SelectListItem> GetAvailableStatuses()
    {
        var availableStatuses = new List<SelectListItem>();
        var enumValues = Enum.GetValues(typeof(StatusEnum));

        foreach (var status in enumValues)
        {
            availableStatuses.Add(new SelectListItem(status.ToString(), status.ToString()));
        }
        
        // Empty status to allow removal of filter
        availableStatuses.Add(new SelectListItem("", ""));
        
        return availableStatuses;
    }
}