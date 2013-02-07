using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PclAnalyzer.Core;
using PclAnalyzer.UI.Services;

namespace PclAnalyzer.UI.ViewModel
{
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
        private bool _excludeThirdPartyLibraries;
        private ObservableCollection<CallInfo> _portableCalls = new ObservableCollection<CallInfo>();
        private ObservableCollection<CallInfo> _nonPortableCalls = new ObservableCollection<CallInfo>();
        private string _portableCallsLabel;
        private string _nonPortableCallsLabel;
        private bool _isBusy;

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
                _portableCallsLabel = "Portable calls:";
                _nonPortableCallsLabel = "Non-portable calls:";
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

        public bool ExcludeThirdPartyLibraries
        {
            get { return _excludeThirdPartyLibraries; }
            set { _excludeThirdPartyLibraries = value; RaisePropertyChanged("ExcludeThirdPartyLibraries"); }
        }

        public ObservableCollection<CallInfo> PortableCalls
        {
            get { return _portableCalls; }
            set { _portableCalls = value; RaisePropertyChanged("PortableCalls"); }
        }

        public ObservableCollection<CallInfo> NonPortableCalls
        {
            get { return _nonPortableCalls; }
            set { _nonPortableCalls = value; RaisePropertyChanged("NonPortableCalls"); }
        }

        public string PortableCallsLabel
        {
            get { return _portableCallsLabel; }
            set { _portableCallsLabel = value; RaisePropertyChanged("PortableCallsLabel"); }
        }

        public string NonPortableCallsLabel
        {
            get { return _nonPortableCallsLabel; }
            set { _nonPortableCallsLabel = value; RaisePropertyChanged("NonPortableCallsLabel"); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged("IsBusy"); }
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
            this.IsBusy = true;
            this.PortableCalls.Clear();
            this.PortableCallsLabel = string.Format("Portable calls ({0}):", "computing...");
            this.NonPortableCalls.Clear();
            this.NonPortableCallsLabel = string.Format("Non-portable calls ({0}):", "computing...");

            Task.Factory.StartNew(RunAnalyzer).ContinueWith((t) =>
            {
                this.IsBusy = false;
            }, 
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void RunAnalyzer()
        {
            var requestedPlatforms = Platforms.None;
            if (this.AllPlatforms)
            {
                requestedPlatforms = Platforms.AllKnown;
            }
            else
            {
                if (this.PlatformNet4) requestedPlatforms |= Platforms.Net4;
                if (this.PlatformNet403) requestedPlatforms |= Platforms.Net403;
                if (this.PlatformNet45) requestedPlatforms |= Platforms.Net45;
                if (this.PlatformNetForWsa) requestedPlatforms |= Platforms.NetForWsa;
                if (this.PlatformSL4) requestedPlatforms |= Platforms.SL4;
                if (this.PlatformSL5) requestedPlatforms |= Platforms.SL5;
                if (this.PlatformWP7) requestedPlatforms |= Platforms.WP7;
                if (this.PlatformWP75) requestedPlatforms |= Platforms.WP75;
                if (this.PlatformWP8) requestedPlatforms |= Platforms.WP8;
                if (this.PlatformXbox360) requestedPlatforms |= Platforms.Xbox360;
            }
            var analyzer = new AnalyzerService(this.AssemblyPath, requestedPlatforms, this.ExcludeThirdPartyLibraries);
            this.PortableCalls = new ObservableCollection<CallInfo>(analyzer.GetPortableCalls());
            this.PortableCallsLabel = string.Format("Portable calls ({0}):", this.PortableCalls.Count);
            this.NonPortableCalls = new ObservableCollection<CallInfo>(analyzer.GetNonPortableCalls());
            this.NonPortableCallsLabel = string.Format("Non-portable calls ({0}):", this.NonPortableCalls.Count);

            this.IsBusy = false;
        }

        private bool CanAnalyze()
        {
            return !string.IsNullOrEmpty(_assemblyPath) && File.Exists(_assemblyPath);
        }
    }
}