
using Ninject;
using Nuntium.Core;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace Nuntium
{
    public class OptionsSubmenuViewModel : BaseViewModel
    {

        #region Public properties

        public ObservableCollection<CultureInfo> LanguageList { get; set; }

        private CultureInfo _SelectedCultureGroup;
        public CultureInfo SelectedCultureGroup
        {
            get => _SelectedCultureGroup;

            set
            {
                if (_SelectedCultureGroup == value)
                    return;

                _SelectedCultureGroup = value;

                IoC.Kernel.Get<CustomRichTextBox>().Language = XmlLanguage.GetLanguage(_SelectedCultureGroup.IetfLanguageTag);
            }
        }

        #endregion

        public OptionsSubmenuViewModel()
        {

            LanguageList = PrepareLanguageList();

            SelectedCultureGroup = CultureInfo.CurrentCulture;

            PerformSpellCheckCommand = new RelayCommandWithParameter((param) => { PerformEditorSpellCheck(IoC.Kernel.Get<CustomRichTextBox>()); });

            ShowSearchSectionCommand = new RelayCommand(() => { IoC.Kernel.Get<IEventAggregator>().GetEvent<ToggleSearchSectionVisibilityEvent>().Publish(); });
        }

        #region Public Commands

        public ICommand ShowSearchSectionCommand { get; set; }

        #endregion

        #region Private Commands

        public ICommand PerformSpellCheckCommand { get; private set; }

        #endregion

        #region Public Command methods

        private void PerformEditorSpellCheck(RichTextBox editor, int startingPoint = 0)
        {
            var popup = new OffsetPopupFactory().CreatePopupOnPoint(editor.GetEditorPointToScreen());

            var spellCheckOptions = new SpellCheckOptions();

            spellCheckOptions.IgnoreCommand = new RelayCommand(() =>
            {
                EditingCommands.IgnoreSpellingError.Execute(null, editor);
                popup.IsOpen = false;
                PerformEditorSpellCheck(editor, startingPoint);
            });

            spellCheckOptions.CorrectionClicked = new RelayCommandWithParameter((correction) =>
            {
                EditingCommands.CorrectSpellingError.Execute(correction, editor);
                popup.IsOpen = false;
                PerformEditorSpellCheck(editor, startingPoint);
            });

            popup.Child = spellCheckOptions;
            spellCheckOptions.Items.Clear();

            var spellCheckResult = editor.SpellCheck(startingPoint);

            if (spellCheckResult.SpellingErrors == null)
                return;

            foreach (var option in spellCheckResult.SpellingErrors)
            {
                spellCheckOptions.Items.Add(option);
            }

            startingPoint = spellCheckResult.endPosition;

            popup.IsOpen = true;

        }

        #endregion

        private ObservableCollection<CultureInfo> PrepareLanguageList()
        {
            var langs = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
            langs.Sort((x, y) => string.Compare(x.DisplayName, y.DisplayName));
            return new ObservableCollection<CultureInfo>(langs);
        }
    }
}
