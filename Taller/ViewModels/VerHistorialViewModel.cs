

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
        private ObservableCollection<Historial> historial;
        private bool isRefreshing;
        private bool isRunning;
        private string filter;
        private List<Historial> historials;
        #endregion

        #region Properties
        public ObservableCollection<Historial> Historial
        {
            get { return this.historial; }
            set { SetValue(ref this.historial, value); }
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
            this.IsRunning = true;

            var response = await this.apiService.GetList<Historial>(
                App.url_webservice,
                App.url_servicePrefix,
                "/Api/historial");

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

            this.historials = (List<Historial>)response.Result;
            this.Historial = new ObservableCollection<Historial>(this.historials);
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
                this.Historial = new ObservableCollection<Historial>(
                    this.historials);
            }
            else
            {
                this.Historial = new ObservableCollection<Historial>(
                    this.historials.Where(
                        l => l.Detalle.ToLower().Contains(this.Filter.ToLower())||  l.Id.ToString().Equals(this.Filter)));
            }
        }
        #endregion
    }
}
