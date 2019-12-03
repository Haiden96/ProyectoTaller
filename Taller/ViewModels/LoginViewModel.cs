using System;
using System.Collections.Generic;
using System.Text;


namespace Taller.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Forms;
    using View;
    using Services;
    public class LoginViewModel: BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsRemembered
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                     "Error",
                    "Email Vacio",
                    "Acceptar");
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Password Vacio",
                    "Acceptar");
                return;
            }
            this.IsRunning = true;
            this.IsEnabled = false;
            this.Email = String.Empty;
            this.Password = String.Empty;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Acceptar");
                return;
            }
            MainViewModel.GetInstance().CargarImagen= new CargarImagenViewModel();
            //mainViewModel.Token = token;
            //mainViewModel.Index = new IndexViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CargarImagenPage());
            this.IsRunning = false;
            this.IsEnabled = true;
            
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.Email = "kevin.haiden96@gmail.com";
            this.Password = "123";
            this.IsRemembered = true;
            this.IsEnabled = true;
        }
        #endregion

       

    }

}
