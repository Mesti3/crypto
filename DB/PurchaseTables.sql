CREATE TABLE Purchase
(
    Id serial NOT NULL  ,
    Symbol varchar(30) NOT NULL,
	ExternalOrderId BIGINT NULL,
    ClientOrderId varchar(50) NULL,
    UnitPrice decimal (14,4) NULL,
    TotalPrice decimal (14,4) NULL,
	Quantity decimal (20,10) NULL, 
    CreateTime timestamp without time zone NOT NULL CONSTRAINT DF_Flyer_Timestamp DEFAULT (now())::timestamp without time zone,
    SaleDate  timestamp without time zone NULL,
    CONSTRAINT PK_Purchase PRIMARY KEY (Id)    
);
CREATE TABLE PurchaseTrade
(
    Id serial NOT NULL,
	PurchaseId int not null,
    TradeId bigint NULL,
    Price decimal (16,6) NULL,
	Quantity decimal (20,10) NULL, 
	Fee decimal (16,6) NULL,
    FeeAsset varchar(30) NULL,
    CONSTRAINT PK_PurchaseTrade PRIMARY KEY (Id)    
);
ALTER TABLE PurchaseTrade
ADD 
    CONSTRAINT FK_PurchaseTradePurchase FOREIGN KEY (PurchaseId) REFERENCES Purchase (Id)