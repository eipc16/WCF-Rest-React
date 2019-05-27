using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfRestService
{
    [ServiceBehavior(InstanceContextMode =InstanceContextMode.Single)]
    public class RestService : IRestService
    {
        private static List<BookEntity> books;

        private string validateBook(BookEntity book)
        {
            if (book.BookTitle == null || book.BookTitle == "")
                return "Niepoprawny tytuł!";

            if (book.Author == null || book.Author == "")
                return "Niepoprawny autor!";

            if (book.PublishYear < 1)
                return "Niepoprawny rok!";

            if (book.Publisher == null || book.Publisher == "")
                return "Niepoprawny wydawca!";

            return "OK";
        }

        public RestService()
        {
            books = new List<BookEntity>()
            {
                new BookEntity(1, "Książka 1", 2017, "Nieznany", "Nieznane"),
                new BookEntity(2, "Książka 2", 2018, "Nieznany", "Nieznane"),
                new BookEntity(3, "Książka 3", 2019, "Nieznany", "Nieznane")
            };
        }

        private long getHighestIndex()
        {
            return books.Max(b => b.BookID);
        }

        public string add(BookEntity book)
        {
            string validation = validateBook(book);

            if (validation != "OK")
                return validation;

            book.BookID = getHighestIndex() + 1;
            books.Add(book);
            return "Dodano książkę z BookID=" + book.BookID;
        }

        public string remove(string id)
        {
            long BookID = long.Parse(id);

            int bookIndex = books.FindIndex(b => b.BookID == BookID);
            if (bookIndex < 0)
                return "Książka z BookID=" + BookID + " nie istnieje!";

            books.RemoveAt(bookIndex);

            return "Usunięto książkę z BookID=" + BookID;
        }

        public string addXML(BookEntity book)
        {
            return add(book);
        }

        public string removeXML(string id)
        {
            return remove(id);
        }

        public List<BookEntity> getAll()
        {
            return books;
        }

        public List<BookEntity> getAllXML()
        {
            return getAll();
        }

        public BookEntity getById(string id)
        {
            long BookID = long.Parse(id);
            int bookIndex = books.FindIndex(b => b.BookID == BookID);

            if (bookIndex < 0)
                throw new Exception("Nie odnaleziono książki z BookID=" + BookID);

            return books[bookIndex];
        }

        public BookEntity getByIdXML(string id)
        {
            return getById(id);
        }

        public string update(string id, BookEntity book)
        {
            long BookID = long.Parse(id);

            string validation = validateBook(book);

            if (validation != "OK")
                return validation;

            int bookIndex = books.FindIndex(b => b.BookID == BookID);

            if (bookIndex < 0)
                return "Książka z BookID=" + BookID + " nie istnieje!";

            book.BookID = BookID;

            books.RemoveAt(bookIndex);
            books.Add(book);

            return "Zaktualizowano książkę z BookID=" + BookID;
        }

        public string updateXML(string id, BookEntity book)
        {
            return update(id, book);
        }

        public List<BookEntity> getByName(string name)
        {
            return books.FindAll(b => b.BookTitle.Contains(name));
        }

        public List<BookEntity> getByNameXML(string name)
        {
            return getByName(name);
        }
    }
}
