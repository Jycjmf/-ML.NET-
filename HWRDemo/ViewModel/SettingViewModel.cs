using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HWRDemo.HwrCore;
using HWRDemo.Utility;
using Microsoft.ML;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;

namespace HWRDemo.ViewModel;

public class SettingViewModel : ObservableRecipient
{
    private static readonly string AssetsPath = Path.Combine(Environment.CurrentDirectory, "assets");
    private readonly ConfigUtility config = Ioc.Default.GetService<ConfigUtility>();
    private readonly MLContext mlContext = Ioc.Default.GetService<MLContext>();
    private ObservableCollection<string> estimateRes = new();
    private string imgPath;

    private string modelPath;

    private string testTsv;
    private string trainTsv;

    public SettingViewModel()
    {
        config.config.ImgPath ??= Path.Combine(AssetsPath, "images");
        config.config.ModelTsv ??= Path.Combine(config.config.ImgPath, "tags.tsv");
        config.config.TestTsv ??= Path.Combine(config.config.ImgPath, "test-tags.tsv");
        config.config.ModelPath ??= Path.Combine(AssetsPath, "inception", "tensorflow_inception_graph.pb");
        trainTsv = config.config.ModelTsv;
        TestTsv = config.config.TestTsv;
        ImgPath = config.config.ImgPath;
        ModelPath = config.config.ModelPath;
        EvaluteCommand = new RelayCommand(async () =>
        {
            var temp = new List<string>();
            await Task.Run(() =>
            {
                var core = new HandwritingDigitalRecognition();
                temp = core.EstimateModel(mlContext);
            });
            Application.Current.Dispatcher.Invoke(() =>
            {
                EstimateRes.Clear();
                temp.ForEach(x => EstimateRes.Add(x));
            });
        });
    }

    public ObservableCollection<string> EstimateRes
    {
        get => estimateRes;
        set
        {
            SetProperty(ref estimateRes, value);
            OnPropertyChanged();
        }
    }

    public ICommand EvaluteCommand { get; }

    public string TrainTsv
    {
        get => trainTsv;
        set
        {
            SetProperty(ref trainTsv, value);
            OnPropertyChanged();
        }
    }

    public string TestTsv
    {
        get => testTsv;
        set
        {
            SetProperty(ref testTsv, value);
            OnPropertyChanged();
        }
    }

    public string ModelPath
    {
        get => modelPath;
        set
        {
            SetProperty(ref modelPath, value);
            OnPropertyChanged();
        }
    }

    public string ImgPath
    {
        get => imgPath;
        set
        {
            SetProperty(ref imgPath, value);
            OnPropertyChanged();
        }
    }
}