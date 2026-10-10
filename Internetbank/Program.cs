namespace Internetbank
{
    internal class Program
    {
        //This method checks if the correct credentials have been inputed when you login. The mehod has 5 paramters and these are username which is the username the user inputed and the same with pinCode except it is for the users password.
        //The two array paramters usernamesArray and pinCodesArray are used to check if the inputed username and password are correct and then lastly the userIndex is outputed so that we later can used that to identify the right account for the right user.

        static bool CheckLoginInfo(string enteredUsername, int pinCode, string[] usernamesArray, int[] pinCodesArray, out int userIndex)
        {
            for (int i = 0; i < usernamesArray.Length; i++)
            {
                if (enteredUsername == usernamesArray[i] && pinCode == pinCodesArray[i])
                {
                    userIndex = i;
                    return true;
                }
            }
            userIndex = -1;
            return false;
        }

        //Method that prints all of the accounts associated with the user and also prints the balances for each account.
        //The method takes 3 paramters, userIndex which is used to located the right account, userAccounts which is where all the accounts are located and then accountBalances which is where all the balancs are stored.

        static void ShowAccounts(int userIndex, List<List<String>> userAccounts, List<List<decimal>> accountBalances)
        {
            for (int i = 0; i < userAccounts[userIndex].Count; i++)
            {
                Console.WriteLine($"{userAccounts[userIndex][i]} : {accountBalances[userIndex][i]:N2}");
            }
            Console.WriteLine("Klicka enter för att komma till huvudmenyn");
            Console.ReadLine();
        }

        //This method handles the transferes between accounts.
        //The method takes 3 paramters, userIndex which is used to located the right account, userAccounts which is where all the accounts are located and then accountBalances which is where all the balancs are stored.

        static void TransferMoney(int userIndex, List<List<String>> userAccounts, List<List<decimal>> accountBalances)
        {
            int fromIndex;
            int selectedAccount;
            if (userAccounts[userIndex].Count <= 1)
            {
                Console.WriteLine("Kan inte överföra med bara ett konto");
                Console.WriteLine("Klicka enter för att komma till huvudmenyn");
                Console.ReadLine();
                return;
            }
            List<int> availablePositions = new List<int>();
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
                        throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
                catch (FormatException) { Console.WriteLine("Ogiltigt val, var snäll skriv in ett nummer!"); }
                catch (ArgumentOutOfRangeException) { Console.WriteLine($"Ogiltigt val, var snäll och skriv in ett nummer mellan 1-{userAccounts[userIndex].Count}"); }
            }
            fromIndex = fromIndex - 1;
            Console.WriteLine("");
            for (int i = 0; i < userAccounts[userIndex].Count; i++)
            {
                if (i == fromIndex)
                {
                    continue;
                }
                availablePositions.Add(i);
            }


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
            }
            Console.WriteLine("");

            int toIndex = availablePositions[selectedAccount - 1];
            decimal transferAmount;
            while (true)
            {
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
            }

            Console.WriteLine("");
            accountBalances[userIndex][fromIndex] -= transferAmount;
            accountBalances[userIndex][toIndex] += transferAmount;
            Console.WriteLine($"{userAccounts[userIndex][fromIndex]} : {accountBalances[userIndex][fromIndex]:N2}");
            Console.WriteLine($"{userAccounts[userIndex][toIndex]} : {accountBalances[userIndex][toIndex]:N2}\n");
            Console.WriteLine("Klicka enter för att komma till huvudmenyn");
            Console.ReadLine();
        }

        //This method handles taking out money from an account.
        //The method takes 3 paramters, userIndex which is used to located the right account, userAccounts which is where all the accounts are located and then accountBalances which is where all the balancs are stored.
        //The method also needs the pinCodesArray so that it can check if the inputed pinCode is correct.
        static void WithdrawMoney(int userIndex, List<List<String>> userAccounts, List<List<decimal>> accountBalances, int[] pinCodesArray)
        {
            int pinAttempts = 0;
            int LockoutTime = 2;
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
                        throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
                catch (FormatException) { Console.WriteLine("Ogiltigt val, var snäll skriv in ett nummer!"); }
                catch (ArgumentOutOfRangeException) { Console.WriteLine($"Ogiltigt val, var snäll och skriv in ett nummer mellan 1-{userAccounts[userIndex].Count}"); }
            }
            Console.WriteLine("");
            while (true)
            {
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
            }

            Console.WriteLine("");
            while (true)
            {
                if (pinAttempts == 3)
                {
                    Console.WriteLine($"Du har använt alla dina försök du måste vänta {LockoutTime} minuter innana du kan försöak igen!");
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
                    }
                }
                Console.Write("Ange pinCode: ");
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
                        Console.WriteLine("Fel pinCode, Försök igen!");
                        pinAttempts++;
                    }
                }
                catch (FormatException) 
                    { 
                        Console.WriteLine("Ogiltigt val, var snäll och skriv in ett nummer!");
                        pinAttempts++;
                    }
            }
            accountBalances[userIndex][accountNumber - 1] -= amount;
            Console.WriteLine($"Nytt saldo: {userAccounts[userIndex][accountNumber - 1]} : {accountBalances[userIndex][accountNumber - 1]:N2} \n");
            Console.WriteLine("Klicka enter för att komma till huvudmenyn");
            Console.ReadLine();
        }



        static void Main(string[] args)
        {

            //Index for remembering which account is currently being used by the user.
            int userIndex = -1;
            //Temp
            string enteredUsername;
            //Integer for the input of the pinCode
            int pinCode = 0;
            //Integer for the input of the menu choice
            int menuChoice = 0;
            //Integer for the amounts of attemps the user has used when trying to login.
            int pinAttempts = 0;
            //Integer for the amount of times the user is allowed to fail when trying to login
            int LockoutTime = 3;
            //Array for the different usernames.
            string[] usernamesArray = { "Theodor", "Anas", "Shenol", "Zandra", "Albion" };
            //Array for the different pincodes associated to each username.
            int[] pinCodesArray = { 1337, 0026, 3337, 4337, 5337 };
            //List for the different accounts.
            List<List<string>> userAccounts = new List<List<string>>
            {  
                new List<string> {"LöneKonto", "SparKonto", "MatKonto", "Övrigt"},
                new List<string> {"LöneKonto", "SparKonto", "ReseKonto"},
                new List<string> {"PrivatKonto", "FondKonto"},
                new List<string> {"LöneKonto"},
                new List<string> {"LöneKonto", "SparKonto", "MatKonto", "Övrigt", "DjurKonto", "HusKonto"}
            };
            //List for the different balances.
            List<List<decimal>> accountBalances = new List<List<decimal>>
            {
                new List<decimal> {23000.54m, 150000.54m, 3000.44m, 1500.45m},
                new List<decimal> {23000.34m, 150000.35m, 15000.35m},
                new List<decimal> {23000, 150000},
                new List<decimal> {23000},
                new List<decimal> { 23000, 150000, 3000, 1500, 2500, 50000}
            };

            Console.WriteLine("Välkommen till TheodorBank skriv in ditt username och pin-kod för att fortsätta! \n");

            while (true)
            {
                Console.Write("username: ");
                enteredUsername = Console.ReadLine();
                Console.Write("Pin-kod (4 siffror): ");
                try
                {
                    pinCode = Int32.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    pinCode = -1;
                }
                Console.WriteLine("");
                if (CheckLoginInfo(enteredUsername, pinCode, usernamesArray, pinCodesArray, out userIndex))
                {
                    menuChoice = 0;
                    while (menuChoice != 4)
                    {
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
                        }
                        Console.WriteLine("");
                        switch (menuChoice)
                        {
                            case 1:
                                ShowAccounts(userIndex, userAccounts, accountBalances);
                                break;
                            case 2:
                                TransferMoney(userIndex, userAccounts, accountBalances);
                                break;
                            case 3:
                                WithdrawMoney(userIndex, userAccounts, accountBalances, pinCodesArray);
                                break;
                            case 4:
                                Console.WriteLine($"Hej då {enteredUsername}! \n");
                                Console.WriteLine("Välkommen till TheodorBank skriv in ditt username och pin-kod för att fortsätta! \n");
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
                    Console.WriteLine("Fel lösenord eller username \n");
                    if (pinAttempts == 3)
                    {
                        Console.WriteLine($"Du har använt alla dina försök du måste vänta {LockoutTime} minuter innana du kan försöak igen!");
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
                        }
                    }
                }
            }
        }
    }
}
