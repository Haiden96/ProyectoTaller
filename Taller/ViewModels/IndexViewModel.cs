using System;
using System.Collections.Generic;

using System.Text;

namespace Taller.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using IBM.Cloud.SDK.Core.Authentication.Iam;
    using IBM.Cloud.SDK.Core.Http;
    using IBM.Watson.VisualRecognition.v3;
    using IBM.Watson.VisualRecognition.v3.Model;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;
    using Taller.Models;
    using Taller.Services;
    using Xamarin.Forms;


    public class IndexViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<Enfermedad> enfermedad;
        private ObservableCollection<Resultado> resultado;
        private bool isRefreshing;
        private string filter;
        private List<Enfermedad> enfermedadList;
        #endregion

        #region Properties
        public ObservableCollection<Enfermedad> Enfermedad
        {
            get { return this.enfermedad; }
            set { SetValue(ref this.enfermedad, value); }
        }
        public ObservableCollection<Resultado> Resultado
        {
            get { return this.resultado; }
            set { SetValue(ref this.resultado, value); }
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
        public IndexViewModel(Stream path)
        {
            this.apiService = new ApiService();
            this.LoadDiaseses( path);
        }
        #endregion

        #region Methods
        private async void LoadDiaseses(Stream path)
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

            var response = await this.apiService.Procesar<Enfermedad>(path);

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
            
            byte[] myBinary = new byte[path.Length];
            Stream stream = new MemoryStream(myBinary);
            var responseCaptura = await this.apiService.Post<Captura>(
            App.url_webservice,
            App.url_servicePrefix,
            "/Api/Captura",
            new Captura(0, App.var_paciente.Id, new byte[2], ""));
            var cap = (Captura)responseCaptura.Result;

            this.enfermedadList = (List<Enfermedad>)response.Result;
            this.Enfermedad = new ObservableCollection<Enfermedad>(this.enfermedadList);
            this.IsRefreshing = false;
            foreach (Enfermedad e in enfermedadList)
            {
                var responsePrediagnostico = await this.apiService.Post<PreDiagnostico>(
                App.url_webservice,
                App.url_servicePrefix,
                "/Api/PreDiagnostico",
                new PreDiagnostico(0, cap.Id, App.var_historial.Id, e.Virus, e.Probabilidad, "Pendiente de verificacion", DateTime.Now, 0, 0));
            }
        }
        #endregion

        #region Commands
        //public ICommand RefreshCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(LoadDiaseses(""));
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
                this.Enfermedad = new ObservableCollection<Enfermedad>(
                    this.enfermedadList);
            }
            else
            {
                this.Enfermedad = new ObservableCollection<Enfermedad>(
                    this.enfermedadList.Where(
                        l => l.Virus.ToLower().Contains(this.Filter.ToLower())));
            }
        }
        #endregion
    }

}
