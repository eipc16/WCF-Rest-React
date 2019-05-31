using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfRestService
{
    interface IBooksRepository
    {
        List<BookEntity> GetBooksByTitle(string bookTitle);

        List<BookEntity> GetBooks();

        BookEntity GetBook(long bookId);

        bool InsertBook(BookEntity bookEntity);

        bool DeleteBook(long bookId);

        bool UpdateBook(long bookId, BookEntity bookEntity);
    }
}
