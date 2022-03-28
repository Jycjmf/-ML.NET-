using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HWRDemo.HwrCore;
using HWRDemo.Model;
using HWRDemo.View;
using Microsoft.ML;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;

namespace HWRDemo.ViewModel;

public class RecognitionViewModel : ObservableRecipient
{
    private readonly HandwritingDigitalRecognition handwritingDigitalRecognition = new();
    private readonly MLContext mlContext = Ioc.Default.GetService<MLContext>();
    private PredictControlModel currentPredictControlModel = new();

    public RecognitionViewModel()
    {
        RecognitionCommand = new RelayCommand(async () =>
        {
            var predictRes = new PredictModel("", "", new List<float>());
            DrawingView.SaveImageAction();
            await Task.Run(() =>
            {
                var model = handwritingDigitalRecognition.GenerateModel(mlContext);
                predictRes = handwritingDigitalRecognition.ClassifySingleImage(mlContext, model);
            });
            Application.Current.Dispatcher.Invoke(() => CurrentPredictControlModel.PredictResult = predictRes);
        });
        ClearCanvasCommand = new RelayCommand(() => { DrawingView.ClearCanvasAction(); });
    }

    public PredictControlModel CurrentPredictControlModel
    {
        get => currentPredictControlModel;
        set
        {
            SetProperty(ref currentPredictControlModel, value);
            OnPropertyChanged();
        }
    }

    public ICommand RecognitionCommand { get; }
    public ICommand ClearCanvasCommand { get; }
}