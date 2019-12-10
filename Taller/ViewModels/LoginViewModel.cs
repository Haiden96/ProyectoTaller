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
    using Taller.Models;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class LoginViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes

        private ObservableCollection<Medico> medico;
        private List<Medico> medicos;
        private ObservableCollection<Paciente> paciente;
        private List<Paciente> pacientes;
        private string email;
        private string password;
        private bool isRunning;
        private int selectedindex;
        private bool isEnabled;
        #endregion

        #region Properties

        public ObservableCollection<Medico> Medico
        {
            get { return this.medico; }
            set { SetValue(ref this.medico, value); }
        }
        public ObservableCollection<Paciente> Paciente
        {
            get { return this.paciente; }
            set { SetValue(ref this.paciente, value); }
        }

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

        public int SelectedIndex
        {
            get { return this.selectedindex; }
            set { SetValue(ref this.selectedindex, value); }
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
        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Registro);
            }
        }
        private async void Registro()
        {
            MainViewModel.GetInstance().Registro = new RegistroViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegistroPage());
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
            this.SelectedIndex = this.selectedindex;
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

            if (this.SelectedIndex == 1)
            {
                var response = await this.apiService.GetList<Medico>(
                "http://192.168.0.12",
                "/WebApi",
                "/Api/Medico");

                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                       "Error",
                        response.Message,
                        "Accept");
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
                }

                this.medicos = (List<Medico>)response.Result;
                this.Medico = new ObservableCollection<Medico>(this.medicos);
                this.Medico = new ObservableCollection<Medico>(
                    this.medico.Where(
                        l => l.Ci.ToLower().Contains(this.email.ToLower()) && l.Password.ToString().Equals(this.Password)));

            }
            else
            {
                var response = await this.apiService.GetList<Paciente>(
                "http://192.168.0.12",
                "/WebApi",
                "/Api/Paciente");

                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                       "Error",
                        response.Message,
                        "Accept");
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
                }
                this.pacientes = (List<Paciente>)response.Result;
                this.Paciente = new ObservableCollection<Paciente>(this.pacientes);
                this.Paciente = new ObservableCollection<Paciente>(
                    this.paciente.Where(
                        l => l.Ci.ToLower().Contains(this.email.ToLower()) && l.Password.ToString().Equals(this.Password)));

            }
            if (this.Paciente != null)
            {
                if (this.Paciente.Count > 0)
                {
                    App.var_paciente = this.Paciente.First();
                    var response = await this.apiService.Get<Historial>(
                        "http://192.168.0.12",
                        "/WebApi",
                        "/Api/historial/" + App.var_paciente.Id);

                    if (!response.IsSuccess)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                           "Error",
                            response.Message,
                            "Accept");
                        await Application.Current.MainPage.Navigation.PopAsync();
                        return;
                    }
                    var historial = (Historial)response.Result;
                    App.var_historial = historial;
                    
                    MainViewModel.GetInstance().CargarImagen = new CargarImagenViewModel();
                    //mainViewModel.Token = token;
                    //mainViewModel.Index = new IndexViewModel();
                    await Application.Current.MainPage.Navigation.PushAsync(new CargarImagenPage());
                    this.Email = String.Empty;
                    this.Password = String.Empty;
                    this.Paciente = null;
                }
            }
            else if (this.Medico != null)
            {
                if (this.Medico.Count > 0)
                {
                    App.var_medico = this.Medico.First();
                    MainViewModel.GetInstance().ListaPaciente = new ListaPacienteVieModel();
                    //mainViewModel.Token = token;
                    //mainViewModel.Index = new IndexViewModel();
                    await Application.Current.MainPage.Navigation.PushAsync(new ListaPacientePage());
                    this.Email = String.Empty;
                    this.Password = String.Empty;
                    this.Medico = null;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                       "Error",
                        "Verifique las credenciales de acceso",
                        "Acceptar");
            }
            this.IsRunning = false;
            this.IsEnabled = true;

        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.Email = String.Empty;
            this.Password = String.Empty;
            this.IsRemembered = true;
            this.IsEnabled = true;
        }
        #endregion



    }

}
