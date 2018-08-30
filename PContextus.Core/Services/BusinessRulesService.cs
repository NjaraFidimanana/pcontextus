using Contextus.Core.Domain;
using PContextus.Core.Domain.Entities;
using PContextus.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PContextus.Core.Services
{
    public class BusinessRulesService : IBusinessRule
    {


        private readonly IRepository _repository;

        public BusinessRulesService(IRepository repository) {

            _repository = repository;
        }


        public async Task<BusinessRule> GetDefinedRule(int context) {

            return await _repository.FindAsync<BusinessRule>(x => x.ContextId==context);
        }


    }
}
