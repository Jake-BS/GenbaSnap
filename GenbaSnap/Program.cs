using GenbaSnap.Models;

Console.WriteLine("Hello, please enter the number of players");
string? stringNOfPlayers = null;
string prompt = "Please enter the number of players:";
while (stringNOfPlayers == null || stringNOfPlayers == "")
{
    stringNOfPlayers = Console.ReadLine();
    if (stringNOfPlayers == null || stringNOfPlayers == "") Console.WriteLine(prompt);
    else 
    {
        try
        {
            int.Parse(stringNOfPlayers);
        }
        catch
        {
            Console.WriteLine("Number of players must be an integer");
            stringNOfPlayers = null;
            Console.WriteLine(prompt);
        }
    }
}
Console.WriteLine("You have chosen to play with " + stringNOfPlayers + " players");
int nOfPlayers = int.Parse(stringNOfPlayers);
var table = new Table(nOfPlayers);
table.Start();
