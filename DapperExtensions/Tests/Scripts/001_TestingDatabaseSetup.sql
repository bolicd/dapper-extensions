BEGIN
    CREATE TABLE [dbo].[TestTable]
    (
        [Id]            INT IDENTITY (1,1) PRIMARY KEY,
        [Year]     INT            NOT NULL,
        [FirstName]        NVARCHAR(155)  NOT NULL,
        [LastName]     NVARCHAR(155)  NOT NULL,
        [DateProcessed] smalldatetime  NOT NULL
    )
END;