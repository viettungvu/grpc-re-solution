using GrpcModels.Exceptions;
using GrpcClient.State.Authenticators;
using GrpcClient.State.Navigators;
using GrpcClient.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GrpcClient.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _renavigator;

        public LoginCommand(LoginViewModel loginViewModel, IAuthenticator authenticator, IRenavigator renavigator)
        {
            _loginViewModel = loginViewModel;
            _authenticator = authenticator;
            _renavigator = renavigator;

            _loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return _loginViewModel.CanLogin && base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _loginViewModel.ErrorMessage = string.Empty;
            string msg = string.Empty;
            try
            {
                bool success = _authenticator.Login(_loginViewModel.Username, _loginViewModel.Password, out msg);
                if (success)
                {
                    _renavigator.Renavigate();
                }
            }
            catch (UserNotFoundException)
            {
                msg = "User không tồn tại";
            }
            catch (InvalidPasswordException)
            {
                msg = "Mật khẩu không chính xác";
            }
            catch (Exception)
            {
                msg = "Đăng nhập thất bại";
            }
            _loginViewModel.ErrorMessage = msg;
        }

        private void LoginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.CanLogin))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
