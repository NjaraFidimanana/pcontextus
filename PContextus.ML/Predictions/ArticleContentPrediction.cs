using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace PContextus.ML.Predictions
{
    public class ArticleContentPrediction
    {
       /* [ColumnName("ContentId")]
        public string ContentId;*/

        [ColumnName("Score")]
        public float RelevantScoring;
    }
}
