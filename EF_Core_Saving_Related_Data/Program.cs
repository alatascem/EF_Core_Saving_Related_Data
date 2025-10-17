
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

Console.WriteLine("Hello, World!");

ApplicationDbContext context = new();

#region One to One Iliskisel Senaryolarda Veri Ekleme
//Eger ki principal entity uzerinden ekleme gerceklestiriliyorsa dependent entity nesnesi verilmek zorunda degildir. Amma velakin, dependent entity uzerinden ekleme islemi gerceklestiriliyorsa eger burada principal entity'in nesnesine ihtiyacimiz zaruridir.
#region 1. Yontem -> Principal Entity Uzerinden Dependent Entity Verisi Ekleme
//Person person = new();
//person.Name = "Cem";
//person.Address = new() { PersonAddress = "Denizli" };

//await context.AddAsync(person);
//await context.SaveChangesAsync();
#endregion
#region 2. Yontem -> Dependent Entity Uzerinden Principal Entity Verisi Ekleme
//Address address = new()
//{
//    PersonAddress = "Ankara",
//    Person = new() { Name = "Ali" }
//};

//await context.AddAsync(address);
//await context.SaveChangesAsync();
#endregion
//class Person
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public Address Address { get; set; }

//}
//class Address
//{
//    public int Id { get; set; }
//    public string PersonAddress { get; set; }
//    public Person Person { get; set; }

//}

//class ApplicationDbContext : DbContext
//{
//    public DbSet<Person> Persons { get; set; }
//    public DbSet<Address> Addresses { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        optionsBuilder.UseSqlServer("Server=DESKTOP-V99G48T;Database=ApplicationDb;Trusted_Connection=True;TrustServerCertificate=True;");
//    }
//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Address>()
//             .HasOne(a => a.Person)
//             .WithOne(p => p.Address)
//             .HasForeignKey<Address>(a => a.Id);
//    }
//}
#endregion
#region One to Many Iliskisel Senaryolarda Veri Ekleme
#region 1. Yontem -> Principal Entity Uzerinden Dependent Entity Verisi Ekleme
#region Nesne Referansi Uzerinden Ekleme
//Blog blog = new() { Name="exampleBlog.com"};
//blog.Posts.Add(new() { Title = "Post1" });
//blog.Posts.Add(new() { Title = "Post2" });
//blog.Posts.Add(new() { Title = "Post3" });

//await context.AddAsync(blog);
//await context.SaveChangesAsync();
#endregion
#region Object Initializer Uzerinden Ekleme
//Blog blog1 = new()
//{
//    Name = "testBlog.com",
//    Posts = new HashSet<Post>() { new() { Title = "Post3" }, new() { Title = "Post5" } }
//};

//await context.AddAsync(blog1);
//await context.SaveChangesAsync();
#endregion
#endregion
#region 2. Yontem -> Foreign Key Kolonu Uzerinden Veri Ekleme
//1. yontem hic olmayan verilerin iliskisel olarak eklenmesini saglarken, bu 2. yontem onceden eklenmis olan bir principal entity verisiyle yeni dependent entity'lerin iliskisel olarak eslestirilmesini saglamaktadir.
//Post post = new()
//{
//    Id = 1,
//    Title = "Post1"
//};

//await context.AddAsync(post);
//await context.SaveChangesAsync();
#endregion
//class Blog
//{
//    public Blog()
//    {
//        Posts = new HashSet<Post>();
//    }
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public ICollection<Post> Posts { get; set; }
//}

//class Post
//{
//    public int Id { get; set; }
//    public int BlogId { get; set; }
//    public string Title { get; set; }
//    public Blog Blog { get; set; }
//}

//class ApplicationDbContext : DbContext
//{
//    public DbSet<Blog> Blogs { get; set; }
//    public DbSet<Post> Posts { get; set; }
//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        optionsBuilder.UseSqlServer("Server=DESKTOP-V99G48T;Database=ApplicationDb;Trusted_Connection=True;TrustServerCertificate=True;");
//    }
//}
#endregion
#region Many to Many Iliskisel Senaryolarda Veri Ekleme
#region 1. Yontem -> Default Convention Ile Tasarlanmis Iliski
//Many to many iliskisi eger ki default convention uzerinden tasarlanmissa kullanilan bir yontemdir.
//Book book = new()
//{
//    BookName = "A Book",
//    Authors = new HashSet<Author>() { new() { AuthorName = "Hasan" }, new() { AuthorName = "Fatma" } }
//};

//await context.AddAsync(book);
//await context.SaveChangesAsync();
//class Book
//{
//    public Book()
//    {
//        Authors = new HashSet<Author>();
//    }
//    public int Id { get; set; }
//    public string BookName { get; set; }
//    public ICollection<Author> Authors { get; set; }
//}

//class Author
//{
//    public Author()
//    {
//        Books = new HashSet<Book>();
//    }
//    public int Id { get; set; }
//    public string AuthorName { get; set; }
//    public ICollection<Book> Books { get; set; }
//}

//class ApplicationDbContext : DbContext
//{
//    public DbSet<Book> Books { get; set; }
//    public DbSet<Author> Authors { get; set; }
//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        optionsBuilder.UseSqlServer("Server=DESKTOP-V99G48T;Database=ApplicationDb;Trusted_Connection=True;TrustServerCertificate=True;");
//    }
//}
#endregion
#region 2. Yontem -> Fluent API Ile Tasarlanmis Iliski
//Many to many iliskisi eger ki fluent api ile tasarlanmissa kullanilan yontemdir.
Author author = new() 
{
    AuthorName = "Ayşe",
    Books = new HashSet<AuthorBook>()
    {
        new() { BookId = 1 },
        new() {Book=new() { BookName="B Book"} }
    }
};

await context.AddAsync(author);
await context.SaveChangesAsync();

class Book
{
    public Book()
    {
        Authors = new HashSet<AuthorBook>();
    }
    public int Id { get; set; }
    public string BookName { get; set; }
    public ICollection<AuthorBook> Authors { get; set; }
}
class AuthorBook
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    public Book Book { get; set; }
    public Author Author { get; set; }
}
class Author
{
    public Author()
    {
        Books = new HashSet<AuthorBook>();
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public ICollection<AuthorBook> Books { get; set; }
}

class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-V99G48T;Database=ApplicationDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorBook>()
             .HasKey(ba => new { ba.AuthorId, ba.BookId });

        modelBuilder.Entity<AuthorBook>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.Authors)
            .HasForeignKey(ba => ba.BookId);

        modelBuilder.Entity<AuthorBook>()
            .HasOne(ba => ba.Author)
            .WithMany(b => b.Books)
            .HasForeignKey(ba => ba.AuthorId);

    }
}
#endregion
#endregion
