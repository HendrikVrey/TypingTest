using System;
using System.Collections.Generic;
using System.Diagnostics;

class TypingTest
{
    static void Main(string[] args)
    {
        //Load dictionary of words
        string[] words = LoadDictionary();

        //Ask user for the time duration
        Console.WriteLine("Select typing test duration (in seconds): 30, 60, 120");
        int testDuration = int.Parse(Console.ReadLine());

        //Prepare random words for display
        Random random = new Random();
        List<string> selectedWords = new List<string>();
        for (int i = 0; i < 500; i++)
        {
            selectedWords.Add(words[random.Next(words.Length)]);
        }

        //Set initial cursor position for typing
        int cursorTop = Console.CursorTop + 1;  // Move below the words

        //Start the timer
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int wordsTyped = 0;
        int totalTyped = 0;  //Total characters typed
        int correctChars = 0;

        //Store each word for validation purposes
        string[] wordList = selectedWords.ToArray();
        int currentWordIndex = 0;
        string currentWord = wordList[currentWordIndex];
        List<char> typedWord = new List<char>();

        //List to store if a word's characters were correct (green or red)
        List<List<ConsoleColor>> typedColors = new List<List<ConsoleColor>>();

        bool timerExpired = false;

        while (!timerExpired)
        {
            //Clear the console and redraw the words with current progress
            Console.SetCursorPosition(0, cursorTop);
            DisplayWords(wordList, typedColors, typedWord, currentWordIndex);

            //Check if the timer has expired
            if (stopwatch.Elapsed.TotalSeconds >= testDuration)
            {
                timerExpired = true;
                break;
            }

            //Get the next key from the user (includes non-printable keys)
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                char userInput = keyInfo.KeyChar;

                //Handle backspace
                if (keyInfo.Key == ConsoleKey.Backspace && typedWord.Count > 0)
                {
                    typedWord.RemoveAt(typedWord.Count - 1);
                    totalTyped--;
                    continue;
                }

                //Handle spacebar to move to the next word
                if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    //Store the color of typed characters for the current word
                    List<ConsoleColor> wordColors = new List<ConsoleColor>();
                    for (int j = 0; j < currentWord.Length; j++)
                    {
                        if (j < typedWord.Count && typedWord[j] == currentWord[j])
                        {
                            wordColors.Add(ConsoleColor.Green);  // Correct letter
                        }
                        else
                        {
                            wordColors.Add(ConsoleColor.Red);  // Incorrect letter
                        }
                    }
                    typedColors.Add(wordColors);

                    //Check if the word is completely correct
                    bool isWordComplete = true;
                    for (int j = 0; j < currentWord.Length; j++)
                    {
                        if (j >= typedWord.Count || typedWord[j] != currentWord[j])
                        {
                            isWordComplete = false;
                            break;
                        }
                    }

                    if (isWordComplete)
                    {
                        wordsTyped++;
                    }

                    //Move to the next word
                    currentWordIndex++;
                    if (currentWordIndex < wordList.Length)
                    {
                        currentWord = wordList[currentWordIndex];
                        typedWord.Clear();
                    }
                    continue;
                }

                //Skip non-printable keys
                if (char.IsControl(userInput)) continue;

                //Store the typed character
                typedWord.Add(userInput);

                //Update total typed characters
                totalTyped++;
            }
        }

        //Stop the timer
        stopwatch.Stop();

        //Clear the console
        Console.Clear();

        //Calculate Words Per Minute (WPM)
        double minutes = testDuration / 60.0;
        int wpm = (int)(wordsTyped / minutes);

        //Display results
        Console.SetCursorPosition(0, 0);  // Move to the top of the console
        Console.WriteLine("Test completed.");
        Console.WriteLine($"Your Words Per Minute (WPM) is: {wpm}");

        //Prompt user to press any key to exit
        Thread.Sleep(5000);
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey(true);
    }

    //Display the words, keeping the correct/incorrect colors for typed words
    static void DisplayWords(string[] wordList, List<List<ConsoleColor>> typedColors, List<char> typedWord, int currentWordIndex)
    {
        for (int i = 0; i < wordList.Length; i++)
        {
            string word = wordList[i];

            if (i < currentWordIndex)  //Words already completed
            {
                List<ConsoleColor> wordColors = typedColors[i];
                for (int j = 0; j < word.Length; j++)
                {
                    Console.ForegroundColor = wordColors[j];
                    Console.Write(word[j]);
                }
            }
            else if (i == currentWordIndex)  //Current word being typed
            {
                for (int j = 0; j < word.Length; j++)
                {
                    if (j < typedWord.Count)
                    {
                        //Correct or incorrect letter based on user input
                        if (typedWord[j] == word[j])
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        Console.Write(typedWord[j]);
                    }
                    else
                    {
                        //Remaining letters in the current word: default white
                        Console.ResetColor();
                        Console.Write(word[j]);
                    }
                }
            }
            else  //Future words not yet typed
            {
                Console.ResetColor();
                Console.Write(word);
            }

            //Add space between words
            if (i < wordList.Length - 1)
            {
                Console.Write(" ");
            }
        }

        //Reset the console color at the end of the line
        Console.ResetColor();
    }

    //Load dictionary TO-DO: Add larger dictionary/Better way to load a dictionary
    static string[] LoadDictionary()
    {
        return new string[] { "apple", "banana", "orange", "grape", "watermelon", "cherry", "strawberry", "peach", "melon", "blueberry",
                              "govern", "go", "like", "leave", "there", "just", "tell", "they", "begin", "how", "can", "all", "year", "it", "good",
                              "set", "each", "right", "here", "where", "change", "if", "man", "long", "one", "new", "about", "right", "well", "give",
                              "how", "eye", "one", "up", "most", "ask", "work", "may", "without", "order", "the", "course", "old", "new", "open", "out",
                              "develop", "who" };
    }
}
