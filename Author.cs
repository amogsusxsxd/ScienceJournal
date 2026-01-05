using System.Text.Json.Serialization;

namespace ScienceJournalism2026
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Affiliation { get; set; } = string.Empty;
        public List<string> ResearchAreas { get; set; } = new();

        [JsonIgnore]
        public string FullName => $"{LastName} {FirstName}";

        public override string ToString()
        {
            return $"{FullName} ({Email}) - {Affiliation}";
        }
    }
}
