using STEGANOMIX.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace STEGANOMIX.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        private MethodHomografyViewModel _homografyViewModel;
        private MethodNovelApproachViewModel _novelApproachViewModel;
        private MethodPolishViewModel _polishViewModel;
        private MethodWhiteTextViewModel _whiteTextViewModel;
        private MethodYCBCRViewModel _YCBCRViewModel;
        private MethodSpojnikiZnacznikiViewModel _methodSpojnikiZnacznikiViewModel;
        private MethodSpojnikiSzablonViewModel _methodSpojnikiSzablonViewModel;
        private ICommand _navigateHomografyCommand;
        private ICommand _navigateNovelApproachCommand;
        private ICommand _navigatePolishMethodCommand;
        private ICommand _navigateWhiteTextCommand;
        private ICommand _navigateYCBCRCommand;
        private ICommand _navigateSpojnikiZnacznikiCommand;
        private ICommand _navigateSpojnikiSzablonCommand;

        public MainWindowViewModel()
        {
            _navigateHomografyCommand = new RelayCommand(x => NavigateHomografy());
            _navigateNovelApproachCommand = new RelayCommand(x => NavigateNovelApproach());
            _navigatePolishMethodCommand = new RelayCommand(x => NavigatePolishMethod());
            _navigateWhiteTextCommand = new RelayCommand(x => NavigateWhiteText());
            _navigateYCBCRCommand = new RelayCommand(x => NavigateYCBCR());
            _navigateSpojnikiZnacznikiCommand = new RelayCommand(x => NavigateSpojnikiZnaczniki());
            _navigateSpojnikiSzablonCommand = new RelayCommand(x => NavigateSpojnikiSzablon());

            _homografyViewModel = new MethodHomografyViewModel();
            _novelApproachViewModel = new MethodNovelApproachViewModel();
            _polishViewModel = new MethodPolishViewModel();
            _whiteTextViewModel = new MethodWhiteTextViewModel();
            _YCBCRViewModel = new MethodYCBCRViewModel();
            _methodSpojnikiZnacznikiViewModel = new MethodSpojnikiZnacznikiViewModel();
            _methodSpojnikiSzablonViewModel = new MethodSpojnikiSzablonViewModel();
    }


        private void NavigateYCBCR()
        {
            CurrentViewModel = _YCBCRViewModel;
        }

        private void NavigateHomografy()
        {
            CurrentViewModel = _homografyViewModel;
        }

        private void NavigatePolishMethod()
        {
            CurrentViewModel = _polishViewModel;
        }

        private void NavigateNovelApproach()
        {
            CurrentViewModel = _novelApproachViewModel;
        }

        private void NavigateWhiteText()
        {
            CurrentViewModel = _whiteTextViewModel;
        }

        private void NavigateSpojnikiSzablon()
        {
            CurrentViewModel = _methodSpojnikiSzablonViewModel;
        }

        private void NavigateSpojnikiZnaczniki()
        {
            CurrentViewModel = _methodSpojnikiZnacznikiViewModel;
        }



        public ICommand NavigateHomografyCommand { get { return _navigateHomografyCommand; } }
        public ICommand NavigateNovelApproachCommand { get { return _navigateNovelApproachCommand; } }
        public ICommand NavigatePolishMethodCommand { get { return _navigatePolishMethodCommand; } }
        public ICommand NavigateWhiteTextCommand { get { return _navigateWhiteTextCommand; } }
        public ICommand NavigateYCBCRCommand { get { return _navigateYCBCRCommand; } }
        public ICommand NavigateSpojnikiZnacznikiCommand { get { return _navigateSpojnikiZnacznikiCommand; } }
        public ICommand NavigateSpojnikiSzablonCommand { get { return _navigateSpojnikiSzablonCommand; } }



        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
