using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]  
    public partial class AppShell: Shell 
    {
        
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddChat), typeof(AddChat));
            Routing.RegisterRoute(nameof(Chat),typeof(Chat));
            Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));
            Routing.RegisterRoute(nameof(EditUser),typeof(EditUser));
        }
       
    }
}