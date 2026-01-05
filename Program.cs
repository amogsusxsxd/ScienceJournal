using System.Text.Json;

namespace ScienceJournalism2026
{
    class Program
    {
        private static Journal journal = null!;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Загрузка данных
            journal = DataManager.LoadJournal();

            Console.WriteLine("====================================");
            Console.WriteLine("НАУЧНЫЙ ЭЛЕКТРОННЫЙ ЖУРНАЛ 2026");
            Console.WriteLine("====================================");

            bool exit = false;
            while (!exit)
            {
                DisplayMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageArticles();
                        break;
                    case "2":
                        ManageAuthors();
                        break;
                    case "3":
                        ViewJournalInfo();
                        break;
                    case "4":
                        SearchArticles();
                        break;
                    case "5":
                        CreateNewIssue();
                        break;
                    case "6":
                        GenerateStatistics();
                        break;
                    case "7":
                        DataManager.SaveJournal(journal);
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Сохранение данных...");
                        DataManager.SaveJournal(journal);
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine($"ЖУРНАЛ: {journal.Name}");
            Console.WriteLine($"Всего статей: {journal.Articles.Count}");
            Console.WriteLine($"Всего авторов: {journal.Authors.Count}");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Управление статьями");
            Console.WriteLine("2. Управление авторами");
            Console.WriteLine("3. Информация о журнале");
            Console.WriteLine("4. Поиск статей");
            Console.WriteLine("5. Создать новый выпуск");
            Console.WriteLine("6. Статистика");
            Console.WriteLine("7. Сохранить данные");
            Console.WriteLine("0. Выход");
            Console.WriteLine("====================================");
            Console.Write("Выберите действие: ");
        }

        static void ManageArticles()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ СТАТЬЯМИ ===");
                Console.WriteLine("1. Просмотреть все статьи");
                Console.WriteLine("2. Добавить новую статью");
                Console.WriteLine("3. Редактировать статью");
                Console.WriteLine("4. Изменить статус статьи");
                Console.WriteLine("5. Просмотреть детали статьи");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayAllArticles();
                        break;
                    case "2":
                        AddNewArticle();
                        break;
                    case "3":
                        EditArticle();
                        break;
                    case "4":
                        ChangeArticleStatus();
                        break;
                    case "5":
                        ViewArticleDetails();
                        break;
                    case "0":
                        back = true;
                        break;
                }

                if (!back && choice != "0")
                {
                    Console.WriteLine("\nНажмите любую клавишу...");
                    Console.ReadKey();
                }
            }
        }

        static void DisplayAllArticles()
        {
            Console.WriteLine("\n=== ВСЕ СТАТЬИ ===");
            if (journal.Articles.Count == 0)
            {
                Console.WriteLine("Статей нет.");
                return;
            }

            foreach (var article in journal.Articles.OrderByDescending(a => a.SubmissionDate))
            {
                Console.WriteLine($"[ID: {article.Id}] {article}");
            }
        }

        static void AddNewArticle()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОЙ СТАТЬИ ===");

            var article = new Article
            {
                Id = journal.NextArticleId
            };

            Console.Write("Название статьи: ");
            article.Title = Console.ReadLine() ?? string.Empty;

            Console.Write("Аннотация: ");
            article.Abstract = Console.ReadLine() ?? string.Empty;

            Console.Write("Ключевые слова (через запятую): ");
            var keywords = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                article.Keywords = keywords.Split(',').Select(k => k.Trim()).ToList();
            }

            Console.WriteLine("\nДоступные авторы:");
            foreach (var author in journal.Authors)
            {
                Console.WriteLine($"[{author.Id}] {author.FullName}");
            }

            Console.Write("ID авторов (через запятую): ");
            var authorIds = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(authorIds))
            {
                article.AuthorIds = authorIds.Split(',')
                    .Select(id => int.TryParse(id.Trim(), out var num) ? num : -1)
                    .Where(id => id > 0)
                    .ToList();
            }

            article.SubmissionDate = DateTime.Now;
            article.Status = ArticleStatus.Submitted;

            Console.Write("DOI (если есть): ");
            article.DOI = Console.ReadLine() ?? string.Empty;

            journal.Articles.Add(article);
            Console.WriteLine($"Статья добавлена с ID: {article.Id}");
        }

        static void ManageAuthors()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ АВТОРАМИ ===");
                Console.WriteLine("1. Просмотреть всех авторов");
                Console.WriteLine("2. Добавить нового автора");
                Console.WriteLine("3. Найти автора по имени");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayAllAuthors();
                        break;
                    case "2":
                        AddNewAuthor();
                        break;
                    case "3":
                        SearchAuthorByName();
                        break;
                    case "0":
                        back = true;
                        break;
                }

                if (!back && choice != "0")
                {
                    Console.WriteLine("\nНажмите любую клавишу...");
                    Console.ReadKey();
                }
            }
        }

        static void DisplayAllAuthors()
        {
            Console.WriteLine("\n=== ВСЕ АВТОРЫ ===");
            if (journal.Authors.Count == 0)
            {
                Console.WriteLine("Авторов нет.");
                return;
            }

            foreach (var author in journal.Authors.OrderBy(a => a.LastName))
            {
                Console.WriteLine(author);
                var articles = journal.Articles.Where(a => a.AuthorIds.Contains(author.Id)).ToList();
                if (articles.Count > 0)
                {
                    Console.WriteLine($"  Статей: {articles.Count}");
                    foreach (var article in articles)
                    {
                        Console.WriteLine($"  - {article.Title} ({article.StatusString})");
                    }
                }
                Console.WriteLine();
            }
        }

        static void AddNewAuthor()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОГО АВТОРА ===");

            var author = new Author
            {
                Id = journal.NextAuthorId
            };

            Console.Write("Фамилия: ");
            author.LastName = Console.ReadLine() ?? string.Empty;

            Console.Write("Имя: ");
            author.FirstName = Console.ReadLine() ?? string.Empty;

            Console.Write("Email: ");
            author.Email = Console.ReadLine() ?? string.Empty;

            Console.Write("Аффилиация: ");
            author.Affiliation = Console.ReadLine() ?? string.Empty;

            Console.Write("Области исследований (через запятую): ");
            var areas = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(areas))
            {
                author.ResearchAreas = areas.Split(',').Select(a => a.Trim()).ToList();
            }

            journal.Authors.Add(author);
            Console.WriteLine($"Автор добавлен с ID: {author.Id}");
        }

        static void ViewJournalInfo()
        {
            Console.Clear();
            Console.WriteLine("=== ИНФОРМАЦИЯ О ЖУРНАЛЕ ===");
            Console.WriteLine($"Название: {journal.Name}");
            Console.WriteLine($"ISSN: {journal.ISSN}");
            Console.WriteLine($"Издатель: {journal.Publisher}");

            Console.WriteLine($"\nВсего статей: {journal.Articles.Count}");
            Console.WriteLine($"  - Опубликовано: {journal.Articles.Count(a => a.Status == ArticleStatus.Published)}");
            Console.WriteLine($"  - На рассмотрении: {journal.Articles.Count(a => a.Status == ArticleStatus.Submitted)}");
            Console.WriteLine($"  - На рецензии: {journal.Articles.Count(a => a.Status == ArticleStatus.UnderReview)}");

            Console.WriteLine($"\nВсего авторов: {journal.Authors.Count}");
            Console.WriteLine($"Всего выпусков: {journal.Issues.Count}");

            if (journal.Issues.Count > 0)
            {
                Console.WriteLine("\nПоследний выпуск:");
                var lastIssue = journal.Issues.OrderByDescending(i => i.PublicationDate).First();
                Console.WriteLine($"Том {lastIssue.Volume}, №{lastIssue.Number} ({lastIssue.PublicationDate:dd.MM.yyyy})");
                Console.WriteLine($"Тема: {lastIssue.Theme}");
                Console.WriteLine($"Статей в выпуске: {lastIssue.ArticleIds.Count}");
            }
        }

        static void SearchArticles()
        {
            Console.Clear();
            Console.WriteLine("=== ПОИСК СТАТЕЙ ===");
            Console.WriteLine("1. Поиск по ключевым словам");
            Console.WriteLine("2. Поиск по автору");
            Console.WriteLine("3. Поиск по статусу");
            Console.Write("Выберите тип поиска: ");

            var choice = Console.ReadLine();
            List<Article> results = new();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите ключевые слова: ");
                    var keywords = Console.ReadLine()?.ToLower() ?? string.Empty;
                    results = journal.Articles.Where(a =>
                        a.Keywords.Any(k => k.ToLower().Contains(keywords)) ||
                        a.Title.ToLower().Contains(keywords) ||
                        a.Abstract.ToLower().Contains(keywords)).ToList();
                    break;

                case "2":
                    Console.Write("Введите фамилию автора: ");
                    var lastName = Console.ReadLine()?.ToLower() ?? string.Empty;
                    var authorIds = journal.Authors
                        .Where(a => a.LastName.ToLower().Contains(lastName))
                        .Select(a => a.Id)
                        .ToList();
                    results = journal.Articles
                        .Where(a => a.AuthorIds.Any(id => authorIds.Contains(id)))
                        .ToList();
                    break;

                case "3":
                    Console.WriteLine("Выберите статус:");
                    Console.WriteLine("1. На рассмотрении");
                    Console.WriteLine("2. На рецензии");
                    Console.WriteLine("3. Опубликовано");
                    Console.WriteLine("4. Отклонено");
                    Console.Write("Выберите: ");
                    var statusChoice = Console.ReadLine();

                    ArticleStatus status = statusChoice switch
                    {
                        "1" => ArticleStatus.Submitted,
                        "2" => ArticleStatus.UnderReview,
                        "3" => ArticleStatus.Published,
                        "4" => ArticleStatus.Rejected,
                        _ => ArticleStatus.Submitted
                    };

                    results = journal.Articles.Where(a => a.Status == status).ToList();
                    break;
            }

            Console.WriteLine($"\nНайдено статей: {results.Count}");
            foreach (var article in results)
            {
                Console.WriteLine($"\n[{article.Id}] {article.Title}");
                Console.WriteLine($"Статус: {article.StatusString}");
                Console.WriteLine($"Дата подачи: {article.SubmissionDate:dd.MM.yyyy}");
                if (article.PublicationDate.HasValue)
                    Console.WriteLine($"Дата публикации: {article.PublicationDate.Value:dd.MM.yyyy}");
                Console.WriteLine($"Авторы: {string.Join(", ", article.AuthorIds.Select(id =>
                    journal.Authors.FirstOrDefault(a => a.Id == id)?.FullName ?? "Неизвестно"))}");
            }
        }

        static void CreateNewIssue()
        {
            Console.Clear();
            Console.WriteLine("=== СОЗДАНИЕ НОВОГО ВЫПУСКА ===");

            var issue = new Issue();

            Console.Write("Том: ");
            if (!int.TryParse(Console.ReadLine(), out int volume))
            {
                Console.WriteLine("Неверный фортом тома.");
                return;
            }
            issue.Volume = volume;

            Console.Write("Номер: ");
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine("Неверный фортом номера.");
                return;
            }
            issue.Number = number;

            Console.Write("Тема выпуска: ");
            issue.Theme = Console.ReadLine() ?? string.Empty;

            issue.PublicationDate = DateTime.Now;

            // Показываем статьи для включения
            var availableArticles = journal.Articles
                .Where(a => a.Status == ArticleStatus.Accepted || a.Status == ArticleStatus.Published)
                .Where(a => !journal.Issues.Any(i => i.ArticleIds.Contains(a.Id)))
                .ToList();

            Console.WriteLine("\nДоступные статьи для включения:");
            foreach (var article in availableArticles)
            {
                Console.WriteLine($"[{article.Id}] {article.Title}");
            }

            Console.Write("ID статей для включения (через запятую): ");
            var articleIds = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(articleIds))
            {
                var ids = articleIds.Split(',')
                    .Select(id => int.TryParse(id.Trim(), out var num) ? num : -1)
                    .Where(id => id > 0);

                foreach (var id in ids)
                {
                    if (availableArticles.Any(a => a.Id == id))
                    {
                        issue.ArticleIds.Add(id);
                        // Обновляем статус статьи
                        var article = journal.Articles.First(a => a.Id == id);
                        article.Status = ArticleStatus.Published;
                        article.PublicationDate = issue.PublicationDate;
                    }
                }
            }

            journal.Issues.Add(issue);
            Console.WriteLine($"Выпуск {issue.Volume}({issue.Number}) создан!");
        }

        static void GenerateStatistics()
        {
            Console.Clear();
            Console.WriteLine("=== СТАТИСТИКА ЖУРНАЛА ===");

            // Статистика по статьям
            var articlesByStatus = journal.Articles
                .GroupBy(a => a.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            Console.WriteLine("\nСтатистика по статусам статей:");
            foreach (var group in articlesByStatus)
            {
                string statusName = group.Status switch
                {
                    ArticleStatus.Submitted => "На рассмотрении",
                    ArticleStatus.UnderReview => "На рецензии",
                    ArticleStatus.RevisionRequired => "Требует доработки",
                    ArticleStatus.Accepted => "Принято",
                    ArticleStatus.Published => "Опубликовано",
                    ArticleStatus.Rejected => "Отклонено",
                    _ => "Неизвестно"
                };
                Console.WriteLine($"  {statusName}: {group.Count}");
            }

            // Статистика по авторам
            var authorsByArticles = journal.Authors
                .Select(a => new
                {
                    Author = a,
                    ArticleCount = journal.Articles.Count(art => art.AuthorIds.Contains(a.Id))
                })
                .OrderByDescending(x => x.ArticleCount)
                .ToList();

            Console.WriteLine("\nТоп авторов по количеству статей:");
            foreach (var authorStat in authorsByArticles.Take(5))
            {
                Console.WriteLine($"  {authorStat.Author.FullName}: {authorStat.ArticleCount} статей");
            }

            // Статистика по месяцам
            var articlesByMonth = journal.Articles
                .Where(a => a.SubmissionDate.Year == DateTime.Now.Year)
                .GroupBy(a => a.SubmissionDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderBy(g => g.Month)
                .ToList();

            Console.WriteLine("\nСтатьи по месяцам (текущий год):");
            foreach (var monthStat in articlesByMonth)
            {
                var monthName = new DateTime(DateTime.Now.Year, monthStat.Month, 1)
                    .ToString("MMMM", new System.Globalization.CultureInfo("ru-RU"));
                Console.WriteLine($"  {monthName}: {monthStat.Count} статей");
            }

            // Общее количество цитирований
            int totalCitations = journal.Articles.Sum(a => a.Citations);
            Console.WriteLine($"\nОбщее количество цитирований: {totalCitations}");

            // Самая цитируемая статья
            var mostCited = journal.Articles.OrderByDescending(a => a.Citations).FirstOrDefault();
            if (mostCited != null && mostCited.Citations > 0)
            {
                Console.WriteLine($"Самая цитируемая статья: \"{mostCited.Title}\" - {mostCited.Citations} цитирований");
            }
        }

        static void EditArticle()
        {
            Console.Write("Введите ID статьи для редактирования: ");
            if (!int.TryParse(Console.ReadLine(), out int articleId))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            var article = journal.Articles.FirstOrDefault(a => a.Id == articleId);
            if (article == null)
            {
                Console.WriteLine("Статья не найдена.");
                return;
            }

            Console.WriteLine($"\nРедактирование статьи: {article.Title}");
            Console.WriteLine("Что вы хотите редактировать?");
            Console.WriteLine("1. Название");
            Console.WriteLine("2. Аннотацию");
            Console.WriteLine("3. Ключевые слова");
            Console.WriteLine("4. Авторов");
            Console.WriteLine("5. DOI");
            Console.Write("Выберите: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Новое название: ");
                    article.Title = Console.ReadLine() ?? article.Title;
                    break;
                case "2":
                    Console.Write("Новая аннотация: ");
                    article.Abstract = Console.ReadLine() ?? article.Abstract;
                    break;
                case "3":
                    Console.Write("Новые ключевые слова (через запятую): ");
                    var keywords = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(keywords))
                    {
                        article.Keywords = keywords.Split(',').Select(k => k.Trim()).ToList();
                    }
                    break;
                case "4":
                    Console.WriteLine("Текущие авторы:");
                    foreach (var authorId in article.AuthorIds)
                    {
                        var author = journal.Authors.FirstOrDefault(a => a.Id == authorId);
                        Console.WriteLine($"  [{authorId}] {author?.FullName ?? "Неизвестно"}");
                    }
                    Console.Write("Новые ID авторов (через запятую): ");
                    var authorIds = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(authorIds))
                    {
                        article.AuthorIds = authorIds.Split(',')
                            .Select(id => int.TryParse(id.Trim(), out var num) ? num : -1)
                            .Where(id => id > 0)
                            .ToList();
                    }
                    break;
                case "5":
                    Console.Write("Новый DOI: ");
                    article.DOI = Console.ReadLine() ?? article.DOI;
                    break;
            }

            Console.WriteLine("Статья обновлена.");
        }

        static void ChangeArticleStatus()
        {
            Console.Write("Введите ID статьи: ");
            if (!int.TryParse(Console.ReadLine(), out int articleId))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            var article = journal.Articles.FirstOrDefault(a => a.Id == articleId);
            if (article == null)
            {
                Console.WriteLine("Статья не найдена.");
                return;
            }

            Console.WriteLine($"Текущий статус: {article.StatusString}");
            Console.WriteLine("Выберите новый статус:");
            Console.WriteLine("1. На рассмотрении");
            Console.WriteLine("2. На рецензии");
            Console.WriteLine("3. Требует доработки");
            Console.WriteLine("4. Принято");
            Console.WriteLine("5. Опубликовано");
            Console.WriteLine("6. Отклонено");
            Console.Write("Выберите: ");

            var choice = Console.ReadLine();
            article.Status = choice switch
            {
                "1" => ArticleStatus.Submitted,
                "2" => ArticleStatus.UnderReview,
                "3" => ArticleStatus.RevisionRequired,
                "4" => ArticleStatus.Accepted,
                "5" => ArticleStatus.Published,
                "6" => ArticleStatus.Rejected,
                _ => article.Status
            };

            if (article.Status == ArticleStatus.Published && !article.PublicationDate.HasValue)
            {
                article.PublicationDate = DateTime.Now;
            }

            Console.WriteLine($"Статус изменен на: {article.StatusString}");
        }

        static void ViewArticleDetails()
        {
            Console.Write("Введите ID статьи: ");
            if (!int.TryParse(Console.ReadLine(), out int articleId))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            var article = journal.Articles.FirstOrDefault(a => a.Id == articleId);
            if (article == null)
            {
                Console.WriteLine("Статья не найдена.");
                return;
            }

            Console.Clear();
            Console.WriteLine("=== ДЕТАЛИ СТАТЬИ ===");
            Console.WriteLine($"ID: {article.Id}");
            Console.WriteLine($"Название: {article.Title}");
            Console.WriteLine($"Статус: {article.StatusString}");
            Console.WriteLine($"DOI: {article.DOI}");
            Console.WriteLine($"Дата подачи: {article.SubmissionDate:dd.MM.yyyy HH:mm}");

            if (article.PublicationDate.HasValue)
                Console.WriteLine($"Дата публикации: {article.PublicationDate.Value:dd.MM.yyyy}");

            Console.WriteLine($"\nАннотация:\n{article.Abstract}");

            Console.WriteLine($"\nКлючевые слова: {string.Join(", ", article.Keywords)}");

            Console.WriteLine("\nАвторы:");
            foreach (var authorId in article.AuthorIds)
            {
                var author = journal.Authors.FirstOrDefault(a => a.Id == authorId);
                Console.WriteLine($"  - {author?.FullName ?? "Неизвестно"} ({author?.Email})");
            }

            Console.WriteLine($"\nКоличество цитирований: {article.Citations}");

            if (article.Reviews.Count > 0)
            {
                Console.WriteLine("\nРецензии:");
                foreach (var review in article.Reviews)
                {
                    var reviewer = journal.Editors.FirstOrDefault(e => e.Id == review.ReviewerId);
                    Console.WriteLine($"  Рецензент: {reviewer?.FullName ?? "Неизвестно"}");
                    Console.WriteLine($"  Дата: {review.ReviewDate:dd.MM.yyyy}");
                    Console.WriteLine($"  Оценка: {review.Score}/10");
                    Console.WriteLine($"  Рекомендация: {(review.Recommendation ? "Принять" : "Отклонить")}");
                    Console.WriteLine($"  Комментарии: {review.Comments}");
                    Console.WriteLine();
                }
            }
        }

        static void SearchAuthorByName()
        {
            Console.Write("Введите фамилию или часть фамилии автора: ");
            var searchTerm = Console.ReadLine()?.ToLower() ?? string.Empty;

            var authors = journal.Authors
                .Where(a => a.LastName.ToLower().Contains(searchTerm) ||
                           a.FirstName.ToLower().Contains(searchTerm) ||
                           a.FullName.ToLower().Contains(searchTerm))
                .ToList();

            Console.WriteLine($"\nНайдено авторов: {authors.Count}");
            foreach (var author in authors)
            {
                Console.WriteLine($"\n{author}");
                var articles = journal.Articles.Where(a => a.AuthorIds.Contains(author.Id)).ToList();
                Console.WriteLine($"Статей: {articles.Count}");

                if (articles.Count > 0)
                {
                    Console.WriteLine("Последние статьи:");
                    foreach (var article in articles.OrderByDescending(a => a.SubmissionDate).Take(3))
                    {
                        Console.WriteLine($"  - {article.Title} ({article.SubmissionDate:yyyy})");
                    }
                }
            }
        }
    }
}