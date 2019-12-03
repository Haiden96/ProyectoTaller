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
    using Taller.Services;
    using Plugin.Media.Abstractions;
    using Plugin.Media;

    public class CargarImagenViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private ImageSource imageSource;
        private MediaFile file;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }
        #endregion

        #region Commands
        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();
            var source = await Application.Current.MainPage.DisplayActionSheet(
                "De donde quiere tomar la imagen?",
                "Cancel",
                null,
                "Galeria",
                "Tomar Foto"
                );
            if (source == "Cancel")
            {
                this.file = null;
                return;
            }
            if (source == "Tomar Foto")
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = DateTime.Now + ".png",
                        PhotoSize = PhotoSize.Medium,
                    }
                    ); 
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();                
            }
            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }

        public ICommand ProcesarCommand
        {
            get
            {
                return new RelayCommand(Procesar);
            }
        }

        private async void Procesar()
        {
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
            this.IsRunning = true;
            this.IsEnabled = false;
            MainViewModel.GetInstance().Index = new IndexViewModel(this.file.GetStream());
            await Application.Current.MainPage.Navigation.PushAsync(new IndexPage());
            this.IsRunning = false;
            this.IsEnabled = true;
        }
        #endregion

        #region Constructors
        public CargarImagenViewModel()
        {
            this.ImageSource = "instagram_logo";
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }
        #endregion



    }

}

