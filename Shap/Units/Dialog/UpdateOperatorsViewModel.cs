namespace Shap.Units.Dialog
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using NynaeveLib.ViewModel;
    using Shap.Common.Commands;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Common.SerialiseModel.Operator;
    using Shap.Interfaces.Io;

    /// <summary>
    /// 
    /// </summary>
    public class UpdateOperatorsViewModel : ViewModelBase
    {
        /// <summary>
        /// The index of the currently selected operator on the combo box.
        /// </summary>
        private int operatorsIndex;

        /// <summary>
        /// The index of the currently selected operator from those assigned to the current class.
        /// </summary>
        private int classOperatorsIndex;

        /// <summary>
        /// Indicates whether the currently selected class operator is a contemporary operator or 
        /// not.
        /// </summary>
        private bool isContemporary;

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
            this.ClassOperators = new ObservableCollection<OperatorListItemViewModel>();
            OperatorDetails operatorDetails = ioControllers.Operator.Read();

            foreach (SingleOperator singleOperator in operatorDetails.Operators)
            {
                OperatorComboBoxItemViewModel viewModel =
                    new OperatorComboBoxItemViewModel(
                        singleOperator.Name,
                        singleOperator.IsActive);
                this.Operators.Add(viewModel);
            }

            foreach(Operator classOperator in details.Operators)
            {
                bool isActive =
                    this.FindActiveState(
                        classOperator.Name);

                OperatorListItemViewModel viewModel =
                    new OperatorListItemViewModel(
                        classOperator.Name,
                        isActive,
                        classOperator.IsContemporary);

                this.ClassOperators.Add(viewModel);
            }

            this.AddCmd = new CommonCommand(this.Add);
        }

        /// <summary>
        /// Gets or sets the index of the currently selected operator on the combo box.
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

        /// <summary>
        /// Gets or sets the index of the currently selected operator from those assigned to the 
        /// current class.
        /// </summary>
        public int ClassOperatorIndex
        {
            get
            {
                return this.classOperatorsIndex;
            }

            set
            {
                if (this.classOperatorsIndex == value)
                {
                    return;
                }

                this.classOperatorsIndex = value;
                this.RaisePropertyChangedEvent(nameof(this.ClassOperatorIndex));
            }
        }

        /// <summary>
        /// Collection of all known operators.
        /// </summary>
        public ObservableCollection<OperatorListItemViewModel> ClassOperators { get; }


        /// <summary>
        /// Ok command.
        /// </summary>
        public ICommand AddCmd { get; private set; }

        /// <summary>
        /// Gets or sets the index of the currently selected operator from those assigned to the 
        /// current class.
        /// </summary>
        public bool IsContemporary
        {
            get
            {
                return this.isContemporary;
            }

            set
            {
                if (this.isContemporary == value)
                {
                    return;
                }

                this.isContemporary = value;
                this.RaisePropertyChangedEvent(nameof(this.IsContemporary));
            }
        }

        /// <summary>
        /// Find the operator with a given name and return its active state.
        /// </summary>
        /// <param name="name">name to find</param>
        /// <returns>is active state</returns>
        private bool FindActiveState(string name)
        {
            foreach (OperatorComboBoxItemViewModel op in this.Operators)
            {
                if (string.Compare(op.Name, name) == 0)
                {
                    return op.IsActive;
                }
            }

            return false;
        }

        /// <summary>
        /// Indicate whether the currently selected operator has already been selected. 
        /// It returns null if operator is not valid.
        /// </summary>
        /// <returns>Has been selected</returns>
        private bool? IsAlreadySelected()
        {
            if (this.OperatorIndex >= 0 && this.OperatorIndex < this.Operators.Count)
            {
                foreach (OperatorListItemViewModel op in this.ClassOperators)
                {
                    if (string.Compare(op.Name, this.Operators[this.OperatorIndex].Name) == 0)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return null;
            }

            return false;
        }

        /// <summary>
        /// Add the currently selected operator.
        /// </summary>
        private void Add()
        {
            bool? isAlreadySelected = this.IsAlreadySelected();

            if (isAlreadySelected == null ||
                isAlreadySelected == true)
            {
                return;
            }

            OperatorComboBoxItemViewModel selectedOperator =
                this.Operators[this.OperatorIndex];

            bool isActive =
                    this.FindActiveState(
                        selectedOperator.Name);

            OperatorListItemViewModel viewModel =
                new OperatorListItemViewModel(
                    selectedOperator.Name,
                    isActive,
                    true);

            this.ClassOperators.Add(viewModel);
        }
    }
}