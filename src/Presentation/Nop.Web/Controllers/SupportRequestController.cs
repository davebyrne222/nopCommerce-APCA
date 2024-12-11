using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Support;
using Nop.Web.Models.Support;
using Nop.Services.Support;

namespace Nop.Web.Controllers;

public class SupportRequestController : BasePublicController
{
    protected readonly ISupportRequestService _supportRequestService;
    protected readonly IWorkContext _workContext;
    protected readonly int _currentUserId;

    public SupportRequestController(ISupportRequestService supportRequestService, IWorkContext workContext)
    {
        _supportRequestService = supportRequestService;
        _workContext = workContext;
        _currentUserId = _workContext.GetCurrentCustomerAsync().Result.Id;
    }
    
    #region Request List
    
    public async Task<IActionResult> List(int pageIndex = 0, int pageSize = 5)
    {
        var requestList = await _supportRequestService.GetUserSupportRequestsAsync(_currentUserId, pageIndex, pageSize);
        
        var viewModel = new SupportListViewModel()
        {
            Requests = requestList.Result.ToList(),
            PageSize = pageSize,
            CurrentPage = pageIndex,
            HasPreviousPage = requestList.Result.HasPreviousPage,
            HasNextPage = requestList.Result.HasNextPage,
            TotalPages = requestList.Result.TotalPages,
        };
        
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_RequestsTable", viewModel);
        }
        
        return View(viewModel);
    }
    
    #endregion
    
    #region Create New Request
    
    public IActionResult Create()
    {
        var model = new SupportRequest();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SupportRequest model)
    {
        if (ModelState.IsValid)
        {
            model.CustomerId = _currentUserId;

            await _supportRequestService.CreateSupportRequestAsync(model);
            
            return RedirectToAction("Chat", new { requestId = model.Id });
        }
        return View(model);
    }
    
    #endregion
    
    #region Chat
    
    public async Task<IActionResult> Chat(int requestId)
    { 
        var supportRequest = await _supportRequestService.GetSupportRequestByIdAsync(requestId);
        
        if (supportRequest.Success == false)
        {
            foreach (var error in supportRequest.Errors)
            {
                Console.Error.WriteLine(error);
            }
            
            return NotFound();
        }
        
        var baseMessages = await _supportRequestService.GetSupportRequestMessagesAsync(requestId);
        var viewModel = new SupportChatViewModel()
        {
            RequestId = supportRequest.Result.Id,
            Subject = supportRequest.Result.Subject,
            Status = supportRequest.Result.Status,
            Messages = baseMessages.Result.ToList()
        };
        
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_UserChatHistoryTable", viewModel);
        }
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Chat(SupportChatViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Any())
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return Json(new { success = false, errors });
        }

        var entityModel = new SupportMessage()
        {
            RequestId = model.RequestId,
            AuthorId = _currentUserId,
            Message = model.NewMessage
        };
    
        await _supportRequestService.CreateSupportMessageAsync(entityModel);

        return Json(new { success = true, message = "successfully added new message" });
    }

    #endregion
    
}