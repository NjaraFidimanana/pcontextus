using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pcontextus.Training
{
    public class SentimentPrediction
    {

        [ColumnName("PredictedLabel")]
        public bool Sentiment;
    }
}
