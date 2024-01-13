using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class User
    {
    public string Name { get; private set; }
    public double Height { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }

    private List<Measurement> measurements = new List<Measurement>();

    public User (string name, double height, string username, string password)
        {
        Name = name;
        Height = height;
        Username = username;
        Password = password;
        }

    // Lisää mittaustulos ja tallenna
    public bool AddMeasurement ()
        {
        Console.WriteLine("Lisää uusi mittaustulos:");

        double weight = ValidDoubleInput("Paino (kiloina): ");

        double bmi = weight / Math.Pow(Height / 100, 2);

        Console.WriteLine($"Painoindeksi: {(int)bmi}");

        measurements.Add(new Measurement(DateTime.Now, weight, bmi));
        SaveResults();

        Console.WriteLine("Haluatko syöttää lisää mittaustuloksia? (K/E)");
        return Console.ReadLine().Trim().ToLower() == "k";
        }

    // Tulosta mittaustulokset
    public void PrintResults ()
        {
        Console.WriteLine("Mittaustulokset:");

        foreach (var result in measurements.OrderBy(m => m.Date))
            {
            Console.WriteLine($"Päivämäärä: {result.Date}, Paino: {result.Weight}, BMI: {(int)result.BMI}");
            }
        }

    // Tulosta keskiarvot
    public void Average ()
        {
        if (measurements.Count > 0)
            {
            double weightSum = 0;
            double bmiSum = 0;

            foreach (var measurement in measurements)
                {
                weightSum += measurement.Weight;
                bmiSum += measurement.BMI;
                }

            double weightAverage = weightSum / measurements.Count;
            double bmiAverage = bmiSum / measurements.Count;

            Console.WriteLine($"Painon keskiarvo: {(int)weightAverage}, BMI:n keskiarvo: {(int)bmiAverage}");
            }
        else
            {
            Console.WriteLine("Ei tuloksia laskettavaksi keskiarvoa");
            }
        }

    // Tulosta tulokset halutulta aikaväliltä
    public void ResultsInRange ()
        {
        Console.WriteLine("Haluatko tulostaa mittaustulokset halutulta aikaväliltä? (K/E)");
        string response = Console.ReadLine();

        if (response.ToLower() == "k")
            {
            DateTime startDate = ValidDate("Anna aloituspäivämäärä (pp.kk.vvvv): ");
            DateTime endDate = ValidDate("Anna lopetuspäivämäärä (pp.kk.vvvv): ");

            var resultsInRange = measurements.Where(m => m.Date >= startDate && m.Date <= endDate).OrderBy(m => m.Date);

            Console.WriteLine($"Mittaustulokset halutulta aikaväliltä ({startDate.ToShortDateString()} - {endDate.ToShortDateString()}):");

            foreach (var result in resultsInRange)
                {
                Console.WriteLine($"Päivämäärä: {result.Date}, Paino: {result.Weight}, BMI: {(int)result.BMI}");
                }

            double weightSum = resultsInRange.Sum(m => m.Weight);
            double bmiSum = resultsInRange.Sum(m => m.BMI);

            double weightAverage = weightSum / resultsInRange.Count();
            double bmiAverage = bmiSum / resultsInRange.Count();

            Console.WriteLine($"Painon keskiarvo halutulta aikaväliltä: {(int)weightAverage}, BMI:n keskiarvo halutulta aikaväliltä: {(int)bmiAverage}");
            }
        Console.ReadLine();
        }

    // Apumetodi, joka varmistaa, että käyttäjän syöte on kelvollinen päivämäärä
    private DateTime ValidDate (string prompt)
        {
        while (true)
            {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (DateTime.TryParse(input, out DateTime result))
                {
                return result;
                }
            else
                {
                Console.WriteLine("Virheellinen syöte. Anna kelvollinen päivämäärä muodossa pp.kk.vvvv.");
                }
            }
        }

    // Apumetodi, joka varmistaa, että käyttäjän syöte on kelvollinen double-arvo
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

    // Tallenna mittaustulokset tiedostoon
    private void SaveResults ()
        {
        string fileName = $"{Username}_results.txt";

        var lines = new List<string>();

        foreach (var measurement in measurements)
            {
            lines.Add($"{measurement.Date}\t{measurement.Weight}\t{measurement.BMI}");
            }

        File.AppendAllLines(fileName, lines);
        }

    // Lataa mittaustulokset tiedostosta
    public void ResultsFromFile ()
        {
        string fileName = $"{Username}_results.txt";

        if (File.Exists(fileName))
            {
            var lines = File.ReadAllLines(fileName).Skip(1);

            foreach (var line in lines)
                {
                var parts = line.Split('\t');
                if (parts.Length == 3 && DateTime.TryParse(parts[0], out DateTime date) &&
                    double.TryParse(parts[1], out double weight) && double.TryParse(parts[2], out double bmi))
                    {
                    measurements.Add(new Measurement(date, weight, bmi));
                    }
                }
            }
        }
    }
