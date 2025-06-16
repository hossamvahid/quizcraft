using log4net;
using src.Application.Interfaces.Repositories;
using src.Application.Utils;
using src.Domain.Enums;
using src.Domain.Models;

namespace src.Application.Services
{
    public class AuthService
    {
        private readonly IDAPI _dapi;
        private ILog _log;

        public AuthService(IDAPI dapi, ILog log)
        {
            _dapi = dapi;
            _log = log;
        }

        public async Task<(ServiceResult, string?)> Register(string? email, string? username, string? password)
        {
            _log.Info($"Register with Email: {email} and username {username}");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                _log.Info("Request was empty");
                return new(ServiceResult.EMPTY_REQUEST, null);
            }

            bool found = await _dapi.Users.AnyAsync(x => x.Email == email);

            if(found)
            {
                _log.Info("User already exists");
                return new(ServiceResult.ALREADY_EXISTS, null);
            }

            password = PasswordHelper.EncryptPassword(password);

            var user = new User
            {
                Email = email,
                Username = username,
                Password = password
            };

            await _dapi.BeginTransactionAsync();
            try
            {
                await _dapi.Users.AddAsync(user);
                await _dapi.CompleteAsync();
                await _dapi.CommitTransactionAsync();
            }
            catch(Exception ex)
            {
                await _dapi.RollbackTransactionAsync();
                throw;
            }

            var token = TokenHelper.GenerateJwtToken(email, Role.USER);

            _log.Info("User successfully registered");
            return new(ServiceResult.OK, token);
        }


        public async Task<(ServiceResult,string?)> Login(string? email, string? password)
        {
            _log.Info($"User: {email} tries to login");

            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return (ServiceResult.EMPTY_REQUEST,null); 
            }

            var adminToken = AdminHelper.VerifyAdmin(email, password);

            if(adminToken is not null)
            {
                return new(ServiceResult.OK, adminToken);           
            }

            _log.Info("Getting the user");
            var user = await _dapi.Users.FirstOrDefaultAsync(x => x.Email == email);

            if(user is null)
            {
                _log.Info("User was not found");
                return (ServiceResult.INVALID_EMAIL,null);  
            }

            _log.Info("User was found");

            _log.Info($"Checking the password for the Email {email}");

            if(PasswordHelper.VerifyPassword(password,user.Password) is false)
            {
                _log.Info("Passwords does not match");
                return new(ServiceResult.INVALID_PASSWORD, null);
            }

            _log.Info("Password does match");

            var token = TokenHelper.GenerateJwtToken(email, user.Role);

            _log.Info("User successfully authenticated");

            return new(ServiceResult.OK, token);
        }
    }
}
