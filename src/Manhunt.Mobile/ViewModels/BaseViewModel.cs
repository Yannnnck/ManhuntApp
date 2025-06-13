using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Manhunt.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        bool _isBusy;
        string _error;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        public string Error
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return false;
            backingStore = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

