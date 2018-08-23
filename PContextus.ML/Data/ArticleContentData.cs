using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace PContextus.ML.Data
{
    public class ArticleContentData
    {
        [Column("0")]
        public string ContentId;

        [Column("1")]
        public string Brand;

        [Column("2")]
        public string Title;

        [Column("3")]
        public float Likes;

        [Column("4")]
        public float GaViews;

        [Column("5")]
        public float RelevantScoring;

    }
}
