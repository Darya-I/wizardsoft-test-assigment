class Program
{
    static async Task Main(string[] args)
    {
        var app = new TreeClientApp("https://localhost:5001/");
        await app.RunAsync();
    }
}
