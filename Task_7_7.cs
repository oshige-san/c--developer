using System;
using System.Collections.Generic;

// Абстрактный класс для доставки
abstract class Delivery
{
    public string Address { get; set; }
    public abstract void Deliver();
}

// Доставка на дом
class HomeDelivery : Delivery
{
    public string CourierName { get; set; }

    public override void Deliver()
    {
        Console.WriteLine($"Delivering to {Address} by {CourierName}");
    }
}

// Доставка в пункт выдачи
class PickPointDelivery : Delivery
{
    public string PickPointCompany { get; set; }
    public string PickPointLocation { get; set; }

    public override void Deliver()
    {
        Console.WriteLine($"Delivering to {PickPointLocation} of {PickPointCompany}");
    }
}

// Доставка в магазин
class ShopDelivery : Delivery
{
    public string ShopName { get; set; }

    public override void Deliver()
    {
        Console.WriteLine($"Delivering to {ShopName} shop");
    }
}

// Класс для описания продукта
class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

// Класс для описания адреса
class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }

    public Address(string street, string city, string postalCode)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
    }

    public override string ToString()
    {
        return $"{Street}, {City}, {PostalCode}";
    }
}

// Класс для описания контактной информации
class ContactInfo
{
    private string _phone;
    private string _email;

    public string Phone
    {
        get => _phone;
        set
        {
            if (IsValidPhone(value))
                _phone = value;
            else
                throw new ArgumentException("Invalid phone number");
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (IsValidEmail(value))
                _email = value;
            else
                throw new ArgumentException("Invalid email address");
        }
    }

    public ContactInfo(string phone, string email)
    {
        Phone = phone;
        Email = email;
    }

    private bool IsValidPhone(string phone)
    {
        return !string.IsNullOrEmpty(phone) && phone.Length >= 10;
    }

    private bool IsValidEmail(string email)
    {
        return !string.IsNullOrEmpty(email) && email.Contains("@");
    }
}

// Обобщенный класс для заказа
class Order<TDelivery, TStruct> where TDelivery : Delivery
{
    public TDelivery Delivery { get; set; }
    public int Number { get; set; }
    public string Description { get; set; }
    public List<Product> Products { get; set; }
    public ContactInfo ContactInfo { get; set; }

    public Order(TDelivery delivery, int number, string description, ContactInfo contactInfo)
    {
        Delivery = delivery;
        Number = number;
        Description = description;
        Products = new List<Product>();
        ContactInfo = contactInfo;
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    public void DisplayAddress()
    {
        Console.WriteLine(Delivery.Address);
    }

    public decimal CalculateTotalPrice()
    {
        decimal total = 0;
        foreach (var product in Products)
        {
            total += product.Price;
        }
        return total;
    }

    // Индексатор для доступа к продуктам
    public Product this[int index]
    {
        get => Products[index];
        set => Products[index] = value;
    }

    // Перегрузка оператора ==
    public static bool operator ==(Order<TDelivery, TStruct> order1, Order<TDelivery, TStruct> order2)
    {
        if (ReferenceEquals(order1, order2)) return true;
        if (order1 is null || order2 is null) return false;
        return order1.Number == order2.Number;
    }

    // Перегрузка оператора !=
    public static bool operator !=(Order<TDelivery, TStruct> order1, Order<TDelivery, TStruct> order2)
    {
        return !(order1 == order2);
    }
}

// Статический класс для валидации адреса
static class AddressValidator
{
    public static bool IsValid(Address address)
    {
        return !string.IsNullOrEmpty(address.Street) &&
               !string.IsNullOrEmpty(address.City) &&
               !string.IsNullOrEmpty(address.PostalCode);
    }
}

// Метод расширения для коллекции продуктов
static class ProductExtensions
{
    public static void DisplayAll(this List<Product> products)
    {
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Name} - {product.Price}");
        }
    }
}

// Обобщенный метод для вычисления общей стоимости
static class OrderExtensions
{
    public static decimal CalculateTotalPrice<TDelivery, TStruct>(this Order<TDelivery, TStruct> order) where TDelivery : Delivery
    {
        decimal total = 0;
        foreach (var product in order.Products)
        {
            total += product.Price;
        }
        return total;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создание адреса и контактной информации
        Address address = new Address("some random address", "Moscow", "12345");
        ContactInfo contactInfo = new ContactInfo("+1234567890", "test@gmail.com");

        // Создание доставки на дом
        HomeDelivery homeDelivery = new HomeDelivery { Address = address.ToString(), CourierName = "Ivan Ivanov" };

        // Создание заказа
        Order<HomeDelivery, object> order = new Order<HomeDelivery, object>(homeDelivery, 1, "Test Order", contactInfo);

        // Добавление продуктов в заказ
        order.AddProduct(new Product("Apples", 1200));
        order.AddProduct(new Product("Cucumber", 20));

        // Вывод информации о заказе
        order.DisplayAddress();
        order.Products.DisplayAll();
        Console.WriteLine($"Total Price: {order.CalculateTotalPrice()}");

        // Выполнение доставки
        order.Delivery.Deliver();

        // Проверка индексатора
        Console.WriteLine($"Product at index 0: {order[0].Name}");

        // Проверка перегрузки оператора
        Order<HomeDelivery, object> order2 = new Order<HomeDelivery, object>(homeDelivery, 1, "Test Order", contactInfo);
        Console.WriteLine($"Are orders equal? {order == order2}");
    }
}
