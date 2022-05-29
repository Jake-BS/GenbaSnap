using GenbaSnap.Models;

Console.WriteLine("Hello, please enter the number of players");
string? stringNOfPlayers = null;
string prompt = "Please enter a number of players (from 2 up to 4):";
int nOfPlayers = 0;
while ((stringNOfPlayers == null || stringNOfPlayers == "") || nOfPlayers < 2 || nOfPlayers > 4)
{
    stringNOfPlayers = Console.ReadLine();
    if (stringNOfPlayers == null || stringNOfPlayers == "") Console.WriteLine(prompt);
    else 
    {
        try
        {
            nOfPlayers = int.Parse(stringNOfPlayers);
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
var table = new Table(nOfPlayers);
table.Start();
