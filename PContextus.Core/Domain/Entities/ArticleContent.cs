
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PContextus.ML.Data;
using PContextus.ML.Predictions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PContextus.Core.Domain.Entities
{
    public class ArticleContent : EntityBase<string>
    {
        public string ContentId { get; set; }

        public string Title { get; set; }

        public string Brand { get; set; }

        public string Market { get; set; }

        public Ratings Ratings { get; set; }

        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float RelevantScoring { get; set; }

        public Category Category { get; set; }

        public string ContentType { get; set; }

        public string ContentImageUrl { get; set; }

        public string Gtins { get; set; }


        public float Views {
            get {
                return Ratings.Analytics.GaViews;
            }
        }

        public float Rated {
            get
            {
                return Ratings.RateReview;
            }
        }

        public float ReviewCount {
            get
            {
                return Ratings.Reviews;
            }
        }

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
