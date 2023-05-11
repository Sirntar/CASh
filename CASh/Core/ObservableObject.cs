using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CASh.Core
{
    /**
     * 
     * Class will notify if property will change.
     * Basically it will allow to bind value change of an object from the code.
     * 
    **/
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
