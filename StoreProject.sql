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

