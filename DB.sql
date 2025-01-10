create database eduDB
use eduDB

CREATE TABLE Persona (
    id INT IDENTITY(1,1) PRIMARY KEY, 
    nombres NVARCHAR(100) NOT NULL,
    primerApellido NVARCHAR(100) NOT NULL,
    segundoApellido NVARCHAR(100) NOT NULL,
    cedula NVARCHAR(20) NOT NULL UNIQUE,
    telefono NVARCHAR(20),
    email NVARCHAR(100),
    direccion NVARCHAR(255),
    contacto NVARCHAR(100),
    pais NVARCHAR(100)     
);

CREATE TABLE Usuario (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100) NOT NULL UNIQUE,
    contrasenia NVARCHAR(255) NOT NULL,
    rol NVARCHAR(50) NOT NULL,
    estado NVARCHAR(1) DEFAULT 'A',
    intento INT,
    idPersona INT FOREIGN KEY (idPersona) REFERENCES Persona(id)    
);




INSERT INTO Persona (nombres, primerApellido, segundoApellido, cedula, telefono, email, direccion, contacto )
VALUES ('John', 'Doe', 'Smith', '1234567890', '555-1234', 'jdoe@example.com', '123 Main St', 'Jane Doe');

INSERT INTO Persona (nombres, primerApellido, segundoApellido, cedula, telefono, email, direccion, contacto)
VALUES ('John', 'Doe', 'Smith', '1231567890', '555-1234', 'jdo1e@example.com', '123 Main St', 'Jane Doe');

INSERT INTO Usuario (user, contrasenia, rol, estado,intento, idPersona)
VALUES ('kbastidz', '1234', 'admin', 'A',0,1);

INSERT INTO Usuario (user, contrasenia, rol, estado,intento, idPersona)
VALUES ('kbastida', '1234', 'admin', 'A',0,2);
