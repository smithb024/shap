using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shap.Common.ViewModel
{
    using NynaeveLib.ViewModel;

    public class LocationCounterViewModel : ViewModelBase
    {
        private string location;
        private int counter;

        public LocationCounterViewModel(string location)
        {
            this.location = location;
            this.counter = 0;
        }

        public string Location => this.location;

        public int Count => this.counter;

        public void AddOne()
        {
            ++this.counter;
            this.OnPropertyChanged(nameof(this.Count));
        }
    }
}