
namespace Taller.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Taller.View;
    using Xamarin.Forms;

    public class MainViewModel
    {
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
            this.Prediagnostico = new DetalleHistorialViewModel(0); 
            await Application.Current.MainPage.Navigation.PushAsync(new DetalleHistorialPage());
        }
    }
}