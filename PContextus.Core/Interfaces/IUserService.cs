using PContextus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PContextus.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserProfile> GetCurrentUserAsync(string urn);
    }
}
