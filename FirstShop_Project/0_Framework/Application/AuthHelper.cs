using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace _0_Framework.Application
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int CurrentAccountId()
        {
            if (IsAuthenticated())
                return int.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "AccountId").Value);
            return 0;
        }

        public AuthViewModel CurrentAccountInfo()
        {
            var claims = _contextAccessor.HttpContext.User.Claims.ToList();

            var result = new AuthViewModel
                (
               int.Parse(claims.FirstOrDefault(x => x.Type == "AccountId").Value),
               int.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value),
                claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value,
               claims.FirstOrDefault(x => x.Type == "UserName").Value,
               new List<int>()

                );

            result.Role = Roles.GetRoleBy(result.RoleId);
            return result;
        }

        public string CurrentAccountRole()
        {
            if (IsAuthenticated())
                return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;

            return null;
        }

        public List<int> GetPermissions()
        {
            if (!IsAuthenticated())
                return new List<int>();

            var permissions = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "permissions")?.Value;
            return JsonConvert.DeserializeObject<List<int>>(permissions);
        }

        public bool IsAuthenticated()
        {
            return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;

            //var claim = _contextAccessor.HttpContext.User.Claims.ToList();
            //return claim.Count > 0;
        }

        public void SignIn(AuthViewModel account)
        {
            var permissions = JsonConvert.SerializeObject(account.Permissions);
            var claim = new List<Claim>
            {
                new Claim("AccountId",account.Id.ToString()),
                new Claim(ClaimTypes.Name,account.FullName),
                new Claim(ClaimTypes.Role,account.RoleId.ToString()),
                new Claim("UserName",account.UserName),
                new Claim("permissions",permissions)
            };


            var claimsIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);


            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
            };


            _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public void SignOut()
        {
            _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
