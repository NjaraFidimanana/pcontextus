using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace pcontextus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ArticleController : Controller
    {
        public ArticleController()
        {



        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value3", "value4" };
        }

        [HttpGet("{language}")]
        public IEnumerable<string> GetContents(string language)
        {
            return new string[] { language, "value2" };
        }

        // GET api/article/language/context/tag
        [HttpGet("{language}/identify/{id}/context/{cxt}/{tag}")]
        public IEnumerable<string> Contents(string language,, string id,int cxt,int tag)
        {
            return new string[] { language, "value: "+cxt };
        }
    }
}
