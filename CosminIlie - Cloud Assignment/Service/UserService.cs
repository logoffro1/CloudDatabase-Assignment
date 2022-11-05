using AutoMapper;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using ShowerShow.DTO;
using ShowerShow.Repository.Interface;
using System;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task AddUserToQueue(CreateUserDTO user)
        {
            await userRepository.AddUserToQueue(user);
        }
        public async Task<GetUserDTO> GetUserById(Guid Id)
        {
            if (await CheckIfUserExistAndActive(Id))
            {
                return await userRepository.GetUserById(Id);
            }
            else
            {
                throw new Exception("User does not exist");
            }
        }
        public async Task<bool> CheckIfUserExistAndActive(Guid userId)
        {
            return await userRepository.CheckIfUserExistAndActive(userId);
        }

        public async Task DeactivateUserAccount(Guid userId, bool isAccountActive)
        {
            if (await CheckIfUserExistAndActive(userId))
            {
                await userRepository.DeactivateUserAccount(userId, isAccountActive);
            }
            else
            {
                throw new Exception("User does not exist");
            }
        }

        public async Task CreateUser(CreateUserDTO userDTO)
        {
            await userRepository.CreateUser(userDTO);
        }
    }
}
