using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taller.ViewModels;
using Xamarin.Forms;

namespace Taller.Infraestructure
{
    public class InstanceLocator
    {
        #region Properties
        public MainViewModel Main
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
        #endregion
    }
}