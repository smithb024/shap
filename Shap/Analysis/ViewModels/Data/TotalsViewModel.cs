using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shap.Analysis.ViewModels.Data
{
    using NynaeveLib.ViewModel;

    public class TotalsViewModel : ViewModelBase
    {
        public TotalsViewModel(
            string name,
            int total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public int Total { get; }
    }
}