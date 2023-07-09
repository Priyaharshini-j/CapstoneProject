
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
  ('David Johnson', 'david.johnson@example.com', 'qwerty', 'In what city were you born?', 'New York', GETDATE());


--CREATING THE DATABASE FOR BOOK
CREATE TABLE Book
(
BookId INT IDENTITY(1,1) PRIMARY KEY,
ISBN VARCHAR(50) CONSTRAINT [Uk_ISBN] UNIQUE,
BookName VARCHAR(225),
Genre VARCHAR(MAX) NOT NULL,
Author VARCHAR(100),
CoverUrl VARCHAR(MAX),
BookDesc VARCHAR(MAX),
Publisher VARCHAR(200),
PublishedDate DATETIME
);

SELECT * FROM Book
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

--CREATING THE TABLE FOR THE SECONDARY TABLE FOR THE PROJECT

--CREATING TABLE FOR COMMUNITYMEMBER
CREATE TABLE CommunityMembers
(
CommunityMemberId INT IDENTITY(1,1) PRIMARY KEY,
CommunityId INT NOT NULL,
UserId INT NOT NULL,
CreatedDate DATETIME DEFAULT SYSDATETIME(),
CONSTRAINT [Fk_CommMember_CommId] FOREIGN KEY (CommunityId) REFERENCES Community(CommunityId),
CONSTRAINT [Fk_CommMember_UserId] FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

--Creating table for the CommunityDiscussion
CREATE TABLE CommunityDiscussion
(
DiscussionId INT IDENTITY(1,1) PRIMARY KEY,
CommunityId INT NOT NULL,
CommunityMemberId INT  NOT NULL,
Discussion VARCHAR(MAX) NOT NULL,
CreatedDate DATETIME DEFAULT SYSDATETIME(),
CONSTRAINT [Fk_CommDiscuss_CommId] FOREIGN KEY (CommunityId) REFERENCES Community(CommunityId),
CONSTRAINT [Fk_CommDiscuss_MemId] FOREIGN KEY (CommunityMemberId) REFERENCES CommunityMembers(CommunityMemberId)
);

--CREATING TABLE FOR THE REPLIES FOR THE Discussion
CREATE TABLE DiscussionReply
(
DiscussionReplyId INT IDENTITY(1,1) PRIMARY KEY,
DiscussionId INT NOT NULL,
CommunityMemberId INT NOT NULL,
DiscussionReply VARCHAR(MAX) NOT NULL,
CreatedDate DATETIME DEFAULT SYSDATETIME(),
CONSTRAINT [Fk_DiscussReply_CommId] FOREIGN KEY (DiscussionId) REFERENCES CommunityDiscussion(DiscussionId),
CONSTRAINT [Fk_DiscussReply_MemId] FOREIGN KEY (CommunityMemberId) REFERENCES CommunityMembers(CommunityMemberId)
);

--Creating the table for Post Like/Dislike
CREATE TABLE PostLike
(
LikeId INT IDENTITY(1,1) PRIMARY KEY,
PostId INT NOT NULL,
UserId INT NOT NULL,
LikeStatus BIT,
CreatedDate DATETIME DEFAULT SYSDATETIME(),
CONSTRAINT [Fk_PostLike_UserId] FOREIGN KEY (UserId) REFERENCES Users(UserId),
CONSTRAINT [Fk_PostId] FOREIGN KEY (PostId) REFERENCES Post(PostId),
);

--CREATING TABLE FOR THE REPLIES FOR THE CRITIQUE
CREATE TABLE CritiqueReply
(
CritiqueReplyId INT IDENTITY(1,1) PRIMARY KEY,
CritiqueId INT NOT NULL,
UserId INT NOT NULL,
Reply VARCHAR(MAX) NOT NULL,
CreatedDate DATETIME DEFAULT SYSDATETIME(),
CONSTRAINT [Fk_CritiqueReply_CritiqueId] FOREIGN KEY (CritiqueId) REFERENCES Critique(CritiqueId),
CONSTRAINT [Fk_CritiqueReply_UserId] FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

--Creating the table for Critique Like/Dislike
CREATE TABLE CritiqueLike
(
CritiqueLikeId INT IDENTITY(1,1) PRIMARY KEY,
CritiqueId INT NOT NULL,
UserId INT NOT NULL,
LikeStatus INT,
CreatedDate DATETIME DEFAULT SYSDATETIME(),
CONSTRAINT [Fk_CritiqueLike_CritiqueId] FOREIGN KEY (CritiqueId) REFERENCES Critique(CritiqueId),
CONSTRAINT [Fk_CritiqueLike_UserId] FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

--Creating the table for followers table
CREATE TABLE Following
(
FollowingId INT IDENTITY(1,1) PRIMARY KEY,
UserId INT NOT NUll,
FollowingUserId INT NOT NULL,
CreatedDate DATE DEFAULT SYSDATETIME(),
CONSTRAINT [Fk_Following_FollowerId] FOREIGN KEY (UserId) REFERENCES Users(UserId),
CONSTRAINT [Fk_Following_FollowingId] FOREIGN KEY (FollowingUserId) REFERENCES Users(UserId),
);

--Community->CommunityMembers->CommunityDiscussion->DiscussionReply
--Critic->CritiqueReply
--Critic->CritiqueLike
--Post->PostLike

SELECT * FROM PostLike;
SELECT * FROM Post
SELECT * FROM CritiqueLike
SELECT * FROM CritiqueReply
SELECT * FROM Critique
SELECT * FROM BookShelf
SELECT * FROM Rating
SELECT * FROM DiscussionReply
SELECT * FROM CommunityDiscussion
SELECT * FROM CommunityMembers
SELECT * FROM Community
SELECT * FROM Book
SELECT * FROM Users

--Drop TABle Following
--DROP TABLE PostLike;
--DROP TABLE Post
--DROP TABLE CritiqueLike
--DROP TABLE CritiqueReply
--DROP TABLE Critique
--DROP TABLE BookShelf
--DROP TABLE Rating
DROP TABLE DiscussionReply
DROP TABLE CommunityDiscussion
DROP TABLE CommunityMembers
DROP TABLE Community
--DROP TABLE Book

DELETE FROM Rating WHERE RatingId=7


--Drop TABLE Users
