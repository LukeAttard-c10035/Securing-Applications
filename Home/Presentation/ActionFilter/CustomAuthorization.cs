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
                var logService = context.HttpContext.RequestServices.GetService<ILogService>();

                string username = context.HttpContext.User.Identity.Name;
                string ftId = context.HttpContext.Request.Query["fileTransferId"].ToString();
                int id = int.Parse(ftId);

                FileTransferViewModel ft = fileService.GetFileTransfer(id);

                if (string.IsNullOrEmpty(username) || ft == null)
                {
                    throw new UnauthorizedAccessException("Access Denied: You do not have the rights to access this file");
                }
                else
                {
                    if (username == ft.UserEmail || username == ft.AuthorizedUsers)
                    {
                        if (ft.ExpiryDate == null || (DateTime.Now < ft.ExpiryDate))
                        {
                            logService.SetupLog(
                                context.HttpContext.Connection.RemoteIpAddress.ToString(),
                                username,
                                $"{username} accessed file {ft.FileName} ID: {ft.Id} Uploader: {ft.UserEmail} Auth User: {ft.AuthorizedUsers}  Expiery: {ft.ExpiryDate}",
                                "Info"
                            );
                            return;
                        }
                        else
                        {
                            logService.SetupLog(
                                context.HttpContext.Connection.RemoteIpAddress.ToString(),
                                username,
                                $"{username} failed access {ft.FileName} ID: {ft.Id} Uploader: {ft.UserEmail} Auth User: {ft.AuthorizedUsers}  Expiery: {ft.ExpiryDate}",
                                "Info"
                            );
                            context.Result = new UnauthorizedResult();
                            throw new UnauthorizedAccessException("Access Denied: File is Expired");
                        }
                    }
                    else
                    {
                        logService.SetupLog(
                            context.HttpContext.Connection.RemoteIpAddress.ToString(),
                            username,
                            $"{username} attempted access {ft.FileName} ID: {ft.Id} Uploader: {ft.UserEmail} Auth User: {ft.AuthorizedUsers}  Expiery: {ft.ExpiryDate}",
                            "Warning"
                        );
                        context.Result = new UnauthorizedResult();
                        throw new UnauthorizedAccessException("Access Denied: You do not have the rights to access this file");
                    }
                }
            }
            catch (Exception ex)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Error", controller = "Files", error = ex.Message }));
            }

        }
    }
}
