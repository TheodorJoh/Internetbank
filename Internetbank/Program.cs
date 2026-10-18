namespace Internetbank
{
    internal class Program
    {
        //Method checks if the entered username and pincode match a user.
        // enteredUsername and enteredPinCode is what the user typed in as their login information. 
        // usernamesArray and pinCodesArray are the "correct" stored credentials that the entered credentials are compared to.
        // userIndex is the index of the matching user or it is -1 if there is no match.
        // Returns true if the entered credentials match with the stored credentials and otherwise false is returned.
        static bool CheckLoginInfo(string enteredUsername, int enteredPinCode, string[] usernamesArray, int[] pinCodesArray, out int userIndex)
        {
            for (int i = 0; i < usernamesArray.Length; i++)
            {
                if (enteredUsername == usernamesArray[i] && enteredPinCode == pinCodesArray[i])
                {
                    userIndex = i;
                    return true;
                }
            }
            userIndex = -1; // -1 means that no user was found
            return false;
        }

        //Method prints all accounts the user has and the balances for each.
        // userIndex is the index for the user that is currently logged in.
        // userAccounts is the account names for all users.
        // accountBalances is the balances for all users with the same index as userAccounts.
        static void ShowAccounts(int userIndex, List<List<String>> userAccounts, List<List<decimal>> accountBalances)
        {
            Console.WriteLine($"--- Konton och saldon ---");
            for (int i = 0; i < userAccounts[userIndex].Count; i++)
            {
                Console.WriteLine($"{userAccounts[userIndex][i]} : {accountBalances[userIndex][i]:N2}"); // N2 format the output so that there are 2 decimals so that the output is in kronor and ören.
            }
            Console.WriteLine("Klicka enter för att komma till huvudmenyn");
            Console.ReadLine();
        }

        //Method moves money between two of the logged in users accounts.
        //The user picks an account to take money from and then they get the option to choose what account to transfer it to and then what amount they want to send.
        // userIndex is the index for the user that is currently logged in.
        // userAccounts is the account names for all users.
        // accountBalances is the balances for all users with the same index as userAccounts.
        // Typing "stop" at any prompt returns the user to the main menu. 
        // The method stops early if the user only has one account
        static void TransferMoney(int userIndex, List<List<String>> userAccounts, List<List<decimal>> accountBalances)
        {
            Console.WriteLine("--- Överföring ---");
            int fromIndex; //Index of the account to take money from
            int selectedAccount; //The number the user picked in the send to list.
            if (userAccounts[userIndex].Count <= 1)
            {
                Console.WriteLine("Kan inte överföra med bara ett konto");
                Console.WriteLine("Klicka enter för att komma till huvudmenyn");
                Console.ReadLine();
                return;
            }
            List<int> availablePositions = new List<int>(); // Holds the index of every account except the account that you want to send money from. So the user can not transfer money to the same account.
            Console.WriteLine("Vilket konto vill du överföra pengar från?");
            for (int i = 0; i < userAccounts[userIndex].Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userAccounts[userIndex][i]} : {accountBalances[userIndex][i]:N2}");
            }


            while (true)
            {
                Console.Write("Välj: ");
                string Input = Console.ReadLine();
                if("stop" == Input.ToLower())
                {
                    return;
                }
                try
                {
                    fromIndex = Int32.Parse(Input);
                    
                    if (fromIndex < 1 || fromIndex > userAccounts[userIndex].Count)
                    {
                        // Throws when the user types a number that is out of range and is handled by the catch below. 
                        throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
                catch (FormatException) { Console.WriteLine("Ogiltigt val, var snäll skriv in ett nummer!"); }
                catch (ArgumentOutOfRangeException) { Console.WriteLine($"Ogiltigt val, var snäll och skriv in ett nummer mellan 1-{userAccounts[userIndex].Count}"); }
                catch (OverflowException) { Console.WriteLine($"Ogiltigt val, var snäll och skriv in ett nummer mellan 1-{userAccounts[userIndex].Count}"); }
            }
            fromIndex = fromIndex - 1; //Subtract one because the list shown to the user starts at 1 while a list starts at 0.
            Console.WriteLine("");
            for (int i = 0; i < userAccounts[userIndex].Count; i++)
            {
                if (i == fromIndex)
                {
                    continue;
                }
                availablePositions.Add(i);
            }

            Console.Clear();
            Console.WriteLine($"Från: {userAccounts[userIndex][fromIndex]}");
            Console.WriteLine("");
            Console.WriteLine("Vilket konto vill du överföra pengar till?");
            for (int i = 0; i < availablePositions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userAccounts[userIndex][availablePositions[i]]} : {accountBalances[userIndex][availablePositions[i]]:N2} ");
            }
            while (true)
            {
                Console.Write("Välj: ");
                string Input = Console.ReadLine();
                if ("stop" == Input.ToLower())
                {
                    return;
                }
                try
                {
                    selectedAccount = Int32.Parse(Input);
                    if (selectedAccount < 1 || selectedAccount > availablePositions.Count)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
                catch (FormatException) { Console.WriteLine("Ogiltigt val, var snäll skriv in ett nummer!"); }
                catch (ArgumentOutOfRangeException) { Console.WriteLine($"Ogiltigt val, var snäll och skriv in ett nummer mellan 1-{availablePositions.Count}"); }
                catch (OverflowException) { Console.WriteLine($"Ogiltigt val, var snäll och skriv in ett nummer mellan 1-{availablePositions.Count}"); }
            }
            Console.WriteLine("");

            int toIndex = availablePositions[selectedAccount - 1];
            decimal transferAmount;
            while (true)
            {
                Console.WriteLine("Hur mycket vill du överföra?");
                Console.WriteLine("");
                Console.Write("Summa: ");
                string Input = Console.ReadLine();
                if ("stop" == Input.ToLower())
                {
                    return;
                }
                try
                {

                    transferAmount = decimal.Parse(Input);
                    if (transferAmount <= 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    if (transferAmount > accountBalances[userIndex][fromIndex])
                    {
                        Console.WriteLine("Du har inte tillräckligt med pengar på kontot");
                        continue;
                    }
                    break;
                }
                catch (FormatException) { Console.WriteLine("Ogiltigt val, var snäll skriv in ett nummer!"); }
                catch (ArgumentOutOfRangeException) { Console.WriteLine("Ogiltigt val, var snäll och skriv in ett nummer större än 0"); }
                catch (OverflowException) { Console.WriteLine("Ogiltigt val, var snäll och skriv in ett nummer större än 0"); }
            }

            Console.Clear();
            Console.WriteLine("--- Nya saldon ---");
            accountBalances[userIndex][fromIndex] -= transferAmount;
            accountBalances[userIndex][toIndex] += transferAmount;
            Console.WriteLine($"{userAccounts[userIndex][fromIndex]} : {accountBalances[userIndex][fromIndex]:N2}");
            Console.WriteLine($"{userAccounts[userIndex][toIndex]} : {accountBalances[userIndex][toIndex]:N2}\n");
            Console.WriteLine("Klicka enter för att komma till huvudmenyn");
            Console.ReadLine();
        }

        //Method that lets the user withdraw money from one account after they confirm with their pincode
        // pinCodesArray is the "correct" stored credentials that the entered credentials are compared to.
        // userIndex is the index of the user that is currently logged in.
        // userAccounts is the account names for all users.
        // accountBalances is the balances for all users with the same index as userAccounts.
        // Typing "stop" at any prompt returns the user to the main menu. 
        static void WithdrawMoney(int userIndex, List<List<String>> userAccounts, List<List<decimal>> accountBalances, int[] pinCodesArray)
        {
            Console.WriteLine("--- Kontoutdrag ---");
            int pinAttempts = 0; //pinAttempts counts wrong pincode attempts.
            int LockoutTime = 3; // Wait time in minutes after 3 wrong pincode attempts. It doubles for each lockout.
            int enteredPinCode;
            decimal amount;
            int accountNumber;
            Console.WriteLine("Vilket konto vill du ta ut pengar från?");
            for (int i = 0; i < userAccounts[userIndex].Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userAccounts[userIndex][i]} : {accountBalances[userIndex][i]:N2}");
            }

            while (true)
            {
                Console.Write("Välj: ");
                string Input = Console.ReadLine();
                if ("stop" == Input.ToLower())
                {
                    return;
                }
                try
                {
                    accountNumber = Int32.Parse(Input);
                    if (accountNumber > userAccounts[userIndex].Count || accountNumber < 1)
                    {
                        // Throws when the user types a number that is out of range and is handled by the catch below. 
                        throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
                catch (FormatException) { Console.WriteLine("Ogiltigt val, var snäll skriv in ett nummer!"); }
                catch (ArgumentOutOfRangeException) { Console.WriteLine($"Ogiltigt val, var snäll och skriv in ett nummer mellan 1-{userAccounts[userIndex].Count}"); }
                catch (OverflowException) { Console.WriteLine($"Ogiltigt val, var snäll och skriv in ett nummer mellan 1-{userAccounts[userIndex].Count}"); }
            }
            Console.WriteLine("");
            while (true)
            {
                Console.WriteLine("Hur mycket pengar vill du ta ut?");
                Console.WriteLine("");
                Console.Write("Summa: ");
                string Input = Console.ReadLine();
                if ("stop" == Input.ToLower())
                {
                    return;
                }
                try
                {
                    amount = decimal.Parse(Input);

                    if (amount <= 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    if (amount > accountBalances[userIndex][accountNumber - 1])
                    {
                        Console.WriteLine("Du har inte tillräckligt med pengar på kontot");
                        continue;
                    }
                    break;
                }
                catch (FormatException) { Console.WriteLine("Ogiltigt val, var snäll och skriv in ett nummer!"); }
                catch (ArgumentOutOfRangeException) { Console.WriteLine("Ogiltigt val, var snäll och skriv in ett nummer större än 0"); }
                catch (OverflowException) { Console.WriteLine("Ogiltigt val, var snäll och skriv in ett nummer större än 0"); }
            }

            Console.WriteLine("");
            while (true)
            {
                if (pinAttempts == 3)
                {
                    Console.Clear();
                    Console.WriteLine("--- Utlåst ---");
                    Console.WriteLine("");
                    Console.WriteLine($"Du har använt alla dina försök du måste vänta {LockoutTime} minuter innan du kan försöka igen!");
                    DateTime startTime = DateTime.UtcNow;
                    TimeSpan breakDuration = TimeSpan.FromMinutes(LockoutTime);
                    while (true)
                    {
                        if (DateTime.UtcNow - startTime > breakDuration)
                        {
                            LockoutTime = LockoutTime * 2;
                            pinAttempts = 0;
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    Console.Clear();
                    Console.WriteLine("--- 3 nya pinkod försök ---");
                }
                Console.Write("Ange pinkod: ");
                string Input = Console.ReadLine();
                if ("stop" == Input.ToLower())
                {
                    return;
                }
                try
                {
                    enteredPinCode = Int32.Parse(Input);
                    if (enteredPinCode == pinCodesArray[userIndex])
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Fel pinkod, Försök igen!");
                        Console.WriteLine();
                        pinAttempts++;
                    }
                }
                catch (FormatException) 
                    { 
                        Console.WriteLine("Ogiltigt val, var snäll och skriv in ett nummer!");
                        pinAttempts++;
                    }
                catch (OverflowException)
                {
                    Console.WriteLine("För stort nummer!");
                    pinAttempts++;
                }
            }
            accountBalances[userIndex][accountNumber - 1] -= amount;
            Console.Clear();
            Console.WriteLine("--- Nytt saldo ---");
            Console.WriteLine($"{userAccounts[userIndex][accountNumber - 1]} : {accountBalances[userIndex][accountNumber - 1]:N2} \n");
            Console.WriteLine("Klicka enter för att komma till huvudmenyn");
            Console.ReadLine();
        }



        static void Main(string[] args)
        {

            // userIndex is the index for the user that is currently logged in and -1 means that nobody is logged in.
            int userIndex = -1;
            // enteredUsername is the username that the user typed in at login.
            string enteredUsername;
            // enteredPinCode is the pincode that the user typed in at login.
            int enteredPinCode = 0;
            // menuChoice is the number that the user picked in the main menu.
            int menuChoice = 0;
            // pinAttempts is the number of failed attempts the user has had in a row.
            int pinAttempts = 0;
            // Wait time in minutes after 3 wrong pincode attempts. It doubles after each lockout.
            int LockoutTime = 3;
            //Array for the different usernames. Position 0 in the array is for user 1 and position 1 is for user 2 and so on.
            string[] usernamesArray = { "Theodor", "Anas", "Shenol", "Zandra", "Albion" };
            //Array for the different pincodes associated to each username. Position 0 in the array is for user 1 and position 1 is for user 2 and so on.
            int[] pinCodesArray = { 1337, 0026, 3337, 4337, 5337 };
            //List for the different accounts the users have. Position 0 in the list is for user 1 and position 1 is for user 2 and so on.
            List<List<string>> userAccounts = new List<List<string>>
            {  
                new List<string> {"LöneKonto", "SparKonto", "MatKonto", "Övrigt"},
                new List<string> {"LöneKonto", "SparKonto", "ReseKonto"},
                new List<string> {"PrivatKonto", "FondKonto"},
                new List<string> {"LöneKonto"},
                new List<string> {"LöneKonto", "SparKonto", "MatKonto", "Övrigt", "DjurKonto", "HusKonto"}
            };
            //List for the different balances the accounts have. Position 0 in the list is for user 1 and position 1 is for user 2 and so on.
            List<List<decimal>> accountBalances = new List<List<decimal>>
            {
                new List<decimal> {23000.54m, 150000.54m, 3000.44m, 1500.45m},
                new List<decimal> {23000.34m, 150000.35m, 15000.35m},
                new List<decimal> {23000, 150000},
                new List<decimal> {23000},
                new List<decimal> { 23000, 150000, 3000, 1500, 2500, 50000}
            };

            Console.WriteLine("Välkommen till TheodorBank skriv in ditt användarnamn och pinkod för att fortsätta! \n");
            //The main loop which runs forever.
            //The loop begins with asking the user for their credentials and if that is correct they get into the main menu and if they do not get it correct the loop comes back around and they get to put in their credentials again.
            //However if the user gets their credentials incorrect 3 times they are locked out for first 3 minutes and then doubles for each lockout.
            while (true)
            {
                Console.Write("Användarnamn: ");
                enteredUsername = Console.ReadLine();
                Console.Write("Pin-kod (4 siffror): ");
                try
                {
                    enteredPinCode = Int32.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    enteredPinCode = -1;
                }
                catch (OverflowException)
                {
                    enteredPinCode = -1;
                }
                Console.WriteLine("");
                if (CheckLoginInfo(enteredUsername, enteredPinCode, usernamesArray, pinCodesArray, out userIndex))
                {
                    menuChoice = 0;
                    while (menuChoice != 4)
                    {
                        Console.Clear();
                        Console.WriteLine($"Välkommen {enteredUsername}!");
                        Console.WriteLine("1. Se dina konton och saldo");
                        Console.WriteLine("2. Överföring mellan konton");
                        Console.WriteLine("3. Ta ut pengar");
                        Console.WriteLine("4. Logga ut");
                        while(true)
                        {
                            Console.Write("Välj (1-4): ");
                            try
                            {
                                menuChoice = Int32.Parse(Console.ReadLine());
                                if( menuChoice < 1 || menuChoice > 4)
                                {
                                    throw new ArgumentOutOfRangeException();
                                }
                                break;
                            }
                            catch (FormatException) { Console.WriteLine("Ogiltigt val, var snäll och skriv in ett nummer!"); }
                            catch (ArgumentOutOfRangeException) { Console.WriteLine("Ogiltigt val, var snäll och skriv in ett nummer mellan 1-4"); }
                            catch (OverflowException) { Console.WriteLine("Ogiltigt val, var snäll och skriv in ett nummer mellan 1-4"); }
                        }
                        Console.WriteLine("");
                        //Calls the method that matches the users choice.
                        switch (menuChoice)
                        {
                            case 1:
                                Console.Clear();
                                ShowAccounts(userIndex, userAccounts, accountBalances);
                                break;
                            case 2:
                                Console.Clear();
                                TransferMoney(userIndex, userAccounts, accountBalances);
                                break;
                            case 3:
                                Console.Clear();
                                WithdrawMoney(userIndex, userAccounts, accountBalances, pinCodesArray);
                                break;
                            // Case 4 logs the user out, resetting them back into the login loop.
                            case 4:
                                Console.Clear();
                                Console.WriteLine($"Hej då {enteredUsername}! \n");
                                Console.WriteLine("Välkommen till TheodorBank skriv in ditt användarnamn och pin-kod för att fortsätta! \n");
                                pinAttempts = 0;
                                enteredUsername = "";
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    pinAttempts++;
                    Console.Clear();
                    Console.WriteLine("Fel lösenord eller användarnamn \n");
                    if (pinAttempts == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("--- Utlåst ---");
                        Console.WriteLine("");
                        Console.WriteLine($"Du har använt alla dina försök du måste vänta {LockoutTime} minuter innan du kan försöka igen!");
                        DateTime startTime = DateTime.UtcNow;
                        TimeSpan breakDuration = TimeSpan.FromMinutes(LockoutTime);
                        while (true)
                        {
                            if (DateTime.UtcNow - startTime > breakDuration)
                            {
                                LockoutTime = LockoutTime * 2;
                                pinAttempts = 0;
                                break;
                            }
                            Thread.Sleep(1000);
                        }
                        Console.Clear();
                        Console.WriteLine("--- 3 nya pinkod försök ---");
                    }
                }
            }
        }
    }
}
