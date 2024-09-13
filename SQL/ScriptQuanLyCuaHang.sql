INSERT INTO dbo.Account (
	UserName ,
	DisplayName ,
	Password ,
	[Type] 

)
VALUES(
	N'Admin', 
	N'Admin',
	N'1',
	1
)

INSERT INTO dbo.Account (
	UserName ,
	DisplayName ,
	Password ,
	[Type] 

)
VALUES(
	N'Employee',
	N'Employee',
	N'1',
	0
)

INSERT INTO dbo.Account (
	UserName ,
	DisplayName ,
	Password ,
	[Type] 

)
VALUES(
	N'IT',
	N'PhanHongHa',
	N'1',
	1
)

SELECT *FROM Account a 

CREATE PROC USP_GetAccountByUserName
@userName nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName
END

DROP  PROC USP_GetAccountByUserName

EXEC dbo.USP_GetAccountByUserName @userName = N'IT'


CREATE PROC USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND Password = @passWord
END

USP_Login @userName = N'IT' , @passWord = N'1'



CREATE PROC USP_GetCustomerList
AS
SELECT * FROM dbo.Customers 

EXEC USP_GetCustomerList

--Thêm Customers
INSERT dbo.Customers 
		(name, address, phonenumber)
VALUES (N'Nhi 1', N'Long Hòa', N'0919807202')
INSERT dbo.Customers 
		(name, address, phonenumber)
VALUES (N'Nhi 2', N'Long Hòa', N'0919807202')
INSERT dbo.Customers 
		(name, address, phonenumber)
VALUES (N'Nhi 3', N'Long Hòa', N'0919807202')
INSERT dbo.Customers 
		(name, address, phonenumber)
VALUES (N'Nhi 4', N'Long Hòa', N'0919807202')
INSERT dbo.Customers 
		(name, address, phonenumber)
VALUES (N'Nhi 5', N'Long Hòa', N'0919807202')

SELECT * FROM Customers c 

--Thêm Category
INSERT dbo.ProductCategory 
		(name)
VALUES (N'Mỹ phẩm')
INSERT  dbo.ProductCategory 
		(name)
VALUES (N'Dép')
INSERT dbo.ProductCategory 
		(name)
VALUES (N'Spa')

SELECT * FROM ProductCategory 

--Thêm Product
INSERT dbo.Product
		(name, idCategory, price)
VALUES (N'Nước hoa 1', 1,  200000)
INSERT dbo.Product
		(name, idCategory, price)
VALUES (N'Nước hoa 2', 1,  200000)
INSERT dbo.Product
		(name, idCategory, price)
VALUES (N'Nước hoa 3', 1,  200000)
INSERT dbo.Product
		(name, idCategory, price)
VALUES (N'Nước hoa 4', 1,  200000)
INSERT  dbo.Product
		(name, idCategory, price)
VALUES (N'Dép 1', 2, 50000)
INSERT  dbo.Product
		(name, idCategory, price)
VALUES (N'Dép 2', 2, 50000)
INSERT  dbo.Product
		(name, idCategory, price)
VALUES (N'Dép 3', 2, 50000)
INSERT  dbo.Product
		(name, idCategory, price)
VALUES (N'Dép 4', 2, 50000)
INSERT  dbo.Product
		(name, idCategory, price)
VALUES (N'Spa 1', 3, 300000)
INSERT  dbo.Product
		(name, idCategory, price)
VALUES (N'Spa 2', 3, 300000)
INSERT  dbo.Product
		(name, idCategory, price)
VALUES (N'Spa 3', 3,300000)
INSERT  dbo.Product
		(name, idCategory, price)
VALUES (N'Spa 4', 3, 300000)

SELECT *FROM Product p 




--Thêm Bill
INSERT dbo.Bill
		(DateCheckIn, DateCheckOut, idCustomer, status)
VALUES (GETDATE(), GETDATE(), 1, 0)
INSERT dbo.Bill
		(DateCheckIn, DateCheckOut,idCustomer, status)
VALUES (GETDATE(), GETDATE(), 2, 0)
INSERT dbo.Bill
		(DateCheckIn, DateCheckOut, idCustomer, status)
VALUES (GETDATE(), GETDATE(), 3, 0)
INSERT dbo.Bill
		(DateCheckIn, DateCheckOut, idCustomer, status)
VALUES (GETDATE(), GETDATE(), 4,  1)


SELECT * FROM Bill b 


--Thêm Bill Info
INSERT dbo.BillInfo
		(idBill, idProduct, count)
VALUES (2, 2, 2)
INSERT dbo.BillInfo
		(idBill, idProduct, count)
VALUES (3, 3, 2)
INSERT dbo.BillInfo
		(idBill, idProduct, count)
VALUES (4, 2, 1)
INSERT dbo.BillInfo
		(idBill, idProduct, count)
VALUES (2, 3, 2)
INSERT dbo.BillInfo
		(idBill, idProduct, count)
VALUES (3, 2, 2)
INSERT dbo.BillInfo
		(idBill, idProduct, count)
VALUES (2, 6, 2)
INSERT dbo.BillInfo
		(idBill, idProduct, count)
VALUES (4, 7, 2)

SELECT * FROM BillInfo bi 


ALTER  TABLE dbo.Bill 
ADD discount INT

ALTER  TABLE dbo.Bill 
ADD totalPrice FLOAT

ALTER  TABLE dbo.Customers 
ADD Status NVARCHAR(100) NOT NULL DEFAULT N'Chưa thanh toán'

CREATE PROC USP_GetListBillByDate
@checkIn date, @checkOut date
AS
BEGIN
	SELECT c.name [Tên bàn], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá], b.totalPrice AS [Tổng tiền]
	FROM dbo.Bill AS b, dbo.Customers AS c
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status =1
	AND c.id = b.idCustomer
END

DROP PROC USP_GetListBillByDate

CREATE  PROC USP_UpdateAccount
@userName NVARCHAR(100), @displayName NVARCHAR(100), @password NVARCHAR(100), @newPassword NVARCHAR(100)
AS
BEGIN
	DECLARE @isRightPass INT = 0
	
	SELECT  @isRightPass = COUNT(*) FROM dbo.Account  WHERE  userName = @userName AND password = @password
	
	IF(@isRightPass =1)
	BEGIN
		IF (@newPassword = NULL OR @newPassword = '')
			BEGIN 
				UPDATE dbo.Account SET DisplayName = @displayName WHERE  userName = @userName
			END
		ELSE 
			UPDATE dbo.Account SET displayName = @displayName, password = @newPassword WHERE  userName = @userName
	END
	
END


CREATE PROC USP_InsertBill
@idCustomer INT
AS
BEGIN
	INSERT dbo.Bill
			(DateCheckIn, DateCheckOut, idCustomer, status, discount)
	VALUES (GETDATE(), NULL, @idCustomer, 0, 0)
END

CREATE PROC USP_InsertBillInfo
@idBill INT, @idProduct INT, @count INT
AS
BEGIN
	INSERT dbo.BillInfo
			(idBill, idProduct, count)
	VALUES (@idBill, @idProduct, @count)
END

ALTER  PROC USP_InsertBillInfo
@idBill INT, @idProduct INT, @count INT
AS
BEGIN
	
	DECLARE @isExitsBillInfo INT
	DECLARE @productCount INT = 1
	
	SELECT @isExitsBillInfo =id, @productCount = count
	FROM dbo.BillInfo 
	WHERE IdBill = @idBill AND IdProduct = @idProduct
	
	IF (@isExitsBillInfo >0)
	BEGIN
		DECLARE @newCount INT = @productCount + @count
		IF(@newCount > 0)
			UPDATE dbo.BillInfo  SET count = @productCount + @count WHERE idProduct = @idProduct
		ELSE 
			DELETE dbo.BillInfo WHERE idBill = @idBill AND idProduct = @idProduct
	END
	ELSE 
		BEGIN 
			INSERT dbo.BillInfo
					(idBill, idProduct, count)
			VALUES (@idBill, @idProduct, @count)
		END
	
END


CREATE TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo  FOR INSERT , UPDATE
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill = idBill FROM Inserted
	DECLARE @idCustomer INT
	SELECT @idCustomer = idCustomer FROM dbo.Bill WHERE id = @idBill AND status =0
	
	DECLARE @count INT
	SELECT  @count = COUNT(*) FROM dbo.BillInfo WHERE  idBill = @idBill
	
	IF (@count > 0)
		BEGIN
			UPDATE dbo.Customers  SET status = N'Chưa thanh toán' Where id = @idCustomer
		END
	ELSE 
		BEGIN
			UPDATE dbo.Customers  SET status = N'Đã thanh toán' Where id = @idCustomer
		END
			
	
END

CREATE TRIGGER UTG_UpdateTable
ON dbo.Customers  FOR UPDATE 
AS
BEGIN
	DECLARE @idCustomer INT
	
	DECLARE @status NVARCHAR(100)
	SELECT  @idCustomer = id, @status = Inserted.status FROM Inserted
	
	DECLARE @idBill INT
	SELECT @idBill = id FROM dbo.Bill WHERE idCustomer = @idCustomer AND status = 0
	
	DECLARE @countBillInfo INT
	SELECT @countBillInfo = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idBill
	
	IF(@countBillInfo > 0 )--AND @status <> N'Chưa thanh toán')
		UPDATE dbo.Customers SET status = N'Chưa thanh toán' WHERE id = @idCustomer
	ELSE-- IF (@countBillInfo <= 0 AND status = N'Đã thanh toán')
		UPDATE  dbo.Customers SET status = N'Đã thanh toán'  WHERE id = @idCustomer
END



CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE 
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBiLL = id FROM Inserted
	DECLARE @idCustomer INT
	SELECT @idCustomer = idCustomer FROM dbo.Bill WHERE id = @idBill
	DECLARE @count INT = 0
	SELECT @count = COUNT(*) FROM dbo.Bill WHERE  idCustomer = @idCustomer AND status =0
	IF(@count = 0)
		UPDATE dbo.Customers SET status = N'Đã thanh toán' WHERE id = @idCustomer
END

-- hàm SQL xử lý tìm kiểm gần đúng chuẩn nhất

CREATE FUNCTION [dbo].[fuConvertToUnsign1] 
(@strInput NVARCHAR(4000) ) 
RETURNS NVARCHAR(4000) 
AS 
BEGIN 
	IF @strInput IS NULL 
	RETURN @strInput 
	IF @strInput = '' 
	RETURN @strInput 
	 
	DECLARE @RT NVARCHAR(4000) 
	DECLARE @SIGN_CHARS NCHAR(137) 
	DECLARE @UNSIGN_CHARS NCHAR(137) 
	SET @SIGN_CHARS = N'ăâđêơưàáảãạằắẳẵặấẩẫầậèéẻẽẹếềểễệ ìíịỉĩòóỏõọồổốỗộớờợỡởùụúủũừứửữựỳýỹỷỵ  ĂÂĐÊƠƯÀÁẢÃẠẰẮẲẴẶẤẨẪẦẬÈÉẺẼẸẾỀỂỄỆ ÌÍỊỈĨÒÓỎÕỌỒỔỐỖỘỚỜỢỠỞÙỤÚỦŨỪỨỬỮỰỲÝỸỶỴ' +NCHAR(272) + NCHAR(208) 
	SET @UNSIGN_CHARS = N'aadeouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOUAAAAAAAAAAAAAAAEEEEEEEEEE IIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYf' 
	
	DECLARE @COUNTER INT 
	DECLARE @COUNTER1 INT 
	SET @COUNTER = 1 
	WHILE (@COUNTER <= LEN(@strInput) ) 
	BEGIN  
		SET @COUNTER1 = 1 
		WHILE (@COUNTER1 <= LEN(@SIGN_CHARS) ) 
		BEGIN 
			IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput, @COUNTER, 1) )
			BEGIN 
				IF @COUNTER=1 
					SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1, 1) + SUBSTRING(@strInput, @COUNTER+1, LEN(@strInput) - 1 ) 
				ELSE 
					SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) + SUBSTRING(@UNSIGN_CHARS, @COUNTER1, 1) + SUBSTRING(@strInput, @COUNTER +1 , LEN(@strInput) - @COUNTER)
		
				BREAK 
			END 
			SET @COUNTER1 = @COUNTER1 +1 
		END 
		SET @COUNTER = @COUNTER + 1 
	END 
	SET @strInput = REPLACE(@strInput,' ','-') 
	RETURN @strInput 
END 


DROP FUNCTION [dbo].[fuConvertToUnsign1] 
