using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Deltas;
using Repositories.Entity;

namespace Services.Interface
{
    public interface ISystemAccountService
    {
        Task<IQueryable<SystemAccount>> GetAllAsync();
        Task<SystemAccount> Update(int id, Delta<SystemAccount> delta);
        Task<SystemAccount> Create(SystemAccount systemAccount);
        Task Delete(int id);
    }
}
