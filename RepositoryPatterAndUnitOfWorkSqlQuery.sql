SELECT * FROM Users
INSERT INTO Users VALUES ('Islam','Elnagar','IslamElnagar@gmail,com')
TRUNCATE TABLE Users;

SET NOCOUNT ON

	Declare @Counter int =1

	while(@Counter<=10000)
	Begin
		DECLARE @ID UNIQUEIDENTIFIER;
		SET @ID = NEWID(); 
		Declare @FirstName nvarchar(50) = 'ABC' + RTRIM(@Counter)
		Declare @LastName  nvarchar(50) = 'xyz' + RTRIM(@Counter)
		Declare @Email	   nvarchar(50) = 'ABC' + RTRIM(@Counter)+'@gmail.com'
		
		INSERT INTO Users VALUES (@ID,@FirstName,@LastName,@Email)
		set @Counter=@Counter+1

		IF(@Counter%1000=0)
			print RTRIM(@Counter)+'rows inserted'
	End