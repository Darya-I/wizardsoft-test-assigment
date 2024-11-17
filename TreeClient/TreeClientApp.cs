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
            Console.WriteLine("1. �������� ������");
            Console.WriteLine("2. ������� ����");
            Console.WriteLine("3. �������� ����");
            Console.WriteLine("4. ������� ����");
            Console.WriteLine("5. �����");
            Console.Write("�������� ��������: ");

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
                    Console.WriteLine("������������ �����. ���������� �����.");
                    break;
            }

            Console.WriteLine("\n������� ����� ������� ��� �����������...");
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
                Console.WriteLine("������ �����.");
                return;
            }

            Console.WriteLine("������� ��������� ������:");
            PrintTree(tree, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"������ ��� �������� ������: {ex.Message}");
        }
    }

    private void PrintTree(IEnumerable<TreeNode> nodes, int level)
    {
        foreach (var node in nodes)
        {
            // ������� ��� ���� � ID � �������
            Console.WriteLine(new string(' ', level * 2) + $"- {node.Name} (ID: {node.Id})");

            // ���������� ������� �������� ��������
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
            var name = InputValidator.GetValidatedString("������� ��� ����: ");
            int? parentId = InputValidator.GetValidatedInt("������� ID ������������� ���� (��� 0 ��� ���������): ");
            parentId = parentId == 0 ? null : parentId;

            var node = new TreeNode { Name = name, ParentId = parentId };
            var createdNode = await _client.CreateNodeAsync(node);

            if (createdNode != null)
                Console.WriteLine($"���� ������ � ID {createdNode.Id}");
            else
                Console.WriteLine("������ ��� �������� ����.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"������ ��� �������� ����: {ex.Message}");
        }
    }

    private async Task UpdateNodeAsync()
    {
        try
        {
            var id = InputValidator.GetValidatedInt("������� ID ���� ��� ����������: ");
            var existingNode = await _client.GetNodeAsync(id);

            if (existingNode == null)
            {
                Console.WriteLine("���� �� ������.");
                return;
            }

            var newName = InputValidator.GetValidatedString($"������� ����� ��� ��� ���� (�������: {existingNode.Name}): ");
            existingNode.Name = newName;

            if (await _client.UpdateNodeAsync(existingNode))
                Console.WriteLine("���� ������� ��������.");
            else
                Console.WriteLine("������ ��� ���������� ����.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"������ ��� ���������� ����: {ex.Message}");
        }
    }

    private async Task DeleteNodeAsync()
    {
        try
        {
            var id = InputValidator.GetValidatedInt("������� ID ���� ��� ��������: ");
            Console.WriteLine($"�� �������, ��� ������ ������� ���� � ID {id}? (y/n)");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation != "y")
            {
                Console.WriteLine("�������� ��������.");
                return;
            }

            if (await _client.DeleteNodeAsync(id))
                Console.WriteLine("���� ������� ������.");
            else
                Console.WriteLine("������ ��� �������� ����.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"������ ��� �������� ����: {ex.Message}");
        }
    }
}
