using System.Text.Json.Serialization;

namespace ScienceJournalism2026
{
    public class Journal
    {
        public string Name { get; set; } = "Научный журнал 2026";
        public string ISSN { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public List<Article> Articles { get; set; } = new();
        public List<Author> Authors { get; set; } = new();
        public List<Editor> Editors { get; set; } = new();
        public List<Issue> Issues { get; set; } = new();

        [JsonIgnore]
        public int NextArticleId => Articles.Count > 0 ? Articles.Max(a => a.Id) + 1 : 1;

        [JsonIgnore]
        public int NextAuthorId => Authors.Count > 0 ? Authors.Max(a => a.Id) + 1 : 1;

        [JsonIgnore]
        public int NextEditorId => Editors.Count > 0 ? Editors.Max(e => e.Id) + 1 : 1;
    }

    public class Editor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Specializations { get; set; } = new();

        [JsonIgnore]
        public string FullName => $"{LastName} {FirstName}";
    }

    public class Issue
    {
        public int Volume { get; set; }
        public int Number { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<int> ArticleIds { get; set; } = new();
        public string Theme { get; set; } = string.Empty;
    }
}
