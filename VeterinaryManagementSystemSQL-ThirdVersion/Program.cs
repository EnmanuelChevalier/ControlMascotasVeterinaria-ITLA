using System.Globalization;
using Microsoft.Data.SqlClient;
string connectionString = @"Server=.\SQLEXPRESS;Database=VeterinaryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;";

try
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        Console.WriteLine("CONEXION EXITOSA CON SQL SERVER.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR DE CONEXION: {ex.Message}");
}

// Main menu
int option = 0;


// Main menu
try
{
    while (option != 4)
    {
        Console.WriteLine("\n=== SISTEMA DE CONTROL DE MASCOTAS ===");
        Console.WriteLine("1. GESTION DE MASCOTAS");
        Console.WriteLine("2. GESTION DE PROPIETARIOS");
        Console.WriteLine("3. GESTION DE CONSULTAS");
        Console.WriteLine("4. SALIR");

        Console.WriteLine("SELECCIONA UNA OPCION:");

        while (!int.TryParse(Console.ReadLine(), out option) ||
               option < 1 || option > 4)
        {
            Console.WriteLine("DATO INVALIDO, INGRESA UN DATO VALIDO:");
        }

        switch (option)
        {
            case 1:
                ShowPetsMenu();
                break;

            case 2:
                ShowOwnersMenu();
                break;

            case 3:
                ShowConsultationsMenu();
                break;

            case 4:
                Console.WriteLine("GRACIAS POR UTILIZAR EL SISTEMA");
                break;
        }
    }
}
catch (Exception ex)
{
    // Handle unexpected errors
    Console.WriteLine("OCURRIO UN ERROR INESPERADO.");
    Console.WriteLine($"ERROR: {ex.Message}");
}

// Ask if the user wants to continue
bool AskToContinue(string message)
{
    while (true)
    {
        Console.WriteLine(message);
        Console.WriteLine("1. SI");
        Console.WriteLine("2. NO");

        string answer = Console.ReadLine()!;

        if (answer == "1")
            return true;

        if (answer == "2")
            return false;

        Console.WriteLine("OPCION INVALIDA, INTENTE DE NUEVO.");
    }
}


// Pet management menu
void ShowPetsMenu()
{
    int petOption = 0;

    while (petOption != 6)
    {
        Console.WriteLine("\n=== MENU DE MASCOTAS ===");
        Console.WriteLine("1. REGISTRAR MASCOTA");
        Console.WriteLine("2. MOSTRAR MASCOTAS");
        Console.WriteLine("3. BUSCAR MASCOTA");
        Console.WriteLine("4. MODIFICAR MASCOTA");
        Console.WriteLine("5. ELIMINAR MASCOTA");
        Console.WriteLine("6. VOLVER");

        Console.WriteLine("SELECCIONA UNA OPCION:");

        while (!int.TryParse(Console.ReadLine(), out petOption) ||
               petOption < 1 || petOption > 6)
        {
            Console.WriteLine("OPCION INVALIDA, INTENTE DE NUEVO:");
        }

        switch (petOption)
        {
            case 1:
                do
                {
                    RegisterPet();
                }
                while (AskToContinue("¿DESEA REGISTRAR OTRA MASCOTA?"));
                break;

            case 2:
                ShowPets();
                break;

            case 3:
                do
                {
                    SearchPet();
                }
                while (AskToContinue("¿DESEA BUSCAR OTRA MASCOTA?"));
                break;

            case 4:
                do
                {
                    UpdatePet();
                }
                while (AskToContinue("¿DESEA MODIFICAR OTRA MASCOTA?"));
                break;

            case 5:
                do
                {
                    DeletePet();
                }
                while (AskToContinue("¿DESEA ELIMINAR OTRA MASCOTA?"));
                break;
        }
    }
}


// Register a new pet
void RegisterPet()
{
    Console.WriteLine("\n=== REGISTRO DE MASCOTAS ===");

    // Show registered owners
    ShowOwners();

    Console.WriteLine("INGRESE EL ID DEL PROPIETARIO:");

    int ownerId;

    while (!int.TryParse(Console.ReadLine(), out ownerId) || ownerId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    // Check if owner exists
    int ownerExists;
    string checkQuery = "SELECT COUNT(*) FROM Owners WHERE OwnerId = @OwnerId";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(checkQuery, connection))
        {
            command.Parameters.AddWithValue("@OwnerId", ownerId);
            ownerExists = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (ownerExists == 0)
    {
        Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
        return;
    }

    // Get pet information
    Console.WriteLine("INGRESE EL NOMBRE DE LA MASCOTA:");
    string name = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("EL NOMBRE NO PUEDE ESTAR VACIO:");
        name = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE LA ESPECIE:");
    string species = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(species))
    {
        Console.WriteLine("LA ESPECIE NO PUEDE ESTAR VACIA:");
        species = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE LA RAZA:");
    string breed = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(breed))
    {
        Console.WriteLine("LA RAZA NO PUEDE ESTAR VACIA:");
        breed = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE LA EDAD:");
    int age;

    while (!int.TryParse(Console.ReadLine(), out age) || age < 0)
    {
        Console.WriteLine("EDAD INVALIDA. INTENTE DE NUEVO:");
    }

    // Insert pet into database
    string insertQuery = "INSERT INTO Pets (Name, Species, Breed, Age, OwnerId) " +
                         "VALUES (@Name, @Species, @Breed, @Age, @OwnerId)";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(insertQuery, connection))
        {
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Species", species);
            command.Parameters.AddWithValue("@Breed", breed);
            command.Parameters.AddWithValue("@Age", age);
            command.Parameters.AddWithValue("@OwnerId", ownerId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("MASCOTA REGISTRADA CORRECTAMENTE.");
            }
            else
            {
                Console.WriteLine("NO SE PUDO REGISTRAR LA MASCOTA.");
            }
        }
    }
}


// Show all registered pets
void ShowPets()
{
    Console.WriteLine("\n=== LISTA DE MASCOTAS ===");

    string query =
        "SELECT Pets.PetId, Pets.Name, Pets.Species, Pets.Breed, " +
        "Pets.Age, Owners.Name " +
        "FROM Pets " +
        "INNER JOIN Owners ON Pets.OwnerId = Owners.OwnerId";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
                    return;
                }

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string species = reader.GetString(2);
                    string breed = reader.GetString(3);
                    int age = reader.GetInt32(4);
                    string ownerName = reader.GetString(5);

                    Console.WriteLine("-------------------------");
                    Console.WriteLine($"ID: {id}");
                    Console.WriteLine($"NOMBRE: {name}");
                    Console.WriteLine($"ESPECIE: {species}");
                    Console.WriteLine($"RAZA: {breed}");
                    Console.WriteLine($"EDAD: {age}");
                    Console.WriteLine($"PROPIETARIO: {ownerName}");
                    Console.WriteLine("-------------------------");
                }
            }
        }
    }
}


// Search a pet by name
void SearchPet()
{
    Console.WriteLine("\n=== BUSCAR MASCOTA ===");

    Console.WriteLine("INGRESE EL NOMBRE DE LA MASCOTA:");
    string searchName = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(searchName))
    {
        Console.WriteLine("EL NOMBRE NO PUEDE ESTAR VACIO:");
        searchName = Console.ReadLine()!;
    }

    string query =
        "SELECT Pets.PetId, Pets.Name, Pets.Species, Pets.Breed, " +
        "Pets.Age, Owners.Name " +
        "FROM Pets " +
        "INNER JOIN Owners ON Pets.OwnerId = Owners.OwnerId " +
        "WHERE Pets.Name = @Name";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Name", searchName);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
                    return;
                }

                while (reader.Read())
                {
                    Console.WriteLine("-------------------------");
                    Console.WriteLine($"ID: {reader.GetInt32(0)}");
                    Console.WriteLine($"NOMBRE: {reader.GetString(1)}");
                    Console.WriteLine($"ESPECIE: {reader.GetString(2)}");
                    Console.WriteLine($"RAZA: {reader.GetString(3)}");
                    Console.WriteLine($"EDAD: {reader.GetInt32(4)}");
                    Console.WriteLine($"PROPIETARIO: {reader.GetString(5)}");
                    Console.WriteLine("-------------------------");
                }
            }
        }
    }
}


// Update an existing pet
void UpdatePet()
{
    Console.WriteLine("\n=== MODIFICAR MASCOTA ===");

    // Show registered pets
    ShowPets();

    Console.WriteLine("INGRESE EL ID DE LA MASCOTA:");
    int petId;

    while (!int.TryParse(Console.ReadLine(), out petId) || petId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    // Check if pet exists
    string checkQuery = "SELECT COUNT(*) FROM Pets WHERE PetId = @PetId";
    int petExists;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(checkQuery, connection))
        {
            command.Parameters.AddWithValue("@PetId", petId);
            petExists = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (petExists == 0)
    {
        Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
        return;
    }

    // Get new pet information
    Console.WriteLine("INGRESE EL NUEVO NOMBRE:");
    string newName = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(newName))
    {
        Console.WriteLine("EL NOMBRE NO PUEDE ESTAR VACIO:");
        newName = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE LA NUEVA ESPECIE:");
    string newSpecies = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(newSpecies))
    {
        Console.WriteLine("LA ESPECIE NO PUEDE ESTAR VACIA:");
        newSpecies = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE LA NUEVA RAZA:");
    string newBreed = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(newBreed))
    {
        Console.WriteLine("LA RAZA NO PUEDE ESTAR VACIA:");
        newBreed = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE LA NUEVA EDAD:");
    int newAge;

    while (!int.TryParse(Console.ReadLine(), out newAge) || newAge < 0)
    {
        Console.WriteLine("EDAD INVALIDA. INTENTE DE NUEVO:");
    }

    ShowOwners();

    Console.WriteLine("INGRESE EL NUEVO ID DEL PROPIETARIO:");
    int newOwnerId;

    while (!int.TryParse(Console.ReadLine(), out newOwnerId) || newOwnerId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    // Check if owner exists
    string ownerQuery = "SELECT COUNT(*) FROM Owners WHERE OwnerId = @OwnerId";
    int ownerExists;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(ownerQuery, connection))
        {
            command.Parameters.AddWithValue("@OwnerId", newOwnerId);
            ownerExists = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (ownerExists == 0)
    {
        Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
        return;
    }

    // Update pet in database
    string updateQuery =
        "UPDATE Pets SET Name = @Name, Species = @Species, " +
        "Breed = @Breed, Age = @Age, OwnerId = @OwnerId " +
        "WHERE PetId = @PetId";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(updateQuery, connection))
        {
            command.Parameters.AddWithValue("@Name", newName);
            command.Parameters.AddWithValue("@Species", newSpecies);
            command.Parameters.AddWithValue("@Breed", newBreed);
            command.Parameters.AddWithValue("@Age", newAge);
            command.Parameters.AddWithValue("@OwnerId", newOwnerId);
            command.Parameters.AddWithValue("@PetId", petId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("MASCOTA MODIFICADA CORRECTAMENTE.");
            }
            else
            {
                Console.WriteLine("NO SE PUDO MODIFICAR LA MASCOTA.");
            }
        }
    }
}


// Delete a pet without consultations
void DeletePet()
{
    Console.WriteLine("\n=== ELIMINAR MASCOTA ===");

    // Check if pets exist
    string checkPetsQuery = "SELECT COUNT(*) FROM Pets";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(checkPetsQuery, connection))
        {
            int petCount = Convert.ToInt32(command.ExecuteScalar());

            if (petCount == 0)
            {
                Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
                return;
            }
        }
    }

    ShowPets();

    Console.WriteLine("INGRESE EL ID DE LA MASCOTA:");
    int petId;

    while (!int.TryParse(Console.ReadLine(), out petId) || petId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    // Check if pet exists
    string checkPetQuery = "SELECT COUNT(*) FROM Pets WHERE PetId = @PetId";
    int petExists;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(checkPetQuery, connection))
        {
            command.Parameters.AddWithValue("@PetId", petId);
            petExists = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (petExists == 0)
    {
        Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
        return;
    }

    // Check if pet has consultations
    string checkConsultationsQuery =
        "SELECT COUNT(*) FROM Consultations WHERE PetId = @PetId";

    int consultationCount;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(checkConsultationsQuery, connection))
        {
            command.Parameters.AddWithValue("@PetId", petId);
            consultationCount = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (consultationCount > 0)
    {
        Console.WriteLine("NO SE PUEDE ELIMINAR, TIENE CONSULTAS REGISTRADAS.");
        return;
    }

    // Delete pet from database
    string deleteQuery = "DELETE FROM Pets WHERE PetId = @PetId";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
        {
            command.Parameters.AddWithValue("@PetId", petId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("MASCOTA ELIMINADA CORRECTAMENTE.");
            }
            else
            {
                Console.WriteLine("NO SE PUDO ELIMINAR LA MASCOTA.");
            }
        }
    }
}

// Owner management menu
void ShowOwnersMenu()
{
    int ownerOption = 0;

    while (ownerOption != 6)
    {
        Console.WriteLine("\n=== MENU DE PROPIETARIOS ===");
        Console.WriteLine("1. REGISTRAR PROPIETARIO");
        Console.WriteLine("2. MOSTRAR PROPIETARIOS");
        Console.WriteLine("3. BUSCAR PROPIETARIO");
        Console.WriteLine("4. MODIFICAR PROPIETARIO");
        Console.WriteLine("5. ELIMINAR PROPIETARIO");
        Console.WriteLine("6. VOLVER");

        Console.WriteLine("SELECCIONA UNA OPCION:");

        while (!int.TryParse(Console.ReadLine(), out ownerOption) ||
               ownerOption < 1 || ownerOption > 6)
        {
            Console.WriteLine("OPCION INVALIDA, INTENTE DE NUEVO:");
        }

        switch (ownerOption)
        {
            case 1:
                do
                {
                    RegisterOwner();
                }
                while (AskToContinue("¿DESEA REGISTRAR OTRO PROPIETARIO?"));
                break;

            case 2:
                ShowOwners();
                break;

            case 3:
                do
                {
                    SearchOwner();
                }
                while (AskToContinue("¿DESEA BUSCAR OTRO PROPIETARIO?"));
                break;

            case 4:
                do
                {
                    UpdateOwner();
                }
                while (AskToContinue("¿DESEA MODIFICAR OTRO PROPIETARIO?"));
                break;

            case 5:
                do
                {
                    DeleteOwner();
                }
                while (AskToContinue("¿DESEA ELIMINAR OTRO PROPIETARIO?"));
                break;
        }
    }
}


// Register a new owner
void RegisterOwner()
{
    Console.WriteLine("\n=== REGISTRO DE PROPIETARIOS ===");

    Console.WriteLine("INGRESE EL NOMBRE DEL PROPIETARIO:");
    string name = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("EL NOMBRE NO PUEDE ESTAR VACIO:");
        name = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE EL TELEFONO:");
    string phone = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(phone))
    {
        Console.WriteLine("EL TELEFONO NO PUEDE ESTAR VACIO:");
        phone = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE LA DIRECCION:");
    string address = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(address))
    {
        Console.WriteLine("LA DIRECCION NO PUEDE ESTAR VACIA:");
        address = Console.ReadLine()!;
    }

    using (SqlConnection conection = new SqlConnection(connectionString))
    {


        string query = "INSERT INTO Owners (Name, Phone, Address) VALUES (@Name, @Phone, @Address)";

        conection.Open();


        using (SqlCommand command = new SqlCommand(query, conection))
        {
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Address", address);

            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine($"FILAS INSERTADAS: {rowsAffected}");
        }
    }

    Console.WriteLine("PROPIETARIO REGISTRADO CORRECTAMENTE.");
}
// Display all registered owners
void ShowOwners()
{
    Console.WriteLine("\n=== LISTA DE PROPIETARIOS ===");







    string query = "SELECT OwnerId, Name, Phone, Address FROM Owners";

    using (SqlConnection conection = new SqlConnection(connectionString))
    {

        conection.Open();


        using (SqlCommand command = new SqlCommand(query, conection))
        {


            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
                    return;
                }


                while (reader.Read())
                {
                    int id = reader.GetInt32(0);  // OwnerId
                    string name = reader.GetString(1); // Name
                    string phone = reader.GetString(2); // Phone
                    string address = reader.GetString(3); // Address

                    Console.WriteLine($"ID: {id}");
                    Console.WriteLine($"NAME: {name}");
                    Console.WriteLine($"PHONE: {phone}");
                    Console.WriteLine($"ADDRESS: {address}");
                }

            }


        }
    }

}


// Search an owner by ID or name
void SearchOwner()
{
    Console.WriteLine("\n=== BUSCAR PROPIETARIO ===");


    Console.WriteLine("1. BUSCAR POR ID");
    Console.WriteLine("2. BUSCAR POR NOMBRE");

    int searchOption;

    while (!int.TryParse(Console.ReadLine(), out searchOption) ||
           searchOption < 1 || searchOption > 2)
    {
        Console.WriteLine("OPCION INVALIDA. INTENTE DE NUEVO:");
    }

    string query;
    int ownerId = 0;
    string searchName = "";

    if (searchOption == 1)
    {
        Console.WriteLine("INGRESE EL ID DEL PROPIETARIO:");


        while (!int.TryParse(Console.ReadLine(), out ownerId) || ownerId <= 0)
        {
            Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
        }

        query = "SELECT OwnerId, Name, Phone, Address FROM Owners WHERE OwnerId = @OwnerId";
    }
    else
    {
        Console.WriteLine("INGRESE EL NOMBRE DEL PROPIETARIO:");
        searchName = Console.ReadLine()!;


        query = "SELECT OwnerId, Name, Phone, Address FROM Owners WHERE Name = @Name";
    }

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        using (SqlCommand command = new SqlCommand(query, connection))
        {

            if (searchOption == 1)
            {
                command.Parameters.AddWithValue("@OwnerId", ownerId);
            }
            else
            {
                command.Parameters.AddWithValue("@Name", searchName);
            }

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
                    return;
                }


                while (reader.Read())
                {
                    int id = reader.GetInt32(0);  // OwnerId
                    string name = reader.GetString(1); // Name
                    string phone = reader.GetString(2); // Phone
                    string address = reader.GetString(3); // Address

                    Console.WriteLine($"ID: {id}");
                    Console.WriteLine($"NAME: {name}");
                    Console.WriteLine($"PHONE: {phone}");
                    Console.WriteLine($"ADDRESS: {address}");
                }
            }
        }


    }

}


// Update an existing owner
void UpdateOwner()
{
    Console.WriteLine("\n=== MODIFICAR PROPIETARIO ===");

    ShowOwners();

    Console.WriteLine("INGRESE EL ID DEL PROPIETARIO:");
    int ownerId;

    while (!int.TryParse(Console.ReadLine(), out ownerId) || ownerId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    // Check if owner exists
    int ownerExists;

    string checkQuery = "SELECT COUNT(*) FROM Owners WHERE OwnerId = @OwnerId";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(checkQuery, connection))
        {
            command.Parameters.AddWithValue("@OwnerId", ownerId);
            ownerExists = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (ownerExists == 0)
    {
        Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
        return;
    }

    string newName = string.Empty;
    string newPhone = string.Empty;
    string newAddress = string.Empty;

    Console.WriteLine("INGRESE EL NUEVO NOMBRE:");
    newName = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(newName))
    {
        Console.WriteLine("EL NOMBRE NO PUEDE ESTAR VACIO:");
        newName = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE EL NUEVO TELEFONO:");
    newPhone = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(newPhone))
    {
        Console.WriteLine("EL TELEFONO NO PUEDE ESTAR VACIO:");
        newPhone = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE LA NUEVA DIRECCION:");
    newAddress = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(newAddress))
    {
        Console.WriteLine("LA DIRECCION NO PUEDE ESTAR VACIA:");
        newAddress = Console.ReadLine()!;
    }

    // Update owner in database
    string query = "UPDATE Owners " +
                   "SET Name = @Name, " +
                   "Phone = @Phone, " +
                   "Address = @Address " +
                   "WHERE OwnerId = @OwnerId";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Name", newName);
            command.Parameters.AddWithValue("@Phone", newPhone);
            command.Parameters.AddWithValue("@Address", newAddress);
            command.Parameters.AddWithValue("@OwnerId", ownerId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected != 0)
            {
                Console.WriteLine("PROPIETARIO MODIFICADO CORRECTAMENTE.");
            }
            else
            {
                Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
            }
        }
    }
}


// Delete an owner without associated pets
void DeleteOwner()
{
    Console.WriteLine("\n=== ELIMINAR PROPIETARIO ===");

    string countQuery = "SELECT COUNT(*) FROM Owners";
    int ownerCount;

    // Check if there are registered owners
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(countQuery, connection))
        {
            ownerCount = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (ownerCount == 0)
    {
        Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
        return;
    }

    ShowOwners();

    Console.WriteLine("INGRESE EL ID DEL PROPIETARIO:");
    int ownerId;

    while (!int.TryParse(Console.ReadLine(), out ownerId) || ownerId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    // Check if owner exists
    string checkQuery = "SELECT COUNT(*) FROM Owners WHERE OwnerId = @OwnerId";
    int ownerExists;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(checkQuery, connection))
        {
            command.Parameters.AddWithValue("@OwnerId", ownerId);
            ownerExists = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (ownerExists == 0)
    {
        Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
        return;
    }

    // Check if owner has associated pets
    string petsQuery = "SELECT COUNT(*) FROM Pets WHERE OwnerId = @OwnerId";
    int hasPets;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(petsQuery, connection))
        {
            command.Parameters.AddWithValue("@OwnerId", ownerId);
            hasPets = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (hasPets > 0)
    {
        Console.WriteLine("NO SE PUEDE ELIMINAR, TODAVIA TIENE MASCOTAS ASOCIADAS.");
        return;
    }

    // Delete owner from database
    string deleteQuery = "DELETE FROM Owners WHERE OwnerId = @OwnerId";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
        {
            command.Parameters.AddWithValue("@OwnerId", ownerId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("PROPIETARIO ELIMINADO CORRECTAMENTE.");
            }
            else
            {
                Console.WriteLine("NO SE PUDO ELIMINAR EL PROPIETARIO.");
            }
        }
    }
}

// Consultation management menu
void ShowConsultationsMenu()
{
    int consultationOption = 0;

    while (consultationOption != 6)
    {
        Console.WriteLine("\n=== MENU DE CONSULTAS ===");
        Console.WriteLine("1. REGISTRAR CONSULTA");
        Console.WriteLine("2. MOSTRAR CONSULTAS");
        Console.WriteLine("3. BUSCAR CONSULTA");
        Console.WriteLine("4. MODIFICAR CONSULTA");
        Console.WriteLine("5. ELIMINAR CONSULTA");
        Console.WriteLine("6. VOLVER");

        Console.WriteLine("SELECCIONA UNA OPCION:");

        while (!int.TryParse(Console.ReadLine(), out consultationOption) ||
               consultationOption < 1 || consultationOption > 6)
        {
            Console.WriteLine("OPCION INVALIDA, INTENTE DE NUEVO:");
        }

        switch (consultationOption)
        {
            case 1:
                do
                {
                    RegisterConsultation();
                }
                while (AskToContinue("¿DESEA REGISTRAR OTRA CONSULTA?"));
                break;

            case 2:
                ShowConsultations();
                break;

            case 3:
                do
                {
                    SearchConsultation();
                }
                while (AskToContinue("¿DESEA BUSCAR OTRA CONSULTA?"));
                break;

            case 4:
                do
                {
                    UpdateConsultation();
                }
                while (AskToContinue("¿DESEA MODIFICAR OTRA CONSULTA?"));
                break;

            case 5:
                do
                {
                    DeleteConsultation();
                }
                while (AskToContinue("¿DESEA ELIMINAR OTRA CONSULTA?"));
                break;
        }
    }
}


// Register a new consultation
void RegisterConsultation()
{
    Console.WriteLine("\n=== REGISTRO DE CONSULTA ===");

    // Show registered pets
    ShowPets();

    Console.WriteLine("INGRESE EL ID DE LA MASCOTA:");
    int petId;

    while (!int.TryParse(Console.ReadLine(), out petId) || petId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    // Check if pet exists
    string checkQuery = "SELECT COUNT(*) FROM Pets WHERE PetId = @PetId";
    int petExists;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(checkQuery, connection))
        {
            command.Parameters.AddWithValue("@PetId", petId);
            petExists = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (petExists == 0)
    {
        Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
        return;
    }

    // Get consultation information
    Console.WriteLine("INGRESE EL MOTIVO DE LA CONSULTA:");
    string reason = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(reason))
    {
        Console.WriteLine("EL MOTIVO NO PUEDE ESTAR VACIO:");
        reason = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE EL DIAGNOSTICO:");
    string diagnosis = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(diagnosis))
    {
        Console.WriteLine("EL DIAGNOSTICO NO PUEDE ESTAR VACIO:");
        diagnosis = Console.ReadLine()!;
    }

    // Insert consultation into database
    string insertQuery =
        "INSERT INTO Consultations (PetId, Date, Reason, Diagnosis) " +
        "VALUES (@PetId, @Date, @Reason, @Diagnosis)";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(insertQuery, connection))
        {
            command.Parameters.AddWithValue("@PetId", petId);
            command.Parameters.AddWithValue("@Date", DateTime.Now);
            command.Parameters.AddWithValue("@Reason", reason);
            command.Parameters.AddWithValue("@Diagnosis", diagnosis);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("CONSULTA REGISTRADA CORRECTAMENTE.");
            }
            else
            {
                Console.WriteLine("NO SE PUDO REGISTRAR LA CONSULTA.");
            }
        }
    }
}


// Display all registered consultations
void ShowConsultations()
{
    Console.WriteLine("\n=== LISTA DE CONSULTAS ===");

    string query =
        "SELECT Consultations.ConsultationId, Consultations.PetId, " +
        "Pets.Name, Consultations.Date, Consultations.Reason, " +
        "Consultations.Diagnosis " +
        "FROM Consultations " +
        "INNER JOIN Pets ON Consultations.PetId = Pets.PetId";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("NO HAY CONSULTAS REGISTRADAS.");
                    return;
                }

                while (reader.Read())
                {
                    Console.WriteLine("-------------------------");
                    Console.WriteLine($"ID: {reader.GetInt32(0)}");
                    Console.WriteLine($"ID DE MASCOTA: {reader.GetInt32(1)}");
                    Console.WriteLine($"MASCOTA: {reader.GetString(2)}");
                    Console.WriteLine($"FECHA: {reader.GetDateTime(3).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.GetCultureInfo("en-US"))}");
                    Console.WriteLine($"MOTIVO: {reader.GetString(4)}");
                    Console.WriteLine($"DIAGNOSTICO: {reader.GetString(5)}");
                    Console.WriteLine("-------------------------");
                }
            }
        }
    }
}


// Search a consultation by ID
void SearchConsultation()
{
    Console.WriteLine("\n=== BUSCAR CONSULTA ===");

    Console.WriteLine("INGRESE EL ID DE LA CONSULTA:");
    int consultationId;

    while (!int.TryParse(Console.ReadLine(), out consultationId) ||
           consultationId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    string query =
        "SELECT Consultations.ConsultationId, Consultations.PetId, " +
        "Pets.Name, Consultations.Date, Consultations.Reason, " +
        "Consultations.Diagnosis " +
        "FROM Consultations " +
        "INNER JOIN Pets ON Consultations.PetId = Pets.PetId " +
        "WHERE Consultations.ConsultationId = @ConsultationId";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@ConsultationId", consultationId);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    Console.WriteLine("NO SE ENCONTRO LA CONSULTA.");
                    return;
                }

                Console.WriteLine("-------------------------");
                Console.WriteLine($"ID: {reader.GetInt32(0)}");
                Console.WriteLine($"ID DE MASCOTA: {reader.GetInt32(1)}");
                Console.WriteLine($"MASCOTA: {reader.GetString(2)}");
                Console.WriteLine($"FECHA: {reader.GetDateTime(3)}");
                Console.WriteLine($"MOTIVO: {reader.GetString(4)}");
                Console.WriteLine($"DIAGNOSTICO: {reader.GetString(5)}");
                Console.WriteLine("-------------------------");
            }
        }
    }
}


// Update an existing consultation
void UpdateConsultation()
{
    Console.WriteLine("\n=== MODIFICAR CONSULTA ===");

    // Show registered consultations
    ShowConsultations();

    Console.WriteLine("INGRESE EL ID DE LA CONSULTA:");
    int consultationId;

    while (!int.TryParse(Console.ReadLine(), out consultationId) ||
           consultationId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    // Check if consultation exists
    string checkQuery =
        "SELECT COUNT(*) FROM Consultations " +
        "WHERE ConsultationId = @ConsultationId";

    int consultationExists;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(checkQuery, connection))
        {
            command.Parameters.AddWithValue("@ConsultationId", consultationId);
            consultationExists = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (consultationExists == 0)
    {
        Console.WriteLine("NO SE ENCONTRO LA CONSULTA.");
        return;
    }

    // Get new consultation information
    Console.WriteLine("INGRESE EL NUEVO MOTIVO:");
    string newReason = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(newReason))
    {
        Console.WriteLine("EL MOTIVO NO PUEDE ESTAR VACIO:");
        newReason = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE EL NUEVO DIAGNOSTICO:");
    string newDiagnosis = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(newDiagnosis))
    {
        Console.WriteLine("EL DIAGNOSTICO NO PUEDE ESTAR VACIO:");
        newDiagnosis = Console.ReadLine()!;
    }

    // Update consultation in database
    string updateQuery =
        "UPDATE Consultations SET Reason = @Reason, " +
        "Diagnosis = @Diagnosis " +
        "WHERE ConsultationId = @ConsultationId";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(updateQuery, connection))
        {
            command.Parameters.AddWithValue("@Reason", newReason);
            command.Parameters.AddWithValue("@Diagnosis", newDiagnosis);
            command.Parameters.AddWithValue("@ConsultationId", consultationId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("CONSULTA MODIFICADA CORRECTAMENTE.");
            }
            else
            {
                Console.WriteLine("NO SE PUDO MODIFICAR LA CONSULTA.");
            }
        }
    }
}


// Delete an existing consultation
void DeleteConsultation()
{
    Console.WriteLine("\n=== ELIMINAR CONSULTA ===");

    // Show registered consultations
    ShowConsultations();

    Console.WriteLine("INGRESE EL ID DE LA CONSULTA:");
    int consultationId;

    while (!int.TryParse(Console.ReadLine(), out consultationId) ||
           consultationId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    // Check if consultation exists
    string checkQuery =
        "SELECT COUNT(*) FROM Consultations " +
        "WHERE ConsultationId = @ConsultationId";

    int consultationExists;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(checkQuery, connection))
        {
            command.Parameters.AddWithValue("@ConsultationId", consultationId);
            consultationExists = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    if (consultationExists == 0)
    {
        Console.WriteLine("NO SE ENCONTRO LA CONSULTA.");
        return;
    }

    // Delete consultation from database
    string deleteQuery =
        "DELETE FROM Consultations WHERE ConsultationId = @ConsultationId";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
        {
            command.Parameters.AddWithValue("@ConsultationId", consultationId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("CONSULTA ELIMINADA CORRECTAMENTE.");
            }
            else
            {
                Console.WriteLine("NO SE PUDO ELIMINAR LA CONSULTA.");
            }
        }
    }
}
