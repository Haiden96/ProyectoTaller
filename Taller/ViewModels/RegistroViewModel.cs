
namespace Taller.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Windows.Input;
    using Taller.Models;
    using Taller.Services;
    using Taller.View;
    using Xamarin.Forms;

    public class RegistroViewModel:BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private int selectedindex;
        private string ci;
        private string nombre;
        private string apellido;
        private string telefono;
        private string especialidad;
        private string codigo;
        private string password;
        private bool espcIsEnabled;
        private bool isRunning;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool EspcIsEnabled
        {
            get { return this.espcIsEnabled; }
            set { SetValue(ref this.espcIsEnabled, value); }
        }
        public string Ci
        {
            get { return this.ci; }
            set { SetValue(ref this.ci, value); }
        }
        public string Nombre
        {
            get { return this.nombre; }
            set { SetValue(ref this.nombre, value); }
        }
        public string Apellido
        {
            get { return this.apellido; }
            set { SetValue(ref this.apellido, value); }
        }
        public string Telefono
        {
            get { return this.telefono; }
            set { SetValue(ref this.telefono, value); }
        }
        public string Especialidad
        {
            get { return this.especialidad; }
            set { SetValue(ref this.especialidad, value); }
        }
        public string Codigo
        {
            get { return this.codigo; }
            set { SetValue(ref this.codigo, value); }
        }
        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public int SelectedIndex
        {
            get { return this.selectedindex; }
            set { SetValue(ref this.selectedindex, value);

                if (this.SelectedIndex == 1)
                {
                    this.EspcIsEnabled = true;
                }
                else
                {
                    this.EspcIsEnabled = false;
                }
            }
        }
        #endregion
        #region Commands
        public ICommand RegistrarCommand
        {
            get
            {
                return new RelayCommand(Registrar);
            }
        }

        private async void Registrar()
        {
            this.IsRunning = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Acceptar");
                return;
            }

            if (this.SelectedIndex == 1)
            {
                var response = await this.apiService.Post<Medico>(
                App.url_webservice,
                App.url_servicePrefix,
                "/Api/Medico",
                new Medico(0,this.Ci, this.Nombre, this.Apellido, this.Telefono, this.Especialidad, this.Codigo, this.Password));

                if (!response.IsSuccess)
                {
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                       "Error",
                        response.Message,
                        "Accept");
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
                }
            }
            else
            {
                var response = await this.apiService.Post<Paciente>(
                App.url_webservice,
                App.url_servicePrefix,
                "/Api/Paciente",
                new Paciente(0, this.Ci, this.Nombre, this.Apellido, this.Telefono, this.Codigo, this.Password));

                if (!response.IsSuccess)
                {
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                       "Error",
                        response.Message,
                        "Accept");
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
                }
                var paciente = (Paciente)response.Result;

                var response1 = await this.apiService.Post<Historial>(
                App.url_webservice,
                App.url_servicePrefix,
                "/Api/Historial",
                new Historial(0,paciente.Id,DateTime.Now,"Nuevo Historial"));

                if (!response.IsSuccess)
                {
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                       "Error",
                        response.Message,
                        "Accept");
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
                }
            }
            this.IsRunning = false;
            MainViewModel.GetInstance().Login = new LoginViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new Login());
        }
        #endregion
        #region Constructors
        public RegistroViewModel()
        {            
            this.EspcIsEnabled = false;
            this.apiService = new ApiService();
            this.Ci = String.Empty;
            this.Nombre = String.Empty;
            this.Apellido = String.Empty;
            this.Telefono = String.Empty;
            this.Especialidad = String.Empty;
            this.Codigo = String.Empty;
            this.Password = String.Empty;
        }
        #endregion
    }
}
