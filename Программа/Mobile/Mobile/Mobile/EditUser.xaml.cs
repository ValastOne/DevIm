using Mobile.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditUser : ContentPage
    {
        string check = "0";
        int id_user;
        Service.ServiceUser user;
        Service.ServiceClient service = new Service.ServiceClient();
        public EditUser()
        {
            InitializeComponent();
            service = new Service.ServiceClient();
            Update();
        }

        public async void Update()
        {
            id_user = Convert.ToInt32(Xamarin.Essentials.SecureStorage.GetAsync("id_rep").Result);
            await Task.Run(() =>
            {
                check = Xamarin.Essentials.SecureStorage.GetAsync("IsNew?").Result;
                if (check != "1")
                {                    
                    user = service.GetUser(id_user);
                    LastNameEntry.Text = user.surname;
                    FirstNameEntry.Text = user.name;
                    LoginEntry.Text = user.login;
                    foreach (var item in RolePicker.Items)
                    {
                        if ((string)item == user.role)
                        {
                            RolePicker.SelectedItem = item;
                            break;
                        }
                    }
                    Main.Title = "Редактирование пользователя";
                    Delete_Button.IsVisible = true;
                }
                else
                {
                    PasswordEntry.IsVisible = true;
                    L_Password.IsVisible = true;
                }
            });

        }

        private void Save_Button_Clicked(object sender, EventArgs e)
        {
            //WaitDowland();
            Save();
           
        }
        private async void Save()
        {
            try
            {                                                   
                if ((LastNameEntry.Text != null) && (FirstNameEntry.Text != null)
                && (LoginEntry.Text != null) && (RolePicker.SelectedIndex != -1))
                {
                    Service.ServiceUser user = new Service.ServiceUser();
                    user.id = id_user;
                    user.surname = LastNameEntry.Text;
                    user.name = FirstNameEntry.Text;                    
                    user.login = LoginEntry.Text;
                    user.role = (string)RolePicker.SelectedItem;
                    if (check == "1")
                    {
                        if (PasswordEntry.Text != null)
                        {
                            user.password=PasswordEntry.Text;
                            service.AddUser(user);
                        }
                           
                        else
                            await DisplayAlert("Ошибка", "Поле пароля не заполнено!", "OK");
                    }
                    else
                    {
                        service.EditUser(user);
                    }
                    DependencyService.Get<IToastService>().Show("Успешно!");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await DisplayAlert("Ошибка",
                            "Не все поля заполнены.\nЗаполниете все поля и повторите попытку.", "OK");
                }                
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
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

        private async void Delete_Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    service.DeleteUser(user);                    
                    
                });
                DependencyService.Get<IToastService>().Show("Успешно!");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }

        }
    }
}