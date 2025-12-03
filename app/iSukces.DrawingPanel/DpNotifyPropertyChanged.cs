using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace iSukces.DrawingPanel;

public class DpNotifyPropertyChanged : INotifyPropertyChanged
{
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetAndNotify<T>(ref T backField, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (Equals(backField, value))
            return false;
        backField = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
