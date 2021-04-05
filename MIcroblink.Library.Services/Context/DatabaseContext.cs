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
        public async Task<IEnumerable<IBook>> SearchBooks(Hashtable filter = null)
        {
            var dt = await _dm.ExecuteProcedure("usp_Book_Search", filter ?? new Hashtable());
            return dt.Map<BookDto>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IBookTitle>> SearchBookTitles(Hashtable filter = null)
        {
            var dt = await _dm.ExecuteProcedure("usp_BookTitle_Search", filter ?? new Hashtable());
            return dt.Map<BookTitleDto>();
        }

        /// <inheritdoc/>
        public async Task<IBookTitle> GetBookTitle(int bookTitleid)
        {
            var dt = await _dm.ExecuteProcedure("usp_BookTitle_Search", new Hashtable() {{ "@Id", bookTitleid }});
            return dt.Map<BookTitleDto>().FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<IBook> GetBook(int bookid)
        {
            var dt = await _dm.ExecuteProcedure("usp_Book_Search", new Hashtable() { { "@Id", bookid } });
            return dt.Map<BookDto>().FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IRent>> RentBookTitle(int bookTitleId, int userId, int durationInDays)
        {
            var availableBook = await GetAvailableBook(bookTitleId);
            if (availableBook == null)
                return new ModelContainer<IRent>()
                {
                    ErrorMessage = ModelContainerErrorMessages.Currently_Unavailable
                };


            var dt = await _dm.ExecuteProcedure("usp_Rent_Create", new Hashtable() { { "@BookId", availableBook.Id }, { "@UserId", userId }, { "@RentDurationInDays", durationInDays } });
            return new ModelContainer<IRent>()
            {
                IsSuccessful = true,
                Model = dt.Map<RentDto>().FirstOrDefault()
            };
        }

        /// <inheritdoc/>        
        public async Task<IModelContainer<IRent>> ReturnRentedBookTitle(int rentId, int bookId, int userId)
        {
            var rent = await GetRent(rentId);
            
            if (rent == null || rent.ReturnDate != null)
                return new ModelContainer<IRent>()
                {
                    ErrorMessage = ModelContainerErrorMessages.Non_Existing_Entity
                };

            var dt = await _dm.ExecuteProcedure("usp_Rent_Return", new Hashtable() { { "@RentId", rentId }, { "@BookId", bookId }, { "@UserId", userId } });
            return new ModelContainer<IRent>()
            {
                IsSuccessful = true,
                Model = dt.Map<RentDto>().FirstOrDefault()
            };
        }

        /// <inheritdoc/>
        public async Task<IRent> GetRent(int rentId)
        {
            var dt = await _dm.ExecuteProcedure("usp_Rent_Search", new Hashtable() { { "@RentId", rentId } });
            return dt.Map<RentDto>().FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IBook>> GetBooks()
        {
            return await SearchBooks(null);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<IBookTitle>> GetBookTitles()
        {
            return SearchBookTitles(null);
        }

        /// <inheritdoc/>
        public async Task<IBook> GetAvailableBook(int bookTitleId)
        {
            var dt = await _dm.ExecuteProcedure("usp_BookTitle_GetAvailableBook", new Hashtable() { { "@BookTitleId", bookTitleId } });
            return dt.Map<BookDto>().FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IUser>> SearchUsers(Hashtable filter = null)
        {
            var dt = await _dm.ExecuteProcedure("usp_User_Search", filter ?? new Hashtable());
            return dt.Map<UserDto>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IUser>> GetUsers()
        {
            return await SearchUsers();
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IUser>> GetUser(int userId)
        {
            var dt = await _dm.ExecuteProcedure("usp_User_Search", new Hashtable() { { "@UserId", userId } });
            var user = dt.Map<UserDto>().FirstOrDefault();
            return new ModelContainer<IUser>
            {
                IsSuccessful = user != null,
                ErrorMessage = user != null ? ModelContainerErrorMessages.None : ModelContainerErrorMessages.Non_Existing_Entity,
                Model = user
            };
        }

        /// <inheritdoc/>
        public async Task<IUser> CreateUser(string firstName, string lastName, DateTime dateOfBirt, bool? isValidMrz = null)
        {
            var dt = await _dm.ExecuteProcedure("usp_User_Create", new Hashtable() { { "@FirstName", firstName }, { "@LastName", lastName }, { "@DateOfBirt", dateOfBirt }, { "@IsValidMrz", isValidMrz } });
            return dt.Map<UserDto>().FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IUser>> UpdateUser(int userId, string firstName, string lastName, DateTime dateOfBirt)
        {
            if(string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                return new ModelContainer<IUser>
                {
                    ErrorMessage = ModelContainerErrorMessages.Invalid_Input
                };

            var userModelContainer = await GetUser(userId);
            if (!userModelContainer.IsSuccessful)
                return userModelContainer;

            var dt = await _dm.ExecuteProcedure("usp_User_Update", new Hashtable() { { "@UserId", userId }, { "@FirstName", firstName }, { "@LastName", lastName }, { "@DateOfBirt", dateOfBirt } });
            return new ModelContainer<IUser>
            {
                IsSuccessful = true,
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
            return new ModelContainer<IUser>
            {
                IsSuccessful = true
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IContactType>> GetContactTypes()
        {
            var dt = await _dm.ExecuteProcedure("usp_ContactType_Search");
            return dt.Map<ContactTypeDto>();
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IEnumerable<IContact>>> GetUserContacts(int userId)
        {
            var userModelContainer = await GetUser(userId);
            if (!userModelContainer.IsSuccessful)
                return new ModelContainer<IEnumerable<IContact>>
                {
                    ErrorMessage = userModelContainer.ErrorMessage
                };


            var dt = await _dm.ExecuteProcedure("usp_User_Contacts", new Hashtable() { { "@UserId", userId } });
            return new ModelContainer<IEnumerable<IContact>>( dt.Map<ContactDto>());
        }

        /// <inheritdoc/>
        public async Task<IContact> CreateContact(int userId, int contactTypeId, string value)
        {
            var dt = await _dm.ExecuteProcedure("usp_Contact_Create", new Hashtable() { { "@UserId", userId }, { "@ContactTypeId", contactTypeId}, { "@Value", value } });
            return dt.Map<ContactDto>().FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IContact>> SearchContacts(Hashtable filter = null)
        {
            var dt = await _dm.ExecuteProcedure("usp_Contact_Search", filter ?? new Hashtable());
            return dt.Map<ContactDto>();
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IContact>> GetContact(int contactId)
        {
            var result = await SearchContacts(new Hashtable() { { "@ContactId", contactId } });
            var contact = result.FirstOrDefault();

            return new ModelContainer<IContact>
            {
                IsSuccessful = contact != null,
                ErrorMessage = contact != null ? ModelContainerErrorMessages.None : ModelContainerErrorMessages.Non_Existing_Entity,
                Model = contact
            };
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IContact>> DeleteContact(int contactId, int userId)
        {
            var contactModelContainer = await GetContact(contactId);
            if (!contactModelContainer.IsSuccessful || contactModelContainer.Model.UserId != userId)
                return new ModelContainer<IContact>
                {
                    ErrorMessage = ModelContainerErrorMessages.Non_Existing_Entity
                };

            await _dm.ExecuteProcedure("usp_Contact_Delete", new Hashtable() { { "@ContactId", contactId }, { "@UserId", userId } });
            return new ModelContainer<IContact>
            {
                IsSuccessful = true
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IReportRentHistory>> GetRentHistoryReport(int limit)
        {
            var dt = await _dm.ExecuteProcedure("usp_Report_RentHistory", new Hashtable() { { "@Limit", limit } });
            return dt.Map<ReportRentHistoryDto>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IReportTopOverdue>> GetTopOverdueReport(int limit)
        {
            var dt = await _dm.ExecuteProcedure("usp_Report_TopOverdue", new Hashtable() { { "@Limit", limit } });
            return dt.Map<ReportTopOverdue>();
        }

        
    }

    
}
