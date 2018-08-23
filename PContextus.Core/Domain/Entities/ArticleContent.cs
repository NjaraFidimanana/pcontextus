
using PContextus.ML.Data;
using PContextus.ML.Predictions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PContextus.Core.Domain.Entities
{
    public class ArticleContent : EntityBase<string>
    {
        public string ContentId;

        public string Title;

        public string Brand;

        public string Market;

        public Ratings Ratings;

        public float RelevantScoring;

        public Category Category;

        public static List<ArticleContentData> Transform(IEnumerable<ArticleContent> dataset) {
            var articleContentData = new List<ArticleContentData>();

            foreach (var data in dataset) {
                var temp = new ArticleContentData {
                    ContentId=data.ContentId,
                    Brand=data.Brand,
                    Title=data.Title,
                    GaViews=data.Ratings.Analytics.GaViews,
                    Likes=data.Ratings.Likes

                };
                articleContentData.Add(temp);

            }

            return articleContentData;

        }

     

    }
}
