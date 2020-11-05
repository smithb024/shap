namespace Shap.Interfaces.Types
{
  using System;
    using NynaeveLib.ViewModel;

  // TODO I think this could be used thoughout the codebase.
  public interface IJnyId : IViewModelBase
  {
    DateTime Date { get; set; }

    string JnyNumber { get; set; }
  }
}