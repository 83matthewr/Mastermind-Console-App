
namespace MastermindApp
{
    class Mastermind
    {
        // Declare consts for game rules
        public const int NUM_GUESSES = 12;
        const int MAX_CODE = 6666;
        const int MIN_CODE = 1111;
        public const int MAX_DIGIT = 6;
        public const int MIN_DIGIT = 1;
        public static int CODE_LENGTH = MAX_CODE.ToString().Length;

        public string secretCode;
        public int guesses;
        public bool success;

        public Mastermind()
        {
            secretCode = GenerateSecretCode();
            guesses = NUM_GUESSES;
            success = false;
        }

        // Return a secret code generated according to game rules
        private string GenerateSecretCode()
        {
            string secretCode = "";
            for (int i = 0; i < CODE_LENGTH; i++)
            {
                secretCode += new Random().Next(MIN_DIGIT, MAX_DIGIT + 1).ToString();
            }
            return secretCode;
        }

        // Validate guessCode string and compare guessCode with secretCode and update game state
        // Return a string of +s and -s for the guess score
        public string NewGuess(string guessCode)
        {
            while (!ValidateGuess(guessCode))
            {
                Console.Write("Invalid Code. Please enter a 4 digit code with digits between 1 and 6.\nEnter a guess code: ");
                guessCode = Console.ReadLine();
            }
            guesses--;
            string guessScore = ScoreGuessCode(guessCode);
            if (guessScore == "++++") success = true;
            return guessScore;
        }

        // Validate a 4 digit code of digits between 1 and 6 entered by the user
        // Return true if valid and false if invalid
        private static bool ValidateGuess(string guessString)
        {
            try
            {
                int guessInt = Int32.Parse(guessString);
                if (guessInt > MAX_CODE || guessInt < MIN_CODE) return false;
                for (int i = 0; i < CODE_LENGTH; i++)
                {
                    int guessDigit = Int32.Parse(guessString.Substring(i, 1));
                    if (guessDigit < MIN_DIGIT || guessDigit > MAX_DIGIT) return false;
                }
                return true;
            } 
            catch 
            {
                return false;
            }
        }

        // Compare guessString with secretCode
        // Return a string of +s and -s for the guess score
        private string ScoreGuessCode(string guessString)
        {
            if (guessString == secretCode)
            {
                return "++++";
            }

            string tempSecretCode = secretCode;
            string tempGuessCode = guessString;
            string guessScore = "";

            // Calculate plusses
            for (int i = 0; i < CODE_LENGTH; i++)
            {
                if (guessString[i] == secretCode[i])
                {
                    guessScore += '+';
                    tempGuessCode = ReplaceCharWithX(tempGuessCode, i);
                    tempSecretCode = ReplaceCharWithX(tempSecretCode, i);
                }
            }

            // Calculate minuses
            for (int i = 0; i < CODE_LENGTH; i++)
            {
                if (tempGuessCode[i] != 'X' && tempSecretCode.Contains(tempGuessCode[i]))
                {
                    guessScore += '-';
                    tempSecretCode = ReplaceCharWithX(tempSecretCode, tempSecretCode.IndexOf(tempGuessCode[i]));
                    tempGuessCode = ReplaceCharWithX(tempGuessCode, i);
                }
            }

            return guessScore;
        }

        // Utility function to return a copy of a string with the char at index replaced with X
        private static string ReplaceCharWithX(string text, int index)
        {
            string newText = text.Insert(index + 1, "X");
            return newText.Remove(index, 1);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Mastermind game = new Mastermind();

                // Introducton and Game Rules
                Console.WriteLine("Welcome to Mastermind!");
                Console.WriteLine("------------------------");
                Console.WriteLine($"Try to guess the secret {Mastermind.CODE_LENGTH} digit code consisting of digits between {Mastermind.MIN_DIGIT} and {Mastermind.MAX_DIGIT}.");
                Console.WriteLine($"You have {Mastermind.NUM_GUESSES} attempts to guess the right code.");
                Console.WriteLine("For each digit in your guess that matches the number and postion of a digit in the secret code,\n" +
                    "the score includes one plus sign.");
                Console.WriteLine("For each digit in your guess that matches the number but not the position of a digit in the secret code,\n" +
                    "the score includes one minus sign.");
                Console.WriteLine("Good Luck!");
                Console.WriteLine("------------------------\n");

                // User Guess loop
                while (game.guesses > 0 && game.success == false)
                {
                    Console.Write("Enter a guess: ");
                    string guessScore = game.NewGuess(Console.ReadLine());
                    Console.WriteLine("Guess Score: " + guessScore);
                    Console.WriteLine("Guesses left: " + game.guesses);
                    Console.WriteLine("------------------------\n");
                }

                // Display game won or lost
                if (game.success == false)
                {
                    Console.WriteLine("You lose :(");
                    Console.WriteLine("The secret code was: " + game.secretCode);
                }
                else
                {
                    Console.WriteLine("Correct Answer: " + game.secretCode);
                    Console.WriteLine("You solved it!");
                }

                // Prompt to start a new game or exit
                Console.Write("\nStart a new game? (Y/N): ");
                string newGame = Console.ReadLine();
                while (newGame != "Y" && newGame != "N")
                {
                    Console.Write("Please enter either Y or N: ");
                    newGame = Console.ReadLine();
                }
                if (newGame == "Y")
                {
                    Console.Clear();
                    continue;
                }
                break;
            }
            return;
        }

    }
}
