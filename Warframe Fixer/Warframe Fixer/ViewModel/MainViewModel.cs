using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Warframe_Fixer.Model;

namespace Warframe_Fixer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IPatcher _patcher;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        private string _steamId;

        public string SteamId
        {
            get => _steamId;
            set => Set(ref _steamId, value);
        }

        private string _steamId64;

        public string SteamId64
        {
            get => _steamId64;
            set => Set(ref _steamId64, value);
        }

        private ObservableCollection<string> _logEntries = new ObservableCollection<string>();

        public ObservableCollection<string> LogEntries
        {
            get => _logEntries;
            set => Set(ref _logEntries, value);
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService, IPatcher patcher)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            _patcher = patcher;
            LogEntries.Add("Hello, Tenno.");
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}

        private RelayCommand _fixCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand FixCommand
        {
            get
            {
                return _fixCommand
                    ?? (_fixCommand = new RelayCommand(
                    () =>
                    {
                        LogEntries.Add("Trying to fix...");
                        Task.Run(() =>
                        {
                            _patcher.SteamId = SteamId;
                            _patcher.Patch();
                            SteamId64 = _patcher.SteamId64;
                        });
                        LogEntries.Add("Fix completed.");
                    }));
            }
        }
    }
}