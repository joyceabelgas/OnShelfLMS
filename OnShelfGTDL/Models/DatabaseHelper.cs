using System.Data;
using System.Data.Common;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace OnShelfGTDL.Models
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        public DatabaseHelper(DBContext dBContext)
        {
            _connectionString = dBContext.LoadConfig();
        }
        public bool SaveUser(string userId, string memberType, string firstName, string? middleName, string lastName, string? suffix,
                             string address, string? emailAddress, string mobileNumber, string status, string userID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_UserInformationSaveRecords", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add Parameters
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@MemberType", memberType);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@MiddleName", middleName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Suffix", suffix);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@MobileNo", mobileNumber);
                    command.Parameters.AddWithValue("@EmailAddress", emailAddress);
                    command.Parameters.AddWithValue("@Status", status == "Active" ? 1 : 0);
                    command.Parameters.AddWithValue("@CreatedBy", userID);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }

        public List<UserViewModel> GetAllUsers()
        {
            List<UserViewModel> users = new List<UserViewModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_UserInformationLoadRecords", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Search", "");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dataSet.Tables[0].Rows)
                            {
                                users.Add(new UserViewModel
                                {
                                    UserID = row["UserID"].ToString(),
                                    MemberType = row["MemberType"].ToString(),
                                    FirstName = row["FirstName"].ToString(),
                                    MiddleName = row["MiddleName"].ToString(),
                                    LastName = row["LastName"].ToString(),
                                    Suffix = row["Suffix"].ToString(),
                                    Address = row["Address"].ToString(),
                                    Email = row["EmailAddress"].ToString(),
                                    MobileNo = row["MobileNo"].ToString(),
                                    Status = Convert.ToBoolean(row["IsActive"]),
                                    CreatedBy = row["CreatedBy"].ToString(),
                                    CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                                });
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data returned.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return users;
        }

        public List<UserViewModel> GetAllTeachers()
        {
            List<UserViewModel> users = new List<UserViewModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_UserInformationLoadRecordsTeacher", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Search", "");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dataSet.Tables[0].Rows)
                            {
                                users.Add(new UserViewModel
                                {
                                    UserID = row["UserID"].ToString(),
                                    MemberType = row["MemberType"].ToString(),
                                    FirstName = row["FirstName"].ToString(),
                                    MiddleName = row["MiddleName"].ToString(),
                                    LastName = row["LastName"].ToString(),
                                    Suffix = row["Suffix"].ToString(),
                                    Address = row["Address"].ToString(),
                                    Email = row["EmailAddress"].ToString(),
                                    MobileNo = row["MobileNo"].ToString(),
                                    Status = Convert.ToBoolean(row["IsActive"]),
                                    CreatedBy = row["CreatedBy"].ToString(),
                                    CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                                });
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data returned.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return users;
        }
        public bool UpdateUser(string userId, string memberType, string firstName, string? middleName, string lastName, string? suffix,
                             string address, string? emailAddress, string mobileNumber, string status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_UserInformationUpdateRecords", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add Parameters
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@MemberType", memberType);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@MiddleName", middleName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Suffix", suffix);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@MobileNo", mobileNumber);
                    command.Parameters.AddWithValue("@EmailAddress", emailAddress);
                    command.Parameters.AddWithValue("@Status", status == "Active" ? 1 : 0);
                    command.Parameters.AddWithValue("@CreatedBy", "admin");

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }

        public List<BookModelView> GetAllBooks()
        {
            List<BookModelView> books = new List<BookModelView>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_ManageBooksLoadRecords", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dataSet.Tables[0].Rows)
                            {
                                books.Add(new BookModelView
                                {
                                    ISNB = row["ISBN"].ToString(),
                                    BookName = row["BookName"].ToString(),
                                    Category = row["Category"].ToString(),
                                    AuthorsName = row["AuthorsName"].ToString(),
                                    BookShelf = row["BookShelf"].ToString(),
                                    Copyright = row["Copyright"].ToString(),
                                    StockQuantity = Convert.ToInt32(row["StockQuantity"]),
                                    PublicationName = row["PublicationName"].ToString(),
                                    Description = row["Description"].ToString(),
                                    BookImage = row["BookImage"] as byte[],
                                    CreatedBy = row["CreatedBy"].ToString(),
                                    CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                                });
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data returned.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return books;
        }

        public List<EBookModelView> GetAllEBooks()
        {
            List<EBookModelView> books = new List<EBookModelView>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_ManageEBooksLoadRecords", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dataSet.Tables[0].Rows)
                            {
                                books.Add(new EBookModelView
                                {
                                    EbookID = Convert.ToInt32(row["EbookID"]),
                                    Title = row["Title"].ToString(),
                                    Category = row["Category"].ToString(),
                                    Authors = row["Authors"].ToString(),
                                    Description = row["Description"].ToString(),
                                    EbookFilePath = row["EbookFilePath"].ToString(),
                                    CoverImageType = row["CoverImageType"].ToString(),
                                    CoverImage = row["CoverImage"] as byte[],
                                    UploadedBy = row["UploadedBy"].ToString(),
                                    DateUploaded = Convert.ToDateTime(row["DateUploaded"])
                                });
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data returned.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return books;
        }

        public bool SaveBook(string isnb, string bookName, string category, string authorsName, string bookShelf, string copyright,
                     int stockQuantity, string publicationName, string description, byte[] bookPicture)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_ManageBooksSaveRecords", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add standard book details
                    command.Parameters.AddWithValue("@ISNB", isnb);
                    command.Parameters.AddWithValue("@BookName", bookName);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@AuthorsName", authorsName);
                    command.Parameters.AddWithValue("@BookShelf", bookShelf ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Copyright", copyright);
                    command.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                    command.Parameters.AddWithValue("@PublicationName", publicationName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UserID", "admin");
                    command.Parameters.AddWithValue("@BookPicture", bookPicture ?? (object)DBNull.Value);
                    // Convert IFormFile to byte[] for SQL 
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving book: " + ex.Message);
                    return false;
                }
            }
        }

        public bool UpdateBook(string isnb, string bookName, string category, string authorsName, string bookShelf, string copyright,
                     int stockQuantity, string publicationName, string description, IFormFile bookPicture)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_ManageBooksUpdateRecords", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add standard book details
                    command.Parameters.AddWithValue("@ISNB", isnb);
                    command.Parameters.AddWithValue("@BookName", bookName);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@AuthorsName", authorsName);
                    command.Parameters.AddWithValue("@BookShelf", bookShelf ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Copyright", copyright);
                    command.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                    command.Parameters.AddWithValue("@PublicationName", publicationName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UserID", "admin");

                    // Convert IFormFile to byte[] for SQL storage
                    byte[] bookImageBytes;
                    if (bookPicture != null && bookPicture.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            bookPicture.CopyTo(memoryStream);
                            bookImageBytes = memoryStream.ToArray();
                        }
                        command.Parameters.AddWithValue("@BookPicture", bookImageBytes);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@BookPicture", null);
                    }

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving book: " + ex.Message);
                    return false;
                }
            }
        }

        public bool DeleteBook(string isbn)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_ManageBooksdeleteRecords", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISBN", isbn);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public string SaveBorrowBooks(string userID, string isbn, DateTime borrowDate, DateTime estimatedReturnDate)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Define the stored procedure
                    using (SqlCommand command = new SqlCommand("sp_BorrowBooksSaveRecords", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@ISBN", isbn);
                        command.Parameters.AddWithValue("@BorrowDate", borrowDate);
                        command.Parameters.AddWithValue("@EstimatedReturnDate", estimatedReturnDate);

                        // Output parameter for result message
                        SqlParameter outputMessage = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputMessage);

                        // Execute the stored procedure
                        int rowsAffected = command.ExecuteNonQuery();

                        // Return success or error message from stored procedure
                        return outputMessage.Value.ToString();
                    }
                }
                catch (SqlException sqlEx)
                {
                    return $"SQL Error: {sqlEx.Message}";
                }
                catch (Exception ex)
                {
                    return $"Error: {ex.Message}";
                }
            }
        }

        
        public string SaveReservationBooks(string userID, string isbn)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Define the stored procedure
                    using (SqlCommand command = new SqlCommand("sp_ReserveBooksSaveRecords", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@ISBN", isbn);

                        // Output parameter for result message
                        SqlParameter outputMessage = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputMessage);

                        // Execute the stored procedure
                        int rowsAffected = command.ExecuteNonQuery();

                        // Return success or error message from stored procedure
                        return outputMessage.Value.ToString();
                    }
                }
                catch (SqlException sqlEx)
                {
                    return $"SQL Error: {sqlEx.Message}";
                }
                catch (Exception ex)
                {
                    return $"Error: {ex.Message}";
                }
            }
        }
        public List<AllUserModel> GetUsers()
        {
            List<AllUserModel> userList = new List<AllUserModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                
                connection.Open();
                SqlCommand command = new SqlCommand("sp_LoadUsers", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userList.Add(new AllUserModel
                    {
                        UserID = reader["UserID"].ToString(),
                        FullName = reader["FullName"].ToString()
                    });
                }
            }

            return userList;
        }

        public List<AllBookModel> GetBooks()
        {
            List<AllBookModel> userList = new List<AllBookModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand("sp_LoadBooks", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userList.Add(new AllBookModel
                    {
                        ISBN = reader["ISBN"].ToString(),
                        Category = reader["Category"].ToString(),
                        BookShelf = reader["BookShelf"].ToString(),
                        AuthorsName = reader["AuthorsName"].ToString(),
                        BookName = reader["BookName"].ToString(),
                        PublicationName = reader["PublicationName"].ToString(),
                        Copyright = reader["Copyright"].ToString(),
                        StockQty = reader["StockQty"] == DBNull.Value ? 0 : Convert.ToInt32(reader["StockQty"]),
                        Description = reader["Description"].ToString()
                    });
                }
            }

            return userList;
        }


        //public List<BorrowBookView> GetAllBorrowedBooks()
        //{
        //    List<BorrowBookView> books = new List<BorrowBookView>();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connectionString))
        //        {
        //            conn.Open();

        //            using (SqlCommand cmd = new SqlCommand("sp_BorrowBooksLoadRecords", conn))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //                DataSet dataSet = new DataSet();
        //                adapter.Fill(dataSet);

        //                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
        //                {
        //                    foreach (DataRow row in dataSet.Tables[0].Rows)
        //                    {
        //                        books.Add(new BorrowBookView
        //                        {
        //                            UserID = row["UserID"].ToString(),
        //                            ISBN = row["ISBN"].ToString(),

        //                            // Parsing DateTime values correctly
        //                            BorrowDate = Convert.ToDateTime(row["BorrowDate"]),
        //                            EstimatedReturnDate = Convert.ToDateTime(row["EstimatedReturnedDate"]),

        //                            // Handle nullable date properly
        //                            ReturnedDate = row["DateofReturn"] == DBNull.Value
        //                                ? (DateTime?)null
        //                                : Convert.ToDateTime(row["DateofReturn"]),

        //                            ApprovedBy = row["ApprovedBy"].ToString(),
        //                            ApprovedDate = Convert.ToDateTime(row["ApprovedDate"])
        //                        });
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine("No data returned.");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }

        //    return books;
        //}

        public async Task<UserAccountModel> LoginUser(string userId, string password)
        {
            UserAccountModel user = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_LoginAccount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", userId);
                        command.Parameters.AddWithValue("@Password", password);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            user = new UserAccountModel
                            {
                                UserID = reader["UserID"].ToString(),
                                FullName = reader["Fullname"].ToString(),
                                Role = reader["Role"].ToString(),
                                IsSuccess = true,
                                Message = reader["Msg"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    user = new UserAccountModel
                    {
                        UserID = "",
                        FullName = "",
                        Role = "",
                        IsSuccess = false,
                        Message = ex.Message
                    };
                }
                return user;
            }
        }
        public List<MenuViewModel> GetMenuItems(string userId, string userRoles)
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            List<MenuViewModel> menuModelItems = new List<MenuViewModel>();
            string _fullName = "";
            string _role = "";
            byte[] _profilePic = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();


                    // Get User Details (FullName & Role)
                    using (SqlCommand userCommand = new SqlCommand("sp_GetUserDetails", connection))
                    {
                        userCommand.CommandType = CommandType.StoredProcedure;
                        userCommand.Parameters.AddWithValue("@UserID", userId);

                        SqlDataReader userReader = userCommand.ExecuteReader();
                        if (userReader.Read())
                        {
                            _fullName = userReader["Fullname"].ToString();
                            _role = userReader["Role"].ToString();
                            _profilePic = userReader["ProfilePicture"] != DBNull.Value
                                        ? (byte[])userReader["ProfilePicture"]
                                        : null;
                        }
                        userReader.Close();
                    }

                    using (SqlCommand command = new SqlCommand("sp_MenuLoadParent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserRole", userRoles);

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            var menuItem = new MenuItem
                            {
                                Name = reader["Name"].ToString(),
                                Url = reader["Url"].ToString(),
                                Icon = reader["Icon"].ToString(),
                            };

                            menuItems.Add(menuItem);
                        }

                        reader.Close();
                    }

                    // Fetch Submenus
                    foreach (var item in menuItems)
                    {
                        using (SqlCommand subCommand = new SqlCommand("sp_MenuLoadChild", connection))
                        {
                            subCommand.CommandType = CommandType.StoredProcedure;
                            subCommand.Parameters.AddWithValue("@ParentName", item.Name);

                            SqlDataReader subReader = subCommand.ExecuteReader();
                            List<MenuItem> subMenus = new List<MenuItem>();

                            while (subReader.Read())
                            {
                                subMenus.Add(new MenuItem
                                {
                                    Name = subReader["Name"].ToString(),
                                    Url = subReader["Url"].ToString()
                                });
                            }

                            item.Submenu = subMenus.Count > 0 ? subMenus : null;
                            subReader.Close();
                        }
                    }

                    var menuModel = new MenuViewModel
                    {
                        MenuItems = menuItems,
                        UserID = userId,   
                        FullName = _fullName, 
                        Role = _role,
                        ProfilePicture = _profilePic
                    };

                    menuModelItems.Add(menuModel);

                }
                catch (Exception ex)
                    {
                        Console.WriteLine("Menu loading error: " + ex.Message);
                    }
            }

            return menuModelItems;
        }

        // Get books
        public async Task<List<BookListModel>> GetBooksList(string category)
        {
            var books = new List<BookListModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetBooks", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Subject", category);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        books.Add(new BookListModel
                        {
                            ISBN = reader["ISBN"].ToString(),
                            BookName = reader["BookName"].ToString(),
                            AuthorsName = reader["AuthorsName"].ToString(),
                            Copyright = reader["Copyright"].ToString(),
                            BookImage = reader["BookImage"] as byte[],
                            Category = reader["CategoryName"].ToString(),
                            Bookshelf = reader["Bookshelf"].ToString(),
                            Stocks = Convert.ToInt32(reader["StockQty"]),
                            Description = reader["Description"].ToString()
                        });
                    }
                }
            }
            return books;
        }

        public async Task<List<EBook>> GetEBooksList(string? category)
        {
            var books = new List<EBook>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                await using var command = new SqlCommand("sp_GetEBooks", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Subject", (object?)category ?? DBNull.Value);
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    books.Add(new EBook
                    {
                        EbookID = reader["EbookID"] != DBNull.Value ? Convert.ToInt32(reader["EbookID"]) : 0,
                        Title = reader["Title"]?.ToString() ?? string.Empty,
                        Category = reader["Category"]?.ToString() ?? string.Empty,
                        Authors = reader["Authors"]?.ToString() ?? string.Empty,
                        Description = reader["Description"]?.ToString() ?? string.Empty,
                        EbookFilePath = reader["EbookFilePath"]?.ToString() ?? string.Empty,
                        CoverImage = reader["CoverImage"] as byte[],
                        CoverImageType = reader["CoverImageType"]?.ToString() ?? string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching e-books: {ex.Message}");
            }
            return books;
        }

        public async Task<BookDetailsModel> GetBookByISBN(string isbn)
        {
            BookDetailsModel book = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetBookDetails", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ISBN", isbn);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        book = new BookDetailsModel
                        {
                            ISBN = reader["ISBN"].ToString(),
                            BookName = reader["BookName"].ToString(),
                            AuthorsName = reader["AuthorsName"].ToString(),
                            Copyright = reader["Copyright"].ToString(),
                            BookImage = reader["BookImage"] as byte[],
                            Category = reader["CategoryName"].ToString(),
                            Bookshelf = reader["Bookshelf"].ToString(),
                            Stocks = Convert.ToInt32(reader["StockQty"]),
                            Description = reader["Description"].ToString()
                        };
                    }
                }
            }

            return book;
        }

        public async Task<List<BorrowBookView>> LoadBorrowBookView()
        {
            List<BorrowBookView> borrow = new List<BorrowBookView>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_BorrowBookLoadRecords", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dataSet.Tables[0].Rows)
                        {
                            borrow.Add(new BorrowBookView
                            {
                                ID = Convert.ToInt32(row["ID"]),
                                UserID = row["UserID"].ToString(),
                                Name = row["Name"].ToString(),
                                ISBN = row["ISBN"].ToString(),
                                BorrowDate = Convert.ToDateTime(row["BorrowDate"]),
                                EstimatedReturnDate = Convert.ToDateTime(row["EstimatedReturnDate"]),
                                ActualReturnDate = row["ActualReturnDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ActualReturnDate"]),
                                Status = row["Status"].ToString(),
                                ApprovedBy = row["ApprovedBy"].ToString(),
                                ApprovedDate = row["ApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ApprovedDate"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Loading error: " + ex.Message);
            }

            return borrow;
        }



        public string UpdateBookBorrowStatus(int id, bool status,string userID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Define the stored procedure
                    using (SqlCommand command = new SqlCommand("sp_BorrowBookUpdateStatus", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@ID", id);
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@Approvedby", userID);

                        // Output parameter for result message
                        SqlParameter outputMessage = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputMessage);

                        // Execute the stored procedure
                        int rowsAffected = command.ExecuteNonQuery();

                        // Return success or error message from stored procedure
                        return outputMessage.Value.ToString();
                    }
                }
                catch (SqlException sqlEx)
                {
                    return $"SQL Error: {sqlEx.Message}";
                }
                catch (Exception ex)
                {
                    return $"Error: {ex.Message}";
                }
            }
        }
        public async Task<List<BorrowedBooksModel>> GetMyBorrowedBooks(string userID)
        {
            List<BorrowedBooksModel> books = new List<BorrowedBooksModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_BorrowedBooksLoadMyBorrowed", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dataSet.Tables[0].Rows)
                            {
                                books.Add(new BorrowedBooksModel
                                {
                                    ISNB = row["ISBN"].ToString(),
                                    BookName = row["BookName"].ToString(),
                                    Category = row["Category"].ToString(),
                                    AuthorsName = row["AuthorsName"].ToString(),
                                    BookShelf = row["BookShelf"].ToString(),
                                    BookImage = row["BookImage"] as byte[],
                                    BorrowedDate = row["BorrowedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["BorrowedDate"]),
                                    OverdueDate = row["OverdueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["OverdueDate"]),
                                    Status = row["Status"].ToString()
                                });
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine("No data returned.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return books;
        }

        public async Task<(string Email, string EmailBody)> GetBorrowApprovalEmailBody(int ID)
        {
            string email = string.Empty;
            string emailBody = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_BorrowBookGetApprovalEmailBody", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                email = reader["UserEmail"].ToString();
                                emailBody = reader["EmailBody"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return (email, emailBody);
        }


        public async Task<List<MyShelf>> GetMyShelf(string userID)
        {
            List<MyShelf> books = new List<MyShelf>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_LoadMyShelf", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dataSet.Tables[0].Rows)
                            {
                                books.Add(new MyShelf
                                {
                                    ISNB = row["ISBN"].ToString(),
                                    BookName = row["BookName"].ToString(),
                                    Category = row["Category"].ToString(),
                                    AuthorsName = row["AuthorsName"].ToString(),
                                    BookShelf = row["BookShelf"].ToString(),
                                    BookImage = row["BookImage"] as byte[]
                                });
                            }

                        }
                        else
                        {
                            Console.WriteLine("No data returned.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return books;
        }

        public async Task<List<MyBooksReservationModel>> GetMyReservation(string userID)
        {
            List<MyBooksReservationModel> books = new List<MyBooksReservationModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("usp_ReserveBooksLoadRecords", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dataSet.Tables[0].Rows)
                            {
                                books.Add(new MyBooksReservationModel
                                {
                                    ISBN = row["ISBN"].ToString(),
                                    BookName = row["BookName"].ToString(),
                                    Category = row["Category"].ToString(),
                                    AuthorsName = row["AuthorsName"].ToString(),
                                    BookShelf = row["BookShelf"].ToString(),
                                    BookImage = row["BookImage"] as byte[],
                                    ReservationDate = Convert.ToDateTime(row["ReservationDate"])
                                });
                            }

                        }
                        else
                        {
                            Console.WriteLine("No data returned.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return books;
        }

        public async Task<List<NotificationsModel>> GetMyNotification(string userID)
        {
            List<NotificationsModel> books = new List<NotificationsModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_LoadNotifications", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dataSet.Tables[0].Rows)
                            {
                                books.Add(new NotificationsModel
                                {
                                    ID = Convert.ToInt32(row["ID"]),
                                    UserID = row["UserID"].ToString(),
                                    Module = row["Module"].ToString(),
                                    Method = row["Method"].ToString(),
                                    Action = row["Action"].ToString(),
                                    Message = row["Message"].ToString(),
                                    IsRead = row["IsRead"].ToString() == "1"? true : false,
                                    Date = Convert.ToDateTime(row["Date"]),
                                });
                            }

                        }
                        else
                        {
                            Console.WriteLine("No data returned.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return books;
        }

        public async Task<bool> MarkAsRead(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_NotificationMarkAsRead", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking notification as read: {ex.Message}");
                return false;
            }
        }
        public UserInformationViewModel GetUserById(string userId)
        {
            UserInformationViewModel user = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_GetUserProfileById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserInformationViewModel
                                {
                                    UserID = reader["UserID"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    MiddleName = reader["MiddleName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Suffix = reader["Suffix"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Email = reader["EmailAddress"].ToString(),
                                    MobileNo = reader["MobileNo"].ToString(),
                                    ProfilePicture = reader["ProfilePicture"] != DBNull.Value
                                        ? (byte[])reader["ProfilePicture"]
                                        : null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by ID: {ex.Message}");
            }

            return user;
        }

        public bool UpdateUserProfile(UserInformationViewModel model, IFormFile profileImage)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_UpdateUserProfile", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", model.UserID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FirstName", model.FirstName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@MiddleName", model.MiddleName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@LastName", model.LastName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Suffix", model.Suffix ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Address", model.Address ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EmailAddress", model.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@MobileNo", model.MobileNo ?? (object)DBNull.Value);

                        if (profileImage != null && profileImage.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                profileImage.CopyTo(ms);
                                byte[] imageData = ms.ToArray();
                                cmd.Parameters.AddWithValue("@ProfilePicture", imageData);
                            }
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ProfilePicture", null);
                        }

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user profile: {ex.Message}");
                return false;
            }
        }
        public bool DeleteUser(string userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UserInformationDeleteUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public bool UpdateUserStatus(string userId, bool status)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UserInformationUpdateUserStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Status", status);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public async Task<DashboardViewModel> LoadDashboardAsync()
        {
            var dashboardViewModel = new DashboardViewModel
            {
                Summary = new DashboardSummary(),
                Users = new List<UserListModel>(),
                Books = new List<BookListsModel>()
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_LoadDashboard", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // First Result: Summary
                        if (await reader.ReadAsync())
                        {
                            dashboardViewModel.Summary.TotalMembers = reader.GetInt32(0);
                            dashboardViewModel.Summary.TotalBorrowed = reader.GetInt32(1);
                            dashboardViewModel.Summary.TotalReserved = reader.GetInt32(2);
                            dashboardViewModel.Summary.TotalOverdue = reader.GetInt32(3);
                        }

                        // Move to second result set (Users)
                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            dashboardViewModel.Users.Add(new UserListModel
                            {
                                UserID = reader["UserID"].ToString(),
                                FullName = reader["Fullname"].ToString(),
                                BooksIssued = Convert.ToInt32(reader["BooksIssued"])
                            });
                        }

                        // Move to third result set (Books)
                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            dashboardViewModel.Books.Add(new BookListsModel
                            {
                                ISBN = reader["ISBN"].ToString(),
                                Title = reader["Title"].ToString(),
                                Author = reader["Author"].ToString(),
                                Quantity = Convert.ToInt32(reader["StockQty"]) // since StockQty is int
                            });
                        }
                    }
                }
            }

            return dashboardViewModel;
        }
        public async Task<(List<HomeBookModel> MostBorrowed, List<HomeBookModel> TodaysPick)> LoadHomeBooksAsync(string userId)
        {
            var mostBorrowed = new List<HomeBookModel>();
            var todaysPick = new List<HomeBookModel>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_loadHome", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    // First result set: Most Borrowed
                    while (await reader.ReadAsync())
                    {
                        mostBorrowed.Add(new HomeBookModel
                        {
                            ISBN = reader["ISBN"].ToString(),
                            BookName = reader["BookName"].ToString(),
                            BookImage = reader["BookImage"] as byte[]
                        });
                    }

                    // Second result set: Today's Picks
                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            todaysPick.Add(new HomeBookModel
                            {
                                ISBN = reader["ISBN"].ToString(),
                                BookName = reader["BookName"].ToString(),
                                BookImage = reader["BookImage"] as byte[]
                            });
                        }
                    }
                }
            }

            return (mostBorrowed, todaysPick);
        }

        public List<RuleModel> GetAllRules()
        {
            var rules = new List<RuleModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT ID, RuleTitle, Description1, Description2, Description3, Description4 FROM tblRulesNRegulation";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rules.Add(new RuleModel
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                RuleTitle = reader["RuleTitle"]?.ToString(),
                                Description1 = reader["Description1"]?.ToString(),
                                Description2 = reader["Description2"]?.ToString(),
                                Description3 = reader["Description3"]?.ToString(),
                                Description4 = reader["Description4"]?.ToString(),
                            });
                        }
                    }
                }
            }

            return rules;
        }

        public void AddRule(RuleModel rule)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO tblRulesNRegulation (RuleTitle, Description1, Description2, Description3, Description4)
                             VALUES (@RuleTitle, @Description1, @Description2, @Description3, @Description4)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RuleTitle", rule.RuleTitle ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description1", rule.Description1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description2", rule.Description2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description3", rule.Description3 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description4", rule.Description4 ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public bool DeleteRule(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM tblRulesNRegulation WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public RuleModel GetRuleById(int id)
        {
            RuleModel rule = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM tblRulesNRegulation WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rule = new RuleModel
                    {
                        ID = (int)reader["Id"],
                        RuleTitle = reader["RuleTitle"].ToString(),
                        Description1 = reader["Description1"].ToString(),
                        Description2 = reader["Description2"].ToString(),
                        Description3 = reader["Description3"].ToString(),
                        Description4 = reader["Description4"].ToString()
                    };
                }
            }

            return rule;
        }

        public void UpdateRule(RuleModel model)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE tblRulesNRegulation
                         SET RuleTitle = @RuleTitle,
                             Description1 = @Description1,
                             Description2 = @Description2,
                             Description3 = @Description3,
                             Description4 = @Description4
                         WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", model.ID);
                    cmd.Parameters.AddWithValue("@RuleTitle", model.RuleTitle);
                    cmd.Parameters.AddWithValue("@Description1", model.Description1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description2", model.Description2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description3", model.Description3 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description4", model.Description4 ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<RulesNRegulationViewModel> GetAllRulesNRegulation()
        {
            var rules = new List<RulesNRegulationViewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT ID, RuleTitle, Description1, Description2, Description3, Description4 FROM tblRulesNRegulation", conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rules.Add(new RulesNRegulationViewModel
                    {
                        ID = (int)reader["ID"],
                        RuleTitle = reader["RuleTitle"].ToString(),
                        Description1 = reader["Description1"].ToString(),
                        Description2 = reader["Description2"]?.ToString(),
                        Description3 = reader["Description3"]?.ToString(),
                        Description4 = reader["Description4"]?.ToString()
                    });
                }
            }

            return rules;
        }
        public string AddToMyShelf(string userId, string isbn)
        {
            string resultMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_MyShelfSave", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ISBN", isbn);

                // OUTPUT parameter
                SqlParameter outputParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 255)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                resultMessage = outputParam.Value.ToString();
            }

            return resultMessage;
        }

        public bool CancelReservation(string isbn)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ReservedCancel", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ISBN", isbn);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public bool RemoveFromShelf(string isbn)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_MyShelfRemoveBook", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ISBN", isbn);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public BorrowReturnViewModel LoadBorrowBookReturn(int id)
        {
            BorrowReturnViewModel result = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_BorrowBooksLoadReturn", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new BorrowReturnViewModel
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            UserID = reader["UserID"].ToString(),
                            ISBN = reader["ISBN"].ToString(),
                            Name = reader["Name"].ToString(),
                            BorrowDate = Convert.ToDateTime(reader["BorrowDate"]),
                            ReturnedDate = Convert.ToDateTime(reader["ReturnedDate"]),
                            OverDue = Convert.ToInt32(reader["OverDue"])
                        };
                    }
                }
            }

            return result;
        }


        public void MarkBookAsReturned(int id, bool withFines)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_MarkAsReturned", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@WithFines", withFines);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void MarkBookAsReturned(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BorrowBooksUpdateReturn", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void MarkBookAsReturnedwFine(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BorrowBooksUpdateReturnwithFine", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<BorrowedBookWithFinesModel> GetBorrowedBooksWithFines()
        {
            List<BorrowedBookWithFinesModel> books = new List<BorrowedBookWithFinesModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetBorrowedBooksWithFines", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new BorrowedBookWithFinesModel
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"]?.ToString(),
                            ISBN = reader["ISBN"]?.ToString(),
                            TotalFines = reader["TotalFines"] == DBNull.Value ? null : (decimal?)reader["TotalFines"],
                            Status = reader["Status"]?.ToString()
                        });
                    }
                }
            }

            return books;
        }

        public List<FineTypeModel> GetFineTypes()
        {
            var fines = new List<FineTypeModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetFineTypes", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fines.Add(new FineTypeModel
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            FineType = reader["FineType"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"])
                        });
                    }
                }
            }

            return fines;
        }

        public decimal GetFineAmountById(int fineId)
        {
            decimal amount = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT Amount FROM tblFineType WHERE ID = @ID", conn))
            {
                cmd.Parameters.AddWithValue("@ID", fineId);
                conn.Open();

                var result = cmd.ExecuteScalar();
                if (result != null)
                    amount = Convert.ToDecimal(result);
            }

            return amount;
        }
        public bool SaveFinesToDatabase(DataTable finesTable, string userId, string isbn, int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SaveFines", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@tbl", finesTable);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }
        public bool UpdateFineStatus(int fineId, string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("sp_FineUpdateStatus", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID", fineId);
                command.Parameters.AddWithValue("@UserId", userId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it)
                    Console.WriteLine($"Error updating fine status: {ex.Message}");
                    return false;
                }
            }
        }
        public DataTable LoadReport(string dateFilter, string status, string category)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Reports", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DateFilterOption", dateFilter);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@CategoryName", category);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }
        public string GetCurrentPassword(string userId)
        {
            string currentPassword = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT [Password] FROM tblUserAccounts WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        currentPassword = result.ToString();
                    }
                }
            }

            return currentPassword;
        }
        public bool ChangePassword(string userId, string newPassword)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ChangePassword", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }
        public async Task<string> GetNotificationMessageAsync(string userId, string role)
        {
            string resultMessage = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_SendNotification", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@Role", role);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        resultMessage = reader["Message"]?.ToString();
                    }
                }
            }

            return resultMessage;
        }
        public bool RegisterUser(RegisterViewModel model, out string message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_RegisterUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@LRN", (object)model.LRN ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MemberTypeID", model.MemberType); // Expecting name (e.g. "Student")
                    cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", (object)model.MiddleName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", model.LastName);
                    cmd.Parameters.AddWithValue("@SuffixID", (object)model.Suffix ?? DBNull.Value); // Pass suffix as string (e.g. "Jr.")
                    cmd.Parameters.AddWithValue("@Address", model.Address);
                    cmd.Parameters.AddWithValue("@EmailAddress", (object)model.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MobileNo", (object)model.MobileNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Grade", (object)model.Grade ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Section", (object)model.Section ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Adviser", (object)model.Adviser ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Password", model.Password); // Or hash before passing

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                message = "Registration successful.";
                return true;
            }
            catch (Exception ex)
            {
                message = "Error during registration: " + ex.Message;
                return false;
            }
        }

        public async Task<(string Email, string EmailBody)> GetRegistrationOTPEmailBody(string userID)
        {
            string email = string.Empty;
            string emailBody = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_OTPSend", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                email = reader["UserEmail"].ToString();
                                emailBody = reader["EmailBody"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return (email, emailBody);
        }

        public bool VerifyOtp(string email, string otp, out string message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_VerifyOtp", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@OTP", otp);

                    SqlParameter isValidParam = new SqlParameter("@IsValid", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(isValidParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    bool isValid = (bool)isValidParam.Value;
                    message = isValid ? "OTP verified successfully." : "Invalid or expired OTP.";
                    return isValid;
                }
            }
            catch (Exception ex)
            {
                message = "Error verifying OTP: " + ex.Message;
                return false;
            }
        }

        public bool ResendOtp(string email)
        {
            bool isValid = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ResendOTP", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmailAddress", email);

                conn.Open();
                cmd.ExecuteNonQuery();
                isValid = true;
            }

            
            return isValid;
        }
        public async Task<(string Email, string EmailBody)> GetRegistrationOTPEmailBodyByEmail(string emailAdd)
        {
            string email = string.Empty;
            string emailBody = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_OTPSendByEmail", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmailAddress", emailAdd);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                email = reader["UserEmail"].ToString();
                                emailBody = reader["EmailBody"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return (email, emailBody);
        }

        //private string GenerateOtp()
        //{
        //    Random random = new Random();
        //    return random.Next(100000, 999999).ToString(); // 6-digit OTP
        //}

        public async Task<bool> SaveEBookAsync(
    string title,
    string category,
    string authors,
    string description,
    string ebookFilePath,
    IFormFile? coverImage,
    string uploadedBy)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_SaveEBook", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Category", category ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Authors", authors ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EbookFilePath", ebookFilePath);
                        cmd.Parameters.AddWithValue("@UploadedBy", uploadedBy ?? "Admin");

                        // Convert IFormFile coverImage → binary data
                        byte[]? imageBytes = null;
                        string? contentType = null;
                        if (coverImage != null)
                        {
                            using (var ms = new MemoryStream())
                            {
                                await coverImage.CopyToAsync(ms);
                                imageBytes = ms.ToArray();
                                contentType = coverImage.ContentType;
                            }
                        }

                        cmd.Parameters.AddWithValue("@CoverImage", (object?)imageBytes ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CoverImageType", (object?)contentType ?? DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving e-book: {ex.Message}");
                return false;
            }
        }



        public async Task<bool> UpdateEBookAsync(
    int ebookId,
    string title,
    string category,
    string authors,
    string description,
    string ebookFilePath,
    IFormFile? coverImage,
    string uploadedBy)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_UpdateEBook", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Id", ebookId);
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Category", category ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Authors", authors ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EbookFilePath", ebookFilePath);
                        cmd.Parameters.AddWithValue("@UploadedBy", uploadedBy ?? "Admin");

                        // Handle new cover image if provided
                        byte[]? imageBytes = null;
                        string? contentType = null;
                        if (coverImage != null)
                        {
                            using (var ms = new MemoryStream())
                            {
                                await coverImage.CopyToAsync(ms);
                                imageBytes = ms.ToArray();
                                contentType = coverImage.ContentType;
                            }
                        }

                        //cmd.Parameters.AddWithValue("@CoverImage", (object?)imageBytes ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("@CoverImageType", (object?)contentType ?? DBNull.Value);
                        cmd.Parameters.Add("@CoverImage", SqlDbType.VarBinary).Value = (object?)imageBytes ?? DBNull.Value;
                        cmd.Parameters.Add("@CoverImageType", SqlDbType.NVarChar, 50).Value = (object?)contentType ?? DBNull.Value;

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating e-book: {ex.Message}");
                return false;
            }
        }


        public EBook? GetEBookById(int ebookId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM tblEBooks WHERE EbookID = @EbookID", conn))
                {
                    cmd.Parameters.AddWithValue("@EbookID", ebookId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new EBook
                            {
                                EbookID = (int)reader["EbookID"],
                                Title = reader["Title"].ToString(),
                                EbookFilePath = reader["EbookFilePath"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
        public bool DeleteEBook(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_ManageEBooksdeleteRecords", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

    }
}

