using System.Collections.Concurrent;

namespace Interview;
/*
 * Добавлен интерфейс ITerm и два его наследника FixedTime
 * и RealTime, чтобы во время тестирования можно было
 * задавать собственное время (с помощью FixedTime).
 */
public interface ITime
{
    DateTime Now { get; }
}

public class FixedTime : ITime
{
    public DateTime Now { get; }
    public FixedTime(DateTime time)
    {
        Now = time;
    }
}

public class RealTime : ITime
{
    public DateTime Now => DateTime.Now;
}
public class Greeter
{
    // Используем потокобезопасную структуру данных вместо LIst.
    static readonly ConcurrentBag<string> Visitors = new();
    static ITime? _timeGetter;
    public Greeter()
    {
        _timeGetter = new RealTime();
    }
    public Greeter(ITime? timeGetter)
    {
        _timeGetter = timeGetter;
    }
    // name и surname теперь вводятся друг за другом, а переменная age убрана за ненадобностью.
    public void PrintInvitation(string name, string surname, bool sex, int friendsCount, bool doNotHaveFriends)
    {
        try
        {
            PrintHello();
            Console.Write(sex ? " Mr " : " Mrs ");
            Console.WriteLine(name + " " + surname + ".");
            Console.WriteLine("We are glad to see you" + (Visitors.Contains(name) ? " again." : "."));
            Visitors.Add(name);
            Console.WriteLine("We want to invite you{0} to the party.", doNotHaveFriends ?
                String.Empty : " and your" + friendsCount + " friends");
        }
        // В этом блоке можед быть только исключение ввода/вывода.
        catch (IOException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // Выносим написание приветсвия в отдельный метод для лучшей читаемости. 
    private void PrintHello()
    {
        // Используем switch вместо if чтобы не вызывать DateTime.Now много раз
        // (чтобы код лучше работал в условиях высокой нагруженности).
        switch (_timeGetter?.Now.Hour)
        {
            case < 10:
                Console.Write("Good morning");
                break;
            case >= 20:
                Console.Write("Good evening");
                break;
            default:
                Console.Write("Good day");
                break;
        }
    }
}
