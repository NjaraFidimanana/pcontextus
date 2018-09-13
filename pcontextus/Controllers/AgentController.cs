using Microsoft.AspNetCore.Mvc;
using PContextus.Core.Interfaces;
using PContextus.ML.Data;
using PContextus.ML.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pcontextus.Controllers
{

    [Route("admin/[controller]/[action]")]
    public class AgentController : Controller
    {
        static readonly string _modelpath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

        private readonly IContentAgentService _agentService;
        public AgentController(IContentAgentService agentService) {

            _agentService = agentService;
        }

        // GET admin/AgentContentScoring
        [HttpGet]
        public bool AgentContentScoring()
        {
            _agentService.UpdateArticleContentScoring();

            return true;
        }


        [HttpGet]
        public bool InsertContentArticle() {

            _agentService.InsertContentScoringAsync();

            return true;
                
         }

        [HttpGet]
        public bool InsertContentProduct() {

            try {
                _agentService.InsertProductAsync();
            }
            catch (Exception ex) {

            }
            

            return true;
        }

    }
}
