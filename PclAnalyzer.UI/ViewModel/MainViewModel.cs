using System;
using System.Collections.ObjectModel;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PclAnalyzer.Core;
using PclAnalyzer.UI.Services;

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
        private bool _allPlatforms;
        private bool _selectedPlatforms;
        private bool _platformNet4;
        private bool _platformNet403;
        private bool _platformNet45;
        private bool _platformNetForWsa;
        private bool _platformSL4;
        private bool _platformSL5;
        private bool _platformWP7;
        private bool _platformWP75;
        private bool _platformWP8;
        private bool _platformXbox360;
        private Platforms _requestedPlatforms;
        private bool _excludeThirdPartyLibraries;
        private ObservableCollection<MethodCall> _portableCalls = new ObservableCollection<MethodCall>();
        private ObservableCollection<MethodCall> _nonPortableCalls = new ObservableCollection<MethodCall>();

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
                _platformNetForWsa = true;
                _platformSL5 = true;
                _platformWP8 = true;
            }
            else
            {
            }

            _allPlatforms = true;

            BrowseCommand = new RelayCommand(Browse, CanBrowse);
            AnalyzeCommand = new RelayCommand(Analyze, CanAnalyze);
        }

        public RelayCommand BrowseCommand { get; private set; }
        public RelayCommand AnalyzeCommand { get; private set; }

        public string AssemblyPath
        {
            get { return _assemblyPath; }
            set { _assemblyPath = value; RaisePropertyChanged("AssemblyPath"); }
        }

        public bool AllPlatforms
        {
            get { return _allPlatforms; }
            set { _allPlatforms = value; RaisePropertyChanged("AllPlatforms"); }
        }

        public bool SelectedPlatforms
        {
            get { return _selectedPlatforms; }
            set { _selectedPlatforms = value; RaisePropertyChanged("SelectedPlatforms"); }
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

        public bool PlatformNetForWsa
        {
            get { return _platformNetForWsa; }
            set { _platformNetForWsa = value; RaisePropertyChanged("PlatformNetForWsa"); }
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

        public bool PlatformXbox360
        {
            get { return _platformXbox360; }
            set { _platformXbox360 = value; RaisePropertyChanged("PlatformXbox360"); }
        }

        public Platforms RequestedPlatforms
        {
            get { return _requestedPlatforms; }
            set { _requestedPlatforms = value; RaisePropertyChanged("RequestedPlatforms"); }
        }

        public bool ExcludeThirdPartyLibraries
        {
            get { return _excludeThirdPartyLibraries; }
            set { _excludeThirdPartyLibraries = value; RaisePropertyChanged("ExcludeThirdPartyLibraries"); }
        }

        public ObservableCollection<MethodCall> PortableCalls
        {
            get { return _portableCalls; }
            set { _portableCalls = value; RaisePropertyChanged("PortableCalls"); }
        }

        public ObservableCollection<MethodCall> NonPortableCalls
        {
            get { return _nonPortableCalls; }
            set { _nonPortableCalls = value; RaisePropertyChanged("NonPortableCalls"); }
        }

        private void Browse()
        {
            string assemblyPath = DialogService.SelectAssembly();
            if (!string.IsNullOrEmpty(assemblyPath))
            {
                this.AssemblyPath = assemblyPath;
            } 
        }

        private bool CanBrowse()
        {
            return true;
        }

        private void Analyze()
        {
            var analyzer = new AnalyzerService(this.AssemblyPath, this.RequestedPlatforms);
            this.PortableCalls = new ObservableCollection<MethodCall>(analyzer.GetPortableCalls());
            this.NonPortableCalls = new ObservableCollection<MethodCall>(analyzer.GetNonPortableCalls());
        }

        private bool CanAnalyze()
        {
            return !string.IsNullOrEmpty(_assemblyPath) && File.Exists(_assemblyPath);
        }
    }
}