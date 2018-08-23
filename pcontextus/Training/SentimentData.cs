using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pcontextus.Training
{
    public class SentimentData
    {
        [Column(ordinal: "0", name: "Label")]
        public float Sentiment;
        [Column(ordinal: "1")]
        public string SentimentText;
    }
}
