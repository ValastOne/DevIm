using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobile.IService;

[assembly: Xamarin.Forms.Dependency(typeof(Mobile.Droid.Service.ToastService))]
namespace Mobile.Droid.Service
{
    internal class ToastService : IToastService
    {
        public void Show(string message)
        {
            Context context = Android.App.Application.Context;
            Toast.MakeText(context, message,ToastLength.Long).Show();
        }
    }
}