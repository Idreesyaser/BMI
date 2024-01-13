using System;

class Program
    {
    static void Main (string[] args)
        {
        var userManager = new UserManager();

        int loginAttempts = 0;
        const int maxLoginAttempts = 3;

        while (true)
            {
            Console.Clear();
            Console.WriteLine("Tervetuloa Paino Oy:n sovellukseen!");
            Console.WriteLine("Valitse toiminto:");
            Console.WriteLine("1. Rekisteröidy");
            Console.WriteLine("2. Kirjaudu sisään");
            Console.WriteLine("3. Lopeta");

            string choice = Console.ReadLine();

            switch (choice)
                {
                case "1":
                    userManager.Register();
                    break;

                case "2":
                    User currentUser = null;

                    while (loginAttempts < maxLoginAttempts)
                        {
                        currentUser = userManager.LogIn();

                        if (currentUser != null)
                            {
                            break;
                            }
                        else
                            {
                            loginAttempts++;
                            Console.WriteLine($"Virheellinen käyttäjätunnus tai salasana. Yrityksiä jäljellä: {maxLoginAttempts - loginAttempts}");
                            }
                        }

                    if (loginAttempts == maxLoginAttempts)
                        {
                        Console.WriteLine("Liian monta virheellistä yritystä. Kirjaudu sisään myöhemmin.");
                        Console.ReadLine();
                        return;
                        }

                    while (currentUser.AddMeasurement())
                        {
                        // Jatka lisäämistä, kunnes käyttäjä ei halua enää syöttää
                        }

                    currentUser.PrintResults();
                    currentUser.Average();
                    currentUser.ResultsInRange();

                    break;

                case "3":
                    Console.WriteLine("Sovellus suljetaan. Näkemiin!");
                    return;

                default:
                    Console.WriteLine("Virheellinen valinta. Yritä uudelleen.");
                    break;
                }
            }
        }
    }