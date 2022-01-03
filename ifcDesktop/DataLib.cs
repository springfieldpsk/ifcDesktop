using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ifcDesktop
{
    class DataLib : INotifyPropertyChanged
    {
        private string _IFCFilePath = null;
        private string _csvFilePath = null;
        private string _csvFileName = null;
        private string _ThrList = null;
        private string _ColList = null;
        private string _CjlList = null;
        private float _ThresholdWidth = 0.2F;
        private float _ThresholdHeight = 0.2F;
        public event PropertyChangedEventHandler PropertyChanged;

        public string IFCFilePath
        {
            get { return _IFCFilePath; }
            set
            {
                _IFCFilePath = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IFCFilePath"));
                }
            }
        }

        public string csvFilePath
        {
            get { return _csvFilePath; }
            set
            {
                _csvFilePath = value; 
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("csvFilePath"));
                }
            }
        }

        public string csvFileName
        {
            get { return _csvFileName; }
            set
            {
                _csvFileName = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("csvFileName"));
                }
            }
        }

        public string ThrList
        {
            get { return _ThrList; }
            set
            {
                _ThrList = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ThrList"));
                }
            }
        }

        public string ColList
        {
            get { return _ColList; }
            set
            {
                _ColList = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ColList"));
                }
            }
        }

        public string CjlList
        {
            get { return _CjlList; }
            set
            {
                _CjlList = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("CjlList"));
                }
            }
        }

        public string ThresholdWidth
        {
            get { return _ThresholdWidth.ToString(); }
            set
            {
                _ThresholdWidth = float.Parse(value);
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ThresholdWidth"));
                }
            }
        }

        public string ThresholdHeight
        {
            get { return _ThresholdHeight.ToString(); }
            set
            {
                _ThresholdHeight = float.Parse(value);
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ThresholdHeight"));
                }
            }
        }
    }
}
