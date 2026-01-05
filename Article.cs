using System.Text.Json.Serialization;

namespace ScienceJournalism2026
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
        public List<string> Keywords { get; set; } = new();
        public List<int> AuthorIds { get; set; } = new();
        public DateTime SubmissionDate { get; set; }
        public DateTime? PublicationDate { get; set; }
        public ArticleStatus Status { get; set; }
        public string Content { get; set; } = string.Empty;
        public string DOI { get; set; } = string.Empty;
        public int Citations { get; set; }
        public List<Review> Reviews { get; set; } = new();

        [JsonIgnore]
        public string StatusString => GetStatusString();

        private string GetStatusString()
        {
            return Status switch
            {
                ArticleStatus.Submitted => "На рассмотрении",
                ArticleStatus.UnderReview => "На рецензии",
                ArticleStatus.RevisionRequired => "Требует доработки",
                ArticleStatus.Accepted => "Принято",
                ArticleStatus.Published => "Опубликовано",
                ArticleStatus.Rejected => "Отклонено",
                _ => "Неизвестно"
            };
        }

        public override string ToString()
        {
            return $"{Title} ({StatusString}) - DOI: {DOI}";
        }
    }

    public enum ArticleStatus
    {
        Submitted,
        UnderReview,
        RevisionRequired,
        Accepted,
        Published,
        Rejected
    }

    public class Review
    {
        public int ReviewerId { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Score { get; set; } // 1-10
        public string Comments { get; set; } = string.Empty;
        public bool Recommendation { get; set; } // true = принять, false = отклонить
    }
}
