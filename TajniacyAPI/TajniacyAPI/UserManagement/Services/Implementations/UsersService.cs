using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TajniacyAPI.JWTAuthentication.Entities;
using TajniacyAPI.UserManagement.DataAccess.Interfaces;
using TajniacyAPI.UserManagement.DataAccess.Model;
using TajniacyAPI.UserManagement.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace TajniacyAPI.UserManagement.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly IUsersUnitOfWork _userUnitOfWork;
        private readonly IMapper _mapper;

        public UsersService(IUsersUnitOfWork userUnitOfWork, IMapper mapper)
        {
            _userUnitOfWork = userUnitOfWork;
            _mapper = mapper;
        }

        public async Task<User> AddUser(User user)
        {
            var oldUser = await _userUnitOfWork.UsersRepo.GetByUsername(user.Username);
            if (oldUser != null)
                throw new Exception($"User with username '{oldUser.Username}' already exists.");

            user.Created = DateTime.UtcNow;
            user.Password = BC.HashPassword(user.Password);
            user.RefreshTokens = new List<RefreshToken>();

            var result = await _userUnitOfWork.UsersRepo.Save(user);
            if (!result)
                throw new Exception($"Can't save user {user.Username} to mongoDB");

            return user;
        }

        public async Task DeleteUser(string id)
        {
            var result = await _userUnitOfWork.UsersRepo.Delete(id);
            if (!result)
                throw new Exception("Cannot delete this user!");
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _userUnitOfWork.UsersRepo.GetAll();
            if (users == null)
                throw new Exception($"Can't get Users from mongoDB");

            return users;
        }

        public async Task<User> GetUser(string id)
        {
            var user = await _userUnitOfWork.UsersRepo.GetById(id);
            if (user == null)
                throw new Exception($"Can't get User from mongoDB");

            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            var existingUser = await _userUnitOfWork.UsersRepo.GetById(user.ID);
            if (existingUser == null)
                throw new Exception($"Can't find card {user.Username} in database.");

            var mappedUser = _mapper.Map(user, existingUser);
            mappedUser.Password = BC.HashPassword(mappedUser.Password);

            mappedUser.Updated = DateTime.Now;

            var result = await _userUnitOfWork.UsersRepo.Update(mappedUser);
            if (!result)
                throw new Exception($"Can't update card {existingUser.Username} in mongoDB");

            return mappedUser;
        }
    }
}
