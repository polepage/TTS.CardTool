using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace TTS.CardTool.UI.ViewModel
{
    class ActionsViewModel: BindableBase
    {
        private DelegateCommand _convertCommand;
        public ICommand ConvertCommand => _convertCommand ??= new DelegateCommand(Convert);

        public void Convert()
        {

        }
    }
}
