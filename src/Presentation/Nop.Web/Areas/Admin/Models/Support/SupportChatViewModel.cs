using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Support;

namespace Nop.Web.Areas.Admin.Models.Support;

public class SupportChatViewModel
{
    // Request meta data
    public int RequestId { get; set; }
    
    public string Subject { get; set; }
    
    public StatusEnum Status { get; set; }
    
    // Request chat history
    public List<SupportMessage> Messages { get; set; }
    
    // New message to user
    [Required(ErrorMessage = "Message must be between 10 and 1000 characters.")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 1000 characters.")]
    public string NewMessage { get; set; }
    
    // Available status' to facilitate admin changing the status
    public static List<SelectListItem> AvailableStatuses { get; private set; } = GetAvailableStatuses();
    
    private static List<SelectListItem> GetAvailableStatuses()
    {
        var availableStatuses = new List<SelectListItem>();
        var enumValues = Enum.GetValues(typeof(StatusEnum));
        
        foreach (var status in enumValues)
        {
            var value = (int)status;
            availableStatuses.Add(new SelectListItem(status.ToString(), value.ToString()));
        }
        
        return availableStatuses;
    }
}