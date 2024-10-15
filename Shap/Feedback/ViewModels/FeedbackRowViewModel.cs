namespace Shap.Feedback.ViewModels
{
    using NynaeveLib.ViewModel;
    using Shap.Types.Enum;

    /// <summary>
    /// View model which supports a single row on the feedback window.
    /// </summary>
    public class FeedbackRowViewModel : ViewModelBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FeedbackRowViewModel"/> class.
        /// </summary>
        /// <param name="priority">The feedback priority</param>
        /// <param name="message">The feedback message</param>
        public FeedbackRowViewModel(
            FeedbackType priority,
            string message)
        {
            this.Priority = priority;
            this.Message = message;
        }

        /// <summary>
        /// Gets the priority of the feedback.
        /// </summary>
        public FeedbackType Priority { get; }

        /// <summary>
        /// Gets the message to feedback.
        /// </summary>
        public string Message { get; }
    }
}
