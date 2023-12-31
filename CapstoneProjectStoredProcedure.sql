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

CREATE OR ALTER PROCEDURE GetUsersCritique
@UserId INT
AS
SELECT * FROM Critique WHERE UserId=@UserId;

--Community->CommunityMembers->CommunityDiscussion->DiscussionReply
--Critic->CritiqueReply
--Critic->CritiqueLike
--Post->PostLike



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
VALUES (@UserName, @UserEmail, @Password,@SecurityQn, @SecurityAns,GETDATE());
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
    @Genre VARCHAR(MAX),
    @Author VARCHAR(100),
    @CoverUrl VARCHAR(MAX),
    @bookDesc VARCHAR(MAX),
    @Publisher VARCHAR(200),
    @PublishedDate DATETIME
AS
BEGIN
    INSERT INTO Book (ISBN, BookName, Genre, Author, CoverUrl, BookDesc, Publisher, PublishedDate)
    VALUES (@ISBN, @BookName, @Genre, @Author, @CoverUrl, @bookDesc, @Publisher, @PublishedDate)
END;

SELECT * FROM BookShelf

CREATE OR ALTER PROCEDURE ListBookInShelf
@UserId INT
AS
SELECT * FROM BookShelf WHERE UserId=@UserId;

CREATE OR ALTER PROCEDURE GetBookByBookId
@BookId INT
AS
SELECT * FROM Book WHERE BookId=@BookId

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
    @BookId INT
AS
BEGIN
    DECLARE @CommunityId INT; -- Declare the CommunityId variable

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

SELECT * FROM Rating

--Procedure to create comm
CREATE OR ALTER PROCEDURE AddRating
	@userRatings INT,
	@UserId INT,
	@BookId INT
AS
BEGIN
    INSERT INTO Rating
    VALUES (@BookId, @UserId, @userRatings, GETDATE());
    SELECT * FROM Book WHERE BookId = @BookId;
END;

CREATE OR ALTER Procedure CritiqueLikeDislike
@CritiqueID INT
AS
BEGIN
SELECT * FROM CritiqueLike WHERE CritiqueId=@CritiqueId;
END;

CREATE OR ALTER Procedure PostLikeDislike
@PostId INT
AS
BEGIN
SELECT * FROM PostLike WHERE PostId=@PostId;
END;

CREATE OR ALTER Procedure DeleteRating
@BookId INT,
@UserId INT
AS
BEGIN 
DELETE FROM Rating WHERE BookId=@BookId AND UserId=@UserId;
END;

CREATE OR ALTER PROCEDURE AddBookShelf
@UserId INT,
@BookId INT,
@ReadingStatus VARCHAR(100)
AS
BEGIN
INSERT INTO BookShelf VALUES (@UserId,@BookId,@ReadingStatus, GETDATE());
END;

CREATE OR ALTER PROCEDURE PostLikeDislike
@UserId INT,
@PostId INT,
@LikeStatus INT
AS
BEGIN
INSERT INTO PostLike VALUES (@PostId,@UserId,@LikeStatus,GETDATE());
SET @PostId = SCOPE_IDENTITY();
END;


CREATE OR ALTER PROCEDURE RemoveBook
@UserId INT,
@BookId INT,
@BookShelfId INT,
@ReadingStatus VARCHAR(100)
AS
BEGIN
DELETE FROM BookShelf WHERE BookShelfId=@BookShelfId AND UserId=@UserId AND BookId=@BookId AND ReadingStatus=@ReadingStatus;
END;

CREATE OR ALTER PROCEDURE ListBookInShelf
@UserId INT
AS
BEGIN
SELECT * FROM BookShelf WHERE UserId=@UserId;
END;

CREATE OR ALTER PROCEDURE EditCommunity
@CommunityId INT,
@CommunityName VARCHAR(225),
@CommunityDesc VARCHAR(MAX),
@CommunityAdmin INT,
@BookId INT
AS
BEGIN
UPDATE Community SET CommunityName=@CommunityName, CommunityDesc=@CommunityDesc WHERE CommunityId=@CommunityId AND BookId=@BookId AND CommunityAdmin=@CommunityAdmin;
END;

CREATE OR ALTER PROCEDURE CreatePost
@PostCaption VARCHAR(225),
@BookId INT,
@Picture VARBINARY(MAX),
@UserId INT
AS
BEGIN
INSERT INTO Post VALUES (@PostCaption, @UserId, @BookId, @Picture, GETDATE());
END;

CREATE OR ALTER PROCEDURE CreateCritique
@BookId INT,
@UserId INT,
@CritiqueDesc VARCHAR(MAX),
@CritiqueId INT OUTPUT
AS
BEGIN
INSERT INTO Critique VALUES (@BookId,@UserId ,@CritiqueDesc,GETDATE());
SET @CritiqueId = SCOPE_IDENTITY();
SELECT * FROM Critique WHERE CritiqueId=@CritiqueId;
END;


CREATE OR ALTER PROCEDURE CreateCritiqueReply
@CritiqueId INT,
@UserId INT,
@Reply VARCHAR(MAX)
AS
BEGIN
INSERT INTO CritiqueReply VALUES(@CritiqueId,@UserId,@Reply,GETDATE());
SELECT * FROM CritiqueReply WHERE CritiqueId=@CritiqueId;
END;


CREATE OR ALTER PROCEDURE LikeDislikeCritique
@CritiqueId INT,
@UserId INT,
@LikeStatus INT
AS
BEGIN
INSERT INTO CritiqueLike VALUES (@CritiqueId,@UserId,@LikeStatus,GETDATE());
END;

CREATE OR ALTER PROCEdURE DeleteCritiqueReply
@CritiqueReplyId INT,
@UserId INT
AS
DELETE FROM CritiqueReply WHERE CritiqueReplyId=@CritiqueReplyId AND UserId=@UserId;

CREATE OR ALTER PROCEDURE DeleteCritique
    @CritiqueId INT,
    @UserId INT
AS
BEGIN
DELETE FROM Critique WHERE CritiqueId=@CritiqueId AND UserId=@UserId;
END;





CREATE OR ALTER PROCEDURE DeleteCritique
    @CritiqueId INT,
    @UserId INT
AS
BEGIN
    IF EXISTS (SELECT * FROM Critique WHERE CritiqueId = @CritiqueId AND UserId = @UserId)
    BEGIN
        DELETE FROM CritiqueReply WHERE CritiqueId = @CritiqueId;
        DELETE FROM CritiqueLike WHERE CritiqueId = @CritiqueId;
        DELETE FROM Critique WHERE CritiqueId = @CritiqueId AND UserId = @UserId;
    END
    ELSE
    BEGIN
        RAISERROR('The critique does not exist or does not belong to the specified user.', 16, 1);
    END
END;


CREATE OR ALTER PROCEDURE EditCritique
@CritiqueId INT,
@BookId INT,
@UserId INT,
@CritiqueDesc VARCHAR(MAX)
AS
BEGIN
UPDATE Critique SET CritiqueDesc=@CritiqueDesc WHERE CritiqueId=@CritiqueId AND UserId=@UserId AND BookId=@BookId;
SELECT * FROM Critique WHERE CritiqueId=@CritiqueId;
END;


CREATE OR ALTER PROCEDURE EditCritiqueReply
@UserId iNT,
@CritiqueReplyId INT,
@CritiqueId INT,
@Reply VARCHAR(MAX)
AS
BEGIN
UPDATE CritiqueReply SET Reply=@Reply WHERE CritiqueReplyId=@CritiqueReplyId AND CritiqueId=@CritiqueId AND UserId=@UserId;
END;

CREATE OR ALTER PROCEDURE GetUserNameByUserId 
@UserId INT
AS
SELECT UserName From Users WHERE UserId=@UserId;

CREATE OR ALTER PROCEDURE GetUserNameByMemberId
    @CommunityMemberId INT,
    @UserId INT OUTPUT
AS
BEGIN
    SELECT @UserId = UserId
    FROM CommunityMembers
    WHERE CommunityMemberId = @CommunityMemberId;

    SELECT UserName
    FROM Users
    WHERE UserId = @UserId;
END;

CREATE OR ALTER PROCEDURE ListDiscussion
@communityId INT
AS
SELECT * FROM CommunityDiscussion WHERE CommunityId =@communityId;





CREATE OR ALTER PROCEDURE ListDiscussionReply
@DiscussionId INT
AS
SELECT * FROM CommunityDiscussion WHERE DiscussionId=@DiscussionId;


CREATE OR ALTER PROCEDURE GetDiscussionByDiscussionId
@discussionId INT
AS
SELECT * FROM CommunityDiscussion WHERE DiscussionId=@discussionId;


CREATE OR ALTER PROCEDURE DeleteDiscussionReply
@DiscussionReplyId INT
AS
DELETE FROM DiscussionReply WHERE DiscussionReplyId=@DiscussionReplyId;

CREATE OR ALTER PROCEDURE GetMemberId
@CommunityId INT,
@UserId INT
AS
SELECT UserID FROM CommunityMembers WHERE CommunityId=@CommunityId AND UserID=@UserId;


CREATE OR ALTER PROCEDURE CreateDiscussion
@CommunityId INT,
@CommunityMemberId INT,
@Discussion VARCHAR(MAX)
AS
BEGIN
INSERT INTO CommunityDiscussion VALUES (@CommunityId, @CommunityMemberId, @Discussion, GETDATE())
END;


CREATE OR ALTER PROCEDURE CreateDiscussionReply
@DiscussionId INT,
@CommunityMemberId INT,
@DiscussionReply VARCHAR(MAX)
AS
BEGIN
INSERT INTO DiscussionReply VALUES (@DiscussionId,@CommunityMemberId,@DiscussionReply,GETDATE())
END;

SELECT * FROM PostLike;
SELECT * FROM DiscussionReply;


CREATE OR ALTER PROCEDURE getUserName
@UserId INT
AS
SELECT UserName FROM Users WHERE UserId=@UserId;
SELECT * FROM Critique

CREATE OR ALTER PROCEDURE CreateCritique
    @BookId INT,
    @UserId INT,
    @CritiqueDesc VARCHAR(MAX)
AS
BEGIN
    DECLARE @CritiqueId INT; -- Declare the CritiqueId variable

    INSERT INTO Critique
    VALUES (@BookId, @UserId, @CritiqueDesc, GETDATE());

    SET @CritiqueId = SCOPE_IDENTITY();

    SELECT * FROM Critique WHERE CritiqueId = @CritiqueId;
END;

SELECT * FROM Users


CREATE OR ALTER PROCEDURE DeleteMember
    @CommunityId INT,
    @UserId INT
AS
BEGIN
    DECLARE @MemId INT;
    SET @MemId = (SELECT CommunityMemberId FROM CommunityMembers WHERE UserId = @UserId);

    DELETE FROM DiscussioReply WHERE CommunityMemberId = @MemId;
    DELETE FROM CommunityDiscussion WHERE CommunityMemberId = @MemId;
END;


SELECT * FROM DiscussionReply
SELECT * FROM CommunityDiscussion
SELECT * FROM CommunityMembers
SELECT * FROM Community



CREATE OR ALTER PROCEDURE DeleteUser
(
  @UserId INT
)
AS
BEGIN


SELECT * FROM Users

  -- Delete the user from the Users table.
  DELETE FROM Users
  WHERE UserId = @UserId;

  -- Delete the user from any related tables.
  DELETE FROM Post
  WHERE UserId = @UserId;
  DELETE FROM Critique
  WHERE UserId = @UserId;
  DELETE FROM Rating
  WHERE UserId = @UserId;
  DELETE FROM BookShelf
  WHERE UserId = @UserId;
  DELETE FROM CommunityMembers
  WHERE UserId = @UserId;
  DELETE FROM CommunityDiscussion
  WHERE CommunityMemberId = @UserId;
  DELETE FROM DiscussionReply
  WHERE CommunityMemberId = @UserId;
  DELETE FROM CritiqueReply
  WHERE UserId = @UserId;
  DELETE FROM CritiqueLike
  WHERE UserId = @UserId;
  DELETE FROM PostLike
  WHERE UserId = @UserId;
  DELETE FROM Following
  WHERE UserId = @UserId;

END;








CREATE OR ALTER PROCEDURE AddMember
@UserId INT,
@CommunityId INT
AS
BEGIN
INSERT INTO CommunityMembers VALUES (@CommunityId,@UserId,GETDATE())
END;

CREATE OR ALTER PROCEDURE ListDiscussion
@communityId INT
AS
SELECT * FROM CommunityDiscussion WHERE CommunityId=@communityId;


CREATE OR ALTER PROCEDURE getUsersRating
@UserId INT
AS
SELECT * From Rating WHERE UserId=@UserId


SELECT * FROM Book

SELECT * FROM CritiqueLike
SELECT * from Critique
SELECT * FROM CritiqueReply
SELECT * from Community
SELECT * FROM Book


CREATE OR ALTER PROCEDURE GetCritiqueReplyById
@CritiqueId INT
AS
SELECT * FROM CritiqueReply WHERE CritiqueId=@CritiqueId
