
using Ninject;
using Nuntium.Core;
using Prism.Events;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Nuntium
{
    public class SearchSectionViewModel : BaseViewModel
    {
        #region Public Properties

        public string Phrase { get; set; }

        public bool IsVisible { get; set; }

        #endregion

        public SearchSectionViewModel()
        {
            SearchCommand = new RelayCommandWithParameter((param) => { Search(IoC.Kernel.Get<CustomRichTextBox>().Document); });

            IoC.Kernel.Get<IEventAggregator>().GetEvent<ToggleSearchSectionVisibilityEvent>().Subscribe(() => IsVisible ^= true);
        }

        #region Private Command

        public ICommand ShowOptionsCommand { get; private set; }

        public ICommand ReplaceCommand { get; private set; }

        public ICommand ReplaceAllCommand { get; private set; }

        public ICommand GoToPrevCommand { get; private set; }

        public ICommand GoToNextCommand { get; private set; }

        public ICommand SearchCommand { get; private set; }

        #endregion

        #region Command Methods

        private void Search(FlowDocument document)
        {

            if (document == null || string.IsNullOrWhiteSpace(Phrase))
                return;

            TextPointer start = document.ContentStart;

            while (start != null)
            {
                string textInRun = start.GetTextInRun(LogicalDirection.Forward);

                if (!string.IsNullOrWhiteSpace(textInRun))
                {
                    int index = textInRun.IndexOf(Phrase);

                    if (index != -1)
                    {
                        TextPointer selectionStart = start.GetPositionAtOffset(index, LogicalDirection.Forward);
                        TextPointer selectionEnd = selectionStart.GetPositionAtOffset(Phrase.Length, LogicalDirection.Forward);

                        //TODO: highlight selected
                    }
                }
                start = start.GetNextContextPosition(LogicalDirection.Forward);
            }

        }

        #endregion
    }
}
