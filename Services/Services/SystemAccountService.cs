using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repositories.Entity;
using Repositories.Interface;
using Services.DTOs;
using Services.Interface;

namespace Services.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SystemAccountService> _logger;
        private readonly AdminDTO _adminConfig;
        public SystemAccountService(IUnitOfWork unitOfWork, ILogger<SystemAccountService> logger, IOptions<AdminDTO> options)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _adminConfig = options.Value;
        }

        public async Task<SystemAccount> SignUp(SignUpRequest model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var query = _unitOfWork.GenericRepository<SystemAccount>()
                                     .GetAll();
                if (query.Any(x => x.AccountEmail.ToUpper() == model.AccountEmail.ToUpper()) 
                  || model.AccountEmail.ToLower() == _adminConfig.Email.ToLower())
                {
                    throw new Exception("Email already exists");
                }
                    int id = (query.Select(x => (int?)x.AccountId)
                                     .Max() ?? 0) + 1;

                var account = new SystemAccount
                {
                    AccountId = (short)id,
                    AccountEmail = model.AccountEmail,
                    AccountPassword = model.AccountPassword,
                    AccountName = model.AccountName,
                    AccountRole = (int)model.AccountRole
                };
                await _unitOfWork.GenericRepository<SystemAccount>().InsertAsync(account);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return account;
                }
                throw new Exception("Failed to create SystemAccount");
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var systemAccount = await _unitOfWork.GenericRepository<SystemAccount>()
                                                     .GetFirstOrDefaultAsync(predicate: _ => _.AccountId == id,
                                                                             includeProperties: "NewsArticles");
                if (systemAccount is null)
                {
                    throw new Exception("SystemAccount not found");
                }
                if (systemAccount.NewsArticles.Any())
                {
                    throw new Exception("Cannot delete SystemAccount with associated NewsArticles");
                }
                _unitOfWork.GenericRepository<SystemAccount>().Delete(systemAccount);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result <= 0)
                {
                    throw new Exception("Failed to delete SystemAccount");
                }
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IQueryable<SystemAccount>> GetAllAsync()
        {
            try
            {
                var result = _unitOfWork.GenericRepository<SystemAccount>().GetAll();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all SystemAccounts");
                throw;
            }
        }

        public async Task<SystemAccount> Update(int id, Delta<SystemAccount> delta)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var systemAccount = await _unitOfWork.GenericRepository<SystemAccount>()
                                                     .GetByIdAsync((short)id);
                if (systemAccount is null)
                {
                    throw new Exception("SystemAccount not found");
                }

                if (delta.GetChangedPropertyNames().Contains(nameof(SystemAccount.AccountId)))
                {
                    throw new Exception("Cannot update AccountId");
                }

                delta.Patch(systemAccount);

                _unitOfWork.GenericRepository<SystemAccount>().Update(systemAccount);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return systemAccount;
                }
                throw new Exception("Failed to update SystemAccount");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<SystemAccount> SignIn(SignInRequest model)
        {
            try
            {
                var systemAccount = await _unitOfWork.GenericRepository<SystemAccount>()
                                               .GetFirstOrDefaultAsync(
                                                        predicate: x => x.AccountEmail.ToUpper() == model.Email.ToUpper() 
                                                                     && x.AccountPassword == model.Password);
                if (systemAccount is null)
                {
                    if (model.Email.ToUpper() == _adminConfig.Email.ToUpper()
                        && model.Password == _adminConfig.Password)
                    {
                        systemAccount = new SystemAccount
                        {
                            AccountId = 0,
                            AccountEmail = _adminConfig.Email,
                            AccountPassword = _adminConfig.Password,
                            AccountName = "Admin",
                            AccountRole = 0
                        };
                        return systemAccount;
                    }
                    throw new Exception("Invalid email or password");
                }
                return systemAccount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                throw;
            }
        }
    }
}
