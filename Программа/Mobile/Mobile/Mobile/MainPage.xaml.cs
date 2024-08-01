using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mobile.IService;
using Xamarin.Forms.PlatformConfiguration;

namespace Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
         
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Main.IsVisible = false;
            Wait.IsVisible = true;
            activityIndocator.IsRunning = true;
            Crypter.Crypter cryp = new Crypter.Crypter();
            try
            {
                Service.ServiceClient Service = new Service.ServiceClient();
                string result = Service.Login(TB_Login.Text, cryp.Encrypt(TB_password.Text));
                if (Service != null)
                {
                    if (result != "-1")
                    {
                        Main.IsVisible = false;
                        Wait.IsVisible = true;
                        activityIndocator.IsRunning = true;
                        ToastShow("Успешный вход");
                        ToChats(Convert.ToInt16(result));
                    }
                    else
                    {
                        await DisplayAlert("Ошибка входа!",
                            "Неправильный логин или пароль!\nПовторите ввод", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка входа!", "Не удалось подключиться к серверу.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка входа!", ex.Message, "OK");
            }
            activityIndocator.IsRunning = false;
            Wait.IsVisible = false;
            Main.IsVisible = true;

            //await DisplayAlert("Ошибка входа!", "Неправильный логин или пароль!\nПовторите ввод", "OK");
            //ToastShow("Успешный вход");
            //Main.IsVisible = false;
            //Wait.IsVisible = true;
            //activityIndocator.IsRunning = true;
            //ToChats(0);
            //activityIndocator.IsRunning = false;
            //Wait.IsVisible = false;
            //Main.IsVisible = true;
        }


        private void ToastShow(string message)
        {
             DependencyService.Get<IToastService>().Show(message);
        }

      

        private async void ToChats(int id_user)
        {
            //await Navigation.PushAsync(new Chats(id_user));
            await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "1");
            await Xamarin.Essentials.SecureStorage.SetAsync("id_user", id_user.ToString());
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//Main");
        }

        private void TB_Login_Completed(object sender, EventArgs e)
        {
            TB_Login.Unfocus();
            TB_password.Focus();
        }
    }
}
