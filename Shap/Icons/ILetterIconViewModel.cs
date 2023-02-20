namespace Shap.Icon
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Interface for the view model which supports the letter icon view.
    /// </summary>
    public interface ILetterIconViewModel
    {
        /// <summary>
        /// Gets the current day.
        /// </summary>
        string Character { get; }

        /// <summary>
        /// Gets or sets a value which indicates whether this day is selected.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the callback to send the current character
        /// </summary>
        Action<string> NewCharacterCallback { get; set; }

        /// <summary>
        /// Show add jny and reset.
        /// </summary>
        ICommand SelectCharacterCmd { get; }
    }
}