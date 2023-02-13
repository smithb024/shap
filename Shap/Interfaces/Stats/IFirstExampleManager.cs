namespace Shap.Interfaces.Stats
{
    using Shap.Interfaces.Types;
    using Shap.Stats;
    using Shap.Types;
    using System.Collections.Generic;

    public interface IFirstExampleManager
    {
        string AllPurposeYear { get; }

        void AppendLocation(FirstExampleType firstExamples, LocalListType localList, string year = "1970");
        void AppendNumber(FirstExampleType firstExamples, LocalListType localList, string year = "1970");
        void CheckNewJnyList(List<IJourneyDetailsType> jnyList);
        bool IsCopLocation(FirstExampleType firstExample, LocalListType listType);
        bool IsCopLocation(string location, IJnyId jnyId, LocalListType listType);
        bool IsCopLocation(string location, LocalListType listType);
        bool IsCopNumber(FirstExampleType firstExample, LocalListType listType);
        bool IsCopNumber(string number, IJnyId jnyId, LocalListType listType);
        bool IsCopNumber(string number, LocalListType listType);
        void LoadAllPurposeList(string year);
        void LoadAnnualList(string year);
        void LoadCompleteList();
        void RunSearchAll();
        void RunSearchYear(string year);
        void SearchLocationsForMatch(IJourneyDetailsType journey);
        void SearchNumbersForMatch(IJourneyDetailsType journey);
    }
}