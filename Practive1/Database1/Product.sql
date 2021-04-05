CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Productname] VARCHAR(50) NOT NULL, 
    [Price] VARCHAR(50) NOT NULL, 
    [ProductDesc] NCHAR(10) NULL, 
    [PublishStatus] NCHAR(10) NOT NULL
)
