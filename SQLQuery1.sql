CREATE TABLE [dbo].[Users] (
    [UserId] INT IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (50) NOT NULL,
    [Password]  NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);


CREATE TABLE [dbo].[Calendars] (
    [CalendarId] INT IDENTITY (1, 1) NOT NULL,
    [UserId] INT IDENTITY (1, 1) NOT NULL,

    CONSTRAINT [PK_dbo.Calendars] PRIMARY KEY CLUSTERED ([CalendarId] ASC),
    CONSTRAINT [FK_dbo.Calendars_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Calendars] (
    [CalendarId] INT IDENTITY (1, 1) NOT NULL,
    [UserId] INT IDENTITY (1, 1) NOT NULL,

    CONSTRAINT [PK_dbo.Calendars] PRIMARY KEY CLUSTERED ([CalendarId] ASC),
    CONSTRAINT [FK_dbo.Calendars_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE
);