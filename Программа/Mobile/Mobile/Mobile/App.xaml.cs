using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
                       
            var isLoogged = Xamarin.Essentials.SecureStorage.GetAsync("isLogged").Result;            
            if (isLoogged == "1")
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
                MainPage.Title = null;
            }
        }

        protected override void OnStart()
        {
            // Код чистки входа
            //Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "0");            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        
    }
}
