using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using pcontextus.Training;
using PContextus.ML.Data;
using PContextus.ML.Models;

namespace pcontextus.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        static readonly string _modelpath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

        static readonly string _datapath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-train.csv");
        static readonly string _testdatapath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-test.csv");


        internal static readonly TaxiTrip Trip1 = new TaxiTrip
        {
            VendorId = "VTS",
            RateCode = "1",
            PassengerCount = 1,
            TripDistance = 10.33f,
            PaymentType = "CSH",
            FareAmount = 0 // predict it. actual = 29.5
        };

        // GET api/values
        [HttpGet]
        public IEnumerable<ArticleContentData> Get()
        {

            /* var model = ArticleContentPredictionModel.Train();

             var rep = ArticleContentPredictionModel.Predict(model);

             model.WriteAsync(_modelpath);*/

            return null;
        }

        public static  PredictionModel<TaxiTrip, TaxiTripFarePrediction> Train2()
        {
            var pipeline = new LearningPipeline();

            pipeline.Add(new TextLoader(_datapath).CreateFrom<TaxiTrip>(useHeader: true, separator: ','));

            pipeline.Add(new ColumnCopier(("FareAmount", "Label")));


            pipeline.Add(new CategoricalOneHotVectorizer("VendorId",
                                             "RateCode",
                                             "PaymentType"));


            pipeline.Add(new ColumnConcatenator("Features",
                                    "VendorId",
                                    "RateCode",
                                    "PassengerCount",
                                    "TripDistance",
                                    "PaymentType"));


            pipeline.Add(new FastTreeRegressor());

            PredictionModel<TaxiTrip, TaxiTripFarePrediction> model = pipeline.Train<TaxiTrip, TaxiTripFarePrediction>();

           // await model.WriteAsync(_modelpath);
            return model;
        }

        private static void Evaluate2(PredictionModel<TaxiTrip, TaxiTripFarePrediction> model)
        {
            var testData = new TextLoader(_testdatapath).CreateFrom<TaxiTrip>(useHeader: true, separator: ',');
            var evaluator = new RegressionEvaluator();
            RegressionMetrics metrics = evaluator.Evaluate(model, testData);

        }

        public static async Task<PredictionModel<SentimentData, SentimentPrediction>> Train()
        {
            //var
            var pipeline = new LearningPipeline();
            var data = new List<SentimentData>() {
                new SentimentData{
                    Sentiment=5.3f,
                    SentimentText="Good product"
                },
                new SentimentData{
                    Sentiment=-9f,
                    SentimentText="fuck article . I Don't Like it"
                },
                new SentimentData{
                    Sentiment=-1.3f,
                    SentimentText="I Don't Like it"
                },
                 new SentimentData{
                    Sentiment=9.3f,
                    SentimentText="I Would recommend it"
                }

            };

            var collection = CollectionDataSource.Create(data);

            pipeline.Add(collection);

            pipeline.Add(new TextFeaturizer("Features", "SentimentText"));

            pipeline.Add(new FastTreeBinaryClassifier() { NumLeaves = 5, NumTrees = 5, MinDocumentsInLeafs = 2 });

            PredictionModel<SentimentData, SentimentPrediction> model =
            pipeline.Train<SentimentData, SentimentPrediction>();

            return model;
  
        }

        public static void Evaluate(PredictionModel<SentimentData, SentimentPrediction> model)
        {
            var testData = new List<SentimentData>() {
                new SentimentData{
                    Sentiment=6f,
                    SentimentText="such good thing"
                },
                new SentimentData{
                    Sentiment=-9.3f,
                    SentimentText="fucking article"
                }
            };

            var collection = CollectionDataSource.Create(testData);
            var evaluator = new BinaryClassificationEvaluator();
            BinaryClassificationMetrics metrics = evaluator.Evaluate(model, collection);

            Console.WriteLine();
            Console.WriteLine("PredictionModel quality metrics evaluation");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {metrics.Auc:P2}");
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");

        }

        public static List<string> Predict(PredictionModel<SentimentData, SentimentPrediction> model)
        {
            IEnumerable<SentimentData> sentiments = new[]
            {
                new SentimentData
                {
                    SentimentText = "Please Recommend this product"
                },
                new SentimentData
                {
                    SentimentText = "Fuck ,I don't like it"
                }
            };

            IEnumerable<SentimentPrediction> predictions = model.Predict(sentiments);


            Console.WriteLine();
            Console.WriteLine("Sentiment Predictions");
            Console.WriteLine("---------------------");

            var sentimentsAndPredictions = sentiments.Zip(predictions, (sentiment, prediction) => (sentiment, prediction));

            List<string> rep = new List<string>();
            foreach (var item in sentimentsAndPredictions)
            {
                Console.WriteLine($"Sentiment: {item.sentiment.SentimentText} | Prediction: {(item.prediction.Sentiment ? "Positive" : "Negative")}");

               rep.Add($"Sentiment: {item.sentiment.SentimentText} | Prediction: {(item.prediction.Sentiment ? "Positive" : "Negative")}");
                    
            }

            return rep;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
