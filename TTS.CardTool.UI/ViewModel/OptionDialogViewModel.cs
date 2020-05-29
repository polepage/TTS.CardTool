using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TTS.CardTool.Processor.Options;
using WPF.Utils.Dialogs;

namespace TTS.CardTool.UI.ViewModel
{
    class OptionDialogViewModel: BaseDialogViewModel
    {
        private readonly IProcessorOptions _options;

        public OptionDialogViewModel(IProcessorOptions options)
        {
            _options = options;
        }

        private DelegateCommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new DelegateCommand(Save);

        private IEnumerable<CardBackViewModel> _cardBacks;
        public IEnumerable<CardBackViewModel> CardBacks => _cardBacks ??= _options.CardBacks.Select(pair => new CardBackViewModel(pair.Key, pair.Value)).ToList();

        private IList<SetMapElementViewModel> _setMap;
        public IList<SetMapElementViewModel> SetMap => _setMap ??= _options.SetMap.Select(pair => new SetMapElementViewModel
        {
            Source = pair.Key,
            Target = pair.Value
        }).ToList();

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            Title = parameters.GetValue<string>(DialogParams.Title);
        }

        private void Save()
        {
            foreach (CardBackViewModel cardBack in CardBacks)
            {
                _options.CardBacks[cardBack.Key].Path = cardBack.Path;
            }

            _options.SetMap.Clear();
            foreach (SetMapElementViewModel mapElement in SetMap.Where(e => !string.IsNullOrWhiteSpace(e.Source) || !string.IsNullOrWhiteSpace(e.Target)))
            {
                _options.SetMap.Add(mapElement.Source, mapElement.Target);
            }

            _options.Save();
            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        public class CardBackViewModel: BindableBase
        {
            public CardBackViewModel(string key, CardBack back)
            {
                Key = key;
                Name = back.Name;
                Path = back.Path;
            }

            public string Key { get; }
            public string Name { get; }

            private string _path;
            public string Path
            {
                get => _path;
                set => SetProperty(ref _path, value);
            }
        }

        public class SetMapElementViewModel: BindableBase
        {
            private string _source;
            public string Source
            {
                get => _source;
                set => SetProperty(ref _source, value);
            }

            private string _target;
            public string Target
            {
                get => _target;
                set => SetProperty(ref _target, value);
            }
        }
    }
}
