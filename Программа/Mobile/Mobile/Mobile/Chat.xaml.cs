using Mobile.IService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Mobile.Chats;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(ID_rep),"rep")]
    [QueryProperty(nameof(title),nameof(title))]
    public partial class Chat : ContentPage
    {
        Service.ServiceClient server;
        class current_message
        {
            public string content {  get; set; }
            public bool visable {  get; set; }
        }
        ObservableCollection<current_message> MessagesList { get; set; }
        
        private int id_own;
        private int id_rep;
        public string title;
        
        public int ID_rep
        {
            get { return id_own; }
            set { id_own = value; }
        }
        public Chat(int id_repб,string name)
        {
            InitializeComponent();
            server = new Service.ServiceClient();
            id_own = Convert.ToInt32(Xamarin.Essentials.SecureStorage.GetAsync("id_user").Result); ;
            id_rep = id_repб;
            Title = name;
            try
            {
                GetChat(id_own, id_rep);
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка загрузки", ex.Message, "OK");
            }
            //Zagluchka();
        }
        public Chat()
        {
            InitializeComponent();
            WaitDowland();
            server = new Service.ServiceClient();
            id_own = Convert.ToInt32(Xamarin.Essentials.SecureStorage.GetAsync("id_user").Result);
            id_rep = Convert.ToInt32(Xamarin.Essentials.SecureStorage.GetAsync("id_rep").Result);
            title = Xamarin.Essentials.SecureStorage.GetAsync("title").Result;
            Window.Title = title;
            Xamarin.Essentials.SecureStorage.Remove("id_rep");
            Xamarin.Essentials.SecureStorage.Remove("title");
            try
            {
                GetChat(id_own, id_rep);
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка загрузки", ex.Message, "OK");
            }
            //Zagluchka();
        }
        private void ToastShow(string message)
        {
            DependencyService.Get<IToastService>().Show(message);
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                Visabel();
                if (TB_Send.Text != null)
                {
                    Task.Run(() => { server.SendMessage(TB_Send.Text, id_own, id_rep); });                    
                    MessagesList.Add(new current_message
                    {
                        content = TB_Send.Text,
                        visable = true
                    }
                    );
                    LV_Messages.ItemsSource = MessagesList;
                    LV_Messages.ScrollTo(LV_Messages.ItemsSource, ScrollToPosition.End, true);
                    TB_Send.Text = "";
                }
                else
                {
                    await DisplayAlert("Сообщение не введено",
                        "Введите содержимое сообщение перед его отправкой.", "ОК");
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

        private void Zagluchka()
        {
            MessagesList = new ObservableCollection<current_message>();
            string[] messanges_1 = new string[] { "Привет", "Салам", "Как дела", "Хорошо" };

            for (int i = 0; i < messanges_1.Length; i++)
            {
                var message = new current_message { content = messanges_1[i] };
                if (i % 2 == 0)
                {
                    message.visable = true;

                }
                else
                {
                    message.visable = false;
                }
                MessagesList.Add(message);
            }
            LV_Messages.ItemsSource = MessagesList;
        }

        private async void GetChat(int owner, int recep)
        {
            try
            {
                await Task.Run(() =>
                {
                    MessagesList = new ObservableCollection<current_message>();
                    var messages = server.GetMessages(owner, recep);
                    if (messages.Count() == 0)
                    {
                        Device.BeginInvokeOnMainThread(Hide);
                    }
                    else
                    {
                        if ((messages[0].content == "Последовательность не содержит элементов"))
                        {
                            Device.BeginInvokeOnMainThread(Hide);
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(Visabel);
                            foreach (Service.ServiceMessage message in messages)
                            {
                                current_message _message = new current_message { content = message.content };
                                if (message.sendler == owner)
                                {
                                    _message.visable = true;
                                }
                                else
                                {
                                    _message.visable = false;
                                }
                                MessagesList.Add(_message);
                            }
                            Device.BeginInvokeOnMainThread(() => { LV_Messages.ItemsSource = MessagesList; });
                        }
                    }
                    Device.BeginInvokeOnMainThread(WaitStop);
                });
            }
            catch (Exception ex)
            {
               await DisplayAlert("Ошибка загрузки", ex.Message, "OK");
            }


        }

        public void Hide()
        {
            
            LV_Messages.IsVisible = false;
            Space.IsVisible = true;
        }

        public void Visabel()
        {
            LV_Messages.IsVisible = true;
            Space.IsVisible = false;
        }
    }
}