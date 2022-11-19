CREATE TABLE Sale
(
    Id serial NOT NULL  ,
    Symbol varchar(30) NOT NULL,
    ClientOrderId varchar(50) NULL,
    Price decimal (14,4) NULL,
	Quantity decimal (20,10) NULL, 
    CreateTime timestamp without time zone NULL,
    CONSTRAINT PK_Sale PRIMARY KEY (Id)    
);
CREATE TABLE SaleTrade
(
    Id serial NOT NULL,
	SaleId int not null,
    TradeId bigint NULL,
    Price decimal (16,6) NULL,
	Quantity decimal (20,10) NULL, 
	Fee decimal (16,6) NULL,
    FeeAsset varchar(30) NULL,
    CONSTRAINT PK_SaleTrade PRIMARY KEY (Id)    
);
ALTER TABLE SaleTrade
ADD 
    CONSTRAINT FK_SaleTradeSale FOREIGN KEY (SaleId) REFERENCES Sale (Id)