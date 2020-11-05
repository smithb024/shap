using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shap.Analysis.ViewModels.Data
{
    using NynaeveLib.ViewModel;

    public class FullYearViewModel : ViewModelBase
    {
        public FullYearViewModel(
            string name,
            int total,
            int jan,
            int feb,
            int mar,
            int apr,
            int may,
            int jun,
            int jul,
            int aug,
            int sept,
            int oct,
            int nov,
            int dec)
        {
            this.Name = name;
            this.Total = total;

            this.Jan = jan;
            this.Feb = feb;
            this.Mar = mar;
            this.Apr = apr;
            this.May = may;
            this.Jun = jun;
            this.Jul = jul;
            this.Aug = aug;
            this.Sept = sept;
            this.Oct = oct;
            this.Nov = nov;
            this.Dec = dec;
        }

        public string Name { get; }

        public int Total { get; }
        public int Jan { get; }
        public int Feb { get; }
        public int Mar { get; }
        public int Apr { get; }
        public int May { get; }
        public int Jun { get; }
        public int Jul { get; }
        public int Aug { get; }
        public int Sept { get; }
        public int Oct { get; }
        public int Nov { get; }
        public int Dec { get; }
    }
}