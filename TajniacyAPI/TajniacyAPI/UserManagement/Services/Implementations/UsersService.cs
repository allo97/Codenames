using AutoMapper;
using BC = BCrypt.Net.BCrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TajniacyAPI.UserManagement.DataAccess.Interfaces;
using TajniacyAPI.UserManagement.DataAccess.Model;
using TajniacyAPI.UserManagement.DataAccess.Model.Dto;
using TajniacyAPI.UserManagement.Services.Interfaces;

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

            existingUser.Password = BC.HashPassword(user.Password);

            _mapper.Map(user, existingUser);
            existingUser.Updated = DateTime.Now;

            var result = await _userUnitOfWork.UsersRepo.Update(existingUser);
            if (!result)
                throw new Exception($"Can't update card {existingUser.Username} in mongoDB");

            return existingUser;
        }
    }
}
