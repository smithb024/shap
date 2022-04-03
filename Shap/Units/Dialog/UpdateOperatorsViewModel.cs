namespace Shap.Units.Dialog
{
    using System.Collections.ObjectModel;
    using NynaeveLib.ViewModel;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Common.SerialiseModel.Operator;
    using Shap.Interfaces.Io;

    public class UpdateOperatorsViewModel : ViewModelBase
    {
        /// <summary>
        /// The index of the currently selected operator.
        /// </summary>
        private int operatorsIndex;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateOperatorsViewModel"/> class.
        /// </summary>
        /// <param name="ioControllers">IO controllers</param>
        /// <param name="details">details of the current class.</param>
        public UpdateOperatorsViewModel(
            IIoControllers ioControllers,
            ClassDetails details)
        {
            this.Operators = new ObservableCollection<OperatorComboBoxItemViewModel>();
            OperatorDetails operatorDetails = ioControllers.Operator.Read();

            foreach(SingleOperator singleOperator in operatorDetails.Operators)
            {
                OperatorComboBoxItemViewModel viewModel =
                    new OperatorComboBoxItemViewModel(
                        singleOperator.Name,
                        singleOperator.IsActive);
                this.Operators.Add(viewModel);
            }
        }

        /// <summary>
        /// Gets or sets the index of the currently selected operator.
        /// </summary>
        public int OperatorIndex
        {
            get
            {
                return this.operatorsIndex;
            }

            set
            {
                if (this.operatorsIndex == value)
                {
                    return;
                }

                this.operatorsIndex = value;
                this.RaisePropertyChangedEvent(nameof(this.OperatorIndex));
            }
        }

        /// <summary>
        /// Collection of all known operators.
        /// </summary>
        public ObservableCollection<OperatorComboBoxItemViewModel> Operators { get; }
    }
}