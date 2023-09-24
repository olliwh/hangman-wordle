using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hangman
{
    public class HangmanClass
    {
        string url = "https://random-word-api.herokuapp.com/word?length=5";
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        public string[]? Get(int id)
        {
            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage response = client.GetAsync($"{url}").Result;
                string json = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<string[]>(json, options);
            }
        }
        public void StartGame()
        {
            string[] word = Get(5);
            char[] lettersToGuess = word[0].ToCharArray();
            var wordToGuess = lettersToGuess.Select(i => '*').ToArray();
            int corectGuesses = 0;
            int wrongGuesses = 0;
            string guesses = "";
            while (true)
            {
                foreach (var sign in DrawMan(wrongGuesses))
                {
                    Console.WriteLine(sign);
                }


                Console.WriteLine(wordToGuess);
                Console.WriteLine();
                Console.WriteLine(guesses);

                if (wrongGuesses == 7)
                {
                    Console.WriteLine("Looser");
                    break;
                }
                Console.WriteLine("guess a letter");
                string? input = Console.ReadLine();
                bool isLetter = Char.TryParse(input, out char guess);
                if (!isLetter) Console.WriteLine("Wrong input");
                else
                {
                    if (word[0].Contains(guess))
                    {
                        for (var i = 0; i < lettersToGuess.Length; i++)
                        {
                            if (lettersToGuess[i] == guess)
                                wordToGuess.SetValue(guess, i);
                        }
                        corectGuesses++;

                        if (new string(wordToGuess) == word[0])
                        {
                            
                            Console.WriteLine("You guessed it");
                            break;
                        }

                    }
                    else if (!guesses.Contains(guess))
                    {
                        guesses += guess.ToString();
                        wrongGuesses++;
                    }
                    else wrongGuesses++;

                    
                }
            }

            Console.WriteLine("The word was: " + word[0]);


            Console.WriteLine("-----------------------------------------------------------");


        }
        public static string[] DrawMan(int wrongGuesses)
        {
            string[] start = { "", "________ ", "|", "|", "|", "|", "|", "|" };
            string[] man2 = { "|  |", "|  0", "|  |", "| /|", "| /|\\", "| /", "| / \\" };
            if (wrongGuesses == 0) return start;
            for (var i = 0; i < wrongGuesses; i++)
            {
                if (i == 0) start[2] = man2[i];
                if (i == 1) start[3] = man2[i];
                if (i == 2) start[4] = man2[i];
                if (i == 3) start[4] = man2[i];
                if (i == 4) start[4] = man2[i];
                if (i == 5) start[5] = man2[i];
                if (i == 6) start[5] = man2[i];
            }
            return start;
        }
    }
}
