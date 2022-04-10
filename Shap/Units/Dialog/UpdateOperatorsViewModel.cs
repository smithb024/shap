﻿namespace Shap.Units.Dialog
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using NynaeveLib.Commands;
    using NynaeveLib.DialogService.Interfaces;
    using NynaeveLib.ViewModel;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Common.SerialiseModel.Operator;
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
            this.classDetails = details;
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
                this.ReassessIsContemporary();
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
                this.SetConfigurationIsContemporary();
            }
        }

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

            Operator newClassOperator =
                new Operator()
                {
                    Name = selectedOperator.Name,
                    IsContemporary = true
                };
            this.classDetails.Operators.Add(newClassOperator);
            this.classDetails.Operators = this.classDetails.Operators.OrderBy(c => c.Name).ToList();

            this.ClassOperators.Clear();

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
        }

        /// <summary>
        /// Set the <see cref="IsContemporary"/> property based on the currently selected 
        /// <see cref="ClassOperators"/> object. This bypasses the property setter because the
        /// property setter is used to handle user input.
        /// </summary>
        /// <remarks>
        /// Ignore the call if <see cref="ClassOperatorIndex"/> is not valid.
        /// </remarks>
        private void ReassessIsContemporary()
        {
            if (this.IsOperatorIndexValid())
            {
                this.isContemporary = this.ClassOperators[this.ClassOperatorIndex].IsContemporary;
                this.RaisePropertyChangedEvent(nameof(this.IsContemporary));
            }
        }

        /// <summary>
        /// Set the <see cref="IsContemporary"/> property based on the currently selected 
        /// <see cref="ClassOperators"/> object. This bypasses the property setter because the
        /// property setter is used to handle user input.
        /// </summary>
        /// <remarks>
        /// Ignore the call if <see cref="ClassOperatorIndex"/> is not valid.
        /// </remarks>
        private void SetConfigurationIsContemporary()
        {
            if (this.IsOperatorIndexValid())
            {
                this.ClassOperators[this.ClassOperatorIndex].SetIsContemporary(
                    this.IsContemporary);
                this.classDetails.Operators[this.ClassOperatorIndex].IsContemporary =
                    this.IsContemporary;
            }
        }

        /// <summary>
        /// Indicates whether the <see cref="ClassOperatorIndex"/> value is valid.
        /// </summary>
        /// <returns>validity flag</returns>
        private bool IsOperatorIndexValid()
        {
            bool isValid =
            this.ClassOperatorIndex >= 0 &&
                this.ClassOperatorIndex < this.ClassOperators.Count;

            return isValid;
        }

        /// <summary>
        /// Select the Ok command.
        /// </summary>
        private void Okay(ICloseable window)
        {
            window?.CloseObject();
        }
    }
}