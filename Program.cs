
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.Json;

class Program
{
    static void Main()
    {

        DB Books = new DB("Books.json", "Clients.json", "BorrowList.json");
        Book book1 = new Book(1, "To Kill a Mockingbird", "Harper Lee", "Fiction", 281, 10);
        Book book2 = new Book(2, "1984", "George Orwell", "Dystopian Fiction", 328, 15);
        Book book3 = new Book(3, "The Great Gatsby", "F. Scott Fitzgerald", "Classic Literature", 180, 8);
        DB.AddBook(book1, "Books.json");
        DB.AddBook(book2, "Books.json");
        DB.AddBook(book3, "Books.json");

    }


    class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int PageNr { get; set; }
        public int NrStock {  get; set; }
        public Book(int id,string title,string author,string genre,int pageNr,int nrStock)
        {
            Id = id;
            Title = Title;
            Author = Author;
            Genre = Genre;
            PageNr = PageNr;
            NrStock = nrStock;
        }

    }

    class Client
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegisterDate { get; set; }

        public string Address { get; set; }
        public string Phone { get; set; }
        public Client(int id, string name, string registerDate, string address, string phone) 
        {
            Id = id;
            Name = name;
            RegisterDate = registerDate;
            Address = address;
            Phone = phone;
        }
       
    }

    class BorrowedBooks
    {
        public int ClientID { get; set; }
        public int BookID { get; set; }
        public string BorrowDate { get; set; }

    }

    class DB
    {
          
        public List<BorrowedBooks> borrowedBooks = new List<BorrowedBooks>();

            public  string BooksourceFile { get; set; }
            public string ClientsourceFile { get; set; }
            public string BorrowListsourceFile { get; set; }


        public static void save(object obj, string sourceFile)
             {
            try
            {
                string jsonData = JsonSerializer.Serialize(obj);
                File.WriteAllText(sourceFile, jsonData);
                Console.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data {ex.Message}");
            }
        }

        public static string load(string sourceFile)
        {
            try
            {
                string jsonData = File.ReadAllText(sourceFile);
               
                return jsonData;
               
            }
            catch (Exception ex)
            {
                throw;
            }
        }
            public static void AddBook(Book book,string BooksourceFile)
            {
                 save(book, BooksourceFile);
            }

           public void deleteBook(Book book) 
            {

            string jsonData = load(BooksourceFile);
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(jsonData);
            books.Remove(book);
            save(books, BooksourceFile);

            }

            public Book SelectBook(Book book)
            {
                string jsonData = load(BooksourceFile);
                List<Book> books = JsonSerializer.Deserialize<List<Book>>(jsonData);

                foreach (Book bookk in books)
                {
                    if (book.Id == bookk.Id)
                    {
                        return book; 
                    }
                }

            
                return null;
            }

           public void updateBook(Book book)
            {
            string jsonData = load(BooksourceFile);
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(jsonData);


            int index = books.FindIndex(bookk => book.Id == book.Id);

            if (index != -1)
            {

                Console.WriteLine("Indicate parameters:");
                books[index].Author = Console.ReadLine();
                books[index].Genre = Console.ReadLine();
                books[index].Title = Console.ReadLine();
                string nrPage = Console.ReadLine();
                int pageNr;
                if (int.TryParse(nrPage, out pageNr))
                {
                   
                    books[index].PageNr = pageNr;
                }
                else
                {
                  
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }


                save(books, BooksourceFile);
                Console.WriteLine("Client updated successfully.");
            }
            else
            {
                Console.WriteLine("Error: Client not found in the list.");
            }
        }

           public static void registerClient(Client client,string ClientSourceFile)
            {

                string jsonData = load(ClientSourceFile);
                List<Client> clients = JsonSerializer.Deserialize<List<Client>>(jsonData);
                clients.Add(client);
                save(clients, ClientSourceFile);
             }

            public void removeClient(Client client,string ClientSourceFile)
            {
             string jsonData = load(ClientSourceFile);
             List<Client> clients = JsonSerializer.Deserialize<List<Client>>(jsonData);
             clients.Remove(client);
             save(clients, ClientSourceFile);
            }

        public Client SelectClient(Client client,string ClientSourceFile)
        {
            string jsonData = load(ClientSourceFile);
            List<Client> clients = JsonSerializer.Deserialize<List<Client>>(jsonData);

            foreach (Client clientt in clients)
            {
                if (clientt.Id == clientt.Id)
                {
                    return clientt;
                }
            }


            return null;
        }

        public void updateClient(Client client,string ClientSourceFile)
            {
            string jsonData = load(ClientSourceFile);
            List<Client> clients = JsonSerializer.Deserialize<List<Client>>(jsonData);
           

            int index = clients.FindIndex(clientt => clientt.Id == client.Id);

            if (index != -1)
            {
               
                Console.WriteLine("Indicate parameters:");
                clients[index].Phone = Console.ReadLine();
                clients[index].Name = Console.ReadLine();
                clients[index].Address = Console.ReadLine();
                clients[index].RegisterDate = Console.ReadLine();

               
                save(clients, ClientSourceFile);
                Console.WriteLine("Client updated successfully.");
            }
            else
            {
                Console.WriteLine("Error: Client not found in the list.");
            }

        }

            
            public void AddborrowedBook(BorrowedBooks obj, string BooksSourceFile)
            {
                
               borrowedBooks.Add(obj);
                save(obj,BooksSourceFile);
            }


            public void RemoveBorrowedBook(BorrowedBooks obj, string BorrowListsourceFile)
            {
              borrowedBooks.Remove(obj);
              save(borrowedBooks, BorrowListsourceFile);
            }

            public void SortBooks(string BookSourceFile,int sortType=1)
            {
            string jsonData = load(BookSourceFile);
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(jsonData);
            if(sortType==1)
            {
                books.Sort((book1, book2) => book1.Title.CompareTo(book2.Title));
            }

            else
            {
                books.Sort((book1, book2) => book1.PageNr.CompareTo(book2.PageNr));
            }

            save(books, BookSourceFile);
          
        }

            public DB(string BookSourceFile, string ClientSourceFile,string BorrowListsourceFile)
            {
                BookSourceFile = BookSourceFile;
                ClientSourceFile = ClientSourceFile;
                BorrowListsourceFile = BorrowListsourceFile;
            }

            
            
    }
}
