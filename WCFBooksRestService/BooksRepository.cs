using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WcfRestService
{
    public class BooksRepository : IBooksRepository
    {

        public bool DeleteBook(long bookId)
        {
            int result = 0;
            
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand sqlCmd = new SqlCommand(@"DELETE FROM Book WHERE BookID = @bookId", conn);
                    sqlCmd.Parameters.AddWithValue("@bookId", bookId);
                    result = sqlCmd.ExecuteNonQuery();
                } catch (Exception)
                {
                    result = 0;
                } finally
                {
                    conn.Close();
                }

            }
            return result == 1 ? true : false;
        }

        public BookEntity GetBook(long bookId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT * FROM Book WHERE BookID = @bookId", conn);
                sqlCmd.Parameters.AddWithValue("@bookId", bookId);

                DataTable dt = new DataTable();

                using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                {
                    da.Fill(dt);
                }

                if(dt.Rows.Count > 0)
                {
                    BookEntity book = new BookEntity();
                    book.BookID = long.Parse(dt.Rows[0]["BookID"].ToString());
                    book.BookTitle = dt.Rows[0]["BookTitle"].ToString();
                    book.Author = dt.Rows[0]["Author"].ToString();
                    book.Publisher = dt.Rows[0]["Publisher"].ToString();
                    book.PublishYear = int.Parse(dt.Rows[0]["PublishYear"].ToString());

                    return book;
                }
            }

            return null;
        }

        public List<BookEntity> GetBooksByTitle(string bookTitle)
        {
            List<BookEntity> list = new List<BookEntity>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT * FROM Book WHERE BookTitle LIKE '%'+@bookTitle+'%'", conn);
                sqlCmd.Parameters.AddWithValue("@bookTitle", bookTitle);

                DataTable dt = new DataTable();

                using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                {
                    da.Fill(dt);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BookEntity book = new BookEntity();
                    book.BookID = long.Parse(dt.Rows[i]["BookID"].ToString());
                    book.BookTitle = dt.Rows[i]["BookTitle"].ToString();
                    book.Author = dt.Rows[i]["Author"].ToString();
                    book.Publisher = dt.Rows[i]["Publisher"].ToString();
                    book.PublishYear = int.Parse(dt.Rows[i]["PublishYear"].ToString());

                    list.Add(book);
                }
            }

            return list;
        }

        
        public List<BookEntity> GetBooks()
        {
            List<BookEntity> list = new List<BookEntity>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT * FROM Book", conn);

                DataTable dt = new DataTable();

                using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                {
                    da.Fill(dt);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BookEntity book = new BookEntity();
                    book.BookID = long.Parse(dt.Rows[i]["BookID"].ToString());
                    book.BookTitle = dt.Rows[i]["BookTitle"].ToString();
                    book.Author = dt.Rows[i]["Author"].ToString();
                    book.Publisher = dt.Rows[i]["Publisher"].ToString();
                    book.PublishYear = int.Parse(dt.Rows[i]["PublishYear"].ToString());

                    list.Add(book);
                }
            }

            return list;
        }

        public bool InsertBook(BookEntity bookEntity)
        {
            int result = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand sqlCmd = new SqlCommand("INSERT INTO Book([BookTitle], [Author], [Publisher], [PublishYear]) VALUES (@bookTitle, @author, @publisher, @publishYear)", conn);
                    sqlCmd.Parameters.AddWithValue("@bookTitle", bookEntity.BookTitle);
                    sqlCmd.Parameters.AddWithValue("@author", bookEntity.Author);
                    sqlCmd.Parameters.AddWithValue("@publisher", bookEntity.Publisher);
                    sqlCmd.Parameters.AddWithValue("@publishYear", bookEntity.PublishYear);

                    result = sqlCmd.ExecuteNonQuery();
                } catch (Exception)
                {
                    result = 0;
                } finally
                {
                    conn.Close();
                }
            }
            return result == 1 ? true : false;
        }

        public bool UpdateBook(long bookId, BookEntity bookEntity)
        {
            int result = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand sqlCmd = new SqlCommand("UPDATE Book SET [BookTitle] = @bookTitle, [Author] = @author, [Publisher] = @publisher, [PublishYear] = @publishYear WHERE BookID = @bookID", conn);
                    sqlCmd.Parameters.AddWithValue("@bookId", bookId);
                    sqlCmd.Parameters.AddWithValue("@bookTitle", bookEntity.BookTitle);
                    sqlCmd.Parameters.AddWithValue("@author", bookEntity.Author);
                    sqlCmd.Parameters.AddWithValue("@publisher", bookEntity.Publisher);
                    sqlCmd.Parameters.AddWithValue("@publishYear", bookEntity.PublishYear);

                    result = sqlCmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    result = 0;
                }
                finally
                {
                    conn.Close();
                }
            }
            return result == 1 ? true : false;
        }
    }
}