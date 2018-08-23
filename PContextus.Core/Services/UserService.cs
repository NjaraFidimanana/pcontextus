using Contextus.Core.Domain;
using MongoDB.Driver;
using PContextus.Core.Domain.Entities;
using PContextus.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PContextus.Core.Services
{
    public class UserService : IUserService
    {

        private readonly IRepository _repository;

        public UserService(IRepository repository) {

            _repository = repository;
        }
        public async System.Threading.Tasks.Task<UserProfile> GetCurrentUserAsync(string urn) {

            var filter = Builders<UserProfile>.Filter.Eq("Urn", urn);

           return  await _repository.FindAsync<UserProfile>(x=>x.Urn.Equals(urn));

        }
    }
}
