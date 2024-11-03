namespace Shap.Types.Enum
{
    /// <summary>
    /// The type of feedback message
    /// </summary>
    public enum FeedbackType
    {
        /// <summary>
        /// The feedback is for a fault or an error.
        /// </summary>
        Fault,

        /// <summary>
        /// Information feedback
        /// </summary>
        Info,

        /// <summary>
        /// Feedback to indicate that a command has been requested.
        /// </summary>
        Command,

        /// <summary>
        /// Feedback to indicate that application navigation has been requested.
        /// </summary>
        Navigation
    }
}
