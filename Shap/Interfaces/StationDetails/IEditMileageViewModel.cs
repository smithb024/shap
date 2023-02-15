namespace Shap.Interfaces.StationDetails
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    /// <summary>
    /// Interface which supports the edit mileage view model.
    /// </summary>
    public interface IEditMileageViewModel
    {
        /// <summary>
        /// view close request event handler
        /// </summary>
        event EventHandler ClosingRequest;

        /// <summary>
        /// Gets or sets the new from location
        /// </summary
        string NewFromStn { get; set; }

        /// <summary>
        /// Gets or sets the new to location
        /// </summary>
        string NewToStn { get; set; }

        /// <summary>
        /// Gets or sets the new route.
        /// </summary>
        string NewViaRoute { get; set; }

        /// <summary>
        /// Gets or sets the new return route
        /// </summary>
        string NewViaReturnRoute { get; set; }

        /// <summary>
        /// Gets or sets the new outbound miles
        /// </summary>
        int NewOutMiles { get; set; }

        /// <summary>
        /// Gets or sets the new inbound miles
        /// </summary>
        int NewBackMiles { get; set; }

        /// <summary>
        /// Gets or sets the new outbound chains
        /// </summary>
        int NewOutChains { get; set; }

        /// <summary>
        /// Gets or sets the new inbound chains
        /// </summary>
        int NewBackChains { get; set; }

        /// <summary>
        /// Gets or sets the index of the selected outbound journey, used during editing.
        /// </summary>
        int JnyToIndex { get; set; }

        /// <summary>
        /// Gets or sets the collection of outbound journies, used during editing.
        /// </summary>
        ObservableCollection<string> JnyToList { get; set; }

        /// <summary>
        /// Gets or sets the index of the selected inbound journey, used during editing.
        /// </summary>
        int JnyFromIndex { get; set; }

        /// <summary>
        /// Gets or sets the collection of inbound journies, used during editing.
        /// </summary>
        ObservableCollection<string> JnyFromList { get; set; }

        /// <summary>
        /// Gets or sets the index of the selected route, used during editing.
        /// </summary>
        int JnyViaIndex { get; set; }

        /// <summary>
        /// Gets or sets the collection of routes, used during editing.
        /// </summary>
        ObservableCollection<string> JnyViaList { get; set; }

        /// <summary>
        /// Gets or sets the view tab index.
        /// </summary>
        int TabIndex { get; set; }

        /// <summary>
        /// Gets or sets the current edit mode. It determine which components to show on the 
        /// edit tab.
        /// </summary>
        bool EditMode { get; set; }

        /// <summary>
        /// Add new command.
        /// </summary>
        ICommand AddNewCmd { get; }

        /// <summary>
        /// Close window command.
        /// </summary>
        ICommand CloseWindowCmd { get; }

        /// <summary>
        /// Refresh command.
        /// </summary>
        ICommand RefreshCmd { get; }

        /// <summary>
        /// Complete edit command.
        /// </summary>
        ICommand CompleteEditCmd { get; }

        /// <summary>
        /// Close window command.
        /// </summary>
        ICommand SelectForEditCmd { get; }
    }
}
