﻿using Models;

namespace Repository
{
    public interface IUserRepository
    {
        bool AddUser(UserModel user);
        bool UpdateUser(UserModel user);
        bool DeleteUser(UserModel user);
    }
}
