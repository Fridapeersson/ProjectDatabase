--DROP TABLE OrderRow
--DROP TABLE Review
--DROP TABLE [Order]
--DROP TABLE Product
--DROP TABLE Category
--DROP TABLE [Description]
--DROP TABLE Manufacture

CREATE TABLE Manufacture
(
	Id int primary key identity not null,
	ManufactureName nvarchar(50) not null unique
)

CREATE TABLE [Description]
(
	Id int primary key identity not null,
	Ingress nvarchar(100) not null unique,
	DescriptionText nvarchar(max) null,
)

CREATE TABLE Category
(
	Id int primary key identity not null,
	CategoryName nvarchar(50) not null unique
)

CREATE TABLE Product
(
	Id int primary key identity not null,
	ProductName nvarchar(50) not null unique,
	ProductPrice money not null,
	DescriptionId int REFERENCES Description(Id) not null,
	CategoryId int REFERENCES Category(Id) not null,
	ManufactureId int REFERENCES Manufacture(Id) not null
)


CREATE TABLE [Order] 
(
	Id int primary key identity not null,
	Orderdate DATETIME2 DEFAULT GETDATE() not null
)

CREATE TABLE Review
(
	Id int primary key identity not null,
	ReviewText nvarchar(max) not null,
	ReviewDate DATETIME2 DEFAULT GETDATE() not null,
	ProductId int REFERENCES Product(Id) ON DELETE CASCADE not null
)

CREATE TABLE OrderRow
(
	Id int primary key identity not null,
	Quantity int not null,
	ProductId int REFERENCES Product(Id) ON DELETE CASCADE  not null,
	OrderId int REFERENCES [Order](Id) ON DELETE CASCADE  not null
)