using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfRestService
{
    [ServiceContract]
    public interface IRestService
    {
        [OperationContract]
        [WebGet(
            UriTemplate = "/json/books",
            ResponseFormat = WebMessageFormat.Json)]
        List<BookEntity> getAll();

        [OperationContract]
        [WebGet(
            UriTemplate = "/xml/books",
            ResponseFormat = WebMessageFormat.Xml)]
        List<BookEntity> getAllXML();

        [OperationContract]
        [WebGet(
            UriTemplate = "/json/books/{id}",
            ResponseFormat = WebMessageFormat.Json)]
        BookEntity getById(string id);

        [OperationContract]
        [WebGet(
            UriTemplate = "/xml/books/{id}",
            ResponseFormat = WebMessageFormat.Xml)]
        BookEntity getByIdXML(string id);

        [OperationContract]
        [WebGet(
        UriTemplate = "/json/books?name={name}",
        ResponseFormat = WebMessageFormat.Json)]
        List<BookEntity> getByName(string name);

        [OperationContract]
        [WebGet(
            UriTemplate = "/xml/books?name={name}",
            ResponseFormat = WebMessageFormat.Xml)]
        List<BookEntity> getByNameXML(string name);

        [OperationContract]
        [WebInvoke(
            UriTemplate = "/json/books/{id}",
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string update(string id, BookEntity book);

        [OperationContract]
        [WebInvoke(
        UriTemplate = "/xml/books/{id}",
        Method = "PUT",
        ResponseFormat = WebMessageFormat.Xml)]
        string updateXML(string id, BookEntity book);

        [OperationContract]
        [WebInvoke(
        UriTemplate = "/json/books",
        Method = "POST",
        ResponseFormat = WebMessageFormat.Json)]
        string add(BookEntity book);

        [OperationContract]
        [WebInvoke(
        UriTemplate = "/xml/books",
        Method = "POST",
        ResponseFormat = WebMessageFormat.Xml)]
        string addXML(BookEntity book);

        [OperationContract]
        [WebInvoke(
        UriTemplate = "/json/books/{id}",
        Method = "DELETE",
        ResponseFormat = WebMessageFormat.Json)]
        string remove(string id);

        [OperationContract]
        [WebInvoke(
        UriTemplate = "/xml/books/{id}",
        Method = "DELETE",
        ResponseFormat = WebMessageFormat.Xml)]
        string removeXML(string id);
    }


    [DataContract(IsReference = false)]
    public class BookEntity
    {
        [DataMember]
        public long BookID;

        [DataMember]
        public string BookTitle;

        [DataMember]
        public string Author;

        [DataMember]
        public string Publisher;

        [DataMember]
        public int PublishYear;

        public BookEntity(long BookID, string BookTitle, int PublishYear, string Author, string Publisher)
        {
            this.BookID = BookID;
            this.BookTitle = BookTitle;
            this.Author = Author;
            this.Publisher = Publisher;
            this.PublishYear = PublishYear;
        }
    }
}
