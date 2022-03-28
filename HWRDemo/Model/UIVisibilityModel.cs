using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace HWRDemo.Model;

internal enum UiElement
{
    Main,
    About,
    Setting
}

public class UIVisibilityModel : ObservableObject
{
    private Visibility aboutVisibility = Visibility.Hidden;
    private Visibility mainVisibility = Visibility.Visible;

    private Visibility settingVisibility = Visibility.Hidden;

    public Visibility MainVisibility
    {
        get => mainVisibility;
        set
        {
            SetProperty(ref mainVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility AboutVisibility
    {
        get => aboutVisibility;
        set
        {
            SetProperty(ref aboutVisibility, value);
            OnPropertyChanged();
        }
    }

    public Visibility SettingVisibility
    {
        get => settingVisibility;
        set
        {
            SetProperty(ref settingVisibility, value);
            OnPropertyChanged();
        }
    }
}