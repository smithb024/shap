namespace Shap.Messages
{
    using Shap.Types.Enum;

    /// <summary>
    /// Message class which is used to report feedback.
    /// </summary>
    public class FeedbackMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FeedbackMessage"/> class.
        /// </summary>
        /// <param name="priority">The feedback priority</param>
        /// <param name="message">The feedback message</param>
        public FeedbackMessage(
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
