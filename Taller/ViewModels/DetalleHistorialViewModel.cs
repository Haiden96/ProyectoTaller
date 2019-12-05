

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
    public class DetalleHistorialViewModel : BaseViewModel
    {

        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<PreDiagnostico> detalleHistorial;
        private bool isRefreshing;
        private bool isRunning;
        private string filter;
        private List<PreDiagnostico> detalleHistorials;
        #endregion

        #region Properties
        public ObservableCollection<PreDiagnostico> DetalleHistorial
        {
            get { return this.detalleHistorial; }
            set { SetValue(ref this.detalleHistorial, value); }
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
        public DetalleHistorialViewModel(int id)
        {
            this.apiService = new ApiService();
            this.LoadDetalleHistorial(1);
        }
        #endregion

        #region Methods
        private async void LoadDetalleHistorial(int id)
        {
            this.IsRefreshing = true;
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

            var response = await this.apiService.GetList<PreDiagnostico>(
                "http://192.168.0.12",
                "/WebApi",
                "/Api/PreDiagnostico");

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

            this.detalleHistorials = (List<PreDiagnostico>)response.Result;
            this.DetalleHistorial = new ObservableCollection<PreDiagnostico>(this.detalleHistorials);
            this.IsRefreshing = false;
            this.IsRunning = false;
        }
        #endregion

        #region Commands
        //public ICommand RefreshCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(LoadHistorial(0));
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
                this.DetalleHistorial = new ObservableCollection<PreDiagnostico>(
                    this.detalleHistorials);
            }
            else
            {
                this.DetalleHistorial = new ObservableCollection<PreDiagnostico>(
                    this.detalleHistorials.Where(
                        l => l.Glosa.ToLower().Contains(this.Filter.ToLower()) || l.Id.ToString().Equals(this.Filter)));
            }
        }
        #endregion
    }
}