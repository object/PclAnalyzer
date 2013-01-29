using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace PclAnalyzer.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private string _assemblyPath;
        private bool _platformNet4;
        private bool _platformNet403;
        private bool _platformNet45;
        private bool _platformSL4;
        private bool _platformSL5;
        private bool _platformWP7;
        private bool _platformWP75;
        private bool _platformWP8;
        private bool _platformNetForWsa;
        private bool _platformXbox360;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                _assemblyPath = @"C:\Projects\PclAnalyzer\bin\Debug\PclAnalyzer.Core.dll";
                _platformNet403 = true;
                _platformNet45 = true;
                _platformSL5 = true;
                _platformWP8 = true;
            }
            else
            {
                _assemblyPath = @"C:\Projects\PclAnalyzer\bin\Debug\PclAnalyzer.Core.dll";
                _platformNet403 = true;
                _platformNet45 = true;
                _platformSL5 = true;
                _platformWP8 = true;
            }

            BrowseCommand = new RelayCommand(Browse, CanBrowse);
            AnalyzeCommand = new RelayCommand(Analyze, CanAnalyze);
        }

        public MainViewModel Self { get { return this; } }

        public RelayCommand BrowseCommand { get; private set; }

        public RelayCommand AnalyzeCommand { get; private set; }

        public string AssemblyPath
        {
            get { return _assemblyPath; }
            set { _assemblyPath = value; RaisePropertyChanged("AssemblyPath"); }
        }

        public bool PlatformNet4
        {
            get { return _platformNet4; }
            set { _platformNet4 = value; RaisePropertyChanged("PlatformNet4"); }
        }

        public bool PlatformNet403
        {
            get { return _platformNet403; }
            set { _platformNet403 = value; RaisePropertyChanged("PlatformNet403"); }
        }

        public bool PlatformNet45
        {
            get { return _platformNet45; }
            set { _platformNet45 = value; RaisePropertyChanged("PlatformNet45"); }
        }

        public bool PlatformSL4
        {
            get { return _platformSL4; }
            set { _platformSL4 = value; RaisePropertyChanged("PlatformSL4"); }
        }

        public bool PlatformSL5
        {
            get { return _platformSL5; }
            set { _platformSL5 = value; RaisePropertyChanged("PlatformSL5"); }
        }

        public bool PlatformWP7
        {
            get { return _platformWP7; }
            set { _platformWP7 = value; RaisePropertyChanged("PlatformWP7"); }
        }

        public bool PlatformWP75
        {
            get { return _platformWP75; }
            set { _platformWP75 = value; RaisePropertyChanged("PlatformWP75"); }
        }

        public bool PlatformWP8
        {
            get { return _platformWP8; }
            set { _platformWP8 = value; RaisePropertyChanged("PlatformWP8"); }
        }

        public bool PlatformNetForWsa
        {
            get { return _platformNetForWsa; }
            set { _platformNetForWsa = value; RaisePropertyChanged("PlatformNetForWsa"); }
        }

        public bool PlatformXbox360
        {
            get { return _platformXbox360; }
            set { _platformXbox360 = value; RaisePropertyChanged("PlatformXbox360"); }
        }

        private void Browse()
        {
            // TODO
            _assemblyPath = "Test";
        }

        private bool CanBrowse()
        {
            return true;
        }

        private void Analyze()
        {
            // TODO
        }

        private bool CanAnalyze()
        {
            return !string.IsNullOrEmpty(_assemblyPath) && File.Exists(_assemblyPath);
        }
    }
}