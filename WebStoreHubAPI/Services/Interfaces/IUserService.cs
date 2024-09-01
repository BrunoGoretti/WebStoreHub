﻿using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> CreateUserAsync(UserModel user);
    }
}
