﻿using Nop.Core;
using Nop.Core.Domain.Support;

namespace Nop.Services.Support;

public interface ISupportRequestService
{
    
    #region Requests
    
    // Create
    public Task<SupportRequestResult> CreateSupportRequestAsync(SupportRequest request); 
    
    
    // Read
    public Task<SupportRequestResult<SupportRequest>> GetSupportRequestByIdAsync(int id);
    
    public Task<SupportRequestResult<IPagedList<SupportRequest>>> GetAllSupportRequestsAsync(
        bool sortByCreatedDateDsc = true,
        string filterByStatus = "",
        string searchQuery = "",
        int pageIndex = 0,
        int pageSize = 5);
    
    public Task<SupportRequestResult<IPagedList<SupportRequest>>> GetUserSupportRequestsAsync(
        int userId, 
        int pageIndex = 0,
        int pageSize = 5);
    
    // Update
    public Task<SupportRequestResult<SupportRequest>> UpdateSupportRequestAsync(SupportRequest request);
    
    
    // Delete
    public Task<SupportRequestResult> DeleteSupportRequestByIdAsync(int requestId);
    
    #endregion
    
    #region Messages
    
    
    // Create
    public Task<SupportRequestResult> CreateSupportMessageAsync(SupportMessage message);
    
    
    // Read
    public Task<SupportRequestResult<IList<SupportMessage>>> GetSupportRequestMessagesAsync(int supportRequestId);
    
    
    // Delete
    public Task<SupportRequestResult> DeleteSupportRequestMessagesAsync(int supportRequestId);
    
    #endregion
}