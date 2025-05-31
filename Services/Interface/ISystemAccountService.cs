using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Deltas;
using Repositories.Entity;
using Services.DTOs;

namespace Services.Interface
{
    public interface ISystemAccountService
    {
        Task<IQueryable<SystemAccount>> GetAllAsync();
        Task<SystemAccount> Update(int id, Delta<SystemAccount> delta);
        Task<SystemAccount> SignUp(SignUpRequest model);
        Task Delete(int id);
        Task<SystemAccount> SignIn(SignInRequest model);
    }
}
