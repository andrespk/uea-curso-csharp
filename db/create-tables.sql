-- Criação da database caso não exista
SELECT 'CREATE DATABASE "minikanban-db"' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'minikanban-db')\gexec

-- Conecta na database minikanban-db
\c "minikanban-db"

CREATE TABLE IF NOT EXISTS users (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Username" VARCHAR(255) UNIQUE NOT NULL,
    "Email" VARCHAR(255) UNIQUE NOT NULL,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "Role" VARCHAR(50) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    "DeletedAt" TIMESTAMP WITH TIME ZONE,
    "IsActive" BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS boards (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Description" TEXT,
    "OwnerId" UUID NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    "DeletedAt" TIMESTAMP WITH TIME ZONE,
    "IsActive" BOOLEAN NOT NULL,
    CONSTRAINT fk_board_owner FOREIGN KEY ("OwnerId") REFERENCES users("Id") ON DELETE RESTRICT
);

CREATE TABLE IF NOT EXISTS board_members (
    "Id" UUID PRIMARY KEY,
    "BoardId" UUID NOT NULL,
    "UserId" UUID NOT NULL,
    "Role" INT NOT NULL,
    "JoinedAt" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    "DeletedAt" TIMESTAMP WITH TIME ZONE,
    "IsActive" BOOLEAN NOT NULL,
    CONSTRAINT fk_boardmember_board FOREIGN KEY ("BoardId") REFERENCES boards("Id") ON DELETE CASCADE,
    CONSTRAINT fk_boardmember_user FOREIGN KEY ("UserId") REFERENCES users("Id") ON DELETE CASCADE,
    CONSTRAINT uq_boardmember UNIQUE ("BoardId", "UserId")
);

CREATE TABLE IF NOT EXISTS kanban_columns (
    "Id" UUID PRIMARY KEY,
    "BoardId" UUID NOT NULL,
    "Name" VARCHAR(255) NOT NULL,
    "Order" INT NOT NULL,
    "WipLimit" INT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    "DeletedAt" TIMESTAMP WITH TIME ZONE,
    "IsActive" BOOLEAN NOT NULL,
    CONSTRAINT fk_column_board FOREIGN KEY ("BoardId") REFERENCES boards("Id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS tags (
    "Id" UUID PRIMARY KEY,
    "BoardId" UUID NOT NULL,
    "Name" VARCHAR(255) NOT NULL,
    "Color" VARCHAR(50),
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    "DeletedAt" TIMESTAMP WITH TIME ZONE,
    "IsActive" BOOLEAN NOT NULL,
    CONSTRAINT fk_tag_board FOREIGN KEY ("BoardId") REFERENCES boards("Id") ON DELETE CASCADE,
    CONSTRAINT uq_tag UNIQUE ("BoardId", "Name")
);

CREATE TABLE IF NOT EXISTS cards (
    "Id" UUID PRIMARY KEY,
    "ColumnId" UUID NOT NULL,
    "CreatedByUserId" UUID NOT NULL,
    "AssignedToUserId" UUID,
    "Title" VARCHAR(255) NOT NULL,
    "Description" TEXT,
    "Priority" INT NOT NULL,
    "DueDate" TIMESTAMP WITH TIME ZONE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    "DeletedAt" TIMESTAMP WITH TIME ZONE,
    "IsActive" BOOLEAN NOT NULL,
    CONSTRAINT fk_card_column FOREIGN KEY ("ColumnId") REFERENCES kanban_columns("Id") ON DELETE CASCADE,
    CONSTRAINT fk_card_createdby FOREIGN KEY ("CreatedByUserId") REFERENCES users("Id") ON DELETE RESTRICT,
    CONSTRAINT fk_card_assignedto FOREIGN KEY ("AssignedToUserId") REFERENCES users("Id") ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS card_tags (
    "CardId" UUID NOT NULL,
    "TagId" UUID NOT NULL,
    PRIMARY KEY ("CardId", "TagId"),
    CONSTRAINT fk_cardtag_card FOREIGN KEY ("CardId") REFERENCES cards("Id") ON DELETE CASCADE,
    CONSTRAINT fk_cardtag_tag FOREIGN KEY ("TagId") REFERENCES tags("Id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS comments (
    "Id" UUID PRIMARY KEY,
    "CardId" UUID NOT NULL,
    "UserId" UUID NOT NULL,
    "Content" TEXT NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    "DeletedAt" TIMESTAMP WITH TIME ZONE,
    "IsActive" BOOLEAN NOT NULL,
    CONSTRAINT fk_comment_card FOREIGN KEY ("CardId") REFERENCES cards("Id") ON DELETE CASCADE,
    CONSTRAINT fk_comment_user FOREIGN KEY ("UserId") REFERENCES users("Id") ON DELETE CASCADE
);

-- Inserção do usuário administrador inicial
INSERT INTO users ("Id", "Name", "Username", "Email", "PasswordHash", "Role", "CreatedAt", "IsActive")
VALUES ('7322166d-5ecb-40a2-8bb3-9552953e0702', 'Administrador', 'admin', 'admin@kanban.com', '008c70392e3abfbd0fa47bbc2ed96aa99bd49e159727fcba0f2e6abeb3a9d601', 'Admin', NOW(), true)
ON CONFLICT ("Username") DO NOTHING;