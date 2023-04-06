using Caliburn.Micro;
using CourseManager.ViewModels;
using System.Windows;

namespace CourseManager
{
    internal class Startup : BootstrapperBase
    {
        public Startup()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<MainViewModel>();
        }
    }
}
