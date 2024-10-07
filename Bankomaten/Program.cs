using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Authentication;
using System.Text;

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

			Login();

			//check if someone logged in:
			if (currentUserIndex > -1)
			{
				//Set title to current username
				Console.Title = $"Inloggad som {Usernames[currentUserIndex]}";
				NavigationMenu();

			}
		}

		static void NavigationMenu()
		{
			Console.Clear();
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
						Withdraw();
						break;
					case "4":
						Logout();
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
					break;
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

		static void Logout()
		{
			Console.Clear();
			currentUserIndex = -1;
			Login();
		}

		static void Withdraw()
		{
			Console.Clear();
			Console.WriteLine("--Ta ut pengar--\n");
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
					int fromAccount = int.Parse(ReadLineWithCancel());
					string fromAccountName = BankAccountNames[currentUserIndex, fromAccount];
					PrintMessage($"Hittade: {fromAccountName}\n", ConsoleColor.Green);
					while (true)
					{
						Console.Write("Summa att ta ut: ");
						if (double.TryParse(ReadLineWithCancel(), out double amount))
						{
							if (amount <= BankAccountBalances[currentUserIndex, fromAccount])
							{
								Console.Write("\nLösenord: ");
								string password = ReadLineWithCancel();
								if (Authenticate(Usernames[currentUserIndex], password))
								{

									BankAccountBalances[currentUserIndex, fromAccount] -= amount;

									PrintMessage($"Du tog ut {amount} SEK från {BankAccountNames[currentUserIndex, fromAccount]}\n", ConsoleColor.Green);

									Console.WriteLine("Du erhåller: ");
									string bankNotes = CalculateBanknotes(amount);
									Console.WriteLine(bankNotes);
									Console.WriteLine($"Nuvarnade Saldo: {BankAccountBalances[currentUserIndex, fromAccount].ToString("N2")} SEK");
                                    Console.WriteLine("Klicka enter för att komma till huvudmenyn");
                                    Console.ReadLine();
									Console.Clear();
								}
								break;
							}
							else
							{
								PrintMessage($"Du kan inte ta ut mer än ditt saldo ({BankAccountBalances[currentUserIndex, fromAccount]} SEK) på konto {BankAccountNames[currentUserIndex, fromAccount]}\n", ConsoleColor.Red);
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
					NavigationMenu();
					break;
				}
			}
		}

		/// <summary>
		/// Calculates the cash return from an withdrawl
		/// </summary>
		/// <param name="amount">Withdrawl amount</param>
		/// <returns>string with amounts and type of bill</returns>
		static string CalculateBanknotes(double amount)
		{
			//banknotes values
			int[] denominations = { 1000, 500, 200, 100, 50, 20, 10, 5, 2, 1 };
			double tmpAmount = amount;
			string bankNotes = "";


			for (int i = 0; i < denominations.Length; i++)
			{
				if (tmpAmount / denominations[i] >= 1)
				{
					int cashAmount = (int)Math.Floor(tmpAmount / denominations[i]);
					if (cashAmount > 0)
					{
						tmpAmount -= cashAmount * denominations[i];
					}
					bankNotes += $"{cashAmount} x {denominations[i]} Kr\n";
				}
			}
			//calculate cents
			if (tmpAmount > 0)
			{
				//add cents
				bankNotes += $"Och {Math.Round(tmpAmount * 100)} öre";
			}
			return bankNotes;
		}
		/// <summary>
		/// Initiates a transaction between two of the users bankaccounts
		/// </summary>
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
					int fromAccount = int.Parse(ReadLineWithCancel());
					string fromAccountName = BankAccountNames[currentUserIndex, fromAccount];
					PrintMessage($"Hittade: {fromAccountName}", ConsoleColor.Green);
					Console.Write("\nTill Konto: ");
					int toAccount = int.Parse(ReadLineWithCancel());
					string toAccountName = BankAccountNames[currentUserIndex, toAccount];

					PrintMessage($"Hittade: {toAccountName}\n", ConsoleColor.Green);

					while (true)
					{
						Console.Write("Summa att överföra: ");
						if (double.TryParse(ReadLineWithCancel(), out double amount))
						{
							if (amount <= BankAccountBalances[currentUserIndex, fromAccount])
							{
								BankAccountBalances[currentUserIndex, fromAccount] -= amount;
								BankAccountBalances[currentUserIndex, toAccount] += amount;
								PrintMessage($"Skickade {amount} SEK från {fromAccountName} till {toAccountName}\n", ConsoleColor.Green);
                                Console.WriteLine("Klicka enter för att komma till huvudmenyn");
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
					NavigationMenu();
					break;
				}
			}
		}

		/// <summary>
		/// Prints a message with color
		/// </summary>
		/// <param name="msg">String to write</param>
		/// <param name="color">Text color</param>
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
			//total balance
			double total = 0;
			for (int i = 0; i < BankAccountBalances.GetLength(1); i++)
			{
				Console.WriteLine($"{BankAccountNames[currentUserIndex, i]} : {BankAccountBalances[currentUserIndex, i].ToString("N2")} SEK");
				total += BankAccountBalances[currentUserIndex, i];
			}
			Console.WriteLine($"\nTotalt: {total.ToString("N2")} SEK");
            Console.WriteLine("Klicka enter för att komma till huvudmenyn");
            Console.ReadLine();
			NavigationMenu();
		}

		static void Login()
		{
			Console.WriteLine("Var god logga in");
			int tries = 3;
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
					NavigationMenu();
					break;
				}
				//Authentication faied
				Console.WriteLine("Fel användarnamn eller lösenord!");
				//Decrease the number of login tries
				tries--;
			}
			//Exceeded limit of login tries
			Console.WriteLine("Du har gjort för många inloggnings försök");

		}
		/// <summary>
		/// Authenticates a username with a password.
		/// </summary>
		/// <param name="loginUsername">Username</param>
		/// <param name="loginPassword">Password</param>
		/// <returns>
		/// Returs true if the correct username and password is used,
		/// Otherwise returs false.
		/// </returns>
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

		/// <summary>
		/// ReadLine that can listen for specific key presses
		/// </summary>
		/// <returns>returns the inputed string</returns>
		static string ReadLineWithCancel()
		{
			string result = null;

			StringBuilder buffer = new StringBuilder();

			/*The key is read passing true for the intercept argument to prevent
			any characters from displaying when the Escape key is pressed.*/

			ConsoleKeyInfo info = Console.ReadKey(true);
			while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
			{
				if (info.Key == ConsoleKey.Backspace && buffer.Length > 0)
				{
					Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
					Console.Write(" ");
					Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
					buffer.Remove(buffer.Length - 1, 1);
					info = Console.ReadKey(true);
				}
				else
				{
					Console.Write(info.KeyChar);
					buffer.Append(info.KeyChar);
					info = Console.ReadKey(true);
				}
			}
			if (info.Key == ConsoleKey.Escape)
			{
				NavigationMenu();
			}

			if (info.Key == ConsoleKey.Enter)
			{
				result = buffer.ToString();
			}

			//WriteLine to make the cursor jump down a line
			Console.WriteLine();
			return result;
		}
	}
}

