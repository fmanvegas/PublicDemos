using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SuperheroDatabaseCreator
{
    public class CreatorDisplayProperties : INotifyPropertyChanged
    {
        private CreatorStateType _state = CreatorStateType.Idle;
        private int _maximum = 1;
        private int _current = 0;
        private string _errorMessage = string.Empty;

        public int Maximum { get => _maximum; set { _maximum = value; OnPropertyChanged(); } }
        public int Current { get => _current; set { _current = value; OnPropertyChanged(); } }
        public CreatorStateType State { get { return _state; } set { _state = value; OnPropertyChanged(); } }
        public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value; OnPropertyChanged(); } }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        internal void Reset()
        {
            Current = 0;
            State = CreatorStateType.Idle;
            ErrorMessage = string.Empty;
        }
    }
}
