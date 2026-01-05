using System.Text.Json;

namespace ScienceJournalism2026
{
    public static class DataManager
    {
        private static readonly string DataPath = Path.Combine("data", "journal_data.json");

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static Journal LoadJournal()
        {
            try
            {
                if (!File.Exists(DataPath))
                {
                    // Создаем новый журнал с демо-данными
                    var demoJournal = CreateDemoJournal(); // Изменено имя переменной
                    SaveJournal(demoJournal);
                    return demoJournal;
                }

                var json = File.ReadAllText(DataPath);
                var loadedJournal = JsonSerializer.Deserialize<Journal>(json, JsonOptions); // Изменено имя переменной

                return loadedJournal ?? CreateDemoJournal();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
                return CreateDemoJournal();
            }
        }

        public static void SaveJournal(Journal journalToSave) // Изменено имя параметра
        {
            try
            {
                // Создаем директорию если её нет
                var directory = Path.GetDirectoryName(DataPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonSerializer.Serialize(journalToSave, JsonOptions);
                File.WriteAllText(DataPath, json);
                Console.WriteLine("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }

        private static Journal CreateDemoJournal()
        {
            var demoJournal = new Journal // Изменено имя переменной
            {
                Name = "Science Journalism 2026",
                ISSN = "1234-5678",
                Publisher = "Научное издательство",
                Authors = new List<Author>
                {
                    new Author
                    {
                        Id = 1,
                        FirstName = "Иван",
                        LastName = "Петров",
                        Email = "i.petrov@university.ru",
                        Affiliation = "МГУ",
                        ResearchAreas = new List<string> { "Физика", "Математика" }
                    },
                    new Author
                    {
                        Id = 2,
                        FirstName = "Мария",
                        LastName = "Сидорова",
                        Email = "m.sidorova@science.ru",
                        Affiliation = "СПбГУ",
                        ResearchAreas = new List<string> { "Биология", "Химия" }
                    }
                },
                Editors = new List<Editor>
                {
                    new Editor
                    {
                        Id = 1,
                        FirstName = "Алексей",
                        LastName = "Иванов",
                        Email = "editor@journal.ru",
                        Specializations = new List<string> { "Естественные науки" }
                    }
                },
                Articles = new List<Article>
                {
                    new Article
                    {
                        Id = 1,
                        Title = "Новые подходы в квантовых вычислениях",
                        Abstract = "В статье рассматриваются перспективные направления...",
                        Keywords = new List<string> { "квантовые вычисления", "физика" },
                        AuthorIds = new List<int> { 1 },
                        SubmissionDate = DateTime.Now.AddDays(-30),
                        PublicationDate = DateTime.Now.AddDays(-7),
                        Status = ArticleStatus.Published,
                        DOI = "10.1234/quantum2026",
                        Citations = 5,
                        Reviews = new List<Review>()
                    },
                    new Article
                    {
                        Id = 2,
                        Title = "Биоразлагаемые полимеры",
                        Abstract = "Исследование новых экологичных материалов...",
                        Keywords = new List<string> { "полимеры", "экология", "химия" },
                        AuthorIds = new List<int> { 2 },
                        SubmissionDate = DateTime.Now.AddDays(-15),
                        Status = ArticleStatus.UnderReview,
                        DOI = "10.1234/polymers2026",
                        Reviews = new List<Review>()
                    }
                },
                Issues = new List<Issue>
                {
                    new Issue
                    {
                        Volume = 1,
                        Number = 1,
                        PublicationDate = DateTime.Now.AddDays(-7),
                        ArticleIds = new List<int> { 1 },
                        Theme = "Научные инновации 2026"
                    }
                }
            };

            return demoJournal;
        }
    }
}