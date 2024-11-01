namespace Shap.Locations.ViewModels.Icons
{
    using System.Windows.Input;
    using Common.Commands;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Interfaces.Locations.ViewModels.Icons;
    using Shap.Common.SerialiseModel.Location;
    using Shap.Interfaces.Io;
    using Shap.Locations.Messages;
    using Shap.Messages;
    using Shap.Types.Enum;
    using NynaeveMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// Supports a row on the selector view.
    /// </summary>
    /// <remarks>
    /// This contains a location and any details about it. It allows the location to be selected
    /// on the location view.
    /// </remarks>
    public class SelectorRowViewModel : ObservableRecipient, ISelectorRowViewModel
    {
        /// <summary>
        /// The IO Controllers for the application.
        /// </summary>
        private IIoControllers ioControllers;

        /// <summary>
        /// Initialises a new instance of the <see cref="SelectorRowViewModel"/> class.
        /// </summary>
        /// <param name="ioControllers">
        /// IO controller manager object.
        /// </param>
        /// <param name="name">
        /// The location name.
        /// </param>
        /// <param name="isValid">
        /// Indicates whether this location is one which is monitored by the application
        /// </param>
        public SelectorRowViewModel(
            IIoControllers ioControllers,
            string name,
            bool isValid = true)
        {
            this.ioControllers = ioControllers;
            this.Name = name;
            this.IsValid = isValid;
            this.SelectLocationCmd = new CommonCommand(this.SelectLocation);

            if (isValid)
            {
                LocationDetails currentLocation =
                    this.ioControllers.Location.Read(
                        name);
                this.TotalFrom = currentLocation.TotalFrom;
                this.TotalTo = currentLocation.TotalTo;
            }
            else
            {
                this.TotalFrom = 0;
                this.TotalTo = 0;
            }
        }

        /// <summary>
        /// Gets the name of the current locaation.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets number from.
        /// </summary>
        public int TotalFrom { get; }

        /// <summary>
        /// Gets the number to.
        /// </summary>
        public int TotalTo { get; }

        /// <summary>
        /// Select the location for display on the location view.
        /// </summary>
        public ICommand SelectLocationCmd { get; }

        /// <summary>
        /// Gets a value which indicates whether this location is currently monitored by the app.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Select this location.
        /// </summary>
        private void SelectLocation()
        {
            if (this.IsValid)
            {
                FeedbackMessage feedbackMessage =
                    new FeedbackMessage(
                        FeedbackType.Navigation,
                        $"LocationIndex - Display {this.Name}.");
                NynaeveMessenger.Default.Send(feedbackMessage);

                DisplayLocationMessage message =
                    new DisplayLocationMessage(
                        this.Name);
                this.Messenger.Send(message);
            }
        }
    }
}