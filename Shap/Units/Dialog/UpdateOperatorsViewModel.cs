namespace Shap.Units.Dialog
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using NynaeveLib.Commands;
    using NynaeveLib.DialogService.Interfaces;
    using NynaeveLib.ViewModel;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Common.SerialiseModel.Operator;
    using Shap.Icons.ComboBoxItems;
    using Shap.Icons.ListViewItems;
    using Shap.Interfaces.Io;

    /// <summary>
    /// View model which supports a dialog used to configure operators.
    /// </summary>
    public class UpdateOperatorsViewModel : ViewModelBase
    {
        /// <summary>
        /// The details of the class which is supported by this dialog.
        /// </summary>
        ClassDetails classDetails;

        /// <summary>
        /// The index of the currently selected operator on the combo box.
        /// </summary>
        private int operatorsIndex;

        /// <summary>
        /// The index of the currently selected operator from those assigned to the current class.
        /// </summary>
        private int classOperatorsIndex;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateOperatorsViewModel"/> class.
        /// </summary>
        /// <param name="ioControllers">IO controllers</param>
        /// <param name="details">details of the current class.</param>
        public UpdateOperatorsViewModel(
            IIoControllers ioControllers,
            ClassDetails details)
        {
            this.classDetails = details;
            this.Operators = new ObservableCollection<OperatorItemViewModel>();
            this.ClassOperators = new ObservableCollection<OperatorListItemViewModel>();
            OperatorDetails operatorDetails = ioControllers.Operator.Read();

            foreach (SingleOperator singleOperator in operatorDetails.Operators)
            {
                OperatorItemViewModel viewModel =
                    new OperatorItemViewModel(
                        singleOperator.Name,
                        singleOperator.IsActive);
                this.Operators.Add(viewModel);
            }

            foreach (Operator classOperator in this.classDetails.Operators)
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
            this.OkCmd = new CommonCommand<ICloseable>(this.Okay);
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
                this.OnPropertyChanged(nameof(this.OperatorIndex));
            }
        }

        /// <summary>
        /// Collection of all known operators.
        /// </summary>
        public ObservableCollection<OperatorItemViewModel> Operators { get; }

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
                this.OnPropertyChanged(nameof(this.ClassOperatorIndex));
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
        /// Ok command.
        /// </summary>
        public ICommand OkCmd { get; private set; }

        /// <summary>
        /// Find the operator with a given name and return its active state.
        /// </summary>
        /// <param name="name">name to find</param>
        /// <returns>is active state</returns>
        private bool FindActiveState(string name)
        {
            foreach (OperatorItemViewModel op in this.Operators)
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

            OperatorItemViewModel selectedOperator = this.Operators[this.OperatorIndex];

            OperatorListItemViewModel viewModel =
                new OperatorListItemViewModel(
                    selectedOperator.Name,
                    selectedOperator.IsActive,
                    true);

            this.ClassOperators.Add(viewModel);
        }

        /// <summary>
        /// Select the Ok command.
        /// </summary>
        private void Okay(ICloseable window)
        {
            this.classDetails.Operators.Clear();

            foreach (IOperatorListItemViewModel locationOperator in this.ClassOperators)
            {
                Operator newOperator =
                    new Operator()
                    {
                        Name = locationOperator.Name,
                        IsContemporary = locationOperator.IsOperatorContemporary
                    };

                this.classDetails.Operators.Add(newOperator);
            }

            this.classDetails.Operators = this.classDetails.Operators.OrderBy(c => c.Name).ToList();

            window?.CloseObject();
        }
    }
}