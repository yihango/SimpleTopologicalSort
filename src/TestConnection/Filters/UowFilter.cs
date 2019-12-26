using EasyUnitOfWork.Uow;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestConnection.Filters
{
    public class UowFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UowFilter(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor))
            {
                await next();
                return;
            }


            using (var uow = _unitOfWorkManager.Begin())
            {
                var result = await next();
                if (result.Exception == null || result.ExceptionHandled)
                {
                    await uow.CompleteAsync();
                }
            }

        }
    }
}
