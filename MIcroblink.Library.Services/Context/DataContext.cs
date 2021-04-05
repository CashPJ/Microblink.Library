using Microblink.Library.Services.Context.Interfaces;
using Microblink.Library.Services.Models.Dto.Interfaces;
using Microblink.Library.Services.Models.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microblink.Library.Services.Context
{

    /// <summary>
    /// Data fascade context
    /// </summary>
    public partial class DataContext : IDataContext
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IMicroblinkContext _microblinkContext;
        private readonly ILogger<DataContext> _logger;

        public DataContext(ILogger<DataContext> logger, IDatabaseContext databaseContext, IMicroblinkContext microblinkContext)
        {
            _logger = logger;
            _microblinkContext = microblinkContext;
            _databaseContext = databaseContext;
        }

        /// <inheritdoc/>
        public async Task<IContact> CreateContact(int userId, int contactTypeId, string value)
        {
            return await _databaseContext.CreateContact(userId, contactTypeId, value);
        }

        /// <inheritdoc/>
        public async Task<IUser> CreateUser(string firstName, string lastName, DateTime dateOfBirt, bool? isValidMrz = null)
        {
            return await _databaseContext.CreateUser(firstName, lastName, dateOfBirt, isValidMrz);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IContact>> DeleteContact(int contactId, int userId)
        {
            return await _databaseContext.DeleteContact(contactId, userId);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IUser>> DeleteUser(int userId)
        {
            return await _databaseContext.DeleteUser(userId);
        }

        /// <inheritdoc/>
        public async Task<IBook> GetAvailableBook(int bookTitleId)
        {
            return await _databaseContext.GetAvailableBook(bookTitleId);
        }

        /// <inheritdoc/>
        public async Task<IBook> GetBook(int bookid)
        {
            return await _databaseContext.GetBook(bookid);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IBook>> GetBooks()
        {
            return await _databaseContext.SearchBooks();
        }

        /// <inheritdoc/>
        public async Task<IBookTitle> GetBookTitle(int bookTitleid)
        {
            return await _databaseContext.GetBookTitle(bookTitleid);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IBookTitle>> GetBookTitles()
        {
            return await _databaseContext.SearchBookTitles();
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IContact>> GetContact(int contactId)
        {
            return await _databaseContext.GetContact(contactId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IContactType>> GetContactTypes()
        {
            return await _databaseContext.GetContactTypes();
        }

        /// <inheritdoc/>
        public async Task<IRent> GetRent(int rentId)
        {
            return await _databaseContext.GetRent(rentId);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IUser>> GetUser(int userId)
        {
            return await _databaseContext.GetUser(userId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IUser>> GetUsers()
        {
            return await _databaseContext.GetUsers();
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IContact>>> GetUserContacts(int userId)
        {
            return await _databaseContext.GetUserContacts(userId);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IRent>> RentBookTitle(int bookTitleId, int userId, int durationInDays)
        {
            return await _databaseContext.RentBookTitle(bookTitleId, userId, durationInDays);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IRent>> ReturnRentedBookTitle(int rentId, int bookId, int userId)
        {
            return await _databaseContext.ReturnRentedBookTitle(rentId, bookId, userId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IBook>> SearchBooks(Hashtable filter = null)
        {
            return await _databaseContext.SearchBooks(filter);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IBookTitle>> SearchBookTitles(Hashtable filter = null)
        {
            return await _databaseContext.SearchBookTitles(filter);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IContact>> SearchContacts(Hashtable filter = null)
        {
            return await _databaseContext.SearchContacts(filter);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IUser>> SearchUsers(Hashtable filter = null)
        {
            return await _databaseContext.SearchUsers(filter);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IUser>> UpdateUser(int userId, string firstName, string lastName, DateTime dateOfBirt)
        {
            return await _databaseContext.UpdateUser(userId, firstName, lastName, dateOfBirt);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IReportRentHistory>> GetRentHistoryReport(int limit)
        {
            return await _databaseContext.GetRentHistoryReport(limit);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IReportTopOverdue>> GetTopOverdueReport(int limit)
        {
            return await _databaseContext.GetTopOverdueReport(limit);
        }

        /// <inheritdoc/>        
        public async Task<IModelContainer<IUser>> OcrData(string idCardbase64image)
        {
            return await _microblinkContext.OcrData(idCardbase64image);
        }
    }
}
