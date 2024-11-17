public static class InputValidator
{
    public static int GetValidatedInt(string prompt)
    {
        int value;
        do
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            if (int.TryParse(input, out value) && value >= 0)
                return value;

            Console.WriteLine("Введите корректное число.");
        } while (true);
    }

    public static string GetValidatedString(string prompt)
    {
        string? input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(input))
                return input;

            Console.WriteLine("Строка не должна быть пустой.");
        } while (true);
    }
}
