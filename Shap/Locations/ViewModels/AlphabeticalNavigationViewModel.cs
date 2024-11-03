namespace Shap.Locations.ViewModels
{
    using System;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Shap.Icon;
    using Shap.Interfaces.Locations.ViewModels;
    using System.Collections.ObjectModel;
    using Shap.Locations.Messages;
    using Shap.Messages;
    using Shap.Types.Enum;
    using NynaeveMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model which supports the selection of locations by first letter.
    /// </summary>
    public class AlphabeticalNavigationViewModel : ObservableRecipient, IAlphabeticalNavigationViewModel
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="AlphabeticalNavigationViewModel"/> class.
        /// </summary>
        public AlphabeticalNavigationViewModel()
        {
            this.Letters = new ObservableCollection<ILetterIconViewModel>();

            this.SetupLetterIcon("A");
            this.SetupLetterIcon("B");
            this.SetupLetterIcon("C");
            this.SetupLetterIcon("D");
            this.SetupLetterIcon("E");
            this.SetupLetterIcon("F");
            this.SetupLetterIcon("G");
            this.SetupLetterIcon("H");
            this.SetupLetterIcon("I");
            this.SetupLetterIcon("J");
            this.SetupLetterIcon("K");
            this.SetupLetterIcon("L");
            this.SetupLetterIcon("M");
            this.SetupLetterIcon("N");
            this.SetupLetterIcon("O");
            this.SetupLetterIcon("P");
            this.SetupLetterIcon("Q");
            this.SetupLetterIcon("R");
            this.SetupLetterIcon("S");
            this.SetupLetterIcon("T");
            this.SetupLetterIcon("U");
            this.SetupLetterIcon("V");
            this.SetupLetterIcon("W");
            this.SetupLetterIcon("X");
            this.SetupLetterIcon("Y");
            this.SetupLetterIcon("Z");
        }

        /// <summary>
        /// Gets the collection of letter icons.
        /// </summary>
        public ObservableCollection<ILetterIconViewModel> Letters { get; }

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the class.
        /// </summary>
        /// <param name="disposing">Is the class being disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach(ILetterIconViewModel viewModel in this.Letters)
                {
                    viewModel.NewCharacterCallback -= this.SetNewCharacter;
                }
            }
        }

        /// <summary>
        /// Set up a new character icon.
        /// </summary>
        /// <param name="character">The character being set up.</param>
        private void SetupLetterIcon(string character)
        {
            ILetterIconViewModel viewModel =
                new LetterIconViewModel(
                    character);

            this.Letters.Add(viewModel);

            viewModel.NewCharacterCallback += this.SetNewCharacter;
        }

        /// <summary>
        /// A character icon has been selected.
        /// </summary>
        /// <param name="character">The character which has been selected</param>
        private void SetNewCharacter(string character)
        {
            foreach(ILetterIconViewModel icon in this.Letters)
            {
                icon.IsSelected =
                    string.Compare(icon.Character, character) == 0;
            }

            FeedbackMessage feedbackMessage =
                new FeedbackMessage(
                    FeedbackType.Navigation,
                    $"LocationIndex - Display locations starting with {character}.");
            NynaeveMessenger.Default.Send(feedbackMessage);

            AlphaSelectorMessage message =
                new AlphaSelectorMessage(
                    character);

            this.Messenger.Send(message);
        }
    }
}