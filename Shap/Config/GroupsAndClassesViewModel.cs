﻿namespace Shap.Config
{
    using System.Windows;
    using System.Windows.Input;
    using NynaeveLib.Commands;
    using NynaeveLib.DialogService.Interfaces;
    using NynaeveLib.ViewModel;

    using Shap.Config.GroupsAndClasses;
    using Shap.Interfaces.Io;
    using Shap.Messages;
    using Shap.Types.Enum;
    using NynaeveMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model class used to manage the groups and classes dialog.
    /// </summary>
    public class GroupsAndClassesViewModel : DialogViewModelBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="GroupsAndClassesViewModel"/> class.
        /// </summary>
        /// <param name="reader">
        /// Groups and classes reader.
        /// </param>
        /// <param name="familyReader">
        /// Family reader.
        /// </param>
        public GroupsAndClassesViewModel(
            IIoControllers ioControllers)
        {
            this.GroupManager =
                new GroupsViewModel(
                    ioControllers);
            this.FamilyManager =
                new FamilyManagerViewModel(
                    ioControllers);
            this.OperatorManager =
                new OperatorManagerViewModel(
                    ioControllers);

            this.CompleteCmd =
                new CommonCommand<ICloseable>(
                    this.SelectComplete,
                    this.CanSelectComplete);
        }

        /// <summary>
        /// The view model which supports the Group Manager part of this view.
        /// </summary>
        public GroupsViewModel GroupManager { get; }

        /// <summary>
        /// The view model which supports the Family Manager part of this view.
        /// </summary>
        public FamilyManagerViewModel FamilyManager { get; }

        /// <summary>
        /// The view model which supports the Operator Manager part of this view.
        /// </summary>
        public OperatorManagerViewModel OperatorManager { get; }

        /// <summary>
        /// Accept changes and save command. The command also dismisses the dialog.
        /// </summary>
        public ICommand CompleteCmd { get; private set; }

        /// <summary>
        /// Select the Ok command.
        /// </summary>
        private void SelectComplete(ICloseable window)
        {
            this.Result = MessageBoxResult.OK;

            FeedbackMessage message =
                new FeedbackMessage(
                    FeedbackType.Command,
                    $"GAC - Complete and Save.");
            NynaeveMessenger.Default.Send(message);

            this.GroupManager.Save();
            this.FamilyManager.Save();
            this.OperatorManager.Save();

            window?.CloseObject();

            FeedbackMessage closeMessage =
                new FeedbackMessage(
                    FeedbackType.Navigation,
                    $"GAC - Close Dialog.");
            NynaeveMessenger.Default.Send(closeMessage);

        }

        /// <summary>
        /// Checks to see if Ok can be selected.
        /// </summary>
        /// <returns>can only select if not null or empty</returns>
        private bool CanSelectComplete(ICloseable window)
        {
            return true;
        }
    }
}