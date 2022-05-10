using Application.Interfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.ActionFilter
{
    public class CustomAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                var fileService = context.HttpContext.RequestServices.GetService<IFilesService>();
                string username = context.HttpContext.User.Identity.Name;
                string ftId = context.HttpContext.Request.Query["fileTransferId"].ToString();
                int id = int.Parse(ftId);

                FileTransferViewModel ft = fileService.GetFileTransfer(id);

                if (string.IsNullOrEmpty(username))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "ErrorMessage", controller = "Home", message = "access denied" }));
                }
                else
                {
                    if (username == ft.UserEmail || username == ft.AuthorizedUsers)
                    {
                        if (ft.ExpiryDate == null || (DateTime.Now < ft.ExpiryDate))
                        {

                            return;
                        }
                        else
                        {
                            context.Result = new UnauthorizedResult();
                            throw new UnauthorizedAccessException();
                        }
                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                        throw new UnauthorizedAccessException();
                    }
                }
            }
            catch (Exception ex)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Error", controller = "Home", message = "access denied" }));
            }

        }
    }
}
