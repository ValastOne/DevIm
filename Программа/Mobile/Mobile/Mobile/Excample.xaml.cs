using Mobile.IService;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Excample : ContentPage
    {
        Service.ServiceClient service;

        public class Users
        {
            public int id { get; set; }
            public string surname { get; set; }
            public string name { get; set; }
            public string login { get; set; }
            public string password { get; set; }
            public string role { get; set; }
        }
        public Excample()
        {
            InitializeComponent();
            Update();
        }

        private async  void Update()
        {
            var id_user = Convert.ToInt16(await Xamarin.Essentials.SecureStorage.GetAsync("id_user"));
            try
            {
                await Task.Run(() =>
                {
                    service = new ServiceClient();
                    var _user = service.GetUser(id_user);
                    Name.Text = _user.name;
                    Surname.Text = _user.surname;
                    Login.Text = _user.role;
                    if (_user.role == "Инженер-программист")
                    {
                        Device.BeginInvokeOnMainThread(Hide_Adm);
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    await DisplayAlert("Ошибка", "Пожалуйста, перезапустите приложение!", "OK");
                }
                else
                {
                    await DisplayAlert("Ошибка", ex.Message, "OK");
                    Update();
                }
            }
        }


        public void Hide_Adm()
        {
            Admin_Button.IsVisible = false;
        }

        private void LogOut_Button_Clicked(object sender, EventArgs e)
        {
            Xamarin.Essentials.SecureStorage.Remove("isLogged");
            DependencyService.Get<IRestartApp>().Restart();
        }

        private void Admin_Button_Clicked(object sender, EventArgs e)
        {
            ToAdmin();
        }

        private async void ToAdmin()
        {
            await Shell.Current.GoToAsync(nameof(AdminPage));
        }
    }
}