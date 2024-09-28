namespace Bankomaten
{
	internal class Program
	{
		public static string[] Usernames = { "karl", "per", "kalle", "roland", "berra" };
		public static string[] Passwords = { "apelsin123", "kolrot", "1234", "volvo", "fotboll" };

		public static int currentUserIndex = -1;

		static void Main(string[] args)
		{
			Console.WriteLine("Välkommen till banken");
			Login();
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
