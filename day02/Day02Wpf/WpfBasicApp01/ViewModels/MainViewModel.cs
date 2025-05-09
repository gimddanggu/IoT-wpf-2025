using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;

namespace WpfBasicApp01.ViewModels
{
    public class MainViewModel : Conductor<Object>
    {
        // 메시지박스, 다이얼로그를 위한 변수
        private readonly IDialogCoordinator _dialogCoordinator;

        private string _greeting;
        public string Greeting 
        {
            get => _greeting;
            set
            {
                _greeting = value;
                NotifyOfPropertyChange(() => Greeting); // 속성값 바뀐 것을 알려줘야 함
            }
        }

        public MainViewModel(IDialogCoordinator dialogCoordinator) {
            _dialogCoordinator = dialogCoordinator;
            Greeting = "Hello, Calibrun.Micro!!";
        }

        public async Task SayHello()
        {
            Greeting = "Hi, everyone~~!!";
            // Winforms 방식
            //MessageBox.Show("Hi, Everyone~!", "Gretting", MessageBoxButton.OK, MessageBoxImage.Information);
            await _dialogCoordinator.ShowMessageAsync(this, "Greeting", "Hi, Everyone~!");
        }
    }
}
