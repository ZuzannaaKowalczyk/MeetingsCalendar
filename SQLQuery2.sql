CREATE TABLE [dbo].[Users] (
    [UserId] INT IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (50) NOT NULL,
    [Password]  NVARCHAR (50) NOT NULL,
    PRIMARY KEY ([UserId] ASC)
);

CREATE TABLE [dbo].[Calendars] (
    [CalendarId] INT IDENTITY (1, 1) NOT NULL,
    [UserId] INT NOT NULL,
    PRIMARY KEY ([CalendarId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
);

CREATE TABLE [dbo].[Events] (
    [CalendarId] INT  NOT NULL,
    [UserId] INT NOT NULL,
    [EventId] INT IDENTITY (1, 1) NOT NULL,
    [Title] NVARCHAR(50) NOT NULL,
    [StartDate] DATETIME NOT NULL,
    [EndDate] DATETIME NOT NULL

    PRIMARY KEY ([EventId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]),
    FOREIGN KEY ([CalendarId]) REFERENCES [dbo].[Calendars] ([CalendarId])
);