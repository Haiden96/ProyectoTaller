using System;
using Xamarin.Forms.Xaml;

namespace Taller
{
    using Taller.Models;
    using Taller.View;
    using Xamarin.Forms;
    public partial class App : Application
    {
        public static Paciente var_paciente { get; set; }
        public static Medico var_medico { get; set; }
        public static Historial var_historial { get; set; }
        public App()
        {
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
