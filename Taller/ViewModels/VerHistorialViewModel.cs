

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

    public class VerHistorialViewModel:BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<Historial> historail;
        private ObservableCollection<DetalleHistorial> detalleHistorial;
        private bool isRefreshing;
        private string filter;
        private List<DetalleHistorial> detalleHistorials;
        #endregion

        #region Properties
        public ObservableCollection<Historial> Historial
        {
            get { return this.historail; }
            set { SetValue(ref this.historail, value); }
        }
        public ObservableCollection<DetalleHistorial> DetalleHistorial
        {
            get { return this.detalleHistorial; }
            set { SetValue(ref this.detalleHistorial, value); }
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
        public VerHistorialViewModel(int id)
        {
            this.apiService = new ApiService();
            this.LoadHistorial(1);
        }
        #endregion

        #region Methods
        private async void LoadHistorial(int id)
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

            var response = await this.apiService.GetList<Enfermedad>(
                "http://restcountries.eu",
                "/rest",
                "/v2/all");

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

            this.detalleHistorials = (List<DetalleHistorial>)response.Result;
            this.DetalleHistorial = new ObservableCollection<DetalleHistorial>(this.detalleHistorials);
            this.IsRefreshing = false;
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
                this.DetalleHistorial = new ObservableCollection<DetalleHistorial>(
                    this.detalleHistorial);
            }
            else
            {
            //    this.DetalleHistorial = new ObservableCollection<DetalleHistorial>(
            //        this.detalleHistorial.Where(
            //            l => l.Virus.ToLower().Contains(this.Filter.ToLower())));
            }
        }
        #endregion
    }
}
