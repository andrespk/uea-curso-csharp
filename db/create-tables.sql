CREATE TABLE IF NOT EXISTS Users (
                        Id UUID PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL,
                        Email VARCHAR(255) UNIQUE NOT NULL,
                        PasswordHash VARCHAR(255) NOT NULL,
                        CreatedAt TIMESTAMP NOT NULL
);

CREATE TABLE IF NOT EXISTS Board (
                       Id UUID PRIMARY KEY,
                       Name VARCHAR(255) NOT NULL,
                       Description TEXT,
                       OwnerId UUID NOT NULL,
                       CreatedAt TIMESTAMP NOT NULL,
                       CONSTRAINT fk_board_owner FOREIGN KEY (OwnerId) REFERENCES "User"(Id)
);

CREATE TABLE IF NOT EXISTS BoardMember (
                             Id UUID PRIMARY KEY,
                             BoardId UUID NOT NULL,
                             UserId UUID NOT NULL,
                             Role VARCHAR(50) NOT NULL CHECK (Role IN ('Owner', 'Admin', 'Member', 'Viewer')),
                             JoinedAt TIMESTAMP NOT NULL,
                             CONSTRAINT fk_boardmember_board FOREIGN KEY (BoardId) REFERENCES Board(Id),
                             CONSTRAINT fk_boardmember_user FOREIGN KEY (UserId) REFERENCES "User"(Id)
);

CREATE TABLE IF NOT EXISTS KanbanColumn (
                              Id UUID PRIMARY KEY,
                              BoardId UUID NOT NULL,
                              Name VARCHAR(255) NOT NULL,
                              "Order" INT NOT NULL,
                              WipLimit INT,
                              CreatedAt TIMESTAMP NOT NULL,
                              CONSTRAINT fk_column_board FOREIGN KEY (BoardId) REFERENCES Board(Id)
);

CREATE TABLE IF NOT EXISTS Tag (
                     Id UUID PRIMARY KEY,
                     BoardId UUID NOT NULL,
                     Name VARCHAR(255) NOT NULL,
                     Color VARCHAR(50),
                     CONSTRAINT fk_tag_board FOREIGN KEY (BoardId) REFERENCES Board(Id)
);

CREATE TABLE IF NOT EXISTS Card (
                      Id UUID PRIMARY KEY,
                      ColumnId UUID NOT NULL,
                      CreatedByUserId UUID NOT NULL,
                      AssignedToUserId UUID, 
                      Title VARCHAR(255) NOT NULL,
                      Description TEXT,
                      Priority INT NOT NULL,
                      CreatedAt TIMESTAMP NOT NULL,
                      DueDate TIMESTAMP,
                      CONSTRAINT fk_card_column FOREIGN KEY (ColumnId) REFERENCES KanbanColumn(Id),
                      CONSTRAINT fk_card_createdby FOREIGN KEY (CreatedByUserId) REFERENCES "User"(Id),
                      CONSTRAINT fk_card_assignedto FOREIGN KEY (AssignedToUserId) REFERENCES "User"(Id)
);

CREATE TABLE IF NOT EXISTS CardTag (
                         CardId UUID NOT NULL,
                         TagId UUID NOT NULL,
                         PRIMARY KEY (CardId, TagId),
                         CONSTRAINT fk_cardtag_card FOREIGN KEY (CardId) REFERENCES Card(Id),
                         CONSTRAINT fk_cardtag_tag FOREIGN KEY (TagId) REFERENCES Tag(Id)
);

CREATE TABLE IF NOT EXISTS Comment (
                         Id UUID PRIMARY KEY,
                         CardId UUID NOT NULL,
                         UserId UUID NOT NULL,
                         Content TEXT NOT NULL,
                         CreatedAt TIMESTAMP NOT NULL,
                         CONSTRAINT fk_comment_card FOREIGN KEY (CardId) REFERENCES Card(Id),
                         CONSTRAINT fk_comment_user FOREIGN KEY (UserId) REFERENCES "User"(Id)
);