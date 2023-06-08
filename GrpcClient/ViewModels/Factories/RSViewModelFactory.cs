using System;
using System.Collections.Generic;
using System.Text;
using GrpcModels;

namespace GrpcClient.ViewModels.Factories
{
    public class RSViewModelFactory : IRSViewModelFactory
    {
        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
        //private readonly CreateViewModel<PortfolioViewModel> _createPortfolioViewModel;
        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        // private readonly CreateViewModel<BuyViewModel> _createBuyViewModel;
        //private readonly CreateViewModel<SellViewModel> _createSellViewModel;

        public RSViewModelFactory(CreateViewModel<HomeViewModel> createHomeViewModel, CreateViewModel<LoginViewModel> createLoginViewModel)
        {
            _createHomeViewModel = createHomeViewModel;
            //_createPortfolioViewModel = createPortfolioViewModel;
            _createLoginViewModel = createLoginViewModel;
            //_createBuyViewModel = createBuyViewModel;
            //_createSellViewModel = createSellViewModel;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Login:
                    return _createLoginViewModel();
                case ViewType.Home:
                    return _createHomeViewModel();
                case ViewType.Portfolio:
                //return _createPortfolioViewModel();
                case ViewType.Buy:
                //return _createBuyViewModel();
                case ViewType.Sell:
                //return _createSellViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }
    }
}
