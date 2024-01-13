using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class UserManager
    {
    private const string UsersFileName = "users.csv";
    private List<User> users;

    public UserManager ()
        {
        LoadUsers();
        }

    public void Register ()
        {
        Console.WriteLine("Rekisteröidy:");

        // Käyttäjän tiedot
        Console.Write("Nimi: ");
        string name = Console.ReadLine();

        double height = ValidDoubleInput("Pituus (senttimetreinä): ");

        Console.Write("Käyttäjätunnus: ");
        string username = Console.ReadLine();

        Console.Write("Salasana: ");
        string password = Console.ReadLine();

        // Uusi käyttäjä luodaan ja lisätään listaan
        var newUser = new User(name, height, username, password);
        users.Add(newUser);

        Console.WriteLine("Rekisteröinti onnistui!");
        Console.ReadLine();

        SaveUsers();
        }

    public User LogIn ()
        {
        Console.WriteLine("Kirjaudu sisään:");

        // Kirjautumistiedot
        Console.Write("Käyttäjätunnus: ");
        string inputUsername = Console.ReadLine();

        Console.Write("Salasana: ");
        string inputPassword = Console.ReadLine();

        // Kirjautuminen ja käyttäjän lataaminen tuloksineen
        var loginUser = users.Find(u => u.Username == inputUsername && u.Password == inputPassword);

        if (loginUser != null)
            {
            loginUser.ResultsFromFile();
            Console.WriteLine($"Tervetuloa, {loginUser.Name}!");
            return loginUser;
            }
        else
            {
            Console.WriteLine("Virheellinen käyttäjätunnus tai salasana.");
            Console.ReadLine ();
            return null;
            }
        }

    // Käyttäjien tiedot lataaminen tiedostosta
    private void LoadUsers ()
        {
        users = new List<User>();

        if (File.Exists(UsersFileName))
            {
            var lines = File.ReadAllLines(UsersFileName).Skip(1);

            foreach (var line in lines)
                {
                var parts = line.Split(',');

                string name = parts[0];
                double height = ValidDoubleString(parts[1]);
                string username = parts[2];
                string password = parts[3];

                var newUser = new User(name, height, username, password);
                users.Add(newUser);
                }
            }
        }

    // Käyttäjien tiedot tallentaminen tiedostoon
    private void SaveUsers ()
        {
        var lines = new List<string> { "Name,Height,Username,Password" };

        lines.AddRange(users.Select(user => $"{user.Name},{user.Height},{user.Username},{user.Password}"));

        File.WriteAllLines(UsersFileName, lines);
        }

    // metodi joka varmistaa, että käyttäjän syöte on kelvollinen double-arvo
    private double ValidDoubleInput (string prompt)
        {
        while (true)
            {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (double.TryParse(input, out double result))
                {
                return result;
                }
            else
                {
                Console.WriteLine("Virheellinen syöte. Anna kelvollinen luku.");
                }
            }
        }

    // Apumetodi, joka varmistaa, että käyttäjän syöte on kelvollinen double-arvo
    private double ValidDoubleString (string input)
        {
        if (double.TryParse(input, out double result))
            {
            return result;
            }
        else
            {
            Console.WriteLine($"Virheellinen syöte ({input}). Käytetään oletusarvoa 0.");
            return 0;
            }
        }
    }
