using Mobile.IService;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddChat : ContentPage
    {
        Service.ServiceClient service;
        bool isDowland = false;

        public class NewUser
        {
            public string name { get; set; }
            public string surname { get; set; }
            public string login { get; set; }
            public int id { get; set; }
        }
        List<NewUser> list_users;
        public AddChat()
        {
            InitializeComponent();
            WaitDowland();
            DowlandUsers();
            //Zaglushka();
        }

        private void TB_Search_Completed(object sender, EventArgs e)
        {
            
        }

        private void Zaglushka()
        {
            List<NewUser> list = new List<NewUser>();
            NewUser user = new NewUser();
            user.id = 1;
            user.login = "AAAA";
            user.name = "Игорь";
            user.surname = "Игорев";
            list.Add(user);
            NewUser user1 = new NewUser();
            user.id = 2;
            user.login = "BBB";
            user.name = "Bигорь";
            user.surname = "ВЫИгорев";
            list.Add(user);
            LV_Users.ItemsSource = list;
        }

        private void LV_Users_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            NewUser selected = e.Item as NewUser;
            OpenChat(selected);
            LV_Users.SelectedItem = null;
        }

        public async void OpenChat(NewUser _user)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync("id_rep", _user.id.ToString());
            await Xamarin.Essentials.SecureStorage.SetAsync("title", _user.name+" " +_user.surname);
            await Shell.Current.GoToAsync(nameof(Chat));            
        }
        private void TB_Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isDowland)
            {
                LV_Users.ItemsSource = list_users.Where(c => (c.surname.Contains(TB_Search.Text)) ||
                    (c.name.Contains(TB_Search.Text)) || (c.login.Contains(TB_Search.Text))).Distinct();
            }
        }

        public async void DowlandUsers()
        {
            try
            {
                await Task.Run(() =>
                {

                    service = new Service.ServiceClient();
                    var users = service.GetUsers();
                    list_users = new List<NewUser>();
                    foreach (var user in users)
                    {
                        NewUser newUser = new NewUser();
                        var _user = service.GetUserWLog(user.ToString());
                        newUser.name = _user.name;
                        newUser.surname = _user.surname;
                        newUser.login = _user.login;
                        newUser.id = _user.id;
                        list_users.Add(newUser);
                    }
                    LV_Users.ItemsSource = list_users;
                    isDowland = true;

                });
                
                Device.BeginInvokeOnMainThread(WaitStop);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Отмена");
            }

        }
        private void WaitDowland()
        {
            Main.IsVisible = false;
            Wait.IsVisible = true;
            activityIndocator.IsRunning = true;
        }

        private void WaitStop()
        {
            activityIndocator.IsRunning = false;
            Wait.IsVisible = false;
            Main.IsVisible = true;
        }
    }
}