using ShowerShow.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IUserRepository
    {

        public Task<bool> CheckIfUserExistAndActive(Guid userId);
        public Task CreateUser(CreateUserDTO userDTO);
        public Task AddUserToQueue(CreateUserDTO user);
        public Task DeactivateUserAccount(Guid userId, bool isAccountActive);
        public Task<GetUserDTO> GetUserById(Guid Id);
    }
}
