using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PContextus.Core.Interfaces
{
   public interface IContentAgentService
   {
       void UpdateArticleContentScoring();

        Task InsertContentScoringAsync();
   }
}
