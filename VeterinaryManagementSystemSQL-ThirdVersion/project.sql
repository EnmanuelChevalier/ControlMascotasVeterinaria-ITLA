

CREATE DATABASE VeterinaryManagementDB;
Use VeterinaryManagementDB;

Create table Owners (
OwnerId INT IDENTITY(1,1) PRIMARY KEY,
Name NVARCHAR(100) NOT NULL,
Phone NVARCHAR(20) NOT NULL,
Address NVARCHAR(150) NOT NULL
);

Use VeterinaryManagementDB;
GO

Create Table Pets(
PetId INT IDENTITY(1,1) PRIMARY KEY,
Name NVARCHAR(100) NOT NULL,
Species NVARCHAR(50) NOT NULL,
Breed NVARCHAR(100) NOT NULL,
Age INT NOT NULL,
OwnerId INT NOT NULL,

FOREIGN KEY(OwnerId) REFERENCES Owners(OwnerId)
);


Use VeterinaryManagementDB;
GO


Create Table Consultations(
ConsultationId INT IDENTITY(1,1) PRIMARY KEY,
PetId INT NOT NULL,
Date DATETIME2 NOT NULL,
Reason NVARCHAR(200) NOT NULL,
Diagnosis NVARCHAR(300) NOT NULL,


FOREIGN KEY(PetId) REFERENCES Pets(PetId)
);

INSERT INTO Owners (Name, Phone, Address)
VALUES
('ENMANUEL', '809-23-3232', 'SIMON'),
('MARIA LOPEZ','809-232-2323','LUPERON');

SELECT * FROM Owners;

INSERT INTO Pets (Name, Species, Breed, Age, OwnerId)
VALUES
('Firulais', 'Perro', 'Pastor Alemán', 3, 1),
('Luna', 'Gato', 'Siamés', 2, 2),
('Toby', 'Perro', 'Golden Retriever', 4, 1);

SELECT * FROM Pets;


INSERT INTO Consultations (PetId, Date, Reason, Diagnosis)
VALUES
(1, GETDATE(), 'Dolor de estomago', 'Gastritis leve'),
(2, GETDATE(), 'Revision general', 'Mascota saludable'),
(3, GETDATE(), 'Vacunacion', 'Vacunacion completada');


SELECT * FROM Consultations;

SELECT 
Pets.Name as PETNAME,
Pets.Species as PETSPECIES,
Owners.Name as OWNERNAME,
Owners.Phone as OWNERPHONE,
Consultations.Reason as CONSULTATIONREASON,
Consultations.Diagnosis as CONSULTATIONDIAGNOSIS
FROM Pets
INNER JOIN Owners
On Pets.OwnerId = Owners.OwnerId
INNER JOIN Consultations
ON Pets.PetId = Consultations.PetId;


SELECT * FROM Owners;