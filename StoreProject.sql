-- Crear la base de datos y seleccionarla
CREATE DATABASE bookStore;
USE bookStore;

-- Crear la tabla book
CREATE TABLE book (
  id INT PRIMARY KEY IDENTITY(1,1),
  name VARCHAR(255),
  author VARCHAR(255),
  price DECIMAL(10,2)
);

-- Crear la tabla my_books
CREATE TABLE my_books (
  id INT PRIMARY KEY IDENTITY(1,1),
  name VARCHAR(255),
  author VARCHAR(255),
  price DECIMAL(10,2)
);

-- Insertar datos en la tabla book
INSERT INTO book (name, author, price) VALUES
  ('Libro 1', 'Autor 1', 19.99),
  ('Libro 2', 'Autor 2', 29.99),
  ('Libro 3', 'Autor 3', 39.99);

  INSERT INTO book (name, author, price) VALUES
  ('The Great Gatsby', 'F. Scott Fitzgerald', 12.99),
  ('To Kill a Mockingbird', 'Harper Lee', 10.99),
  ('1984', 'George Orwell', 9.99),
  ('Pride and Prejudice', 'Jane Austen', 8.99),
  ('The Catcher in the Rye', 'J.D. Salinger', 11.99);



 CREATE PROCEDURE sp_InsertBook
    @Name VARCHAR(255),
    @Author VARCHAR(255),
    @Price DECIMAL(10, 2)
AS
BEGIN
    INSERT INTO book (Name, Author, Price)
    VALUES (@Name, @Author, @Price)
END;

CREATE PROCEDURE sp_GetAllBooks
AS
BEGIN
    SELECT * FROM book;
END;

exec sp_GetAllBooks


CREATE PROCEDURE sp_UpdateBook
    @Id INT,
    @Name VARCHAR(255),
    @Author VARCHAR(255),
    @Price DECIMAL(10, 2)
AS
BEGIN
    UPDATE book
    SET Name = @Name, Author = @Author, Price = @Price
    WHERE Id = @Id;
END;


CREATE PROCEDURE sp_DeleteBook
    @Id INT
AS
BEGIN
    DELETE FROM book WHERE Id = @Id;
END;


CREATE PROCEDURE sp_GetBookById
    @Id INT
AS
BEGIN
    SELECT * FROM book WHERE Id = @Id;
END;

-- segundo trabla miListbooks


CREATE PROCEDURE sp_InsertMyBook
    @Name VARCHAR(255),
    @Author VARCHAR(255),
    @Price DECIMAL(10, 2)
AS
BEGIN
    INSERT INTO my_books (Name, Author, Price)
    VALUES (@Name, @Author, @Price)
END;

drop procedure sp_InsertMyBook


CREATE PROCEDURE sp_GetAllMyBooks
AS
BEGIN
    SELECT * FROM my_books;
END;

exec sp_GetAllMyBooks

CREATE PROCEDURE sp_UpdateMyBook
    @Id INT,
    @Name VARCHAR(255),
    @Author VARCHAR(255),
    @Price DECIMAL(10, 2)
AS
BEGIN
    UPDATE my_books
    SET Name = @Name, Author = @Author, Price = @Price
    WHERE Id = @Id;
END;

CREATE PROCEDURE sp_DeleteMyBook
    @Id INT
AS
BEGIN
    DELETE FROM my_books WHERE Id = @Id;
END;

CREATE PROCEDURE sp_GetMyBookById
    @Id INT
AS
BEGIN
    SELECT * FROM my_books WHERE Id = @Id;
END;

-- LOGIN  NO IMPLEMENTADO

CREATE TABLE usuario (
 idUsuario int primary key identity(1,1),
 correo  varchar(100),
 contraseña varchar(500)

)
CREATE PROCEDURE sp_RegistrarUsuario(
    @correo varchar(100),
    @contraseña varchar(500),
	@Registrado bit output,
	@Mensaje varchar(100)
	)
AS
BEGIN
    -- Verificar si el correo ya está registrado
    IF EXISTS (SELECT * FROM usuario WHERE correo = @correo)
    BEGIN
        INSERT INTO usuario (correo, contraseña) VALUES (@correo, @contraseña)
		set @Registrado = 1
		set @Mensaje = 'Usario Registrado'
    END
	else
	 BEGIN
		set @Registrado = 0
		set @Mensaje = 'correo ya existe'
	 end
    
END;

CREATE PROCEDURE sp_ValidarUsuario(
    @correo varchar(100),
    @contraseña varchar(500)
 )
AS
BEGIN
    -- Verificar si el correo y la contraseña coinciden
    IF( EXISTS (SELECT * FROM usuario WHERE correo = @correo AND contraseña = @contraseña))
    
          SELECT idUsuario FROM usuario WHERE correo = @correo AND contraseña = @contraseña;
    
    ELSE
		select '0'
    
END;
