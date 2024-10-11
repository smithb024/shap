namespace Shap.Config.GroupsAndClasses
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using NynaeveLib.Commands;
    using NynaeveLib.ViewModel;
    using Shap.Common.SerialiseModel.Operator;
    using Shap.Interfaces.Io;

    /// <summary>
    /// View model which manages the ability to add and update operators
    /// </summary>
    public class OperatorManagerViewModel : ViewModelBase, IViewModelBase
    {
        /// <summary>
        /// IO Controllers.
        /// </summary>
        private readonly IIoControllers ioControllers;

        /// <summary>
        /// The name of the operator to be added.
        /// </summary>
        private string name;

        /// <summary>
        /// The index of the currently selected operator.
        /// </summary>
        private int operatorsIndex;

        /// <summary>
        /// The deserialised operator, read from the config file.
        /// </summary>
        private OperatorDetails serialisedOperators;

        /// <summary>
        /// Initialises a new instance of the <see cref="OperatorManagerViewModel"/> class.
        /// </summary>
        /// <param name="ioControllers">IO Controllers</param>
        public OperatorManagerViewModel(
            IIoControllers ioControllers)
        {
            this.ioControllers = ioControllers;
            this.serialisedOperators = this.ioControllers.Operator.Read();
            this.Operators = new ObservableCollection<OperatorConfigViewModel>();

            if (this.serialisedOperators != null)
            {
                foreach (SingleOperator singleOperator in this.serialisedOperators.Operators)
                {
                    OperatorConfigViewModel newOperator =
                        new OperatorConfigViewModel(
                            singleOperator.Name,
                            singleOperator.IsActive);
                    this.Operators.Add(newOperator);
                }
            }
            else
            {
                this.serialisedOperators = new OperatorDetails();
                this.serialisedOperators.Operators = new List<SingleOperator>();
            }

            this.AddOperator = new CommonCommand(this.Add, this.CanAdd);
            this.RetireOperator = new CommonCommand(this.Retire, this.CanRetire);
        }

        /// <summary>
        /// Gets or sets a new operator name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged(nameof(this.Name));
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
                this.OnPropertyChanged(nameof(this.OperatorIndex));
            }
        }

        /// <summary>
        /// Collection of all known families.
        /// </summary>
        public ObservableCollection<OperatorConfigViewModel> Operators { get; }

        /// <summary>
        /// Add an item.
        /// </summary>
        public ICommand AddOperator { get; private set; }

        /// <summary>
        /// Retire an operator.
        /// </summary>
        public ICommand RetireOperator { get; private set; }

        /// <summary>
        /// Save the updates.
        /// </summary>
        public void Save()
        {
            this.ioControllers.Operator.Write(this.serialisedOperators);
        }

        /// <summary>
        /// Add the string as a new operator. Reset the input.
        /// </summary>
        private void Add()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                return;
            }

            SingleOperator newSingleOperator =
                new SingleOperator()
                {
                    Name = this.Name,
                    IsActive = true
                };

            this.serialisedOperators.Operators.Add(newSingleOperator);
            this.serialisedOperators.Operators = 
                this.serialisedOperators.Operators.OrderBy(o => o.Name).ToList();

            OperatorConfigViewModel newOperator =
                        new OperatorConfigViewModel(
                            this.Name,
                            true);
            this.Operators.Add(newOperator);
            this.OnPropertyChanged(nameof(this.Operators));

            this.Name = string.Empty;
        }

        /// <summary>
        /// Indicates whether the <see cref="AddOperator"/> command can be run. It requires a valid
        /// <see cref="Name"/> value;
        /// </summary>
        /// <returns>validity flag</returns>
        private bool CanAdd()
        {
            return !string.IsNullOrEmpty(this.Name);
        }

        /// <summary>
        /// Retire/reinstate the selected operator.
        /// </summary>
        private void Retire()
        {
            if (!this.IsSelectionValid())
            {
                return;
            }

            this.Operators[this.OperatorIndex].ToggleActive();

            SingleOperator modelOperator =
                this.serialisedOperators.Operators.Find(
                    o => string.Compare(o.Name, this.Operators[this.OperatorIndex].Name) == 0);
            modelOperator.IsActive = this.Operators[this.OperatorIndex].IsActive;
        }

        /// <summary>
        /// Indicates whether the <see cref="RetireOperator"/> command can be run. It requires a valid
        /// <see cref="OperatorIndex"/> value;
        /// </summary>
        /// <returns>validity flag</returns>
        private bool CanRetire()
        {
            return this.IsSelectionValid();
        }

        /// <summary>
        /// Indicates whether the currently selected operator is a valid one.
        /// </summary>
        /// <returns>validity flag</returns>
        private bool IsSelectionValid()
        {
            bool isValid =
                this.OperatorIndex >= 0 &&
                this.OperatorIndex < this.Operators.Count;

            return isValid;
        }
    }
}