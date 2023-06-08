using System;
using System.Collections.Generic;
using System.Text;
using GrpcModels;
using GrpcClient.ViewModels;

namespace GrpcClient.ViewModels.Factories
{
    public interface IRSViewModelFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
