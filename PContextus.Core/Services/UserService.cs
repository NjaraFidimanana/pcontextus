using Contextus.Core.Domain;
using MongoDB.Driver;
using PContextus.Core.Domain.Entities;
using PContextus.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return await _repository.FindAsync<UserProfile>(x => x.Urn.Equals(urn));

        }

        public async Task<IEnumerable<UserProfile>> GetUsers() {

            return await _repository.GetAllAsync<UserProfile>();

        }


        public async Task<UserProfile> UpdateCustomerSegment(string urn, string segmentedCode) {

            var userProfile = await GetCurrentUserAsync(urn);

            var segments = userProfile.Segments;

            var isUpdated = segments.Any(x => x.SegmentedCode.Equals(segmentedCode));

            if (!isUpdated) {
                var segment = await _repository.FindAsync<Segmentation>(x => x.SegmentedCode.Equals(segmentedCode));

                if (segment != null) {
                    segment.UpdateExpiryDay();
                    segments.Add(segment);

                    var filter = Builders<UserProfile>.Filter.Eq("Urn", urn);

                    var updateDef = Builders<UserProfile>.Update.Set("Segments", segments);
                    await _repository.UpdateOneAsync(userProfile, updateDef, filter);
                }
            }
            return userProfile;
        }

   
    }
}
