namespace Shap.Feedback.ViewModels
{
    using NynaeveLib.ViewModel;
    using Shap.Messages;
    using System.Collections.ObjectModel;
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
            this.Rows = new ObservableCollection<FeedbackRowViewModel>();
            NynaeveMessenger.Default.Register<FeedbackMessage>(this, this.RunPrimaryMessage);
        }

        /// <summary>
        /// Gets the collection of rows to display on the view model.
        /// </summary>
        public ObservableCollection<FeedbackRowViewModel> Rows { get; set; }

        /// <summary>
        /// A <see cref="FeedbackMessage"/> message has been recived from the messenger.
        /// Insert it at the top of the feedback window.
        /// </summary>
        /// <remarks>
        /// The feedback window only supports 10 rows, therefore if this results in 11 rows, ensure
        /// that the oldest one is deleted.
        /// </remarks>
        /// <param name="message">The message</param>
        private void RunPrimaryMessage(FeedbackMessage message)
        {
            if (this.Rows.Count == 10)
            {
                this.Rows.RemoveAt(9);
            }

            FeedbackRowViewModel newRow =
                new FeedbackRowViewModel(
                    message.Priority,
                    message.Message);

            this.Rows.Insert(0, newRow);
        }
    }
}
