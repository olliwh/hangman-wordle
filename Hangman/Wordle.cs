using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hangman
{
    public class Wordle
    {
        string url = "https://random-word-api.herokuapp.com/word?length=5";
        string checkGuessUrl = "https://api.dictionaryapi.dev/api/v2/entries/en/";
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        public string[]? Get()
        {
            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage response = client.GetAsync($"{url}").Result;
                string json = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<string[]>(json, options);
            }
        }
        //System.Net.HttpStatusCode
        public  string CheckWord(string word)
        {
            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage response = client.GetAsync($"{checkGuessUrl}{word}").Result;
                var json = response.StatusCode;
                return json.ToString();
            }
        }
        public void StartGame()
        {
            string[] word = Get();
            foreach(string w  in word)
            {
                Console.Write(w + " ");
            }
            while (CheckWord(word[0]) != "OK")
            {
                word = Get();
            }
            char[] lettersToGuess = word[0].ToCharArray();
            char[] wordToGuess = lettersToGuess.Select(i => '*').ToArray();
            char[] corectPlace = new char[word.Length];
            List<char> wrongPlace = new List<char>();
            int wrongGuesses = 0;
            List<string> guesses = new List<string>();
            while (true)
            {

                Console.WriteLine("------------------------------");
                Console.WriteLine(wordToGuess);
                Console.WriteLine("------------------------------");
                Console.Write("Correct letter in wroing place: ");
                foreach(var l in wrongPlace)
                {
                    Console.Write(l.ToString() + " ");
                }
                Console.WriteLine();
                foreach(var g in guesses)
                {
                    Console.WriteLine(g);
                }

                if (wrongGuesses == 6)
                {
                    Console.WriteLine("Looser");
                    break;
                }
                Console.WriteLine("Guess a word");
                string guess = Console.ReadLine();
                
                if (guess.Length != 5) Console.WriteLine("wrong input");
                else
                {
                    if (CheckWord(guess) != "OK") Console.WriteLine("not a word");
                    else
                    {
                        guesses.Add(guess);
                        char[] guessArray = guess.ToCharArray();

                        for (var i = 0; i < guessArray.Length; i++)
                        {
                            if (guessArray[i] == lettersToGuess[i])
                            {
                                wordToGuess[i] = guessArray[i];
                                wrongPlace.Remove(guessArray[i]);
                            }
                            else
                            {
                                for (var j = 0; j < guessArray.Length; j++)
                                {
                                    if (guessArray[j] == lettersToGuess[i] && j != i)
                                    {
                                        if (!wrongPlace.Contains(guessArray[j])) wrongPlace.Add(guessArray[j]);
                                    }
                                }
                            }
                        }
                    }

                }
                if(guesses.Count == 6)
                {
                    Console.WriteLine("Looser the word was: ");
                    Console.Write(word[0]);
                    break;
                }
                if (new string(guess) == word[0])
                {
                    Console.WriteLine("You Found it");
                    break;
                }

            }
        }
    }
}
