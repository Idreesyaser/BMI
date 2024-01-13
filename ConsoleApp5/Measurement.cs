using System;

class Measurement
    {
    public DateTime Date { get; }
    public double Weight { get; }
    public double BMI { get; }

    public Measurement (DateTime date, double weight, double bmi)
        {
        Date = date;
        Weight = weight;
        BMI = bmi;
        }
    }
