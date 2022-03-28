using System.Windows;
using HWRDemo.Utility;
using HWRDemo.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace HWRDemo;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Ioc.Default.ConfigureServices(
            new ServiceCollection()
                .AddSingleton<RecognitionViewModel>()
                .AddSingleton<ConfigUtility>()
                .AddSingleton<SettingViewModel>()
                .AddSingleton<MLContext>()
                .AddSingleton<MainViewModel>()
                .BuildServiceProvider());
    }
}