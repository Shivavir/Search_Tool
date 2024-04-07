using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Search_Tool
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _selectedWebsite;
        public string SelectedWebsite
        {
            get { return _selectedWebsite; }
            set
            {
                _selectedWebsite = value;
                OnPropertyChanged();
            }
        }

        private string _searchResult;
        public string SearchResult
        {
            get { return _searchResult; }
            set
            {
                _searchResult = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Websites { get; set; }

        public MainViewModel()
        {
            Websites = new ObservableCollection<string>
            {
                "Google",
                "Bing",
                "Yahoo",
                "DuckDuckGo",
                "Stack Overflow"
            };
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
