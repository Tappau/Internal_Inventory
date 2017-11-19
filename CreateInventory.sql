CREATE TABLE Publisher(
  Publisher_ID int IDENTITY(1,1) PRIMARY KEY,
  Pub_Name nvarchar(255) NOT NULL,
  Year_Began int,
  notes nvarchar(MAX),
  URL varchar(255),

  CONSTRAINT chk_pubName_not_zero CHECK (DATALENGTH(Pub_Name) > 0)
);
GO
CREATE INDEX idx_PublisherName ON Publisher (Pub_Name)
GO
CREATE TABLE Series(
  Series_ID int IDENTITY(1,1) PRIMARY KEY,
  Series_Name nvarchar(255) NOT NULL,
  Year_Began int,
  Year_End int,
  dimensions varchar(255),
  paperStock varchar(255),
  Publisher_ID int,
  CONSTRAINT fk_series_publisher FOREIGN KEY (Publisher_ID) REFERENCES Publisher(Publisher_ID),
  CONSTRAINT chk_name_not_zero CHECK (DATALENGTH(Series_Name) > '')
);
GO
CREATE INDEX idx_SeriesName ON Series(Series_Name);
CREATE INDEX idx_publisher_fk ON Series(Publisher_ID);
GO
CREATE TABLE BoxStore (
  BoxID int IDENTITY(1,1) PRIMARY KEY,
  BoxName varchar(255),
  QR_Data varchar(500),
  isActive bit DEFAULT 1,
  Notes nvarchar(MAX),
);
GO
CREATE TABLE Issue(
  Issue_ID int IDENTITY(1,1) PRIMARY KEY,
  Series_ID int,
  Box_ID int,

  Number varchar(6) NOT NULL, -- CHK to ensure blank entry such as space, tabs
  publication_date varchar(255),
  page_count decimal(10, 2),
  frequency varchar(255),
  editor varchar(1600),
  ISBN varchar(13),
  barcode varchar(38),
  title varchar(255),
  IsActive bit DEFAULT 1,

  CONSTRAINT chk_number_not_empty CHECK (Number <> ''),
  CONSTRAINT fk_Series_Issue FOREIGN KEY(Series_ID) REFERENCES Series(Series_ID),
  CONSTRAINT fk_Series_BoxStore FOREIGN KEY(Box_ID) REFERENCES BoxStore(BoxID),
);
GO
CREATE INDEX idx_series_fk ON Issue(Series_ID);
CREATE INDEX idx_issue_barcode ON Issue(barcode);
CREATE INDEX idx_issue_isbn ON Issue(ISBN);
CREATE INDEX idx_issue_number ON Issue(Number);
GO
CREATE TABLE Grade(
  GradeID int IDENTITY(1,1) PRIMARY KEY,
  Name varchar(6),
);
GO
INSERT INTO Grade(Name) VALUES ('NM-');
INSERT INTO Grade(Name) VALUES ('VF/NM+');
INSERT INTO Grade(Name) VALUES ('VF/NM-');
INSERT INTO Grade(Name) VALUES ('VF/NM');
INSERT INTO Grade(Name) VALUES ('VF/VF+');
INSERT INTO Grade(Name) VALUES ('VF+');
INSERT INTO Grade(Name) VALUES ('VF');
INSERT INTO Grade(Name) VALUES ('VF-');
INSERT INTO Grade(Name) VALUES ('F+');
INSERT INTO Grade(Name) VALUES ('F');
INSERT INTO Grade(Name) VALUES ('F-');
INSERT INTO Grade(Name) VALUES ('VG+');
INSERT INTO Grade(Name) VALUES ('VG');
INSERT INTO Grade(Name) VALUES ('VG-');
INSERT INTO Grade(Name) VALUES ('G+');
INSERT INTO Grade(Name) VALUES ('G');
INSERT INTO Grade(Name) VALUES ('G-');
GO
CREATE TABLE IssueCondition(
  IssueCondition_ID int IDENTITY(1,1) PRIMARY KEY,
  Issue_ID int,
  Grade_ID int,
  quantity int DEFAULT 1,

  CONSTRAINT fk_IssueCondition_Issue FOREIGN KEY(Issue_ID) REFERENCES Issue(Issue_ID),
  CONSTRAINT fk_IssueCondition_Grade FOREIGN KEY(Grade_ID) REFERENCES Grade(GradeID),
  CONSTRAINT uq_IssueID_AND_GradeID UNIQUE (Issue_ID,Grade_ID)
);
--GO
--CREATE TABLE Customer (
--Customer_ID int IDENTITY(1,1) PRIMARY KEY,
--FirstName nvarchar(255),
--LastName nvarchar(255),
--AddressLine1 nvarchar(500) NOT NULL,
--AddressLine2 nvarchar(500),
--AddressLine3 nvarchar(500),
--City nvarchar(255),
--Postcode_ZIP nvarchar(20) NOT NULL,
--Country nvarchar(255)
--);
--GO
--CREATE TABLE Orders(
--Order_ID int IDENTITY(1,1) PRIMARY KEY,
--Customer_ID int NOT NULL,
--OrderDate date,
--ShippedDate date,
--Comments nvarchar(MAX),

--CONSTRAINT fk_Orders_Customers FOREIGN KEY(Customer_ID) REFERENCES Customer(Customer_ID)
--);
--GO
--CREATE TABLE OrderDetail(
--OrderDetail_ID int IDENTITY(1,1) PRIMARY KEY,
--Order_ID int,
--Item_ID int,
--quantity int DEFAULT 1,
--price decimal(14, 2),

--CONSTRAINT fk_OrderDetail_Order FOREIGN KEY(Order_ID) REFERENCES Orders(Order_ID),
--CONSTRAINT fk_OrderDetail_IssueCondition FOREIGN KEY(Item_ID) REFERENCES IssueCondition(IssueCondition_ID)
--);
--GO
--CREATE TABLE Person(
-- Person_ID int IDENTITY(1,1) PRIMARY KEY,
-- FirstName nvarchar(255),
-- LastName nvarchar(255)
--);