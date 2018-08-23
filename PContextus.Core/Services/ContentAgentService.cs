using Contextus.Core.Domain;
using Microsoft.Office.Interop.Excel;
using MongoDB.Driver;
using PContextus.Core.Domain.Entities;
using PContextus.Core.Interfaces;
using PContextus.ML.Data;
using PContextus.ML.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace PContextus.Core.Services
{
    public class ContentAgentService : IContentAgentService
    {

        static readonly string _modelpath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

        static readonly string feedArticlePath = "PContextus_Article_{lang}.xml";

        private readonly IRepository _repository;

        public ContentAgentService(IRepository repository) {

            _repository = repository;

        }

   
        public void UpdateArticleContentScoring() {

         var country = "EN-GB";

          var articleContents = GetFeedArticle(country);
         
          var dataset=  ArticleContent.Transform(articleContents);
            
              var model = ArticleContentPredictionModel.Train();

           var rep = ArticleContentPredictionModel.Predict(model,dataset);

           model.WriteAsync(_modelpath);

            foreach (var item in rep) {

                var temp=articleContents.FirstOrDefault(x => x.ContentId.Equals(item.Item1.ContentId));

                var keyValue = new KeyValuePair<string, float>("RelevantScoring", item.Item2.RelevantScoring);

                var filter = Builders<ArticleContent>.Filter.Eq("ContentId", item.Item1.ContentId);

                _repository.UpdateOneAsync(temp,keyValue, filter);
            }
            
        }

        public async Task InsertContentScoringAsync() {

            var country = "EN-GB";

            var articleContents = GetFeedArticle(country);

            foreach (var rep in articleContents) {

                await _repository.InsertAsync(rep);
            }      
        }

        /// <summary>
        /// Get Feed Article
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ArticleContent> GetFeedArticle(string country) {

            var downloadedFile = Path.Combine(
               Environment.CurrentDirectory,"Data/feed", feedArticlePath.Replace("{lang}", country));

            var articleContents = new List<ArticleContent>();

            var contentXml = File.ReadAllText(downloadedFile);

            var document = XDocument.Parse(contentXml);
            float gar = 0f;
            float garesgistration = 0f;
            float likes = 0f;
            float gaviews = 0f;
            articleContents = (from product in document.Descendants("Article")
      
                                let rep=float.TryParse(product.Element("GaTrialRating")?.Value, out gar)
                               let rep2 = float.TryParse(product.Element("GaRegistrationRating")?.Value, out garesgistration)
                             
                               select new ArticleContent
                                  {
                                      ContentId = CheckAttrValue(product.Element("ContentId")?.Value),
                                      Brand= CheckAttrValue(product.Element("Brand")?.Value),
                                      Title= CheckAttrValue(product.Element("Title")?.Value),
                                      Market= CheckAttrValue(product.Element("Market")?.Value),
                                      Category =new Category {
                                          Parent= CheckAttrValue(product.Element("Category")?.Value),
                                          SubCategory= CheckAttrValue(product.Element("Subcategory")?.Value)
                                      },
                                      Ratings =new Ratings {
                                          Likes= float.TryParse(product.Element("Likes")?.Value, out likes) ? likes :0f,
                                          Analytics=new Analytics {
                                              GaViews= float.TryParse(product.Element("GaViews")?.Value, out gaviews) ? gaviews : 0f,
                                              GaTrialRating = gar,
                                             GaRegistrationRating = garesgistration
                                          }

                                      }

                                  }).ToList();


            return articleContents;
        }

        private string CheckAttrValue(string value)
        {
            return value ?? "";
        }
    }
}
