

namespace Taller.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using Taller.Models;
    using Taller.Services;
    using Taller.View;
    using Xamarin.Forms;

    public class PacienteItemViewModel : Paciente
    {
        #region Services
        private ApiService apiService;
        #endregion
        Historial historial;
        #region Commands
        public ICommand SelectPacienteHistorialCommand
        {
            get
            {
                return new RelayCommand(SelectPacienteHistorial);
            }
        }

        private async void SelectPacienteHistorial()
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
            App.var_paciente = this;
            var response = await this.apiService.Get<Historial>(
                "http://192.168.0.12",
                "/WebApi",
                "/Api/historial/"+this.Id);

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
            MainViewModel.GetInstance().Prediagnostico = new DetalleHistorialViewModel(this.historial);
            await Application.Current.MainPage.Navigation.PushAsync(new DetalleHistorialPage());

        }

        public PacienteItemViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion

    }
}
