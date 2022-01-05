using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ifcDesktop
{
    class subWinLib : INotifyPropertyChanged
    {
        private string _TextView = null;

        public event PropertyChangedEventHandler PropertyChanged;
        public string TextView
        {
            get { return _TextView; }
            set
            {
                _TextView = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("TextView"));
                }
            }
        }
    }
}
