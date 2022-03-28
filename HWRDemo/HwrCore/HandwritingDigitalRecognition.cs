using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HWRDemo.Model;
using HWRDemo.Utility;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace HWRDemo.HwrCore;

public class HandwritingDigitalRecognition
{
    private static readonly string AssetsPath = Path.Combine(Environment.CurrentDirectory, "assets");
    private static readonly string PredictSingleImage = Environment.CurrentDirectory + "\\temp.png";
    private readonly ConfigUtility config = Ioc.Default.GetService<ConfigUtility>();

    public HandwritingDigitalRecognition()
    {
        config.config.ImgPath ??= Path.Combine(AssetsPath, "images");
        config.config.ModelTsv ??= Path.Combine(config.config.ImgPath, "tags.tsv");
        config.config.TestTsv ??= Path.Combine(config.config.ImgPath, "test-tags.tsv");
        config.config.ModelPath ??= Path.Combine(AssetsPath, "inception", "tensorflow_inception_graph.pb");
    }

    public PredictModel ClassifySingleImage(MLContext mlContext, ITransformer model)
    {
        var imageData = new ImageData
        {
            ImagePath = PredictSingleImage
        };
        var predictor = mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(model);
        var prediction = predictor.Predict(imageData);
        return new PredictModel(imageData.ImagePath, prediction.PredictedLabelValue, new List<float>(prediction.Score));
    }

    public ITransformer GenerateModel(MLContext mlContext)
    {
        IEstimator<ITransformer> pipeline = mlContext.Transforms
            .LoadImages("input", config.config.ImgPath, nameof(ImageData.ImagePath))
            // The image transforms transform the images into the model's expected format.
            .Append(mlContext.Transforms.ResizeImages("input", InceptionSettings.ImageWidth,
                InceptionSettings.ImageHeight, "input"))
            .Append(mlContext.Transforms.ExtractPixels("input", interleavePixelColors: InceptionSettings.ChannelsLast,
                offsetImage: InceptionSettings.Mean))
            .Append(mlContext.Model.LoadTensorFlowModel(config.config.ModelPath)
                .ScoreTensorFlowModel(new[] {"softmax2_pre_activation"}, new[] {"input"}, true))
            .Append(mlContext.Transforms.Conversion.MapValueToKey("LabelKey", "Label"))
            .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy("LabelKey",
                "softmax2_pre_activation"))
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelValue", "PredictedLabel"))
            .AppendCacheCheckpoint(mlContext);
        var trainingData = mlContext.Data.LoadFromTextFile<ImageData>(config.config.ModelTsv, hasHeader: false);
        var model = pipeline.Fit(trainingData);
        return model;
    }

    public List<string> EstimateModel(MLContext mlContext)
    {
        var model = GenerateModel(mlContext);
        //评估模型
        var testData = mlContext.Data.LoadFromTextFile<ImageData>(config.config.TestTsv, hasHeader: false);
        var predictions = model.Transform(testData);

        // Create an IEnumerable for the predictions for displaying results
        var imagePredictionData = mlContext.Data.CreateEnumerable<ImagePrediction>(predictions, true);
        var tempList = new List<string>();
        foreach (var prediction in imagePredictionData)
            tempList.Add(
                $"Image: {Path.GetFileName(prediction.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score.Max()} ");
        var metrics =
            mlContext.MulticlassClassification.Evaluate(predictions,
                "LabelKey",
                predictedLabelColumnName: "PredictedLabel");
        tempList.Add($"LogLoss:{metrics.LogLoss}");
        return tempList;
    }

    public class ImageData
    {
        [LoadColumn(0)] public string ImagePath;

        [LoadColumn(1)] public string Label;
    }

    public class ImagePrediction : ImageData
    {
        public string PredictedLabelValue;
        public float[] Score;
    }

    private struct InceptionSettings
    {
        public const int ImageHeight = 224;
        public const int ImageWidth = 224;
        public const float Mean = 117;
        public const float Scale = 1;
        public const bool ChannelsLast = true;
    }
}