using PContextus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PContextus.Core.Interfaces
{
    public interface ISegmentService
    {
        Task<IEnumerable<Segmentation>> GetSegments();


    }
}
