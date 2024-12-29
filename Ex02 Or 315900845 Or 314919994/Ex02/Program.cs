using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Checkers!");


            string player1Name = GetPlayerName("Player 1");
            int boardSize = GetBoardSize();

            // אפשרות לבחור אם לשחק נגד שחקן נוסף או נגד המחשב
            Console.WriteLine("Do you want to play against another player or the computer?");
            Console.WriteLine("Enter '1' for two players or '2' for playing against the computer.");
            string playerModeChoice = Console.ReadLine();

            Player player2;

            if (playerModeChoice == "1")
            {
                string player2Name = GetPlayerName("Player 2");
                player2 = new HumanPlayer(player2Name, 'O');
            }
            else
            {
                // בקשת רמת קושי מהמחשב
                Console.WriteLine("Choose difficulty level: Easy (e), Regular (r), Hard (h)");
                string difficultyChoice = Console.ReadLine().ToLower();

                // אם המשתמש בחר אפשרות לא תקינה, נבקש לבחור שוב
                while (difficultyChoice != "e" && difficultyChoice != "r" && difficultyChoice != "h")
                {
                    Console.WriteLine("Invalid choice. Please enter 'e', 'r', or 'h' for the difficulty level.");
                    difficultyChoice = Console.ReadLine().ToLower();
                }
                // כאן ניצור שחקן ממוחשב עבור Player 2
                player2 = new ComputerPlayer("Computer", 'O', difficultyChoice);
            }

            Player player1 = new HumanPlayer(player1Name, 'X');

            Game game = new Game(player1, player2, boardSize);
            game.Start();
        }

        private static string GetPlayerName(string playerPrompt)
        {
            while (true)
            {
                Console.WriteLine($"{playerPrompt}, enter your name (max 20 characters, no spaces):");
                string name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name) && name.Length <= 20 && !name.Contains(" "))
                    return name;

                Console.WriteLine("Invalid name. Please try again.");
            }
        }

        private static int GetBoardSize()
        {
            while (true)
            {
                Console.WriteLine("Enter board size (6, 8, or 10):");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int size) && (size == 6 || size == 8 || size == 10))
                    return size;

                Console.WriteLine("Invalid board size. Please enter 6, 8, or 10.");
            }
        }

        // מחלקת שחקן ממוחשב
        public class ComputerPlayer : Player
        {
            private string _difficulty;

            public ComputerPlayer(string name, char symbol, string difficulty) : base(name, symbol)
            {
                _difficulty = difficulty; // רמת הקושי שנבחרה
            }

            // מתוד הלקיחה של מהלך אוטומטי מהמחשב לפי רמת הקושי
            public void MakeMove(Board board)
            {
                switch (_difficulty)
                {
                    case "e": // Easy - בחירת מהלך אקראי
                        MakeEasyMove(board);
                        break;
                    case "r": // Regular - בחירת מהלך יחסית חכמה
                        MakeRegularMove(board);
                        break;
                    case "h": // Hard - בחירת מהלך אופטימלי יותר (אלגוריתם מתקדם)
                        MakeHardMove(board);
                        break;
                }
            }

            // מהלך אקראי - רמת קושי Easy
            private void MakeEasyMove(Board board)
            {
                Random random = new Random();
                List<Move> possibleMoves = GetPossibleMoves(board);
                if (possibleMoves.Any())
                {
                    Move randomMove = possibleMoves[random.Next(possibleMoves.Count)];
                    board.MovePiece(randomMove.FromRow, randomMove.FromCol, randomMove.ToRow, randomMove.ToCol);
                    Console.WriteLine($"{Name} (Easy) moved from {randomMove.FromRow},{randomMove.FromCol} to {randomMove.ToRow},{randomMove.ToCol}");
                }
            }

            // מהלך רגיל - רמת קושי Regular (פשוט יותר מחושב)
            private void MakeRegularMove(Board board)
            {
                List<Move> possibleMoves = GetPossibleMoves(board);
                if (possibleMoves.Any())
                {
                    // לבחור את המהלך הכי טוב מתוך המהלכים האפשריים
                    Move bestMove = possibleMoves.First(); // יש מקום לשפר כאן (לפי אלגוריתם)
                    board.MovePiece(bestMove.FromRow, bestMove.FromCol, bestMove.ToRow, bestMove.ToCol);
                    Console.WriteLine($"{Name} (Regular) moved from {bestMove.FromRow},{bestMove.FromCol} to {bestMove.ToRow},{bestMove.ToCol}");
                }
            }

            // מהלך קשה - רמת קושי Hard (אלגוריתם מתקדם שמחפש את המהלך הכי טוב)
            private void MakeHardMove(Board board)
            {
                List<Move> possibleMoves = GetPossibleMoves(board);
                if (possibleMoves.Any())
                {
                    // בחירת המהלך הטוב ביותר - זהו אלגוריתם מתקדם שדורש יותר חישובים
                    Move bestMove = possibleMoves.Last(); // נבחר את המהלך האחרון (דוגמא פשוטה)
                    board.MovePiece(bestMove.FromRow, bestMove.FromCol, bestMove.ToRow, bestMove.ToCol);
                    Console.WriteLine($"{Name} (Hard) moved from {bestMove.FromRow},{bestMove.FromCol} to {bestMove.ToRow},{bestMove.ToCol}");
                }
            }

            // פונקציה שמחזירה את המהלכים האפשריים למחשב
            private List<Move> GetPossibleMoves(Board board)
            {
                // לדוגמה, מחזירים רשימה ריקה (זהו רק placeholder וצריך לממש את הלוגיקה)
                return new List<Move>();
            }
        }

        // מחלקת Move לאחסון המהלך
        public class Move
        {
            public int FromRow { get; set; }
            public int FromCol { get; set; }
            public int ToRow { get; set; }
            public int ToCol { get; set; }
        }

    }
}
