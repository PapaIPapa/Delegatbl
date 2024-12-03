using System;
public interface IConverter<in T, out U>
{
    U Convert(T value);
}
public class StringToIntConverter : IConverter<string, int>
{
    public int Convert(string value)
    {
        return int.Parse(value);
    }
}
public class ObjectToStringConverter : IConverter<object, string>
{
    public string Convert(object value)
    {
        return value?.ToString() ?? "null";
    }
}

public abstract class Animal
{
    public string Name { get; set; }
    public abstract void SayHello();
}

public class Dog : Animal
{
    public override void SayHello()
    {
        Console.WriteLine($"Гав! Меня зовут {Name}");
    }
}

public class Cat : Animal
{
    public override void SayHello()
    {
        Console.WriteLine($"Мяу! Меня зовут {Name}");
    }
}

public class Calculator
{
    public static double Add(double x, double y) => x + y;
    public static double Subtract(double x, double y) => x - y;
    public static double Multiply(double x, double y) => x * y;
    public static double Divide(double x, double y) => y == 0 ? double.NaN : x / y;
}


public class Program
{
    public static U[] ConvertArray<T, U>(T[] array, IConverter<T, U> converter)
    {
        U[] result = new U[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            result[i] = converter.Convert(array[i]);
        }
        return result;
    }

    public static void HelloAnimals(List<Animal> animals, Action<Animal> greeter)
    {
        foreach (var animal in animals)
        {
            greeter(animal);
        }
    }

    public static double Calculate(double x, double y, Func<double, double, double> operation)
    {
        return operation(x, y);
    }
    public static void Main(string[] args)
    {
        Console.WriteLine("Convert");
        string[] strings = { "1", "2", "3" };
        object[] objects = { 1, "hello", 3.14 };

        IConverter<object, string> objectToStringConverter = new ObjectToStringConverter();
        string[] convertedStrings = ConvertArray(strings, objectToStringConverter);

        int[] convertedInts = ConvertArray(strings, new StringToIntConverter());
        Console.WriteLine(string.Join(", ", convertedStrings));
        Console.WriteLine(string.Join(", ", convertedInts));

        string[] convertedObjects = ConvertArray(objects, objectToStringConverter);
        Console.WriteLine(string.Join(", ", convertedObjects));

        //
        Console.WriteLine("Animals");
        List<Animal> animals = new List<Animal>() { new Dog { Name = "Бобик" }, new Cat { Name = "Мурзик" } };

        Action<Animal> greetAll = animal => animal.SayHello();
        Action<Dog> greetDog = dog => dog.SayHello();
        HelloAnimals(animals, greetAll);

        HelloAnimals(animals, a => a.SayHello());

        //
        Console.WriteLine("Calc");
        double x = 10;
        double y = 5;

        Console.WriteLine($"Сложение: {Calculate(x, y, Calculator.Add)}");
        Console.WriteLine($"Вычитание: {Calculate(x, y, Calculator.Subtract)}");
        Console.WriteLine($"Умножение: {Calculate(x, y, Calculator.Multiply)}");
        Console.WriteLine($"Деление: {Calculate(x, y, Calculator.Divide)}");
    }
}