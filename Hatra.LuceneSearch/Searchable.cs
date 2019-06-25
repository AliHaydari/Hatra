using Lucene.Net.Documents;
using Lucene.Net.Index;
using System.Collections.Generic;

namespace Hatra.LuceneSearch
{
    public abstract class Searchable
    {
        public static readonly Dictionary<Field, string> FieldStrings = new Dictionary<Field, string>
        {
            {Field.Description, "Description"},
            {Field.DescriptionPath, "DescriptionPath"},
            {Field.Href, "Href"},
            {Field.Id, "Id"},
            {Field.Title, "Title"}
        };

        public static readonly Dictionary<Field, string> AnalyzedFields = new Dictionary<Field, string>
        {
            {Field.Description, FieldStrings[Field.Description] },
            {Field.Title, FieldStrings[Field.Title] }
        };

        public abstract string Description { get; }
        public abstract string DescriptionPath { get; }
        public abstract string Href { get; }
        public abstract int Id { get; }
        public abstract string Title { get; }

        public enum Field
        {
            Description,
            DescriptionPath,
            Href,
            Id,
            Title
        }

        public IEnumerable<IIndexableField> GetFields()
        {
            return new Lucene.Net.Documents.Field[]
            {
                new TextField(AnalyzedFields[Field.Description], Description, Lucene.Net.Documents.Field.Store.NO),
                new TextField(AnalyzedFields[Field.Title], Title, Lucene.Net.Documents.Field.Store.YES){ Boost = 4.0f },
                new StringField(FieldStrings[Field.Id], Id.ToString(), Lucene.Net.Documents.Field.Store.YES),
                new StringField(FieldStrings[Field.DescriptionPath], DescriptionPath, Lucene.Net.Documents.Field.Store.YES),
                new StringField(FieldStrings[Field.Href], Href, Lucene.Net.Documents.Field.Store.YES)
            };
        }
    }
}
