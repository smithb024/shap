namespace Shap.Feedback.ViewModels
{
    using NynaeveLib.ViewModel;
    using Shap.Messages;
    using NynaeveMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// The view model which supports the feedback view.
    /// </summary>
    public class FeedbackViewModel : ViewModelBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FeedbackViewModel"/> class.
        /// </summary>
        public FeedbackViewModel()
        {
            NynaeveMessenger.Default.Register<FeedbackMessage>(this, this.RunPrimaryMessage);
        }

        /// <summary>
        /// A <see cref="FeedbackMessage"/> message has been recived from the messenger.
        /// </summary>
        /// <param name="message">The message</param>
        private void RunPrimaryMessage(FeedbackMessage message)
        {
        }
    }
}
