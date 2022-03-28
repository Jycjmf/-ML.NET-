using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace HWRDemo.Model;

public class PredictModel : ObservableObject
{
    private string name;

    private string predictValue;

    private ObservableCollection<float> score;

    public PredictModel(string name, string predictValue, List<float> score)
    {
        this.name = name;
        this.predictValue = predictValue;
        this.score = new ObservableCollection<float>(score);
    }

    public string Name
    {
        get => name;
        set
        {
            SetProperty(ref name, value);
            OnPropertyChanged();
        }
    }

    public string PredictValue
    {
        get => predictValue;
        set
        {
            SetProperty(ref predictValue, value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<float> Score
    {
        get => score;
        set
        {
            SetProperty(ref score, value);
            OnPropertyChanged();
        }
    }
}