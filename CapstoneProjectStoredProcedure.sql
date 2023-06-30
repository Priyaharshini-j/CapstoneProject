--Creating the procedure to check the User credentials
CREATE OR ALTER PROCEDURE ValidateLogin
@UserEmail Varchar(50),
@UserPassword VARCHAR(25)
AS
BEGIN 
SELECT * FROM Users WHERE UserEmail = @UserEmail AND Password = @UserPassword;
END;

--Procedure for retrieving the user by id
CREATE OR ALTER PROCEDURE GetUserById
@UserId INT
AS
BEGIN
SELECT * FROM Users WHERE UserId=@UserId;
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
UPDATE Users SET Password =@Password , SecurityQn = @SecurityQn, SecurityAns = @SecurityAns WHERE UserId= @UserId;
END;

--Creating Procedure to Delete User Profile
CREATE OR ALTER PROCEDURE UpdateUser
@UserId INT
AS
BEGIN 
DELETE FROM Users WHERE UserId = @UserId;
END;

CREATE OR ALTER PROCEDURE GetBookId
@ISBN VARCHAR(50)
AS
BEGIN
SELECT * FROM Book WHERE ISBN=@ISBN;
END;

--Creating procedure for retriving bookID
CREATE OR ALTER PROCEDURE GetBookId
@ISBN VARCHAR(50)
AS
BEGIN
SELECT * FROM dbo.Book WHERE ISBN = @ISBN;
END;

--Creating procdure for inserting and returning book id
CREATE OR ALTER PROCEDURE InsertBook
    @ISBN VARCHAR(50),
    @BookName VARCHAR(225),
    @BookVol VARCHAR(50),
    @Genre VARCHAR(MAX),
    @Author VARCHAR(100),
    @CoverUrl VARCHAR(MAX),
    @bookDesc VARCHAR(MAX),
    @Publisher VARCHAR(200),
    @PublishedDate DATETIME,
    @BookId INT OUTPUT
AS
BEGIN
    INSERT INTO Book (ISBN, BookName, BookVol, Genre, Author, CoverUrl, BookDesc, Publisher, PublishedDate)
    VALUES (@ISBN, @BookName, @BookVol, @Genre, @Author, @CoverUrl, @bookDesc, @Publisher, @PublishedDate)
END;

--Creating the procedure to retrive the community by BookId

CREATE OR ALTER PROCEDURE GetCommunityBookId
@BookId INT
AS
BEGIN
SELECT * FROM Community WHERE BookId=@BookId;
END;

--reating the procedure to retrive the community by UserId
CREATE OR ALTER PROCEDURE GetCommunityByUserId
@UserID INT
AS
BEGIN
SELECT * FROM Community WHERE CommunityAdmin = @UserID;
END;

--creating the procedure to retrive the post based on the bookId
CREATE OR ALTER PROCEDURE GetPostByBookId
@BookId INT
AS
BEGIN
SELECT * FROM Post WHERE BookId= @BookId;
END;

--Creating the procedure for retreiving the post created ny the users
CREATE OR ALTER PROCEDURE GetUsersPost
@UserId INT
AS
BEGIN
SELECT * FROM Post WHERE UserId= @UserId;
END;

--Get Critique by bookID
CREATE OR ALTER PROCEDURE GetCritiqueByBookId
@BookId INT
AS
BEGIN
SELECT * FROM Critique WHERE BookId = @BookId;
END;

--creating procedure for the rating retrival based on bookId
CREATE OR ALTER PROCEDURE getRatingsByBookId
@BookId INT
AS
BEGIN
SELECT * FROM Rating WHERE BookId= @BookId;
END;

--Procedure to create comm
CREATE OR ALTER PROCEDURE CreateCommunity
    @CommunityName VARCHAR(225),
    @CommunityDesc VARCHAR(MAX),
    @CommunityAdmin INT,
    @BookId INT,
    @CommunityId INT OUTPUT
AS
BEGIN
    INSERT INTO Community
    VALUES (@CommunityName, @CommunityDesc, @CommunityAdmin, @BookId, GETDATE());

    SET @CommunityId = SCOPE_IDENTITY();

    SELECT
        @CommunityId AS CommunityId,
        @CommunityName AS CommunityName,
        @CommunityDesc AS CommunityDesc,
        @CommunityAdmin AS CommunityAdmin,
        @BookId AS BookId,
        GETDATE() AS CreatedDate;
        
    INSERT INTO CommunityMembers
    VALUES (@CommunityId, @CommunityAdmin, GETDATE());
END;

--Procedure to delte community
CREATE OR ALTER PROCEDURE DeleteCommunity
@CommunityId INT,
@UserId INT
AS
BEGIN
DELETE FROM CommunityMembers WHERE CommunityId = @CommunityId;
DELETE FROM Community WHERE CommunityId = @CommunityId AND CommunityAdmin = @UserId;
END;

CREATE OR ALTER PROCEDURE AddMember
@UserId INT,
@CommunityId INT
AS
BEGIN
INSERT INTO CommunityMembers VALUES (@CommunityId, @UserId, GETDATE());
END;

--Procedure to count members in community
CREATE OR ALTER PROCEDURE GetCommunityMembersCount
@CommunityId INT
AS
BEGIN
SELECT * FROM CommunityMembers WHERE CommunityId = @CommunityId;
END;