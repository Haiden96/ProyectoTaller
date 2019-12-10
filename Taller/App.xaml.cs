using System;
using Xamarin.Forms.Xaml;

namespace Taller
{
    using Taller.Models;
    using Taller.View;
    using Xamarin.Forms;
    public partial class App : Application
    {
        //public string urlBase = "http://ziihaideniiz-001-site1.itempurl.com";
        //public string servicePrefix = "/servicio";
        public static String url_webservice { get; set; }
        public static String url_servicePrefix { get; set; }
        public static Paciente var_paciente { get; set; }
        public static Medico var_medico { get; set; }
        public static Historial var_historial { get; set; }
        public App()
        {
            //url_webservice = App.url_webservice;
            //url_servicePrefix = App.url_servicePrefix;
            url_webservice = "http://ziihaideniiz-001-site1.itempurl.com";
            url_servicePrefix = "/servicio";
            InitializeComponent();

            MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
