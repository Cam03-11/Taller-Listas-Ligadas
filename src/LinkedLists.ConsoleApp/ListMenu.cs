using LinkedLists.Backend;

namespace LinkedLists.ConsoleApp;

public class ListMenu<T> where T : IComparable<T>
{
    private readonly DoublyLinkedList<T> _list = new();
    private readonly Func<string, T> _parse;

    public ListMenu(Func<string, T> parse)
    {
        _parse = parse;
    }

    public void Run()
    {
        string option;
        do
        {
            option = ShowMenu();
            Console.WriteLine();

            switch (option)
            {
                case "1": Add(); break;
                case "2": ShowValues(_list.GetForward()); break;
                case "3": ShowValues(_list.GetBackward()); break;
                case "4": SortDescending(); break;
                case "5": ShowModes(); break;
                case "6": ShowGraph(); break;
                case "7": Exists(); break;
                case "8": RemoveFirst(); break;
                case "9": RemoveAll(); break;
                case "0": Console.WriteLine("Hasta luego."); break;
                default: Console.WriteLine("Opcion no valida."); break;
            }

            Console.WriteLine();
        } while (option != "0");
    }

    private static string ShowMenu()
    {
        Console.WriteLine("1. Adicionar.                  6. Mostrar grafico.");
        Console.WriteLine("2. Mostrar hacia adelante.     7. Existe.");
        Console.WriteLine("3. Mostrar hacia atras.        8. Eliminar una ocurrencia.");
        Console.WriteLine("4. Ordenar descendentemente.   9. Eliminar todas las ocurrencias.");
        Console.WriteLine("5. Mostrar la(s) moda(s).      0. Salir.");
        Console.Write("Elija una opcion: ");
        return Console.ReadLine()?.Trim() ?? "0";
    }

    private void Add()
    {
        Console.Write("Ingrese uno o varios valores separados por coma: ");
        var input = Console.ReadLine() ?? string.Empty;
        var parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var added = 0;
        foreach (var part in parts)
        {
            if (TryRead(part, out var value))
            {
                _list.Add(value);
                added++;
            }
        }

        Console.WriteLine($"Se adicionaron {added} elemento(s).");
    }

    private void ShowValues(IEnumerable<T> values)
    {
        if (IsEmptyList())
        {
            return;
        }

        Console.WriteLine(string.Join(", ", values));
    }

    private void SortDescending()
    {
        if (IsEmptyList())
        {
            return;
        }

        _list.SortDescending();
        Console.WriteLine("La lista quedo ordenada descendentemente:");
        Console.WriteLine(string.Join(", ", _list.GetForward()));
    }

    private void ShowModes()
    {
        if (IsEmptyList())
        {
            return;
        }

        var modes = _list.GetModes();
        if (modes.Count == 1)
        {
            Console.WriteLine($"La moda es {modes[0]}.");
            return;
        }

        var allButLast = string.Join(", ", modes.Take(modes.Count - 1));
        Console.WriteLine($"Las modas son {allButLast} y {modes[^1]}.");
    }

    private void ShowGraph()
    {
        if (IsEmptyList())
        {
            return;
        }

        var occurrences = _list.GetOccurrences();
        var width = occurrences.Max(o => o.Value.ToString()!.Length);

        foreach (var (value, count) in occurrences)
        {
            Console.WriteLine($"{value.ToString()!.PadRight(width)} {new string('*', count)}");
        }
    }

    private void Exists()
    {
        if (!AskValue("Ingrese el valor a buscar: ", out var value))
        {
            return;
        }

        Console.WriteLine(_list.Exists(value)
            ? $"Si existe el {value} en la lista."
            : $"No existe el {value} en la lista.");
    }

    private void RemoveFirst()
    {
        if (IsEmptyList() || !AskValue("Ingrese el valor a eliminar: ", out var value))
        {
            return;
        }

        Console.WriteLine(_list.RemoveFirst(value)
            ? $"Se elimino la primera ocurrencia de {value}."
            : $"No existe el {value} en la lista.");
    }

    private void RemoveAll()
    {
        if (IsEmptyList() || !AskValue("Ingrese el valor a eliminar: ", out var value))
        {
            return;
        }

        var removed = _list.RemoveAll(value);
        Console.WriteLine(removed > 0
            ? $"Se eliminaron {removed} ocurrencia(s) de {value}."
            : $"No existe el {value} en la lista.");
    }

    private bool IsEmptyList()
    {
        if (_list.IsEmpty)
        {
            Console.WriteLine("La lista esta vacia.");
            return true;
        }

        return false;
    }

    private bool AskValue(string message, out T value)
    {
        Console.Write(message);
        return TryRead(Console.ReadLine()?.Trim() ?? string.Empty, out value);
    }

    private bool TryRead(string input, out T value)
    {
        value = default!;

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Debe ingresar un valor.");
            return false;
        }

        try
        {
            value = _parse(input);
            return true;
        }
        catch (Exception ex) when (ex is FormatException or OverflowException)
        {
            Console.WriteLine($"'{input}' no es un valor valido.");
            return false;
        }
    }
}
