using Microsoft.EntityFrameworkCore;

namespace ConsoleApp13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //using (ApplicationContext db = new ApplicationContext())
            //{
            //    db.Database.EnsureDeleted();
            //    db.Database.EnsureCreated();

            //    var authors = new Author[]
            //    {
            //        new Author("Taras Shevchenko"),
            //        new Author("Jeorge Orwell")
            //    };
            //    db.Authors.AddRange(authors);

            //    var genres = new Genre[]
            //    {
            //        new Genre("Roman"),
            //        new Genre("Poetry")
            //    };

            //    db.Genres.AddRange(genres);

            //    var books = new Book[]
            //    {
            //        new Book("1984",200m){Author=authors[1],Genre=genres[0]},
            //        new Book("Animal Farm",250m){Author=authors[1],Genre=genres[0]},
            //        new Book("Kobsar",500m){Author=authors[0],Genre=genres[1]},

            //    };
            //    db.Books.AddRange(books);
            //    db.SaveChanges();
            //}

            using (ApplicationContext db = new ApplicationContext())
            {
                //1 Получить количество книг определенного жанра.
                int countBooksByGenre=db.Books.Count(b=>b.Genre.Name=="Roman");
                //2 Получить минимальную цену для книг определенного автора.
                decimal minPriceByAuthor=db.Books.Where(a=>a.Author.Name== "Taras Shevchenko").Min(b=>b.Price);
                //3 Получить среднюю цену книг в определенном жанре.
                var midPriceByGenre=db.Books.Where(a=>a.Genre.Name=="Roman").Average(b=>b.Price);
                //4 Получить суммарную стоимость всех книг определенного автора.
                var sumPriceByAuthor=db.Books.Where(a => a.Author.Name == "Taras Shevchenko").Sum(b=>b.Price);
                //5 Выполнить группировку книг по жанрам.
                var booksByGenre = db.Books.GroupBy(b => b.Genre.Name);
                //6 Выбрать только названия книг определенного жанра.
                var titlesByGenre=db.Books.Where(a => a.Genre.Name == "Roman").Select(a=>a.Title);
                //7 Выбрать все книги, кроме тех, что относятся к определенному жанру, используя метод Except.
                var booksExcept=db.Books.Except(db.Books.Where(a => a.Genre.Name == "Roman"));
                //8 Объединить книги от двух авторов, используя метод Union.
                var booksUnited = db.Books.Where(a => a.Author.Name == "Taras Shevchenko").Union(db.Books.Where(a => a.Author.Name == "Jeorge Orwell"));
                //9 Достать 5-ть самых дорогих книг.
                var topExpensiveBooks=db.Books.OrderByDescending(a=>a.Price).Take(5).ToList();
                //10 Пропустить первые 10 книг и взять следующие 5.
                var books = db.Books.Skip(10).Take(5);
            }
        }
    }


    public class Genre
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public ICollection<Book>Books { get; set; }
        public Genre(string name)
        {
            Name = name;
        }
    }

    public class Author
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public ICollection<Book> Books { get; set; }
        public Author(string name)
        {
            Name = name;
        }
    }

    public class Book
    {
        public Book(string title,decimal price)
        {
            Title = title;
            Price = price;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorID { get; set; }
        public int GenreId {  get; set; }
        public decimal Price { get; set; }
        public Author Author { get; set; }
        public Genre Genre { get; set; }    


    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=shop;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasOne(a => a.Author).WithMany(a => a.Books);
            modelBuilder.Entity<Book>().HasOne(a => a.Genre).WithMany(a => a.Books);

        }
    }
}
