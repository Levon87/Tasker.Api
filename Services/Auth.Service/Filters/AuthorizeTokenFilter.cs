using Auth.Service.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Service.Filters
{
    public class AuthorizeTokenFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var accessToken = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(accessToken))
            {
                var token = accessToken.Replace("Bearer ", "");

                if (!UserTokenMapping.ExistToken(token))
                {
                    context.HttpContext.Response.StatusCode = 401;
                    return;
                }
            };
        }
    }
}
