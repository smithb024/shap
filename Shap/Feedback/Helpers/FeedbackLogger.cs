namespace Shap.Feedback.Helpers
{
    using NynaeveLib.Logger;
    using NynaeveLib.Messenger;
    using Shap.Interfaces.Feedback.Helpers;
    using Shap.Messages;

    /// <summary>
    /// Class which is used to log the UI feedback.
    /// </summary>
    public class FeedbackLogger : IFeedbackLogger
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FeedbackLogger"/> class.
        /// </summary>
        public FeedbackLogger() 
        {
            Messenger.Default.Register<FeedbackMessage>(this, this.ReceiveFeedbackMessage);
        }

        /// <summary>
        /// Log the <paramref name="message"/> in the program log.
        /// </summary>
        /// <param name="message">The message</param>
        private void ReceiveFeedbackMessage(FeedbackMessage message)
        {
            Logger.Instance.WriteLog(
                $"{message.Priority}: {message.Message}");
        }
    }
}
