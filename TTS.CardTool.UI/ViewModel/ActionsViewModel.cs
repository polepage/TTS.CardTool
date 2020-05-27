using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Windows.Input;
using TTS.CardTool.Converter;
using TTS.CardTool.UI.Events;

namespace TTS.CardTool.UI.ViewModel
{
    class ActionsViewModel: BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDeckConverter _deckConverter;

        public ActionsViewModel(IEventAggregator eventAggregator, IDeckConverter deckConverter)
        {
            _eventAggregator = eventAggregator;
            _deckConverter = deckConverter;

            _eventAggregator.GetEvent<PostInput>().Subscribe(InputReceived);
        }

        private DelegateCommand _convertCommand;
        public ICommand ConvertCommand => _convertCommand ??= new DelegateCommand(Convert);

        public void Convert()
        {
            _eventAggregator.GetEvent<RequestInput>().Publish();
        }

        private void InputReceived(string input)
        {
            string result = _deckConverter.Convert(input);
            _eventAggregator.GetEvent<PostOutput>().Publish(result);
        }
    }
}
