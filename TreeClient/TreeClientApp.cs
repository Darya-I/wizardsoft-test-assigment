public class TreeClientApp
{
    private readonly TreeApiClient _client;

    public TreeClientApp(string baseUrl)
    {
        _client = new TreeApiClient(baseUrl);
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==== Tree API Client ====");
            Console.WriteLine("1. Показать дерево");
            Console.WriteLine("2. Создать узел");
            Console.WriteLine("3. Обновить узел");
            Console.WriteLine("4. Удалить узел");
            Console.WriteLine("5. Выйти");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ShowTreeAsync();
                    break;
                case "2":
                    await CreateNodeAsync();
                    break;
                case "3":
                    await UpdateNodeAsync();
                    break;
                case "4":
                    await DeleteNodeAsync();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    private async Task ShowTreeAsync()
    {
        try
        {
            var tree = await _client.GetTreeAsync();
            if (tree == null || !tree.Any())
            {
                Console.WriteLine("Дерево пусто.");
                return;
            }

            Console.WriteLine("Текущая структура дерева:");
            PrintTree(tree, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке дерева: {ex.Message}");
        }
    }

    private void PrintTree(IEnumerable<TreeNode> nodes, int level)
    {
        foreach (var node in nodes)
        {
            // Выводим имя узла и ID в скобках
            Console.WriteLine(new string(' ', level * 2) + $"- {node.Name} (ID: {node.Id})");

            // Рекурсивно выводим дочерние элементы
            if (node.Children != null && node.Children.Any())
            {
                PrintTree(node.Children, level + 1);
            }
        }
    }


    private async Task CreateNodeAsync()
    {
        try
        {
            var name = InputValidator.GetValidatedString("Введите имя узла: ");
            int? parentId = InputValidator.GetValidatedInt("Введите ID родительского узла (или 0 для корневого): ");
            parentId = parentId == 0 ? null : parentId;

            var node = new TreeNode { Name = name, ParentId = parentId };
            var createdNode = await _client.CreateNodeAsync(node);

            if (createdNode != null)
                Console.WriteLine($"Узел создан с ID {createdNode.Id}");
            else
                Console.WriteLine("Ошибка при создании узла.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании узла: {ex.Message}");
        }
    }

    private async Task UpdateNodeAsync()
    {
        try
        {
            var id = InputValidator.GetValidatedInt("Введите ID узла для обновления: ");
            var existingNode = await _client.GetNodeAsync(id);

            if (existingNode == null)
            {
                Console.WriteLine("Узел не найден.");
                return;
            }

            var newName = InputValidator.GetValidatedString($"Введите новое имя для узла (текущее: {existingNode.Name}): ");
            existingNode.Name = newName;

            if (await _client.UpdateNodeAsync(existingNode))
                Console.WriteLine("Узел успешно обновлен.");
            else
                Console.WriteLine("Ошибка при обновлении узла.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении узла: {ex.Message}");
        }
    }

    private async Task DeleteNodeAsync()
    {
        try
        {
            var id = InputValidator.GetValidatedInt("Введите ID узла для удаления: ");
            Console.WriteLine($"Вы уверены, что хотите удалить узел с ID {id}? (y/n)");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation != "y")
            {
                Console.WriteLine("Удаление отменено.");
                return;
            }

            if (await _client.DeleteNodeAsync(id))
                Console.WriteLine("Узел успешно удален.");
            else
                Console.WriteLine("Ошибка при удалении узла.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении узла: {ex.Message}");
        }
    }
}
