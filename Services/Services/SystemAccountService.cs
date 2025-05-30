using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.Extensions.Logging;
using Repositories.Entity;
using Repositories.Interface;
using Services.Interface;

namespace Services.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SystemAccountService> _logger;
        public SystemAccountService(IUnitOfWork unitOfWork, ILogger<SystemAccountService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<SystemAccount> Create(SystemAccount systemAccount)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.GenericRepository<SystemAccount>().InsertAsync(systemAccount);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return systemAccount;
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
                if (result > 0)
                {
                    await _unitOfWork.CommitTransactionAsync();
                }
                throw new Exception("Failed to delete SystemAccount");
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
                                                     .GetByIdAsync(id);
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
    }
}
