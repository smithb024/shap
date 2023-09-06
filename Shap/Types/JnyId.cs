namespace Shap.Types
{
    using System;
    using NynaeveLib.ViewModel;

    using Shap.Interfaces.Types;

    // TODO I think this could be used thoughout the codebase.
    public class JnyId : ViewModelBase, IJnyId
    {
        private DateTime date;
        private string jnyNumber;

        public JnyId(DateTime date, string jnyNumber)
        {
            this.date = date;
            this.jnyNumber = jnyNumber;
        }

        public DateTime Date
        {
            get
            {
                return this.date;
            }

            set
            {
                this.date = value;
                this.OnPropertyChanged("Date");
            }
        }

        public string JnyNumber
        {
            get
            {
                return this.jnyNumber;
            }

            set
            {
                this.jnyNumber = value;
                this.OnPropertyChanged("JnyNumber");
            }
        }
    }
}