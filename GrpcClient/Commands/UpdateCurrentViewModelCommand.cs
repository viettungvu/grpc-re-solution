using System;
using System.Windows.Input;
using GrpcModels;
using GrpcClient.State.Navigators;
using GrpcClient.ViewModels;
using GrpcClient.ViewModels.Factories;

namespace GrpcClient.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly INavigator _navigator;
        private readonly IRSViewModelFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(INavigator navigator, IRSViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;

                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }
    }
}