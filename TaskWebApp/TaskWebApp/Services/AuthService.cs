﻿using TaskWebApp.DbStuff.Models;
using TaskWebApp.DbStuff.Repositories;

namespace TaskWebApp.Services
{
    public class AuthService
    {
        private UserRepository _userRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public const string LOCALE_TYPE = "locale";

        public AuthService(UserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;// HttpContext == null
        }

        public User? GetCurrentUser()
        {
            var id = GetCurrentUserId();
            if (id == null)
            {
                return null;
            }

            return _userRepository.GetById(id.Value);
        }

        public int? GetCurrentUserId()
        {
            // HttpContext != null
            var idStr = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (idStr == null)
            {
                return null;
            }

            var id = int.Parse(idStr);
            return id;
        }

        public string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value ?? "Гость";
        }

        public string GetCurrentUserRole()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "role")?.Value ?? "";
        }


        public bool IsAdmin()
        {
            return GetCurrentUserRole() == "admin";
        }

        public string GetCurrentUserLocale()
        {
            return _httpContextAccessor.HttpContext.User
                .Claims.FirstOrDefault(x => x.Type == LOCALE_TYPE)
                ?.Value ?? "en-EN";
        }

        public bool IsAuthenticated()
        {
            return GetCurrentUserId() != null;
        }
    }
}
