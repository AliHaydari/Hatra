﻿using System.Collections.Generic;

namespace Hatra.LuceneSearch
{
    public class SearchResultCollection
    {
        private List<SearchResult> _data;

        public int Count { get; set; }

        public List<SearchResult> Data {
            get => _data ?? (_data = new List<SearchResult>());
            set => _data = value;
        }
    }
}
