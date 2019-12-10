

namespace Taller.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using Taller.Models;
    using Taller.Services;
    using Xamarin.Forms;

    public class ListaPacienteVieModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<PacienteItemViewModel> paciente;
        private List<Paciente> pacientes;
        private bool isRunning;
        private bool isRefreshing;
        private string filter;
        #endregion

        #region Properties
        public ObservableCollection<PacienteItemViewModel> Paciente
        {
            get { return this.paciente; }
            set { SetValue(ref this.paciente, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                this.Search();
            }
        }
        #endregion


        #region Constructors
        public ListaPacienteVieModel()
        {
            this.apiService = new ApiService();
            this.LoadPacientes();
        }
        #endregion

        #region Methods
        private async void LoadPacientes()
        {
            this.IsRefreshing = false;
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            this.IsRunning = true;
            var response = await this.apiService.GetList<Paciente>(
                "http://192.168.0.12",
                "/WebApi",
                "/Api/Paciente");

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                   "Error",
                    response.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            this.pacientes = (List<Paciente>)response.Result;
            this.Paciente = new ObservableCollection<PacienteItemViewModel>(
                this.ToPacienteItemVieModel(this.pacientes));
            this.IsRefreshing = false;
            this.IsRunning = false;
        }

        #region Metodos
        private IEnumerable<PacienteItemViewModel> ToPacienteItemVieModel(List<Paciente> pacientes)
        {
            return this.pacientes.Select(l => new PacienteItemViewModel
            {
                Id=l.Id,
                Ci = l.Ci,
                Nombres = l.Nombres,
                Apellidos = l.Apellidos,
                Telefono = l.Telefono,
                Codigo = l.Codigo,
                Password = l.Password,

            });

            
        } 
        #endregion

        #endregion

        #region Commands
        //public ICommand RefreshCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(LoadPacientes());
        //    }
        //}
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Paciente = new ObservableCollection<PacienteItemViewModel>(
                    this.ToPacienteItemVieModel(this.pacientes));
            }
            else
            {
                this.Paciente = new ObservableCollection<PacienteItemViewModel>(
                     this.ToPacienteItemVieModel(this.pacientes).Where(
                        l => l.Nombres.ToLower().Contains(this.Filter.ToLower())|| 
                        l.Apellidos.ToLower().Contains(this.Filter.ToLower()) || 
                        l.Ci.ToLower().Contains(this.Filter.ToLower())));
            }
        }
        #endregion
    }

}