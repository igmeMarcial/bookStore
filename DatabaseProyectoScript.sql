
CREATE TABLE usuario (
  idUsuario SERIAL PRIMARY KEY,
  correo VARCHAR(100) UNIQUE,
  contraseña VARCHAR(500)
);

-- Crear la tabla book
CREATE TABLE book (
  id SERIAL PRIMARY KEY,
  name VARCHAR(255),
  author VARCHAR(255),
  price DECIMAL(10,2),
  image_url VARCHAR(500)
);

-- Insertar datos de ejemplo en la tabla book
INSERT INTO book (name, author, price, image_url) VALUES
  ('Libro 1', 'Autor 1', 19.99, 'https://www.w3schools.com/w3images/fjords.jpg'),
  ('Libro 2', 'Autor 2', 29.99, 'https://www.w3schools.com/w3images/fjords.jpg'),
  ('Libro 3', 'Autor 3', 39.99, 'https://www.w3schools.com/w3images/fjords.jpg'),
  ('The Great Gatsby', 'F. Scott Fitzgerald', 12.99, 'https://www.w3schools.com/w3images/fjords.jpg'),
  ('To Kill a Mockingbird', 'Harper Lee', 10.99, 'https://www.w3schools.com/w3images/fjords.jpg'),
  ('1984', 'George Orwell', 9.99, 'https://www.w3schools.com/w3images/fjords.jpg'),
  ('Pride and Prejudice', 'Jane Austen', 8.99, 'https://www.w3schools.com/w3images/fjords.jpg'),
  ('The Catcher in the Rye', 'J.D. Salinger', 11.99, 'https://www.w3schools.com/w3images/fjords.jpg');

-- Procedimiento para insertar un libro
CREATE OR REPLACE FUNCTION sp_InsertBook(
    Name VARCHAR,
    Author VARCHAR,
    Price DECIMAL,
    ImageUrl VARCHAR
)
RETURNS VOID AS
$$
BEGIN
    INSERT INTO book (name, author, price, image_url)
    VALUES (Name, Author, Price, ImageUrl);
END;
$$ LANGUAGE plpgsql;

-- Procedimiento para obtener todos los libros
CREATE OR REPLACE FUNCTION sp_GetAllBooks()
RETURNS TABLE(
    id INT,
    name VARCHAR,
    author VARCHAR,
    price DECIMAL,
    image_url VARCHAR
) AS
$$
BEGIN
    RETURN QUERY SELECT book.id, book.name, book.author, book.price, book.image_url FROM book;
END;
$$ LANGUAGE plpgsql;

-- Procedimiento para actualizar un libro
CREATE OR REPLACE FUNCTION sp_UpdateBook(
    Id INT,
    Name VARCHAR,
    Author VARCHAR,
    Price DECIMAL,
    ImageUrl VARCHAR
)
RETURNS VOID AS
$$
BEGIN
    UPDATE book
    SET name = Name,
        author = Author,
        price = Price,
        image_url = ImageUrl
    WHERE id = Id;
END;
$$ LANGUAGE plpgsql;

-- Procedimiento para eliminar un libro
CREATE OR REPLACE FUNCTION sp_DeleteBook(Id INT)
RETURNS VOID AS
$$
BEGIN
    DELETE FROM book WHERE id = Id;
END;
$$ LANGUAGE plpgsql;

-- Procedimiento para obtener un libro por ID
CREATE OR REPLACE FUNCTION sp_GetBookById(BookId INT)
RETURNS TABLE(
    id INT,
    name VARCHAR,
    author VARCHAR,
    price DECIMAL,
    image_url VARCHAR
) AS
$$
BEGIN
    RETURN QUERY 
    SELECT b.id, b.name, b.author, b.price, b.image_url
    FROM book b
    WHERE b.id = BookId;
END;
$$ LANGUAGE plpgsql;



-- Procedimiento para registrar un usuario
CREATE OR REPLACE FUNCTION sp_RegistrarUsuario(
    correo VARCHAR,
    contraseña VARCHAR,
    OUT Registrado BOOLEAN,
    OUT Mensaje VARCHAR
)
AS
$$
BEGIN
    IF EXISTS (SELECT 1 FROM usuario WHERE correo = correo) THEN
        Registrado := FALSE;
        Mensaje := 'Correo ya existe';
    ELSE
        INSERT INTO usuario (correo, contraseña) VALUES (correo, contraseña);
        Registrado := TRUE;
        Mensaje := 'Usuario registrado';
    END IF;
END;
$$ LANGUAGE plpgsql;

-- Procedimiento para validar un usuario
CREATE OR REPLACE FUNCTION sp_ValidarUsuario(
    correo VARCHAR,
    contraseña VARCHAR
)
RETURNS INT AS
$$
BEGIN
    IF EXISTS (SELECT 1 FROM usuario WHERE correo = correo AND contraseña = contraseña) THEN
        RETURN (SELECT idUsuario FROM usuario WHERE correo = correo AND contraseña = contraseña);
    ELSE
        RETURN 0;
    END IF;
END;
$$ LANGUAGE plpgsql;