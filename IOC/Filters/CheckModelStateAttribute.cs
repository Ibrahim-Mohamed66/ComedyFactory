using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IOC.Services.Filters;

public class CheckModelStateAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var message = new List<string>();
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Select(x => x.Value.Errors)
             .Where(y => y.Count > 0)
             .Select(s => s.Select(s => s.ErrorMessage).FirstOrDefault())
             .ToList();

            message.AddRange(errors.OfType<string>());

            //context.Result = new OkObjectResult(new  { Result = APIStatus.Failed.DisplayName(), Msg = message });

            return; //short circuit the request
        }

        await next();
    }
}
