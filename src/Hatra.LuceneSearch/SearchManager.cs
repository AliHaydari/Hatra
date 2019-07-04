using Hatra.ViewModels;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Hatra.LuceneSearch
{
    public class SearchManager : ISearchManager
    {
        private static FSDirectory _directory;
        private readonly IHostingEnvironment _env;
        private const LuceneVersion LuceneVersion = Lucene.Net.Util.LuceneVersion.LUCENE_48;

        public SearchManager(IHostingEnvironment env)
        {
            _env = env;
        }

        private FSDirectory Directory {
            get {
                if (_directory != null)
                {
                    return _directory;
                }

                var info = System.IO.Directory.CreateDirectory(LuceneDir);
                return _directory = FSDirectory.Open(info);
            }
        }

        private string LuceneDir => Path.Combine(_env.ContentRootPath, "Lucene_Index");

        public void AddToIndex(params LuceneSearchModel[] searchables)
        {
            DeleteFromIndex(searchables);

            UseWriter(x =>
            {
                foreach (var searchable in searchables)
                {
                    var doc = new Document();
                    foreach (var field in GetFields(searchable))
                    {
                        doc.Add(field);
                    }
                    x.AddDocument(doc);
                }
            });
        }

        public void AddToIndex(params PageViewModel[] searchables)
        {
            var list = MapToLuceneSearchModel(searchables);

            DeleteFromIndex(list);

            UseWriter(x =>
            {
                foreach (var searchable in list)
                {
                    var doc = new Document();
                    foreach (var field in GetFields(searchable))
                    {
                        doc.Add(field);
                    }
                    x.AddDocument(doc);
                }
            });
        }

        private void UseWriter(Action<IndexWriter> action)
        {
            using (var analyzer = new StandardAnalyzer(LuceneVersion))
            {
                using (var writer = new IndexWriter(Directory, new IndexWriterConfig(LuceneVersion, analyzer)))
                {
                    action(writer);
                    writer.Commit();
                }
            }
        }

        public void DeleteFromIndex(params LuceneSearchModel[] searchables)
        {
            UseWriter(x =>
            {
                foreach (var searchable in searchables)
                {
                    var searchQuery = new TermQuery(new Term(StronglyTyped.PropertyName<LuceneSearchModel>(p => p.PageId), searchable.PageId.Value.ToString(CultureInfo.InvariantCulture)));
                    x.DeleteDocuments(searchQuery);
                }
            });
        }

        public void DeleteFromIndex(params PageViewModel[] searchables)
        {
            var list = MapToLuceneSearchModel(searchables);

            UseWriter(x =>
            {
                foreach (var searchable in list)
                {
                    var searchQuery = new TermQuery(new Term(StronglyTyped.PropertyName<LuceneSearchModel>(p => p.PageId), searchable.PageId.Value.ToString(CultureInfo.InvariantCulture)));
                    x.DeleteDocuments(searchQuery);
                }
            });
        }

        public void Clear()
        {
            UseWriter(x => x.DeleteAll());
        }

        private IEnumerable<IIndexableField> GetFields(LuceneSearchModel model)
        {
            return new Field[]
            {
                    new TextField(nameof(model.PageId), model.PageId?.ToString(), Lucene.Net.Documents.Field.Store.YES),
                    new TextField(nameof(model.Title), model.Title, Lucene.Net.Documents.Field.Store.YES){ Boost = 4.0f },
                    new StringField(nameof(model.BriefDescription),model.BriefDescription, Lucene.Net.Documents.Field.Store.YES),
                    new StringField(nameof(model.Body), model.Body??"", Lucene.Net.Documents.Field.Store.YES),
                    new StringField(nameof(model.SlugUrl), model.SlugUrl??"", Lucene.Net.Documents.Field.Store.YES),
                    new StringField(nameof(model.Image), model.Image??"", Lucene.Net.Documents.Field.Store.YES),
                    new StringField(nameof(model.CategoryId), model.CategoryId?.ToString()??"", Lucene.Net.Documents.Field.Store.YES),
                    new StringField(nameof(model.CategoryName), model.CategoryName??"", Lucene.Net.Documents.Field.Store.YES),
                    new StringField(nameof(model.IsShow), Convert.ToString(model.IsShow), Lucene.Net.Documents.Field.Store.YES),
            };
        }

        public SearchResultCollection Search(string searchQuery, int hitsStart, int hitsStop, string[] fields)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return new SearchResultCollection();
            }

            const int hitsLimit = 100;
            SearchResultCollection results;
            using (var analyzer = new StandardAnalyzer(LuceneVersion))
            {
                using (var reader = DirectoryReader.Open(Directory))
                {
                    var searcher = new IndexSearcher(reader);
                    var parser = new MultiFieldQueryParser(LuceneVersion, fields, analyzer);
                    var query = parser.Parse(QueryParserBase.Escape(searchQuery.Trim()));
                    var hits = searcher.Search(query, null, hitsLimit, Sort.RELEVANCE).ScoreDocs;
                    results = new SearchResultCollection
                    {
                        Count = hits.Length,
                        Data = hits.Where((x, i) => i >= hitsStart && i < hitsStop)
                            .Select(x => new SearchResult(searcher.Doc(x.Doc)))
                            .ToList()
                    };

                    //var res=new LuceneSearchModel()
                }
            }
            //return results;
            return new SearchResultCollection();
        }

        private IEnumerable<LuceneSearchModel> _search(string searchQuery, string[] searchFields)
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
                return new List<LuceneSearchModel>();

            try
            {
                // set up lucene searcher
                using (var reader = DirectoryReader.Open(Directory))
                {
                    var searcher = new IndexSearcher(reader);
                    const int hitsLimit = 1000;
                    var analyzer = new StandardAnalyzer(LuceneVersion);


                    var parser = new MultiFieldQueryParser(LuceneVersion, searchFields, analyzer);
                    Query query = parseQuery(searchQuery, parser);
                    ScoreDoc[] hits = searcher.Search(query, null, hitsLimit, Sort.RELEVANCE).ScoreDocs;

                    if (hits.Length == 0)
                    {
                        searchQuery = searchByPartialWords(searchQuery);
                        query = parseQuery(searchQuery, parser);
                        hits = searcher.Search(query, hitsLimit).ScoreDocs;
                    }

                    IEnumerable<LuceneSearchModel> results = _mapLuceneToDataList(hits, searcher);
                    //analyzer.Close();
                    //searcher.Dispose();
                    return results;
                }
            }
            catch (Exception e)
            {
                return new List<LuceneSearchModel>();
            }
        }

        public IEnumerable<LuceneSearchModel> Search(string input, params string[] fieldsName)
        {
            if (string.IsNullOrEmpty(input))
                return new List<LuceneSearchModel>();

            IEnumerable<string> terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);
            return _search(input, fieldsName);
        }

        private Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        private string searchByPartialWords(string bodyTerm)
        {
            bodyTerm = bodyTerm.Replace("*", "").Replace("?", "");
            IEnumerable<string> terms = bodyTerm
                .Trim()
                .Replace("-", " ")
                .Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.Trim() + "*");
            bodyTerm = string.Join(" ", terms);
            return bodyTerm;
        }

        private LuceneSearchModel[] MapToLuceneSearchModel(PageViewModel[] viewModels)
        {
            var list = new List<LuceneSearchModel>();

            foreach (var viewModel in viewModels)
            {
                list.Add(new LuceneSearchModel()
                {
                    PageId = viewModel.Id,
                    Title = viewModel.Title,
                    BriefDescription = viewModel.BriefDescription,
                    Body = viewModel.Body,
                    Image = viewModel.Image,
                    SlugUrl = viewModel.SlugUrl,
                    CategoryId = viewModel.CategoryId,
                    CategoryName = viewModel.CategoryName,
                    IsShow = viewModel.IsShow,
                });
            }

            return list.ToArray();
        }

        private LuceneSearchModel _mapLuceneDocumentToData(Document doc)
        {
            var a1 = doc.Get("PageId") ?? "0";
            var a2 = doc.Get("Title") ?? "";
            var a3 = doc.Get("BriefDescription") ?? "";
            var a4 = doc.Get("Body") ?? "";
            var a5 = doc.Get("Image") ?? "";
            var a6 = doc.Get("SlugUrl") ?? "";
            var a7 = doc.Get("CategoryId") ?? "0";
            var a8 = doc.Get("CategoryName") ?? "";
            var a9 = doc.Get("IsShow") ?? "";

            return new LuceneSearchModel
            {
                PageId = Convert.ToInt32(a1), // Convert.ToInt32(doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.PageId))),
                Title = a2, // doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Title ?? "")),
                BriefDescription = a3, // doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.BriefDescription ?? "")),
                Body = a4, // doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Body ?? "")),
                Image = a5, // doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.Image ?? "")),
                SlugUrl = a6, // doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.SlugUrl ?? "")),
                CategoryId = a7 == "" ? 0 : Convert.ToInt32(a7), // Convert.ToInt32(doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.CategoryId))),
                CategoryName = a8, // doc.Get(StronglyTyped.PropertyName<LuceneSearchModel>(x => x.CategoryName ?? "")),
                IsShow = Convert.ToBoolean(a9),
            };
        }

        private IEnumerable<LuceneSearchModel> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();
        }

        private IEnumerable<LuceneSearchModel> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }
    }
}
