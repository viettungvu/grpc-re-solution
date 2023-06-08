using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GrpcClient.ViewModels;

namespace GrpcClient.State.Navigators
{
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        event Action StateChanged;
    }
}
