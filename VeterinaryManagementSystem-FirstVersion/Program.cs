

// Main lists
int option = 0;

List<Dictionary<string, string>> mascotas = new List<Dictionary<string, string>>();
int nextIdMascota = 1;

List<Dictionary<string, string>> propietarios = new List<Dictionary<string, string>>();
int nextIdPropietario = 1;

List<Dictionary<string, string>> querys = new List<Dictionary<string, string>>();
int nextIdQuery = 1;

// Main menu
while (option != 4)
{
    try
    {
        Console.WriteLine("=== SISTEMA DE CONTROL DE MASCOTAS ===");
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
                MostrarMenuMascotas();
                break;

            case 2:
                MostrarMenuPropietarios();
                break;

            case 3:
                MostrarMenuQuerys();
                break;

            case 4:
                Console.WriteLine("GRACIAS POR UTILIZAR EL SISTEMA");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR INESPERADO: {ex.Message}");
        Console.WriteLine("EL SISTEMA VOLVERA AL MENU PRINCIPAL.");
    }
}

// Ask if the user wants to continue
bool AskToContinue(string message)
{
    string answer;

    while (true)
    {
        Console.WriteLine(message);
        Console.WriteLine("1. SI");
        Console.WriteLine("2. NO");

        answer = Console.ReadLine()!;

        if (answer == "1")
        {
            return true;
        }

        if (answer == "2")
        {
            return false;
        }

        Console.WriteLine("OPCION INVALIDA, INTENTE DE NUEVO.");
    }
}





void MostrarMenuMascotas()
{
    int petOption = 0;

    while (petOption != 6)
    {
        Console.WriteLine("\n=== MENU MASCOTAS ===");
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
            Console.WriteLine("OPCION INVALIDA, INTENTA DE NUEVO:");
        }

        switch (petOption)
        {
            case 1:
                do
                {
                    RegistrarMascotas(mascotas, propietarios);
                }
                while (AskToContinue("¿DESEA REGISTRAR OTRA MASCOTA?"));
                break;

            case 2:
                MostrarMascotas(mascotas);
                break;

            case 3:
                do
                {
                    BuscarMascota(mascotas);
                }
                while (AskToContinue("¿DESEA BUSCAR OTRA MASCOTA?"));
                break;

            case 4:
                do
                {
                    ModificarMascota(mascotas, propietarios);
                }
                while (AskToContinue("¿DESEA MODIFICAR OTRA MASCOTA?"));
                break;

            case 5:
                do
                {
                    EliminarMascota(mascotas, querys);
                }
                while (AskToContinue("¿DESEA ELIMINAR OTRA MASCOTA?"));
                break;
        }
    }
}




void RegistrarMascotas(
    List<Dictionary<string, string>> mascotas,
    List<Dictionary<string, string>> propietarios)
{
    // Check registered owners
    if (propietarios.Count == 0)
    {
        Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
        Console.WriteLine("REGISTRE UN PROPIETARIO PRIMERO.");
        return;
    }

    Console.WriteLine("\n=== REGISTRO DE MASCOTAS ===");

    // Show available owners
    MostrarPropietarios(propietarios);

    Console.WriteLine("INGRESE EL ID DEL PROPIETARIO:");
    int ownerId;

    while (!int.TryParse(Console.ReadLine(), out ownerId) || ownerId <= 0)
    {
        Console.WriteLine("ID INVALIDO, INTENTE DE NUEVO:");
    }

    bool ownerFound = false;

    foreach (var owner in propietarios)
    {
        if (owner["ID"] == ownerId.ToString())
        {
            ownerFound = true;
            break;
        }
    }

    if (!ownerFound)
    {
        Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
        return;
    }

    Dictionary<string, string> pet = new Dictionary<string, string>();

    // Save pet ID
    pet.Add("ID", nextIdMascota.ToString());

    Console.WriteLine("INGRESE EL NOMBRE DE LA MASCOTA:");
    string petName = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(petName))
    {
        Console.WriteLine("NOMBRE INVALIDO, INTENTE DE NUEVO:");
        petName = Console.ReadLine()!;
    }

    pet.Add("NAME", petName);

    Console.WriteLine("INGRESE LA ESPECIE:");
    string species = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(species))
    {
        Console.WriteLine("ESPECIE INVALIDA, INTENTE DE NUEVO:");
        species = Console.ReadLine()!;
    }

    pet.Add("SPECIES", species);

    Console.WriteLine("INGRESE LA RAZA:");
    string breed = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(breed))
    {
        Console.WriteLine("RAZA INVALIDA, INTENTE DE NUEVO:");
        breed = Console.ReadLine()!;
    }

    pet.Add("BREED", breed);

    Console.WriteLine("INGRESE LA EDAD:");
    int petAge;

    while (!int.TryParse(Console.ReadLine(), out petAge) || petAge < 0)
    {
        Console.WriteLine("EDAD INVALIDA, INGRESE UNA EDAD MAYOR O IGUAL A CERO:");
    }

    pet.Add("AGE", petAge.ToString());

    // Connect pet with owner
    pet.Add("OWNER_ID", ownerId.ToString());

    mascotas.Add(pet);
    nextIdMascota++;

    Console.WriteLine("MASCOTA REGISTRADA CORRECTAMENTE.");
}




void MostrarMascotas(List<Dictionary<string, string>> mascotas)
{
    Console.WriteLine("\n=== LISTA DE MASCOTAS ===");

    if (mascotas.Count == 0)
    {
        Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
        return;
    }

    foreach (var pet in mascotas)
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine($"ID: {pet["ID"]}");
        Console.WriteLine($"NOMBRE: {pet["NAME"]}");
        Console.WriteLine($"ESPECIE: {pet["SPECIES"]}");
        Console.WriteLine($"RAZA: {pet["BREED"]}");
        Console.WriteLine($"EDAD: {pet["AGE"]}");
        Console.WriteLine($"ID DEL PROPIETARIO: {pet["OWNER_ID"]}");
    }
}



void BuscarMascota(List<Dictionary<string, string>> mascotas)
{
    if (mascotas.Count == 0)
    {
        Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
        return;
    }

    Console.WriteLine("INGRESE EL NOMBRE DE LA MASCOTA:");
    string searchedName = Console.ReadLine()!;

    bool petFound = false;

    foreach (var pet in mascotas)
    {
        if (string.Equals(
            pet["NAME"],
            searchedName,
            StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine($"ID: {pet["ID"]}");
            Console.WriteLine($"NOMBRE: {pet["NAME"]}");
            Console.WriteLine($"ESPECIE: {pet["SPECIES"]}");
            Console.WriteLine($"RAZA: {pet["BREED"]}");
            Console.WriteLine($"EDAD: {pet["AGE"]}");
            Console.WriteLine($"ID DEL PROPIETARIO: {pet["OWNER_ID"]}");

            petFound = true;
            break;
        }
    }

    if (!petFound)
    {
        Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
    }
}



void ModificarMascota(
    List<Dictionary<string, string>> mascotas,
    List<Dictionary<string, string>> propietarios)
{
    if (mascotas.Count == 0)
    {
        Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
        return;
    }

    Console.WriteLine("INGRESE EL ID DE LA MASCOTA QUE DESEA MODIFICAR:");
    int petId;

    while (!int.TryParse(Console.ReadLine(), out petId) || petId <= 0)
    {
        Console.WriteLine("ID INVALIDO, INTENTE DE NUEVO:");
    }

    Dictionary<string, string>? selectedPet =
        mascotas.Find(pet => pet["ID"] == petId.ToString());

    if (selectedPet == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
        return;
    }

    int updateOption = 0;

    while (updateOption != 6)
    {
        Console.WriteLine("\n=== MODIFICAR MASCOTA ===");
        Console.WriteLine("1. NOMBRE");
        Console.WriteLine("2. ESPECIE");
        Console.WriteLine("3. RAZA");
        Console.WriteLine("4. EDAD");
        Console.WriteLine("5. PROPIETARIO");
        Console.WriteLine("6. VOLVER");

        Console.WriteLine("SELECCIONA QUE DESEA MODIFICAR:");

        while (!int.TryParse(Console.ReadLine(), out updateOption) ||
               updateOption < 1 || updateOption > 6)
        {
            Console.WriteLine("OPCION INVALIDA, INTENTA DE NUEVO:");
        }

        string newValue;

        switch (updateOption)
        {
            case 1:
                Console.WriteLine("INGRESE EL NUEVO NOMBRE:");
                newValue = Console.ReadLine()!;

                while (string.IsNullOrWhiteSpace(newValue))
                {
                    Console.WriteLine("NOMBRE INVALIDO:");
                    newValue = Console.ReadLine()!;
                }

                selectedPet["NAME"] = newValue;
                Console.WriteLine("NOMBRE ACTUALIZADO.");
                break;

            case 2:
                Console.WriteLine("INGRESE LA NUEVA ESPECIE:");
                newValue = Console.ReadLine()!;

                while (string.IsNullOrWhiteSpace(newValue))
                {
                    Console.WriteLine("ESPECIE INVALIDA:");
                    newValue = Console.ReadLine()!;
                }

                selectedPet["SPECIES"] = newValue;
                Console.WriteLine("ESPECIE ACTUALIZADA.");
                break;

            case 3:
                Console.WriteLine("INGRESE LA NUEVA RAZA:");
                newValue = Console.ReadLine()!;

                while (string.IsNullOrWhiteSpace(newValue))
                {
                    Console.WriteLine("RAZA INVALIDA:");
                    newValue = Console.ReadLine()!;
                }

                selectedPet["BREED"] = newValue;
                Console.WriteLine("RAZA ACTUALIZADA.");
                break;

            case 4:
                Console.WriteLine("INGRESE LA NUEVA EDAD:");
                int newAge;

                while (!int.TryParse(Console.ReadLine(), out newAge) || newAge < 0)
                {
                    Console.WriteLine("EDAD INVALIDA:");
                }

                selectedPet["AGE"] = newAge.ToString();
                Console.WriteLine("EDAD ACTUALIZADA.");
                break;

            case 5:
                MostrarPropietarios(propietarios);

                Console.WriteLine("INGRESE EL ID DEL NUEVO PROPIETARIO:");
                int newOwnerId;

                while (!int.TryParse(Console.ReadLine(), out newOwnerId) ||
                       newOwnerId <= 0)
                {
                    Console.WriteLine("ID INVALIDO:");
                }

                bool ownerFound = false;

                foreach (var owner in propietarios)
                {
                    if (owner["ID"] == newOwnerId.ToString())
                    {
                        ownerFound = true;
                        break;
                    }
                }

                if (ownerFound)
                {
                    selectedPet["OWNER_ID"] = newOwnerId.ToString();
                    Console.WriteLine("PROPIETARIO ACTUALIZADO.");
                }
                else
                {
                    Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
                }

                break;
        }
    }
}



void EliminarMascota(
    List<Dictionary<string, string>> mascotas,
    List<Dictionary<string, string>> querys)
{
    if (mascotas.Count == 0)
    {
        Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
        return;
    }

    Console.WriteLine("INGRESE EL ID DE LA MASCOTA QUE DESEA ELIMINAR:");
    int petId;

    while (!int.TryParse(Console.ReadLine(), out petId) || petId <= 0)
    {
        Console.WriteLine("ID INVALIDO, INTENTE DE NUEVO:");
    }

    Dictionary<string, string>? selectedPet =
        mascotas.Find(pet => pet["ID"] == petId.ToString());

    if (selectedPet == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
        return;
    }

    // Check registered consultations
    bool hasQueries = querys.Any(query => query["PET_ID"] == petId.ToString());

    if (hasQueries)
    {
        Console.WriteLine("NO SE PUEDE ELIMINAR ESTA MASCOTA.");
        Console.WriteLine("TIENE CONSULTAS REGISTRADAS.");
        return;
    }

    mascotas.Remove(selectedPet);

    Console.WriteLine("MASCOTA ELIMINADA CORRECTAMENTE.");
}



void MostrarMenuPropietarios()
{
    int ownerOption = 0;

    while (ownerOption != 6)
    {
        Console.WriteLine("\n=== MENU PROPIETARIOS ===");
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
            Console.WriteLine("OPCION INVALIDA, INTENTA DE NUEVO:");
        }

        switch (ownerOption)
        {
            case 1:
                do
                {
                    RegistrarPropietario(propietarios);
                }
                while (AskToContinue("¿DESEA REGISTRAR OTRO PROPIETARIO?"));
                break;

            case 2:
                MostrarPropietarios(propietarios);
                break;

            case 3:
                do
                {
                    BuscarPropietario(propietarios);
                }
                while (AskToContinue("¿DESEA BUSCAR OTRO PROPIETARIO?"));
                break;

            case 4:
                do
                {
                    ModificarPropietario(propietarios);
                }
                while (AskToContinue("¿DESEA MODIFICAR OTRO PROPIETARIO?"));
                break;

            case 5:
                do
                {
                    EliminarPropietario(propietarios, mascotas);
                }
                while (AskToContinue("¿DESEA ELIMINAR OTRO PROPIETARIO?"));
                break;
        }
    }
}




void RegistrarPropietario(List<Dictionary<string, string>> propietarios)
{
    Console.WriteLine("\n=== REGISTRO DE PROPIETARIO ===");

    Dictionary<string, string> owner = new Dictionary<string, string>();

    // Save owner ID
    owner.Add("ID", nextIdPropietario.ToString());

    Console.WriteLine("INGRESE EL NOMBRE DEL PROPIETARIO:");
    string ownerName = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(ownerName))
    {
        Console.WriteLine("NOMBRE INVALIDO, INTENTE DE NUEVO:");
        ownerName = Console.ReadLine()!;
    }

    owner.Add("NAME", ownerName);

    Console.WriteLine("INGRESE EL TELEFONO:");
    string ownerPhone = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(ownerPhone))
    {
        Console.WriteLine("TELEFONO INVALIDO, INTENTE DE NUEVO:");
        ownerPhone = Console.ReadLine()!;
    }

    owner.Add("PHONE", ownerPhone);

    Console.WriteLine("INGRESE LA DIRECCION:");
    string ownerAddress = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(ownerAddress))
    {
        Console.WriteLine("DIRECCION INVALIDA, INTENTE DE NUEVO:");
        ownerAddress = Console.ReadLine()!;
    }

    owner.Add("ADDRESS", ownerAddress);

    propietarios.Add(owner);
    nextIdPropietario++;

    Console.WriteLine("PROPIETARIO REGISTRADO CORRECTAMENTE.");
}



void MostrarPropietarios(List<Dictionary<string, string>> propietarios)
{
    Console.WriteLine("\n=== LISTA DE PROPIETARIOS ===");

    if (propietarios.Count == 0)
    {
        Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
        return;
    }

    foreach (var owner in propietarios)
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine($"ID: {owner["ID"]}");
        Console.WriteLine($"NOMBRE: {owner["NAME"]}");
        Console.WriteLine($"TELEFONO: {owner["PHONE"]}");
        Console.WriteLine($"DIRECCION: {owner["ADDRESS"]}");
    }
}



void BuscarPropietario(List<Dictionary<string, string>> propietarios)
{
    if (propietarios.Count == 0)
    {
        Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
        return;
    }

    Console.WriteLine("\n=== BUSCAR PROPIETARIO ===");
    Console.WriteLine("1. BUSCAR POR ID");
    Console.WriteLine("2. BUSCAR POR NOMBRE");

    int searchOption;

    while (!int.TryParse(Console.ReadLine(), out searchOption) ||
           searchOption < 1 || searchOption > 2)
    {
        Console.WriteLine("OPCION INVALIDA, INTENTE DE NUEVO:");
    }

    bool ownerFound = false;

    if (searchOption == 1)
    {
        Console.WriteLine("INGRESE EL ID DEL PROPIETARIO:");
        int ownerId;

        while (!int.TryParse(Console.ReadLine(), out ownerId) || ownerId <= 0)
        {
            Console.WriteLine("ID INVALIDO:");
        }

        foreach (var owner in propietarios)
        {
            if (owner["ID"] == ownerId.ToString())
            {
                Console.WriteLine($"ID: {owner["ID"]}");
                Console.WriteLine($"NOMBRE: {owner["NAME"]}");
                Console.WriteLine($"TELEFONO: {owner["PHONE"]}");
                Console.WriteLine($"DIRECCION: {owner["ADDRESS"]}");

                ownerFound = true;
                break;
            }
        }
    }
    else
    {
        Console.WriteLine("INGRESE EL NOMBRE DEL PROPIETARIO:");
        string searchedName = Console.ReadLine()!;

        foreach (var owner in propietarios)
        {
            if (string.Equals(
                owner["NAME"],
                searchedName,
                StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"ID: {owner["ID"]}");
                Console.WriteLine($"NOMBRE: {owner["NAME"]}");
                Console.WriteLine($"TELEFONO: {owner["PHONE"]}");
                Console.WriteLine($"DIRECCION: {owner["ADDRESS"]}");

                ownerFound = true;
                break;
            }
        }
    }

    if (!ownerFound)
    {
        Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
    }
}




void ModificarPropietario(List<Dictionary<string, string>> propietarios)
{
    if (propietarios.Count == 0)
    {
        Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
        return;
    }

    Console.WriteLine("INGRESE EL ID DEL PROPIETARIO QUE DESEA MODIFICAR:");
    int ownerId;

    while (!int.TryParse(Console.ReadLine(), out ownerId) || ownerId <= 0)
    {
        Console.WriteLine("ID INVALIDO:");
    }

    Dictionary<string, string>? selectedOwner =
        propietarios.Find(owner => owner["ID"] == ownerId.ToString());

    if (selectedOwner == null)
    {
        Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
        return;
    }

    int updateOption = 0;

    while (updateOption != 4)
    {
        Console.WriteLine("\n=== MODIFICAR PROPIETARIO ===");
        Console.WriteLine("1. NOMBRE");
        Console.WriteLine("2. TELEFONO");
        Console.WriteLine("3. DIRECCION");
        Console.WriteLine("4. VOLVER");

        Console.WriteLine("SELECCIONA UNA OPCION:");

        while (!int.TryParse(Console.ReadLine(), out updateOption) ||
               updateOption < 1 || updateOption > 4)
        {
            Console.WriteLine("OPCION INVALIDA:");
        }

        string newValue;

        switch (updateOption)
        {
            case 1:
                Console.WriteLine("INGRESE EL NUEVO NOMBRE:");
                newValue = Console.ReadLine()!;

                while (string.IsNullOrWhiteSpace(newValue))
                {
                    Console.WriteLine("NOMBRE INVALIDO:");
                    newValue = Console.ReadLine()!;
                }

                selectedOwner["NAME"] = newValue;
                Console.WriteLine("NOMBRE ACTUALIZADO.");
                break;

            case 2:
                Console.WriteLine("INGRESE EL NUEVO TELEFONO:");
                newValue = Console.ReadLine()!;

                while (string.IsNullOrWhiteSpace(newValue))
                {
                    Console.WriteLine("TELEFONO INVALIDO:");
                    newValue = Console.ReadLine()!;
                }

                selectedOwner["PHONE"] = newValue;
                Console.WriteLine("TELEFONO ACTUALIZADO.");
                break;

            case 3:
                Console.WriteLine("INGRESE LA NUEVA DIRECCION:");
                newValue = Console.ReadLine()!;

                while (string.IsNullOrWhiteSpace(newValue))
                {
                    Console.WriteLine("DIRECCION INVALIDA:");
                    newValue = Console.ReadLine()!;
                }

                selectedOwner["ADDRESS"] = newValue;
                Console.WriteLine("DIRECCION ACTUALIZADA.");
                break;
        }
    }
}




void EliminarPropietario(
    List<Dictionary<string, string>> propietarios,
    List<Dictionary<string, string>> mascotas)
{
    if (propietarios.Count == 0)
    {
        Console.WriteLine("NO HAY PROPIETARIOS REGISTRADOS.");
        return;
    }

    Console.WriteLine("INGRESE EL ID DEL PROPIETARIO QUE DESEA ELIMINAR:");
    int ownerId;

    while (!int.TryParse(Console.ReadLine(), out ownerId) || ownerId <= 0)
    {
        Console.WriteLine("ID INVALIDO:");
    }

    Dictionary<string, string>? selectedOwner =
        propietarios.Find(owner => owner["ID"] == ownerId.ToString());

    if (selectedOwner == null)
    {
        Console.WriteLine("NO SE ENCONTRO EL PROPIETARIO.");
        return;
    }

    // Check assigned pets
    bool hasPets = mascotas.Any(
        pet => pet["OWNER_ID"] == ownerId.ToString());

    if (hasPets)
    {
        Console.WriteLine("NO SE PUEDE ELIMINAR EL PROPIETARIO.");
        Console.WriteLine("TODAVIA TIENE MASCOTAS ASOCIADAS.");
        Console.WriteLine("CAMBIE EL PROPIETARIO DE SUS MASCOTAS PRIMERO.");
        return;
    }

    propietarios.Remove(selectedOwner);

    Console.WriteLine("PROPIETARIO ELIMINADO CORRECTAMENTE.");
}



void MostrarMenuQuerys()
{
    int queryOption = 0;

    while (queryOption != 6)
    {
        Console.WriteLine("\n=== MENU DE CONSULTAS ===");
        Console.WriteLine("1. REGISTRAR CONSULTA");
        Console.WriteLine("2. MOSTRAR CONSULTAS");
        Console.WriteLine("3. BUSCAR CONSULTA");
        Console.WriteLine("4. MODIFICAR CONSULTA");
        Console.WriteLine("5. ELIMINAR CONSULTA");
        Console.WriteLine("6. VOLVER");

        Console.WriteLine("SELECCIONA UNA OPCION:");

        while (!int.TryParse(Console.ReadLine(), out queryOption) ||
               queryOption < 1 || queryOption > 6)
        {
            Console.WriteLine("OPCION INVALIDA, INTENTA DE NUEVO:");
        }

        switch (queryOption)
        {
            case 1:
                do
                {
                    RegistrarQuery(querys, mascotas);
                }
                while (AskToContinue("¿DESEA REGISTRAR OTRA CONSULTA?"));
                break;

            case 2:
                MostrarQuerys(querys);
                break;

            case 3:
                do
                {
                    BuscarQuery(querys);
                }
                while (AskToContinue("¿DESEA BUSCAR OTRA CONSULTA?"));
                break;

            case 4:
                do
                {
                    ModificarQuery(querys);
                }
                while (AskToContinue("¿DESEA MODIFICAR OTRA CONSULTA?"));
                break;

            case 5:
                do
                {
                    EliminarQuery(querys);
                }
                while (AskToContinue("¿DESEA ELIMINAR OTRA CONSULTA?"));
                break;
        }
    }
}




void RegistrarQuery(
    List<Dictionary<string, string>> querys,
    List<Dictionary<string, string>> mascotas)
{
    if (mascotas.Count == 0)
    {
        Console.WriteLine("NO HAY MASCOTAS REGISTRADAS.");
        return;
    }

    Console.WriteLine("\n=== REGISTRO DE CONSULTA ===");

    // Show registered pets
    MostrarMascotas(mascotas);

    Console.WriteLine("INGRESE EL ID DE LA MASCOTA:");
    int petId;

    while (!int.TryParse(Console.ReadLine(), out petId) || petId <= 0)
    {
        Console.WriteLine("ID INVALIDO, INTENTE DE NUEVO:");
    }

    Dictionary<string, string>? selectedPet =
        mascotas.Find(pet => pet["ID"] == petId.ToString());

    if (selectedPet == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA MASCOTA.");
        return;
    }

    Dictionary<string, string> query = new Dictionary<string, string>();

    // Save query information
    query.Add("ID", nextIdQuery.ToString());
    query.Add("PET_ID", petId.ToString());
    query.Add("DATE", DateTime.Now.ToString());

    Console.WriteLine("INGRESE EL MOTIVO DE LA CONSULTA:");
    string reason = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(reason))
    {
        Console.WriteLine("MOTIVO INVALIDO, INTENTE DE NUEVO:");
        reason = Console.ReadLine()!;
    }

    query.Add("REASON", reason);

    Console.WriteLine("INGRESE EL DIAGNOSTICO:");
    string diagnosis = Console.ReadLine()!;

    while (string.IsNullOrWhiteSpace(diagnosis))
    {
        Console.WriteLine("DIAGNOSTICO INVALIDO, INTENTE DE NUEVO:");
        diagnosis = Console.ReadLine()!;
    }

    query.Add("DIAGNOSIS", diagnosis);

    querys.Add(query);
    nextIdQuery++;

    Console.WriteLine("CONSULTA REGISTRADA CORRECTAMENTE.");
}



void MostrarQuerys(List<Dictionary<string, string>> querys)
{
    Console.WriteLine("\n=== LISTA DE CONSULTAS ===");

    if (querys.Count == 0)
    {
        Console.WriteLine("NO HAY CONSULTAS REGISTRADAS.");
        return;
    }

    foreach (var query in querys)
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine($"ID CONSULTA: {query["ID"]}");
        Console.WriteLine($"ID MASCOTA: {query["PET_ID"]}");
        Console.WriteLine($"FECHA: {query["DATE"]}");
        Console.WriteLine($"MOTIVO: {query["REASON"]}");
        Console.WriteLine($"DIAGNOSTICO: {query["DIAGNOSIS"]}");
    }
}



void BuscarQuery(List<Dictionary<string, string>> querys)
{
    if (querys.Count == 0)
    {
        Console.WriteLine("NO HAY CONSULTAS REGISTRADAS.");
        return;
    }

    Console.WriteLine("INGRESE EL ID DE LA CONSULTA:");
    int queryId;

    while (!int.TryParse(Console.ReadLine(), out queryId) || queryId <= 0)
    {
        Console.WriteLine("ID INVALIDO, INTENTE DE NUEVO:");
    }

    Dictionary<string, string>? selectedQuery =
        querys.Find(query => query["ID"] == queryId.ToString());

    if (selectedQuery == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA CONSULTA.");
        return;
    }

    Console.WriteLine("-------------------------");
    Console.WriteLine($"ID CONSULTA: {selectedQuery["ID"]}");
    Console.WriteLine($"ID MASCOTA: {selectedQuery["PET_ID"]}");
    Console.WriteLine($"FECHA: {selectedQuery["DATE"]}");
    Console.WriteLine($"MOTIVO: {selectedQuery["REASON"]}");
    Console.WriteLine($"DIAGNOSTICO: {selectedQuery["DIAGNOSIS"]}");
}



void ModificarQuery(List<Dictionary<string, string>> querys)
{
    if (querys.Count == 0)
    {
        Console.WriteLine("NO HAY CONSULTAS REGISTRADAS.");
        return;
    }

    Console.WriteLine("INGRESE EL ID DE LA CONSULTA QUE DESEA MODIFICAR:");
    int queryId;

    while (!int.TryParse(Console.ReadLine(), out queryId) || queryId <= 0)
    {
        Console.WriteLine("ID INVALIDO, INTENTE DE NUEVO:");
    }

    Dictionary<string, string>? selectedQuery =
        querys.Find(query => query["ID"] == queryId.ToString());

    if (selectedQuery == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA CONSULTA.");
        return;
    }

    int updateOption = 0;

    while (updateOption != 3)
    {
        Console.WriteLine("\n=== MODIFICAR CONSULTA ===");
        Console.WriteLine("1. MOTIVO");
        Console.WriteLine("2. DIAGNOSTICO");
        Console.WriteLine("3. VOLVER");

        Console.WriteLine("SELECCIONA UNA OPCION:");

        while (!int.TryParse(Console.ReadLine(), out updateOption) ||
               updateOption < 1 || updateOption > 3)
        {
            Console.WriteLine("OPCION INVALIDA:");
        }

        string newValue;

        switch (updateOption)
        {
            case 1:
                Console.WriteLine("INGRESE EL NUEVO MOTIVO:");
                newValue = Console.ReadLine()!;

                while (string.IsNullOrWhiteSpace(newValue))
                {
                    Console.WriteLine("MOTIVO INVALIDO:");
                    newValue = Console.ReadLine()!;
                }

                selectedQuery["REASON"] = newValue;
                Console.WriteLine("MOTIVO ACTUALIZADO.");
                break;

            case 2:
                Console.WriteLine("INGRESE EL NUEVO DIAGNOSTICO:");
                newValue = Console.ReadLine()!;

                while (string.IsNullOrWhiteSpace(newValue))
                {
                    Console.WriteLine("DIAGNOSTICO INVALIDO:");
                    newValue = Console.ReadLine()!;
                }

                selectedQuery["DIAGNOSIS"] = newValue;
                Console.WriteLine("DIAGNOSTICO ACTUALIZADO.");
                break;
        }
    }
}




void EliminarQuery(List<Dictionary<string, string>> querys)
{
    if (querys.Count == 0)
    {
        Console.WriteLine("NO HAY CONSULTAS REGISTRADAS.");
        return;
    }

    Console.WriteLine("INGRESE EL ID DE LA CONSULTA QUE DESEA ELIMINAR:");
    int queryId;

    while (!int.TryParse(Console.ReadLine(), out queryId) || queryId <= 0)
    {
        Console.WriteLine("ID INVALIDO, INTENTE DE NUEVO:");
    }

    Dictionary<string, string>? selectedQuery =
        querys.Find(query => query["ID"] == queryId.ToString());

    if (selectedQuery == null)
    {
        Console.WriteLine("NO SE ENCONTRO LA CONSULTA.");
        return;
    }

    querys.Remove(selectedQuery);

    Console.WriteLine("CONSULTA ELIMINADA CORRECTAMENTE.");
}

