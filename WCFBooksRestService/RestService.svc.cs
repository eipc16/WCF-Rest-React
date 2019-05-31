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

        private static IBooksRepository booksRepository;

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
            booksRepository = new BooksRepository();
        }

        public string add(BookEntity book)
        {
            string validation = validateBook(book);

            if (validation != "OK")
                return validation;

            if(booksRepository.InsertBook(book))
            {
                return "Dodano książkę z BookID=" + book.BookID;
            }

            return "Nie udało się dodać książki";

        }

        public string remove(string id)
        {
            long BookID = long.Parse(id);

            if (booksRepository.DeleteBook(long.Parse(id)))
            {
                return "Usunięto książkę z BookID=" + id;
            }

            return $"Książka z BookID={id} nie istnieje!";
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
            return booksRepository.GetBooks();
        }

        public List<BookEntity> getAllXML()
        {
            return getAll();
        }

        public BookEntity getById(string id)
        {
            BookEntity book = booksRepository.GetBook(long.Parse(id));

            if (book == null)
                throw new Exception("Nie odnaleziono książki z BookID=" + id);

            return book;
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

            if (booksRepository.UpdateBook(BookID, book))
            {
                return "Zaktualizowano książkę z BookID=" + book.BookID;
            }

            return "Nie udało się dodać książki";
        }

        public string updateXML(string id, BookEntity book)
        {
            return update(id, book);
        }

        public List<BookEntity> getByName(string name)
        {
            return booksRepository.GetBooksByTitle(name);            
        }

        public List<BookEntity> getByNameXML(string name)
        {
            return getByName(name);
        }
    }
}
