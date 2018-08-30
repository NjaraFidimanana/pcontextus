using PContextus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PContextus.Core.Interfaces
{
    public interface IBusinessRule
    {
        Task<BusinessRule> GetDefinedRule(int context);

    }
}
