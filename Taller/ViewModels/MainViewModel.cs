
namespace Taller.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Taller.Models;
    using Taller.Services;
    using Taller.View;
    using Xamarin.Forms;

    public class MainViewModel
    {

        #region Services
        private ApiService apiService =new ApiService();
        #endregion

        Historial historial;

        #region ViewModels
        public LoginViewModel Login
        {
            get;
            set;
        }
        public IndexViewModel Index
        {
            get;
            set;
        }
        public CargarImagenViewModel CargarImagen
        {
            get;
            set;
        }
        public VerHistorialViewModel Historial
        {
            get;
            set;
        }
        public DetalleHistorialViewModel Prediagnostico
        {
            get;
            set;
        }
        public ListaPacienteVieModel ListaPaciente
        {
            get;
            set;
        }
        public RegistroViewModel Registro
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
        }
        #endregion


        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        public ICommand VerHistorial
        {
            get
            {
                return new RelayCommand(GoToHistorial);
            }
        }

        private async void GoToHistorial()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            var response = await this.apiService.Get<Historial>(
                App.url_webservice,
                App.url_servicePrefix,
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
            this.historial = (Historial)response.Result;
            App.var_historial = this.historial;
            this.Prediagnostico = new DetalleHistorialViewModel(historial); 
            await Application.Current.MainPage.Navigation.PushAsync(new DetalleHistorialPage());
        }
    }
}