
// Main lists
int option = 0;

List<Pet> pets = new();
int nextPetId = 1;

List<Owner> owners = new();
int nextOwnerId = 1;

List<Consultation> consultations = new();
int nextConsultationId = 1;

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
                    RegisterPet(pets, owners);
                }
                while (AskToContinue("¿DESEA REGISTRAR OTRA MASCOTA?"));
                break;

            case 2:
                ShowPets(pets, owners);
                break;

            case 3:
                do
                {
                    SearchPet(pets);
                }
                while (AskToContinue("¿DESEA BUSCAR OTRA MASCOTA?"));
                break;

            case 4:
                do
                {
                    UpdatePet(pets, owners);
                }
                while (AskToContinue("¿DESEA MODIFICAR OTRA MASCOTA?"));
                break;

            case 5:
                do
                {
                    DeletePet(pets, consultations);
                }
                while (AskToContinue("¿DESEA ELIMINAR OTRA MASCOTA?"));
                break;
        }
    }
}



// Register a new pet
void RegisterPet(List<Pet> pets, List<Owner> owners)
{
    if (owners.Count == 0)
    {
        Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
        return;
    }

    Console.WriteLine("\n=== REGISTRO DE MASCOTAS ===");

    ShowOwners(owners);

    Console.WriteLine("INGRESE EL ID DEL PROPIETARIO:");

    int ownerId;

    while (!int.TryParse(Console.ReadLine(), out ownerId) ||
           ownerId <= 0 ||
           !owners.Any(owner => owner.Id == ownerId))
    {
        Console.WriteLine("ID INVALIDO O INEXISTENTE. INTENTE DE NUEVO:");
    }

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

    // Create pet object
    Pet newPet = new Pet
    {
        Id = nextPetId,
        Name = name,
        Species = species,
        Breed = breed,
        Age = age,
        OwnerId = ownerId
    };

    pets.Add(newPet);
    nextPetId++;

    Console.WriteLine("MASCOTA REGISTRADA CORRECTAMENTE.");
}

// Display all registered pets
void ShowPets(List<Pet> pets, List<Owner> owners)
{
    Console.WriteLine("\n=== LISTA DE MASCOTAS ===");

    if (pets.Count == 0)
    {
        Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
        return;
    }

    foreach (Pet pet in pets)
    {
        Owner? petOwner = owners.Find(owner => owner.Id == pet.OwnerId);

        Console.WriteLine("-------------------------");
        Console.WriteLine($"ID: {pet.Id}");
        Console.WriteLine($"NOMBRE: {pet.Name}");
        Console.WriteLine($"ESPECIE: {pet.Species}");
        Console.WriteLine($"RAZA: {pet.Breed}");
        Console.WriteLine($"EDAD: {pet.Age}");
        Console.WriteLine($"PROPIETARIO: {petOwner?.Name ?? "NO ENCONTRADO"}");
        Console.WriteLine("-------------------------");
    }
}

// Search a pet by name
void SearchPet(List<Pet> pets)
{
    Console.WriteLine("\n=== BUSCAR MASCOTA ===");

    if (pets.Count == 0)
    {
        Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
        return;
    }

    Console.WriteLine("INGRESE EL NOMBRE DE LA MASCOTA:");
    string searchName = Console.ReadLine()!;

    Pet? foundPet = pets.Find(pet =>
        string.Equals(pet.Name, searchName, StringComparison.OrdinalIgnoreCase));

    if (foundPet == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
        return;
    }

    Console.WriteLine($"ID: {foundPet.Id}");
    Console.WriteLine($"NOMBRE: {foundPet.Name}");
    Console.WriteLine($"ESPECIE: {foundPet.Species}");
    Console.WriteLine($"RAZA: {foundPet.Breed}");
    Console.WriteLine($"EDAD: {foundPet.Age}");
}
// Update an existing pet
void UpdatePet(List<Pet> pets, List<Owner> owners)
{
    Console.WriteLine("\n=== MODIFICAR MASCOTA ===");

    if (pets.Count == 0)
    {
        Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
        return;
    }

    ShowPets(pets, owners);

    Console.WriteLine("INGRESE EL ID DE LA MASCOTA:");
    int petId;

    while (!int.TryParse(Console.ReadLine(), out petId) || petId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    Pet? selectedPet = pets.Find(pet => pet.Id == petId);

    if (selectedPet == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
        return;
    }

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

    ShowOwners(owners);

    Console.WriteLine("INGRESE EL NUEVO ID DEL PROPIETARIO:");
    int newOwnerId;

    while (!int.TryParse(Console.ReadLine(), out newOwnerId) ||
           newOwnerId <= 0 ||
           !owners.Any(owner => owner.Id == newOwnerId))
    {
        Console.WriteLine("ID INVALIDO O INEXISTENTE. INTENTE DE NUEVO:");
    }

    // Update pet properties
    selectedPet.Name = newName;
    selectedPet.Species = newSpecies;
    selectedPet.Breed = newBreed;
    selectedPet.Age = newAge;
    selectedPet.OwnerId = newOwnerId;

    Console.WriteLine("MASCOTA MODIFICADA CORRECTAMENTE.");
}


// Delete a pet without consultations
void DeletePet(List<Pet> pets, List<Consultation> consultations)
{
    Console.WriteLine("\n=== ELIMINAR MASCOTA ===");

    if (pets.Count == 0)
    {
        Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
        return;
    }

    Console.WriteLine("INGRESE EL ID DE LA MASCOTA:");
    int petId;

    while (!int.TryParse(Console.ReadLine(), out petId) || petId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    Pet? selectedPet = pets.Find(pet => pet.Id == petId);

    if (selectedPet == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
        return;
    }

    bool hasConsultations = consultations.Any(consultation =>
        consultation.PetId == selectedPet.Id);

    if (hasConsultations)
    {
        Console.WriteLine("NO SE PUEDE ELIMINAR, TIENE CONSULTAS REGISTRADAS.");
        return;
    }

    pets.Remove(selectedPet);

    Console.WriteLine("MASCOTA ELIMINADA CORRECTAMENTE.");
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
                    RegisterOwner(owners);
                }
                while (AskToContinue("¿DESEA REGISTRAR OTRO PROPIETARIO?"));
                break;

            case 2:
                ShowOwners(owners);
                break;

            case 3:
                do
                {
                    SearchOwner(owners);
                }
                while (AskToContinue("¿DESEA BUSCAR OTRO PROPIETARIO?"));
                break;

            case 4:
                do
                {
                    UpdateOwner(owners);
                }
                while (AskToContinue("¿DESEA MODIFICAR OTRO PROPIETARIO?"));
                break;

            case 5:
                do
                {
                    DeleteOwner(owners, pets);
                }
                while (AskToContinue("¿DESEA ELIMINAR OTRO PROPIETARIO?"));
                break;
        }
    }
}


// Register a new owner
void RegisterOwner(List<Owner> owners)
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

    // Create owner object
    Owner newOwner = new Owner
    {
        Id = nextOwnerId,
        Name = name,
        Phone = phone,
        Address = address
    };

    owners.Add(newOwner);
    nextOwnerId++;

    Console.WriteLine("PROPIETARIO REGISTRADO CORRECTAMENTE.");
}
// Display all registered owners
void ShowOwners(List<Owner> owners)
{
    Console.WriteLine("\n=== LISTA DE PROPIETARIOS ===");

    if (owners.Count == 0)
    {
        Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
        return;
    }

    foreach (Owner owner in owners)
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine($"ID: {owner.Id}");
        Console.WriteLine($"NOMBRE: {owner.Name}");
        Console.WriteLine($"TELEFONO: {owner.Phone}");
        Console.WriteLine($"DIRECCION: {owner.Address}");
        Console.WriteLine("-------------------------");
    }
}


// Search an owner by ID or name
void SearchOwner(List<Owner> owners)
{
    Console.WriteLine("\n=== BUSCAR PROPIETARIO ===");

    if (owners.Count == 0)
    {
        Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
        return;
    }

    Console.WriteLine("1. BUSCAR POR ID");
    Console.WriteLine("2. BUSCAR POR NOMBRE");

    int searchOption;

    while (!int.TryParse(Console.ReadLine(), out searchOption) ||
           searchOption < 1 || searchOption > 2)
    {
        Console.WriteLine("OPCION INVALIDA. INTENTE DE NUEVO:");
    }

    Owner? foundOwner = null;

    if (searchOption == 1)
    {
        Console.WriteLine("INGRESE EL ID DEL PROPIETARIO:");
        int ownerId;

        while (!int.TryParse(Console.ReadLine(), out ownerId) || ownerId <= 0)
        {
            Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
        }

        foundOwner = owners.Find(owner => owner.Id == ownerId);
    }
    else
    {
        Console.WriteLine("INGRESE EL NOMBRE DEL PROPIETARIO:");
        string searchName = Console.ReadLine()!;

        foundOwner = owners.Find(owner =>
            string.Equals(owner.Name, searchName, StringComparison.OrdinalIgnoreCase));
    }

    if (foundOwner == null)
    {
        Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
        return;
    }

    Console.WriteLine($"ID: {foundOwner.Id}");
    Console.WriteLine($"NOMBRE: {foundOwner.Name}");
    Console.WriteLine($"TELEFONO: {foundOwner.Phone}");
    Console.WriteLine($"DIRECCION: {foundOwner.Address}");
}


// Update an existing owner
void UpdateOwner(List<Owner> owners)
{
    Console.WriteLine("\n=== MODIFICAR PROPIETARIO ===");

    if (owners.Count == 0)
    {
        Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
        return;
    }

    ShowOwners(owners);

    Console.WriteLine("INGRESE EL ID DEL PROPIETARIO:");
    int ownerId;

    while (!int.TryParse(Console.ReadLine(), out ownerId) || ownerId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    Owner? selectedOwner = owners.Find(owner => owner.Id == ownerId);

    if (selectedOwner == null)
    {
        Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
        return;
    }

    Console.WriteLine("INGRESE EL NUEVO NOMBRE:");
    string newName = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(newName))
    {
        Console.WriteLine("EL NOMBRE NO PUEDE ESTAR VACIO:");
        newName = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE EL NUEVO TELEFONO:");
    string newPhone = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(newPhone))
    {
        Console.WriteLine("EL TELEFONO NO PUEDE ESTAR VACIO:");
        newPhone = Console.ReadLine()!;
    }

    Console.WriteLine("INGRESE LA NUEVA DIRECCION:");
    string newAddress = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(newAddress))
    {
        Console.WriteLine("LA DIRECCION NO PUEDE ESTAR VACIA:");
        newAddress = Console.ReadLine()!;
    }

    // Update owner properties
    selectedOwner.Name = newName;
    selectedOwner.Phone = newPhone;
    selectedOwner.Address = newAddress;

    Console.WriteLine("PROPIETARIO MODIFICADO CORRECTAMENTE.");
}

// Delete an owner without associated pets
void DeleteOwner(List<Owner> owners, List<Pet> pets)
{
    Console.WriteLine("\n=== ELIMINAR PROPIETARIO ===");

    if (owners.Count == 0)
    {
        Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
        return;
    }

    ShowOwners(owners);

    Console.WriteLine("INGRESE EL ID DEL PROPIETARIO:");
    int ownerId;

    while (!int.TryParse(Console.ReadLine(), out ownerId) || ownerId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    Owner? selectedOwner = owners.Find(owner => owner.Id == ownerId);

    if (selectedOwner == null)
    {
        Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
        return;
    }

    bool hasPets = pets.Any(pet => pet.OwnerId == selectedOwner.Id);

    if (hasPets)
    {
        Console.WriteLine("NO SE PUEDE ELIMINAR, TODAVIA TIENE MASCOTAS ASOCIADAS.");
        return;
    }

    owners.Remove(selectedOwner);

    Console.WriteLine("PROPIETARIO ELIMINADO CORRECTAMENTE.");
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
                    RegisterConsultation(consultations, pets, owners);
                }
                while (AskToContinue("¿DESEA REGISTRAR OTRA CONSULTA?"));
                break;

            case 2:
                ShowConsultations(consultations);
                break;

            case 3:
                do
                {
                    SearchConsultation(consultations);
                }
                while (AskToContinue("¿DESEA BUSCAR OTRA CONSULTA?"));
                break;

            case 4:
                do
                {
                    UpdateConsultation(consultations);
                }
                while (AskToContinue("¿DESEA MODIFICAR OTRA CONSULTA?"));
                break;

            case 5:
                do
                {
                    DeleteConsultation(consultations);
                }
                while (AskToContinue("¿DESEA ELIMINAR OTRA CONSULTA?"));
                break;
        }
    }
}



// Register a new consultation
void RegisterConsultation(
    List<Consultation> consultations,
    List<Pet> pets,
    List<Owner> owners)
{
    if (pets.Count == 0)
    {
        Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
        return;
    }

    Console.WriteLine("\n=== REGISTRO DE CONSULTA ===");

    ShowPets(pets, owners);

    Console.WriteLine("INGRESE EL ID DE LA MASCOTA:");

    int petId;

    while (!int.TryParse(Console.ReadLine(), out petId) || petId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    Pet? selectedPet = pets.Find(pet => pet.Id == petId);

    if (selectedPet == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
        return;
    }

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

    // Create consultation object
    Consultation newConsultation = new Consultation
    {
        Id = nextConsultationId,
        PetId = petId,
        Date = DateTime.Now,
        Reason = reason,
        Diagnosis = diagnosis
    };

    consultations.Add(newConsultation);
    nextConsultationId++;

    Console.WriteLine("CONSULTA REGISTRADA CORRECTAMENTE.");
}


// Display all registered consultations
void ShowConsultations(List<Consultation> consultations)
{
    Console.WriteLine("\n=== LISTA DE CONSULTAS ===");

    if (consultations.Count == 0)
    {
        Console.WriteLine("NO HAY CONSULTAS REGISTRADAS.");
        return;
    }

    foreach (Consultation consultation in consultations)
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine($"ID: {consultation.Id}");
        Console.WriteLine($"ID DE MASCOTA: {consultation.PetId}");
        Console.WriteLine($"FECHA: {consultation.Date}");
        Console.WriteLine($"MOTIVO: {consultation.Reason}");
        Console.WriteLine($"DIAGNOSTICO: {consultation.Diagnosis}");
        Console.WriteLine("-------------------------");
    }
}

// Search a consultation by ID
void SearchConsultation(List<Consultation> consultations)
{
    Console.WriteLine("\n=== BUSCAR CONSULTA ===");

    if (consultations.Count == 0)
    {
        Console.WriteLine("NO HAY CONSULTAS REGISTRADAS.");
        return;
    }

    Console.WriteLine("INGRESE EL ID DE LA CONSULTA:");
    int consultationId;

    while (!int.TryParse(Console.ReadLine(), out consultationId) ||
           consultationId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    Consultation? foundConsultation = consultations.Find(
        consultation => consultation.Id == consultationId);

    if (foundConsultation == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA CONSULTA.");
        return;
    }

    Console.WriteLine($"ID: {foundConsultation.Id}");
    Console.WriteLine($"ID DE MASCOTA: {foundConsultation.PetId}");
    Console.WriteLine($"FECHA: {foundConsultation.Date}");
    Console.WriteLine($"MOTIVO: {foundConsultation.Reason}");
    Console.WriteLine($"DIAGNOSTICO: {foundConsultation.Diagnosis}");
}


// Update an existing consultation
void UpdateConsultation(List<Consultation> consultations)
{
    Console.WriteLine("\n=== MODIFICAR CONSULTA ===");

    if (consultations.Count == 0)
    {
        Console.WriteLine("NO HAY CONSULTAS REGISTRADAS.");
        return;
    }

    ShowConsultations(consultations);

    Console.WriteLine("INGRESE EL ID DE LA CONSULTA:");
    int consultationId;

    while (!int.TryParse(Console.ReadLine(), out consultationId) ||
           consultationId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    Consultation? selectedConsultation = consultations.Find(
        consultation => consultation.Id == consultationId);

    if (selectedConsultation == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA CONSULTA.");
        return;
    }

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

    // Update consultation properties
    selectedConsultation.Reason = newReason;
    selectedConsultation.Diagnosis = newDiagnosis;

    Console.WriteLine("CONSULTA MODIFICADA CORRECTAMENTE.");
}


// Delete an existing consultation
void DeleteConsultation(List<Consultation> consultations)
{
    Console.WriteLine("\n=== ELIMINAR CONSULTA ===");

    if (consultations.Count == 0)
    {
        Console.WriteLine("NO HAY CONSULTAS REGISTRADAS.");
        return;
    }

    ShowConsultations(consultations);

    Console.WriteLine("INGRESE EL ID DE LA CONSULTA:");
    int consultationId;

    while (!int.TryParse(Console.ReadLine(), out consultationId) ||
           consultationId <= 0)
    {
        Console.WriteLine("ID INVALIDO. INTENTE DE NUEVO:");
    }

    Consultation? selectedConsultation = consultations.Find(
        consultation => consultation.Id == consultationId);

    if (selectedConsultation == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA CONSULTA.");
        return;
    }

    consultations.Remove(selectedConsultation);

    Console.WriteLine("CONSULTA ELIMINADA CORRECTAMENTE.");
}