using Hatra.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hatra.LuceneSearch
{
    public interface ISearchManager
    {
        void AddToIndex(params LuceneSearchModel[] searchables);
        void AddToIndex(params PageViewModel[] searchables);
        void Clear();
        void DeleteFromIndex(params LuceneSearchModel[] searchables);
        void DeleteFromIndex(params PageViewModel[] searchables);
        SearchResultCollection Search(string searchQuery, int hitsStart, int hitsStop, string[] fields);
        IEnumerable<LuceneSearchModel> Search(string input, params string[] fieldsName);
    }
}
