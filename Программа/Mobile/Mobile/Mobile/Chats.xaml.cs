using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Chats : ContentPage
    {
        bool first =true;
        public class current_chat
        {
            public string last_message { get; set; }
            public int second_user { get; set; }
            public string DateTime { get; set; }

            public string title { get; set; }
        }
        ObservableCollection<current_chat> MessagesList { get; set; }
        Service.ServiceClient server = new Service.ServiceClient();

        int id_owner;

        public Chats()
        {
            InitializeComponent();
            id_owner = Convert.ToInt32(Xamarin.Essentials.SecureStorage.GetAsync("id_user").Result);
            WaitDowland();
            Update();
            //Zaglushka();
            //Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            //{
            //    Window.Title = "Обновление";
            //    Update();
            //    Window.Title = "Сообщения";
            //    return true;
            //});

        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var item = e.Item as current_chat;

            OpenChat(item);
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        public async void OpenChat(current_chat _user)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync("id_rep", _user.second_user.ToString());
            await Xamarin.Essentials.SecureStorage.SetAsync("title", _user.title);
            await Shell.Current.GoToAsync(nameof(Chat));
        }

        private async void Update()
        {
            try
            {
                await Task.Run(() =>
                {
                    // Загрузка чатов
                    var chats = server.GetChatsUsers(id_owner);                   
                    MessagesList = new ObservableCollection<current_chat>();
                    // Вывод чатов
                    for (int i = 0; i < chats.Length; i++)
                    {
                        current_chat chat = new current_chat();                            
                        // Получение последнего сообщения
                        chat.last_message = server.GetLastMessage(chats[i]);
                        if (chat.last_message == null)
                        {
                            break;
                        }
                        // Сокращение длины сообщения
                        if (chat.last_message != null)
                        {
                            if (chat.last_message.Length > 20)
                            {
                                chat.last_message = chat.last_message.Substring(0, 18) + "...";
                            }
                        }
                        var user = server.GetSecondUser(id_owner, chats[i]);
                        chat.second_user = user.id;
                        // Вывод названия чата                 
                        chat.title = user.name + " " + user.surname;

                        MessagesList.Add(chat);

                    }
                    if (MessagesList.Count != 0)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            LV_Chats.ItemsSource = MessagesList;
                            Null_Mess.IsVisible = false;
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Main.IsVisible = false;
                            Null_Mess.IsVisible = true;
                        });
                    }
                    Device.BeginInvokeOnMainThread(WaitStop);
                                     
                    
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
        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void ToChat(string second)
        {

            //WaitDowland();
            //await Navigation.PushAsync(new Chat(2, 3, second));
            await Shell.Current.GoToAsync(nameof(Chat));
            //WaitStop();
        }

        private void Zaglushka()
        {
            current_chat chat = new current_chat();
            MessagesList = new ObservableCollection<current_chat>();
            chat = new current_chat
            {
                last_message = "Привет",
                title = "Денис",
                DateTime = "13:43"
            };
            MessagesList.Add(chat);

            chat = new current_chat
            {
                last_message = "Ок, договрились. Давай тогда в четверг, в 10 часов утра у Башни",
                title = "Роман",
                DateTime = "14:50"
            };
            foreach (current_chat chated in MessagesList)
            {
                if (chat.last_message.Length > 20)
                {
                    chat.last_message = chat.last_message.Substring(0, 18) + "...";
                }
            }

            MessagesList.Add(chat);
            LV_Chats.ItemsSource = MessagesList;

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

        protected override bool OnBackButtonPressed()
        {

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("Выход?", "Вы действительно хотите выйти?", "Да", "Нет"))
                {
                    base.OnBackButtonPressed();
                    await Navigation.PopAsync();
                }
            });

            return true;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
          await  Shell.Current.GoToAsync(nameof(AddChat));
        }
    }
}