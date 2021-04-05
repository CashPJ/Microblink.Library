using Microblink.Library.Services.Models.Dto.Interfaces;
using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microblink.Library.Services.Context.Interfaces
{
    /// <summary>
    /// Database context
    /// </summary>
    public interface IDatabaseContext
    {
        /// <summary>
        /// Filter books
        /// <para>If filter is null or empty, all results will be returned.</para>
        /// </summary>
        /// <param name="filter"></param>
        Task<IEnumerable<IBook>> SearchBooks(Hashtable filter = null);

        /// <summary>
        /// Returns all books
        /// </summary>
        Task<IEnumerable<IBook>> GetBooks();

        /// <summary>
        /// Filter book titles
        /// <para>If filter is null or empty, all results will be returned.</para>
        /// </summary>
        /// <param name="filter"></param>
        Task<IEnumerable<IBookTitle>> SearchBookTitles(Hashtable filter = null);

        /// <summary>
        /// Returns all book titles
        /// </summary>
        Task<IEnumerable<IBookTitle>> GetBookTitles();
        

        /// <summary>
        /// Returns specific book title
        /// </summary>
        /// <param name="bookTitleid"></param>
        Task<IBookTitle> GetBookTitle(int bookTitleid);

        /// <summary>
        /// Returns specific book
        /// </summary>
        /// <param name="bookid"></param>
        Task<IBook> GetBook(int bookid);

        /// <summary>
        /// Rents book title
        /// </summary>
        /// <param name="bookTitleId"></param>
        /// <param name="userId"></param>
        /// <param name="durationInDays"></param>
        /// <returns></returns>
        Task<IModelContainer<IRent>> RentBookTitle(int bookTitleId, int userId, int durationInDays);

        /// <summary>
        /// Return rented book title
        /// </summary>
        /// <param name="rentId"></param>
        /// <param name="bookId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IModelContainer<IRent>> ReturnRentedBookTitle(int rentId, int bookId, int userId);

        /// <summary>
        /// Gets available book id for book title.
        /// </summary>
        /// <param name="bookTitleId"></param>
        /// <returns></returns>
        Task<IBook> GetAvailableBook(int bookTitleId);

        /// <summary>
        /// Gets book rent data
        /// </summary>
        /// <param name="rentId"></param>
        /// <returns></returns>
        Task<IRent> GetRent(int rentId);

        /// <summary>
        /// Filter users
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<IUser>> SearchUsers(Hashtable filter = null);

        /// <summary>
        /// Gets all users
        /// </summary>
        Task<IEnumerable<IUser>> GetUsers();

        
        /// <summary>
        /// Gets specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IModelContainer<IUser>> GetUser(int userId);

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dateOfBirt"></param>
        /// <returns></returns>
        Task<IUser> CreateUser(string firstName, string lastName, DateTime dateOfBirt, bool? isValidMrz = null);

        /// <summary>
        /// Updates user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dateOfBirt"></param>
        /// <returns></returns>
        Task<IModelContainer<IUser>> UpdateUser(int userId, string firstName, string lastName, DateTime dateOfBirt);

        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IModelContainer<IUser>> DeleteUser(int userId);

        /// <summary>
        /// Gets contact types codebook
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IContactType>> GetContactTypes();

        /// <summary>
        /// Gets users contacts
        /// </summary>
        /// <returns></returns>
        Task<IModelContainer<IEnumerable<IContact>>> GetUserContacts(int userId);

        /// <summary>
        /// Creates users contact
        /// </summary>
        /// <returns></returns>
        Task<IContact> CreateContact(int userId, int contactTypeId, string value);

        /// <summary>
        /// Deletes users contact
        /// </summary>
        /// <returns></returns>
        Task<IModelContainer<IContact>> DeleteContact(int contactId, int userId);

        /// <summary>
        /// Search contacts
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<IContact>> SearchContacts(Hashtable filter = null);

        /// <summary>
        /// Gets specific contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Task<IModelContainer<IContact>> GetContact(int contactId);


        /// <summary>
        /// Gets rent history report
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<IEnumerable<IReportRentHistory>> GetRentHistoryReport(int limit = int.MaxValue);

        /// <summary>
        /// Gets top total overdue users report
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<IEnumerable<IReportTopOverdue>> GetTopOverdueReport(int limit = int.MaxValue);
    }
}
