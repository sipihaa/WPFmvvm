using System.Windows;
using WPFmvvm.Infrastructure.Commands.Base;

namespace WPFmvvm.Infrastructure.Commands
{
    internal class CloseApplicationCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}
