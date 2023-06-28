
--Creating a table for storing the user details 
CREATE TABLE Users (
  UserId INT IDENTITY(1,1) PRIMARY KEY,
  UserName VARCHAR(25) CONSTRAINT [UK_UserName] UNIQUE,
  UserEmail VARCHAR(50) CONSTRAINT [UK_UserEmail] UNIQUE,
  Password VARCHAR(25),
  SecurityQn VARCHAR(225),
  SecurityAns VARCHAR(225),
  CreatedDate DATETIME DEFAULT GETDATE()
);


--For checking functionality I am entering the data manually
-- Inserting sample data into the Users table
INSERT INTO Users ( UserName, UserEmail, Password, SecurityQn, SecurityAns, CreatedDate)
VALUES
  ('John Doe', 'john.doe@example.com', 'password123', 'What is your favorite color?', 'Blue', GETDATE()),
  ('Jane Smith', 'jane.smith@example.com', 'abc123', 'What is your pet name?', 'Max', GETDATE()),
  ('David Johnson', 'david.johnson@example.com', 'qwerty', 'What city were you born in?', 'New York', GETDATE());

--Creating the procedure to check the User credentials
CREATE OR ALTER PROCEDURE ValidateLogin
@UserEmail Varchar(50),
@UserPassword VARCHAR(25)
AS
BEGIN 
SELECT * FROM Users WHERE UserEmail = @UserEmail AND Password = @UserPassword;
END;

--Creating the procedure to Creating a user profile
CREATE OR ALTER PROCEDURE CreateUser
@UserName VARCHAR(25),
@UserEmail VARCHAR(50),
@Password VARCHAR(25),
@SecurityQn VARCHAR(225),
@SecurityAns VARCHAR(225)
AS
BEGIN
INSERT INTO USERS 
VALUES (@UserName, @UserEmail, @Password,@SecurityQn, @SecurityQn,GETDATE());
END

--Creating Procedure to update the profile
CREATE OR ALTER PROCEDURE UpdateUser
@UserId INT,
@Password VARCHAR(25),
@SecurityQn VARCHAR(225),
@SecurityAns VARCHAR(225)
AS
BEGIN
UPDATE Users SET Password =@Password , SecurityQn = @SecurityQn, SecurityAns = @SecurityAns
END;

--Creating Procedure to Delete User Profile
CREATE OR ALTER PROCEDURE UpdateUser
@UserId INT
AS
BEGIN 
DELETE FROM Users WHERE UserId = @UserId;
END;

--CREATING THE DATABASE FOR BOOK
CREATE TABLE Book
(
BookId INT IDENTITY(1,1) PRIMARY KEY,
ISBN VARCHAR(50) CONSTRAINT [Uk_ISBN] UNIQUE,
BookName VARCHAR(225),
BookVol VARCHAR(50),
Genre VARCHAR(MAX) NOT NULL,
Author VARCHAR(100),
Publisher VARCHAR(200)
);

--CREATING THE TABLE FOR COMMUNITY
CREATE TABLE Community
(
CommunityId INT IDENTITY(1,1) PRIMARY KEY,
CommunityName VARCHAR (225) UNIQUE,
CommunityDesc VARCHAR(MAX),
CommunityAdmin INT NOT NULL,
BookId INT NOT NULL,
CreatedDate DATETIME  DEFAULT SYSDATETIME() NOT NULL,
CONSTRAINT [Fk_UserId] FOREIGN KEY (CommunityAdmin) REFERENCES Users(UserId),
CONSTRAINT [Fk_BookId] FOREIGN KEY (BookId) REFERENCES Book(BookId),
);

--CREATING TABLE FOR Post
CREATE TABLE Post
(
PostId INT IDENTITY(1,1) PRIMARY KEY,
PostCaption VARCHAR(255),
UserId INT NOT NULL,
BookId INT NOT NULL,
Picture VARBINARY(MAX) NOT NULL,
CreatedDate DATE DEFAULT SYSDATETIME(),
CONSTRAINT [Fk_Post_UserId] FOREIGN KEY (UserId) REFERENCES Users(UserId),
CONSTRAINT [Fk_Post_BookId] FOREIGN KEY (BookId) REFERENCES Book(BookId),
);

--Creating the table for Critic
CREATE TABLE Critique
(
CritiqueId INT IDENTITY(1,1) PRIMARY KEY,
BookId INT NOT NULL,
UserId INT NOT NULL,
CritiqueDesc VARCHAR(MAX) NOT NULL,
CreatedDate DATE DEFAULT SYSDATETIME(),
CONSTRAINT [Fk_Critique_UserId] FOREIGN KEY (UserId) REFERENCES Users(UserId),
CONSTRAINT [Fk_Critique_BookId] FOREIGN KEY (BookId) REFERENCES Book(BookId),
);

--CREATING TABLE FOR Rating 
CREATE TABLE Rating
(
RatingId INT IDENTITY(1,1) PRIMARY KEY,
BookId INT NOT NULL,
UserId INT NOT NULL,
Rating INT NOT NULL,
CreatedDate DATE DEFAULT SYSDATETIME(),
CONSTRAINT [Fk_Rating_UserId] FOREIGN KEY (UserId) REFERENCES Users(UserId),
CONSTRAINT [Fk_Rating_BookId] FOREIGN KEY (BookId) REFERENCES Book(BookId),
);

--CREATING TABLE FOR BookShelves
CREATE TABLE BookShelf
(
BookShelfId INT IDENTITY(1,1) PRIMARY KEY,
UserId INT NOT NULL,
BookId INT NOT NULL,
ReadingStatus VARCHAR(50) NOT NULL,
CreatedDate DATE DEFAULT SYSDATETIME(),
CONSTRAINT [Fk_Shelves_UserId] FOREIGN KEY (UserId) REFERENCES Users(UserId),
CONSTRAINT [Fk_Shelves_BookId] FOREIGN KEY (BookId) REFERENCES Book(BookId)
);

