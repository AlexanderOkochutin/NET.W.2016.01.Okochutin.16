CREATE TABLE [IngradientCategory] (
	CategoryId integer IDENTITY(1,1) PRIMARY KEY,
	CategoryName varchar(50) NOT NULL,
)
GO
CREATE TABLE [Ingradient] (
	IngradientId integer IDENTITY(1,1) PRIMARY KEY,
	IngradientName varchar(50) NOT NULL,
	TotalWeight integer NOT NULL,
	CategoryId integer NOT NULL,
)
GO
CREATE TABLE [ShawarmaRecipe] (
	ShawarmaRecipeId integer IDENTITY(1,1) PRIMARY KEY,
	ShawarmaId integer NOT NULL,
	IngradientId integer NOT NULL,
	Weight integer NOT NULL,
)
GO
CREATE TABLE [Shawarma] (
	ShawarmaId integer IDENTITY(1,1) PRIMARY KEY,
	ShawarmaName varchar(50) NOT NULL,
	CookingTime integer NOT NULL,
)
GO
CREATE TABLE [OrderHeader] (
	OrderHeaderId integer IDENTITY(1,1) PRIMARY KEY,
	OrderDate date NOT NULL,
	SellerId integer NOT NULL,
)
GO
CREATE TABLE [OrderDetails] (
	OrderHeaderId integer NOT NULL,
	ShawarmaId integer NOT NULL,
	Quantity integer NOT NULL
)
GO
CREATE TABLE [SellingPointCategory] (
	SellingPointCategoryId integer IDENTITY(1,1) PRIMARY KEY,
	SellingPointCategoryName varchar(50) NOT NULL,
)
GO
CREATE TABLE [SellingPoint] (
	SellingPointId integer IDENTITY(1,1) PRIMARY KEY,
	Address varchar(50) NOT NULL,
	SellingPointCategoryId integer NOT NULL,
	ShawarmaTitle varchar(50) NOT NULL,
)
GO
CREATE TABLE [PriceController] (
	PriceControllerId integer IDENTITY(1,1) PRIMARY KEY,
	ShawarmaId integer NOT NULL,
	Price decimal NOT NULL,
	SellingPointId integer NOT NULL,
	Comment text NOT NULL,
)
GO
CREATE TABLE [Seller] (
	SellerId integer IDENTITY(1,1) PRIMARY KEY,
	SellerName varchar(50) NOT NULL,
	SellerPointId integer NOT NULL,
)
GO
CREATE TABLE [TimeController] (
	TimeControllerId integer IDENTITY(1,1) PRIMARY KEY,
	SellerId integer NOT NULL,
	WorkStart datetime NOT NULL,
	WorkEnd datetime NOT NULL,
)
GO

ALTER TABLE [Ingradient] WITH CHECK ADD CONSTRAINT [Ingradient_fk0] FOREIGN KEY ([CategoryId]) REFERENCES [IngradientCategory]([CategoryId])
ON UPDATE CASCADE
GO
ALTER TABLE [Ingradient] CHECK CONSTRAINT [Ingradient_fk0]
GO

ALTER TABLE [ShawarmaRecipe] WITH CHECK ADD CONSTRAINT [ShawarmaRecipe_fk0] FOREIGN KEY ([ShawarmaId]) REFERENCES [Shawarma]([ShawarmaId])
ON UPDATE CASCADE
GO
ALTER TABLE [ShawarmaRecipe] CHECK CONSTRAINT [ShawarmaRecipe_fk0]
GO
ALTER TABLE [ShawarmaRecipe] WITH CHECK ADD CONSTRAINT [ShawarmaRecipe_fk1] FOREIGN KEY ([IngradientId]) REFERENCES [Ingradient]([IngradientId])
ON UPDATE CASCADE
GO
ALTER TABLE [ShawarmaRecipe] CHECK CONSTRAINT [ShawarmaRecipe_fk1]
GO


ALTER TABLE [OrderHeader] WITH CHECK ADD CONSTRAINT [OrderHeader_fk0] FOREIGN KEY ([SellerId]) REFERENCES [Seller]([SellerId])
ON UPDATE CASCADE
GO
ALTER TABLE [OrderHeader] CHECK CONSTRAINT [OrderHeader_fk0]
GO

ALTER TABLE [OrderDetails] WITH CHECK ADD CONSTRAINT [OrderDetails_fk0] FOREIGN KEY ([OrderHeaderId]) REFERENCES [OrderHeader]([OrderHeaderId])
ON UPDATE CASCADE
GO
ALTER TABLE [OrderDetails] CHECK CONSTRAINT [OrderDetails_fk0]
GO
ALTER TABLE [OrderDetails] WITH CHECK ADD CONSTRAINT [OrderDetails_fk1] FOREIGN KEY ([ShawarmaId]) REFERENCES [Shawarma]([ShawarmaId])
ON UPDATE CASCADE
GO
ALTER TABLE [OrderDetails] CHECK CONSTRAINT [OrderDetails_fk1]
GO


ALTER TABLE [SellingPoint] WITH CHECK ADD CONSTRAINT [SellingPoint_fk0] FOREIGN KEY ([SellingPointCategoryId]) REFERENCES [SellingPointCategory]([SellingPointCategoryId])
ON UPDATE CASCADE
GO
ALTER TABLE [SellingPoint] CHECK CONSTRAINT [SellingPoint_fk0]
GO

ALTER TABLE [PriceController] WITH CHECK ADD CONSTRAINT [PriceController_fk0] FOREIGN KEY ([ShawarmaId]) REFERENCES [Shawarma]([ShawarmaId])
ON UPDATE CASCADE
GO
ALTER TABLE [PriceController] CHECK CONSTRAINT [PriceController_fk0]
GO
ALTER TABLE [PriceController] WITH CHECK ADD CONSTRAINT [PriceController_fk1] FOREIGN KEY ([SellingPointId]) REFERENCES [SellingPoint]([SellingPointId])
ON UPDATE CASCADE
GO
ALTER TABLE [PriceController] CHECK CONSTRAINT [PriceController_fk1]
GO

ALTER TABLE [Seller] WITH CHECK ADD CONSTRAINT [Seller_fk0] FOREIGN KEY ([SellerPointId]) REFERENCES [SellingPoint]([SellingPointId])
ON UPDATE CASCADE
GO
ALTER TABLE [Seller] CHECK CONSTRAINT [Seller_fk0]
GO

ALTER TABLE [TimeController] WITH CHECK ADD CONSTRAINT [TimeController_fk0] FOREIGN KEY ([SellerId]) REFERENCES [Seller]([SellerId])
ON UPDATE CASCADE
GO
ALTER TABLE [TimeController] CHECK CONSTRAINT [TimeController_fk0]
GO