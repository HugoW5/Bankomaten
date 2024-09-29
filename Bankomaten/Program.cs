using System.Dynamic;
using System.Security.AccessControl;

namespace Bankomaten
{
	internal class Program
	{
		public static string[] Usernames = { "karl", "per", "kalle", "roland", "berra" };
		public static string[] Passwords = { "apelsin123", "kolrot", "1234", "volvo", "fotboll" };

		public static string[,] BankAccountNames =
		{
			{"Lönekonto", "Sparkonto" },
			{"Lönekonto", "Sparkonto" },
			{"Lönekonto", "Sparkonto" },
			{"Lönekonto", "Sparkonto" },
			{"Lönekonto", "Sparkonto" }
		};

		public static double[,] BankAccountBalances = {
			{1000.00, 5000.00},
			{1000.00, 5000.00},
			{1000.00, 5000.00},
			{1000.00, 5000.00},
			{1000.00, 5000.00}
		};


		public static int currentUserIndex = -1;

		static void Main(string[] args)
		{
			Console.WriteLine("Välkommen till banken\n");

			//Login();
			currentUserIndex = 0;

			//check if someone logged in:
			if (currentUserIndex > -1)
			{
				//Set title to current username
				Console.Title = $"Inloggad som {Usernames[currentUserIndex]}";
				Console.Clear();
				NavigationMenu();

			}
		}


		static void NavigationMenu()
		{
			int errorCount = 0;
			while (true)
			{
				Console.SetCursorPosition(0, 0);
				Console.WriteLine("1. Se dina konton och saldo");
				Console.WriteLine("2. Överföring mellan konton");
				Console.WriteLine("3. Ta ut pengar");
				Console.WriteLine("4. Logga ut");
				Console.Write("Val:          ");
				Console.SetCursorPosition(5, Console.GetCursorPosition().Top);
				string? userChoise = Console.ReadLine().Trim();

				bool error = false;
				switch (userChoise)
				{
					case "1":
						AccountsOverview();
						break;
					case "2":
						Transaction();
						break;
					case "3":
						break;
					case "4":
						break;
					default:
						error = true;
						errorCount++;
						printError(errorCount);
						break;
				}
				//Clear errors if a correct opiton is selected
				if (!error)
				{
					errorCount = 0;
					Console.SetCursorPosition(0, 6);
					Console.Write("                  ");
				}
			}



			void printError(int errorCount)
			{
				Console.SetCursorPosition(0, 6);
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Ogiltig val ({errorCount})");
				Console.ForegroundColor = ConsoleColor.White;
			}


		}




		static void Transaction()
		{
			Console.Clear();
			Console.WriteLine("--Konton--\n");
			for (int i = 0; i < BankAccountBalances.GetLength(1); i++)
			{
				Console.WriteLine($"{i}) {BankAccountNames[currentUserIndex, i]} : {BankAccountBalances[currentUserIndex, i].ToString("N2")} SEK");
			}
			while (true)
			{
				bool error = false;
				try
				{
					Console.Write("Från Konto: ");
					int fromAccount = int.Parse(Console.ReadLine());
					string fromAccountName = BankAccountNames[currentUserIndex, fromAccount];
					PrintMessage($"Hittade: {fromAccountName}", ConsoleColor.Green);
					Console.Write("\nTill Konto: ");
					int toAccount = int.Parse(Console.ReadLine());
					string toAccountName = BankAccountNames[currentUserIndex, toAccount];

					PrintMessage($"Hittade: {toAccountName}\n", ConsoleColor.Green);

					while (true)
					{
						Console.Write("Summa att överföra: ");
						if (double.TryParse(Console.ReadLine(), out double amount))
						{
							if (amount <= BankAccountBalances[currentUserIndex, fromAccount])
							{
								BankAccountBalances[currentUserIndex, fromAccount] -= amount;
								BankAccountBalances[currentUserIndex, toAccount] += amount;
								PrintMessage($"Skickade {amount} SEK från {fromAccountName} till {toAccountName}", ConsoleColor.Green);
								Console.ReadLine();
								Console.Clear();
								break;
							}
							else
							{
								PrintMessage($"Du kan inte sicka mer än ditt saldo ({BankAccountBalances[currentUserIndex, fromAccount]} SEK) på konto {BankAccountNames[currentUserIndex, fromAccount]}\n", ConsoleColor.Red);
							}


						}
					}
				}
				catch (Exception e)
				{
					error = true;
					PrintMessage("Fel\n", ConsoleColor.Red);
				}
				if (!error)
				{
					Console.Clear();
					break;
				}
			}
		}

		static void PrintMessage(string msg, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.Write(msg);
			Console.ForegroundColor = ConsoleColor.White;
		}

		static void AccountsOverview()
		{
			Console.Clear();
			Console.WriteLine("--Konto Översikt--\n");
			double total = 0;
			for (int i = 0; i < BankAccountBalances.GetLength(1); i++)
			{
				Console.WriteLine($"{BankAccountNames[currentUserIndex, i]} : {BankAccountBalances[currentUserIndex, i].ToString("N2")} SEK");
				total += BankAccountBalances[currentUserIndex, i];
			}
			Console.WriteLine($"\nTotalt: {total.ToString("N2")} SEK");
			Console.ReadLine();
			Console.Clear();
		}

		static void Login()
		{
			Console.WriteLine("Var god logga in");
			int tries = 5;
			while (tries > 0)
			{
				Console.Write("Användarnamn: ");
				string? inputUsername = Console.ReadLine();
				Console.Write("Lösenord: ");
				string? inputPassword = Console.ReadLine();

				if (Authenticate(inputUsername.Trim(), inputPassword.Trim()))
				{
					//Get inputUsername postion in Usernames[]
					int index = Usernames.ToList().IndexOf(inputUsername);
					currentUserIndex = index;
					return;
				}
				//Authentication faied
				Console.WriteLine("Fel användarnamn eller lösenord!");
				//Decrease the number of login tries
				tries--;
			}
			//Exceeded limit of login tries
			Console.WriteLine("Du har gjort för många inloggnings försök");

		}

		static bool Authenticate(string loginUsername, string loginPassword)
		{
			for (int i = 0; i < Usernames.Length; i++)
			{
				//Loop and check matches in Username[] and Password[]
				if (loginUsername == Usernames[i])
				{
					if (loginPassword == Passwords[i])
					{
						return true;
					}
				}
			}
			//No match
			return false;
		}
	}
}

