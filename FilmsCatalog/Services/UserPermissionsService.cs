using System;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FilmsCatalog.Services
{
    public class UserPermissionsService : IUserPermissionsService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;

        public UserPermissionsService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        private HttpContext HttpContext => this.httpContextAccessor.HttpContext;

        public Boolean CanEditFilm(Film film)
        {
            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            return userManager.GetUserId(this.httpContextAccessor.HttpContext.User) == film.CreatorId;
        }


        public Boolean CanEditCatalog(Catalog catalog)
        {
            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            return userManager.GetUserId(this.httpContextAccessor.HttpContext.User) == catalog.CreatorId;
        }
    }
}
