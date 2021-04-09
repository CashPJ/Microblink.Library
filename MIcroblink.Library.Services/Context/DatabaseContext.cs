using Mapper;
using Microblink.Library.Services.Context.Config.Interfaces;
using Microblink.Library.Services.Context.Interfaces;
using Microblink.Library.Services.Managers;
using Microblink.Library.Services.Models;
using Microblink.Library.Services.Models.Dto;
using Microblink.Library.Services.Models.Dto.Interfaces;
using Microblink.Library.Services.Models.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblink.Library.Services.Services.Context
{
    /// <summary>
    /// Database context
    /// </summary>
    public class DatabaseContext : IDatabaseContext
    {
        private readonly DatabaseManager _dm;
        private readonly IDataContextConfig _config;
        private readonly ILogger<DatabaseContext> _logger;


        public DatabaseContext(ILogger<DatabaseContext> logger, IDataContextConfig config)
        {
            _config = config;
            _logger = logger;
            _dm = DatabaseManager.Create(_config.DatabaseContextConfig.DatabaseConnectionString);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IBook>>> SearchBooks(Hashtable filter = null)
        {
            var dt = await _dm.ExecuteProcedure("usp_Book_Search", filter ?? new Hashtable());
            return new ModelContainer<IEnumerable<IBook>>(dt.Map<BookDto>(), ActionType.Read, ResponseStatusCode.Ok);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IBookTitle>>> SearchBookTitles(Hashtable filter = null)
        {
            var dt = await _dm.ExecuteProcedure("usp_BookTitle_Search", filter ?? new Hashtable());
            return new ModelContainer<IEnumerable<IBookTitle>> (dt.Map<BookTitleDto>(), ActionType.Read, ResponseStatusCode.Ok);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IBookTitle>> GetBookTitle(int bookTitleid)
        {
            var dt = await _dm.ExecuteProcedure("usp_BookTitle_Search", new Hashtable() {{ "@Id", bookTitleid }});
            var bookTitle = dt.Map<BookTitleDto>().FirstOrDefault();

            if (bookTitle == null)
                new ModelContainer<IBookTitle>(ActionType.Read, ResponseStatusCode.NotFound);

            return new ModelContainer<IBookTitle>(bookTitle, ActionType.Read, ResponseStatusCode.Ok); ;
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IBook>> GetBook(int bookid)
        {
            var dt = await _dm.ExecuteProcedure("usp_Book_Search", new Hashtable() { { "@Id", bookid } });
            var book = dt.Map<BookDto>().FirstOrDefault();

            if (book == null)
                new ModelContainer<IBook>(ActionType.Read, ResponseStatusCode.NotFound);

            return new ModelContainer<IBook>(book, ActionType.Read, ResponseStatusCode.Ok); ;
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IRent>> RentBookTitle(int bookTitleId, int userId, int durationInDays)
        {
            var result = await GetAvailableBook(bookTitleId);
            var availableBook = result.Model;
            if (availableBook == null)
                return new ModelContainer<IRent>(ActionType.Create, ResponseStatusCode.UnprocessableEntity)
                {
                    ErrorMessage = ModelContainerErrorMessages.Currently_Unavailable
                };


            var dt = await _dm.ExecuteProcedure("usp_Rent_Create", new Hashtable() { { "@BookId", availableBook.Id }, { "@UserId", userId }, { "@RentDurationInDays", durationInDays } });
            return new ModelContainer<IRent>(ActionType.Create, ResponseStatusCode.Ok)
            {
                Model = dt.Map<RentDto>().FirstOrDefault()
            };
        }

        /// <inheritdoc/>        
        public async Task<IModelContainer<IRent>> ReturnRentedBookTitle(int rentId, int bookId, int userId)
        {
            var result = await GetRent(rentId);
            var rent = result.Model;

            if (rent == null || rent.ReturnDate != null)
                return new ModelContainer<IRent>(ActionType.Update, ResponseStatusCode.UnprocessableEntity)
                {
                    ErrorMessage = ModelContainerErrorMessages.Non_Existing_Entity
                };

            var dt = await _dm.ExecuteProcedure("usp_Rent_Return", new Hashtable() { { "@RentId", rentId }, { "@BookId", bookId }, { "@UserId", userId } });
            return new ModelContainer<IRent>(ActionType.Update, ResponseStatusCode.Ok)
            {
                Model = dt.Map<RentDto>().FirstOrDefault()
            };
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IRent>> GetRent(int rentId)
        {
            var dt = await _dm.ExecuteProcedure("usp_Rent_Search", new Hashtable() { { "@RentId", rentId } });
            var rent = dt.Map<RentDto>().FirstOrDefault();

            if (rent == null)
                return new ModelContainer<IRent>(ActionType.Read, ResponseStatusCode.NotFound);

            return new ModelContainer<IRent>(rent, ActionType.Read, ResponseStatusCode.Ok);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IBook>>> GetBooks()
        {
            return await SearchBooks(null);
        }

        /// <inheritdoc/>
        public Task<IModelContainer<IEnumerable<IBookTitle>>> GetBookTitles()
        {
            return SearchBookTitles(null);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IBook>> GetAvailableBook(int bookTitleId)
        {
            var dt = await _dm.ExecuteProcedure("usp_BookTitle_GetAvailableBook", new Hashtable() { { "@BookTitleId", bookTitleId } });
            var book = dt.Map<BookDto>().FirstOrDefault();

            if (book == null)
                return new ModelContainer<IBook>(ActionType.Read, ResponseStatusCode.NotFound);

            return new ModelContainer<IBook>(book, ActionType.Read, ResponseStatusCode.Ok);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IUser>>> SearchUsers(Hashtable filter = null)
        {
            var dt = await _dm.ExecuteProcedure("usp_User_Search", filter ?? new Hashtable());
            return new ModelContainer<IEnumerable<IUser>>(dt.Map<UserDto>(), ActionType.Read, ResponseStatusCode.Ok);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IUser>>> GetUsers()
        {
            return await SearchUsers();
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IUser>> GetUser(int userId)
        {
            var dt = await _dm.ExecuteProcedure("usp_User_Search", new Hashtable() { { "@UserId", userId } });
            var user = dt.Map<UserDto>().FirstOrDefault();

            if(user == null)
                return new ModelContainer<IUser>(ActionType.Read, ResponseStatusCode.NotFound)
                {
                    ErrorMessage = ModelContainerErrorMessages.Non_Existing_Entity
                };

            return new ModelContainer<IUser>(ActionType.Read, ResponseStatusCode.Ok)
            {
                Model = user
            };
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IUser>> CreateUser(string firstName, string lastName, DateTime dateOfBirt, bool? isValidMrz = null)
        {
            var dt = await _dm.ExecuteProcedure("usp_User_Create", new Hashtable() { { "@FirstName", firstName }, { "@LastName", lastName }, { "@DateOfBirt", dateOfBirt }, { "@IsValidMrz", isValidMrz } });
            var user = dt.Map<UserDto>().FirstOrDefault();
            return new ModelContainer<IUser>(ActionType.Create, ResponseStatusCode.Created)
            {
                Model = user
            };
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IUser>> UpdateUser(int userId, string firstName, string lastName, DateTime dateOfBirt)
        {
            if(string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                return new ModelContainer<IUser>(ActionType.Update, ResponseStatusCode.BadRequest)
                {
                    ErrorMessage = ModelContainerErrorMessages.Invalid_Input
                };

            var userModelContainer = await GetUser(userId);
            if (!userModelContainer.IsSuccessful)
                return userModelContainer;

            var dt = await _dm.ExecuteProcedure("usp_User_Update", new Hashtable() { { "@UserId", userId }, { "@FirstName", firstName }, { "@LastName", lastName }, { "@DateOfBirt", dateOfBirt } });
            return new ModelContainer<IUser>(ActionType.Update, ResponseStatusCode.Ok)
            {
                Model = dt.Map<UserDto>().FirstOrDefault()
            };
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IUser>> DeleteUser(int userId)
        {
            var userModelContainer = await GetUser(userId);
            if (!userModelContainer.IsSuccessful)
                return userModelContainer;

            await _dm.ExecuteProcedure("usp_User_Delete", new Hashtable() { { "@UserId", userId } });
            return new ModelContainer<IUser>(ActionType.Delete, ResponseStatusCode.NoContent);
        }
        //todo: stao
        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IContactType>>> GetContactTypes()
        {
            var dt = await _dm.ExecuteProcedure("usp_ContactType_Search");
            return new ModelContainer<IEnumerable<IContactType>>(ActionType.Read, ResponseStatusCode.Ok)
            {
                Model = dt.Map<ContactTypeDto>()
            };
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IContact>>> GetUserContacts(int userId)
        {
            var userModelContainer = await GetUser(userId);
            if (!userModelContainer.IsSuccessful)
                return new ModelContainer<IEnumerable<IContact>>(ActionType.Read, userModelContainer.ResponseStatusCode)
                {
                    ErrorMessage = userModelContainer.ErrorMessage
                };

            var dt = await _dm.ExecuteProcedure("usp_User_Contacts", new Hashtable() { { "@UserId", userId } });
            return new ModelContainer<IEnumerable<IContact>>(dt.Map<ContactDto>(), ActionType.Read, ResponseStatusCode.Ok);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IContact>> CreateContact(int userId, int contactTypeId, string value)
        {
            var dt = await _dm.ExecuteProcedure("usp_Contact_Create", new Hashtable() { { "@UserId", userId }, { "@ContactTypeId", contactTypeId}, { "@Value", value } });
            return new ModelContainer<IContact>(dt.Map<ContactDto>().FirstOrDefault(), ActionType.Create, ResponseStatusCode.Created);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IContact>>> SearchContacts(Hashtable filter = null)
        {
            var dt = await _dm.ExecuteProcedure("usp_Contact_Search", filter ?? new Hashtable());
            return new ModelContainer<IEnumerable<IContact>>(dt.Map<ContactDto>(), ActionType.Read, ResponseStatusCode.Ok);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IContact>> GetContact(int contactId)
        {
            var result = await SearchContacts(new Hashtable() { { "@ContactId", contactId } });
            var contact = result.Model.FirstOrDefault();

            if(contact == null)
                return new ModelContainer<IContact>(ActionType.Read, ResponseStatusCode.NotFound)
                {
                    ErrorMessage = ModelContainerErrorMessages.Non_Existing_Entity
                };

            return new ModelContainer<IContact>(contact, ActionType.Read, ResponseStatusCode.Ok);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IContact>> DeleteContact(int contactId, int userId)
        {
            var contactModelContainer = await GetContact(contactId);
            if (!contactModelContainer.IsSuccessful || contactModelContainer.Model.UserId != userId)
                return new ModelContainer<IContact>(ActionType.Delete, ResponseStatusCode.NotFound)
                {
                    ErrorMessage = ModelContainerErrorMessages.Non_Existing_Entity
                };

            await _dm.ExecuteProcedure("usp_Contact_Delete", new Hashtable() { { "@ContactId", contactId }, { "@UserId", userId } });
            return new ModelContainer<IContact>(ActionType.Delete, ResponseStatusCode.NoContent);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IReportRentHistory>>> GetRentHistoryReport(int limit)
        {
            var dt = await _dm.ExecuteProcedure("usp_Report_RentHistory", new Hashtable() { { "@Limit", limit } });
            return new ModelContainer<IEnumerable<IReportRentHistory>>(dt.Map<ReportRentHistoryDto>(), ActionType.Read, ResponseStatusCode.Ok);
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IReportTopOverdue>>> GetTopOverdueReport(int limit)
        {
            var dt = await _dm.ExecuteProcedure("usp_Report_TopOverdue", new Hashtable() { { "@Limit", limit } });
            return new ModelContainer<IEnumerable<IReportTopOverdue>>(dt.Map<ReportTopOverdue>(), ActionType.Read, ResponseStatusCode.Ok);
        }

        
    }

    
}
