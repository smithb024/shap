namespace Shap.Icon
{
    using System;
    using System.Windows.Input;
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Common.Commands;

    /// <summary>
    /// View model which supports the letter icon view.
    /// </summary>
    public class LetterIconViewModel : ObservableRecipient, ILetterIconViewModel
    {
        /// <summary>
        /// Character callback
        /// </summary>
        private Action<string> newCharacterCallback;

        /// <summary>
        /// day is selected.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Initialise a new instance of the <see cref="LetterIconViewModel"/> class.
        /// </summary>
        /// <param name="character">The character to display on the icon</param>
        public LetterIconViewModel(
          string character)
        {
            this.Character = character;
            this.isSelected = false;
            this.SelectCharacterCmd = new CommonCommand(this.SendCurrentCharacter);
        }

        /// <summary>
        /// Gets the current day.
        /// </summary>
        public string Character { get; }

        /// <summary>
        /// Gets or sets a value which indicates whether this day is selected.
        /// </summary>
        public bool IsSelected
        {
            get => this.isSelected;

            set
            {
                this.isSelected = value;
                OnPropertyChanged(nameof(this.IsSelected));
            }
        }

        /// <summary>
        /// Gets or sets the callback to send the current character
        /// </summary>
        public Action<string> NewCharacterCallback
        {
            get => this.newCharacterCallback;

            set => this.newCharacterCallback = value;
        }

        /// <summary>
        /// Show add jny and reset.
        /// </summary>
        public ICommand SelectCharacterCmd { get; }

        /// <summary>
        /// Send the current date via <see cref="NewDateCallback"/>.
        /// </summary>
        private void SendCurrentCharacter()
        {
            if (this.NewCharacterCallback != null)
            {
                this.NewCharacterCallback.Invoke(this.Character);
            }
        }
    }
}