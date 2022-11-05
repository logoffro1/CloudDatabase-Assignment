using AutoMapper;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    public class UserRepository:IUserRepository
    {
        private DatabaseContext dbContext;
   

        public UserRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddUserToQueue(CreateUserDTO userDTO)
        {            
                string qName = Environment.GetEnvironmentVariable("CreateUserQueue");
                string connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                QueueClientOptions clientOpt = new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64 };

                QueueClient qClient = new QueueClient(connString, qName, clientOpt);
                var jsonOpt = new JsonSerializerOptions() { WriteIndented = true };
                string userJson = JsonSerializer.Serialize<CreateUserDTO>(userDTO, jsonOpt);
                await qClient.SendMessageAsync(userJson);                            
        }
        public async Task CreateUser(CreateUserDTO userDTO)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateUserDTO, User>()));
            User fullUser = mapper.Map<User>(userDTO);
            fullUser.PasswordHash = PasswordHasher.HashPassword(fullUser.PasswordHash);
            dbContext.Users?.Add(fullUser);
            await dbContext.SaveChangesAsync();
        }
        public async Task<GetUserDTO> GetUserById(Guid userId)
        {
                await dbContext.SaveChangesAsync();
                User user = dbContext.Users.Where(acc => acc.isAccountActive == true).FirstOrDefault(u => u.Id == userId);
                Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));
                GetUserDTO userDTO = mapper.Map<GetUserDTO>(user);
                return userDTO;         
        }

        public async Task<bool> CheckIfUserExistAndActive(Guid userId)
        {
            await dbContext.SaveChangesAsync();          
            if (dbContext.Users.Where(a => a.isAccountActive == true).Count(x => x.Id == userId) > 0)           
                return true;
            else               
                return false;
        }
       
       
     
        public async Task<bool> CheckIfEmailExist(string email)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.Users.Count(x => x.Email == email)>0)
                return true;
            else
                return false;
        } 
        
        public async Task DeactivateUserAccount(Guid userId, bool isAccountActive)
        {
                await dbContext.SaveChangesAsync();
                User user = dbContext.Users.FirstOrDefault(x => x.Id == userId);
                user.isAccountActive = isAccountActive;
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();                     
        }
        private List<GetUserDTO> ConvertGetDtos(List<User> users)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));
            List<GetUserDTO> userdtos = new List<GetUserDTO>();

            users.ForEach(delegate (User u) {
                userdtos.Add(mapper.Map<GetUserDTO>(u));
            });
            return userdtos;
        }

        public async Task<bool> CheckIfEmailExist(Guid userId, string wantedEmail)
        {
            await dbContext.SaveChangesAsync();
            User user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user.Email == wantedEmail) 
                return false;
            else
            {
                return await CheckIfEmailExist(wantedEmail);
            }
        }
    }
}
