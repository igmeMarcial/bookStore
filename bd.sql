use bookStore 


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
  ('Libro 1', 'Autor 1', 19.99, 'https://picsum.photos/200/300?random=1'),
  ('Libro 2', 'Autor 2', 29.99, 'https://picsum.photos/200/300?random=2'),
  ('Libro 3', 'Autor 3', 39.99, 'https://picsum.photos/200/300?random=3'),
  ('The Great Gatsby', 'F. Scott Fitzgerald', 12.99, 'https://picsum.photos/200/300?random=4'),
  ('To Kill a Mockingbird', 'Harper Lee', 10.99, 'https://picsum.photos/200/300?random=5'),
  ('1984', 'George Orwell', 9.99, 'https://picsum.photos/200/300?random=6'),
  ('Pride and Prejudice', 'Jane Austen', 8.99, 'https://picsum.photos/200/300?random=7'),
  ('The Catcher in the Rye', 'J.D. Salinger', 11.99, 'https://picsum.photos/200/300?random=8');

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
    _Id INT,         -- Use an underscore to distinguish parameters
    _Name VARCHAR,
    _Author VARCHAR,
    _Price DECIMAL,
    _ImageUrl VARCHAR
)
RETURNS VOID AS
$$
BEGIN
    UPDATE book
    SET name = _Name,         -- Using prefixed parameters
        author = _Author,
        price = _Price,
        image_url = _ImageUrl
    WHERE book.id = _Id;      -- Use prefixed parameter to avoid ambiguity
END;
$$ LANGUAGE plpgsql;

SELECT sp_UpdateBook(1, 'Updated Book Title', 'Updated Author', 19.99, 'https://picsum.photos/200/300');

-- Procedimiento para eliminar un libro
CREATE OR REPLACE FUNCTION sp_DeleteBook(_Id INT)  -- Use an underscore to distinguish the parameter
RETURNS VOID AS
$$
BEGIN
    DELETE FROM book WHERE book.id = _Id;  -- Use prefixed parameter to avoid ambiguity
END;
$$ LANGUAGE plpgsql;

-- Procedimiento para obtener un libro por ID
CREATE OR REPLACE FUNCTION sp_GetBookById(_BookId INT)  -- Use an underscore to distinguish the parameter
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
    WHERE b.id = _BookId;  -- Use prefixed parameter to avoid ambiguity
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


SELECT * FROM pg_proc WHERE proname = 'sp_insertbook';










