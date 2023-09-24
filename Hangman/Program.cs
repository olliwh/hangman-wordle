// See https://aka.ms/new-console-template for more information
using Hangman;

Console.WriteLine("Hello, World!");
HangmanClass hm = new HangmanClass();
Wordle w = new Wordle();
while (true)
{
    //hm.StartGame();
    w.StartGame();
    Console.WriteLine("Try again? (Y/N");
    string answer = Console.ReadLine().ToLower();
    if (answer == "n") break;

}
