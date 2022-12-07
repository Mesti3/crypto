CREATE TABLE Sale
(
    Id serial NOT NULL  ,
    Symbol varchar(30) NOT NULL,
	ExternalOrderId BIGINT NULL,
    ClientOrderId varchar(50) NULL,
    PurchaseId bigint NULL,
    UnitPrice decimal (14,4) NULL,
    TotalPrice decimal (14,4) NULL,
	Quantity decimal (20,10) NULL, 
    CreateTime timestamp without time zone NOT NULL CONSTRAINT DF_Flyer_Timestamp DEFAULT (now())::timestamp without time zone,
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