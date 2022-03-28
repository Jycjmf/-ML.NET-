using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace HWRDemo.ViewModel;

internal class ViewModelLocator
{
    public RecognitionViewModel RecognitionViewModel => Ioc.Default.GetService<RecognitionViewModel>();
    public SettingViewModel settingViewModel => Ioc.Default.GetService<SettingViewModel>();
    public MainViewModel MainViewModel => Ioc.Default.GetService<MainViewModel>();
}