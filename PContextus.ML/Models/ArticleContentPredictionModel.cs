using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using PContextus.ML.Data;
using PContextus.ML.Predictions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PContextus.ML.Models
{
    public class ArticleContentPredictionModel
    {

        static readonly string _datapath = Path.Combine(Environment.CurrentDirectory, "Data", "content-relevant-train.csv");
        static readonly string _testdatapath = Path.Combine(Environment.CurrentDirectory, "Data", "content-relevant-train-test.csv");


        public static PredictionModel<ArticleContentData, ArticleContentPrediction> Train()
        {
            var pipeline = new LearningPipeline();
           
            pipeline.Add(new TextLoader(_datapath).CreateFrom<ArticleContentData>(useHeader: true, separator: ','));

            pipeline.Add(new ColumnCopier(("RelevantScoring", "Label")));

            pipeline.Add(new CategoricalHashOneHotVectorizer("ContentId", "Brand", "Title"));

            pipeline.Add(new ColumnConcatenator("Features", "Likes", "GaViews"));

            pipeline.Add(new FastTreeRegressor());

            PredictionModel<ArticleContentData, ArticleContentPrediction> model =
            pipeline.Train<ArticleContentData, ArticleContentPrediction>();

            Evaluate(model, pipeline);

            return model;

        }


        public static void Evaluate(PredictionModel<ArticleContentData, ArticleContentPrediction> model, LearningPipeline pipeline)
        {
            var testData = new TextLoader(_testdatapath).CreateFrom<ArticleContentData>(useHeader: true, separator: ',');
            var evaluator = new RegressionEvaluator();
            RegressionMetrics metrics = evaluator.Evaluate(model, testData);


        }

        public static IEnumerable<(ArticleContentData, ArticleContentPrediction)> Predict(PredictionModel<ArticleContentData, ArticleContentPrediction> model, IEnumerable<ArticleContentData> dataSet)
        {

            List<ArticleContentData> result = new List<ArticleContentData>();

            var predictions = model.Predict(dataSet);

           var response = dataSet.Zip(predictions, (data, prediction) =>(data, prediction));

           return response;

        }
    }
}
