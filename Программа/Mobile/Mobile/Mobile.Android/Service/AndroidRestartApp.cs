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
using Mobile.Droid.Service;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidRestartApp))]
namespace Mobile.Droid.Service
{

    internal class AndroidRestartApp : IRestartApp
    {
        public void Restart()
        {
            var context = Android.App.Application.Context;
            var intent = context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.ClearTask | ActivityFlags.NewTask);
            context.StartActivity(intent);

            Process.KillProcess(Process.MyPid());
        }
    }
}