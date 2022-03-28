using System;
using System.Windows;
using System.Windows.Input;
using HWRDemo.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace HWRDemo.ViewModel;

public class MainViewModel : ObservableRecipient
{
    private UIVisibilityModel currentVisibilityModel = new();

    public MainViewModel()
    {
        SwitchPage(UiElement.Main);
        ChangePageCommand = new RelayCommand<string>(x =>
        {
            switch (Convert.ToInt32(x))
            {
                case 0:
                    SwitchPage(UiElement.Main);
                    break;
                case 1:
                    SwitchPage(UiElement.About);
                    break;
                case 2:
                    SwitchPage(UiElement.Setting);
                    break;
            }
        });
    }

    public UIVisibilityModel CurrentVisibilityModel
    {
        get => currentVisibilityModel;
        set
        {
            SetProperty(ref currentVisibilityModel, value);
            OnPropertyChanged();
        }
    }

    public ICommand ChangePageCommand { get; }

    private void SwitchPage(UiElement element)
    {
        switch (element)
        {
            case UiElement.Main:
                CurrentVisibilityModel.MainVisibility = Visibility.Visible;
                CurrentVisibilityModel.AboutVisibility = Visibility.Hidden;
                CurrentVisibilityModel.SettingVisibility = Visibility.Hidden;
                break;
            case UiElement.About:
                CurrentVisibilityModel.MainVisibility = Visibility.Hidden;
                CurrentVisibilityModel.AboutVisibility = Visibility.Visible;
                CurrentVisibilityModel.SettingVisibility = Visibility.Hidden;
                break;
            case UiElement.Setting:
                CurrentVisibilityModel.MainVisibility = Visibility.Hidden;
                CurrentVisibilityModel.AboutVisibility = Visibility.Hidden;
                CurrentVisibilityModel.SettingVisibility = Visibility.Visible;
                break;
        }
    }
}