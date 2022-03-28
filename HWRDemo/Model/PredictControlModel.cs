using System.Collections.Generic;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace HWRDemo.Model;

public class PredictControlModel : ObservableObject
{
    private PredictModel predictResult = new("?", "?", new List<float>());

    public PredictModel PredictResult
    {
        get => predictResult;
        set
        {
            SetProperty(ref predictResult, value);
            OnPropertyChanged();
        }
    }
}