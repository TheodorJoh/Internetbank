namespace Internetbank
{
    internal class Program
    {
        //This method checks if the correct credentials have been inputed when you login. The mehod has 5 paramters and these are AnvändarNamn which is the username the user inputed and the same with PinKod except it is for the users password.
        //The two array paramters AnvändarNamnArray and PinKodArray are used to check if the inputed username and password are correct and then lastly the UserIndex is outputed so that we later can used that to identify the right account for the right user.
        
        static bool CheckLogin(string AnvändarNamn, int PinKod, string[] AnvändarNamnArray, int[] PinKodArray, out int UserIndex)
        {
            for(int i = 0; i < AnvändarNamnArray.Length;i++)
            {
                if(AnvändarNamn == AnvändarNamnArray[i] && PinKod == PinKodArray[i])
                {
                    UserIndex = i;
                    return true;
                }
            }
            UserIndex = -1;
            return false;
        }

        //Method that prints all of the accounts associated with the user and also prints the balances for each account.
        //The method takes 3 paramters, UserIndex which is used to located the right account, AnvändarKonton which is where all the accounts are located and then KontoSaldon which is where all the balancs are stored.

        static void VisaKonton(int UserIndex, List<List<String>> AnvändarKonton, List<List<decimal>> KontoSaldon)
        {
            for (int i = 0; i < AnvändarKonton[UserIndex].Count; i++)
            {
                Console.WriteLine($"{AnvändarKonton[UserIndex][i]} : {KontoSaldon[UserIndex][i]:N2}");
            }

        }

        //This method handles the transferes between accounts.
        //The method takes 3 paramters, UserIndex which is used to located the right account, AnvändarKonton which is where all the accounts are located and then KontoSaldon which is where all the balancs are stored.
        
          static void KontoÖverföring(int UserIndex, List<List<String>> AnvändarKonton, List<List<decimal>> KontoSaldon)
        {
            while (true)
            {
                if (AnvändarKonton[UserIndex].Count <= 1)
                {
                    Console.WriteLine("Kan inte överföra med bara ett konto");
                    break;
                }
                List<int> Positioner = new List<int>();
                Console.WriteLine("Vilket konto vill du överföra pengar från?");
                for (int i = 0; i < AnvändarKonton[UserIndex].Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {AnvändarKonton[UserIndex][i]} : {KontoSaldon[UserIndex][i]:N2}");
                }

                Console.Write("Välj: ");
                if (!(int.TryParse(Console.ReadLine(), out int FrånIndex) && FrånIndex <= AnvändarKonton[UserIndex].Count && FrånIndex >= 1))
                {
                    Console.WriteLine("Ogiltigt val");
                    continue;
                }
                FrånIndex = FrånIndex - 1;
                Console.WriteLine("");
                for (int i = 0; i < AnvändarKonton[UserIndex].Count; i++)
                {
                    if(i == FrånIndex)
                    {
                        continue;
                    }
                    Positioner.Add(i);
                }


                Console.WriteLine("Vilket konto vill du överföra pengar till?");
                for (int i = 0; i < Positioner.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {AnvändarKonton[UserIndex][Positioner[i]]} : {KontoSaldon[UserIndex][Positioner[i]]:N2} ");
                }
                Console.Write("Välj: ");
                if (!(int.TryParse(Console.ReadLine(), out int Val) && Val <= Positioner.Count && Val >= 1))
                {
                    Console.WriteLine("Ogiltigt val");
                    continue;
                }
                Console.WriteLine("");
               
               int TillIndex = Positioner[Val - 1];

                Console.WriteLine("Hur mycket vill du överföra? \n");
                Console.Write("Summa: ");

                if (!(decimal.TryParse(Console.ReadLine(), out decimal Överföra) && Överföra > 0 ))
                {
                    Console.WriteLine("Ogiltigt val");
                    continue;
                }

                if(Överföra > KontoSaldon[UserIndex][FrånIndex])
                {
                    Console.WriteLine("Du har inte tillräckligt med pengar på kontot");
                    continue;
                }
                Console.WriteLine("");
                KontoSaldon[UserIndex][FrånIndex] -= Överföra;
                KontoSaldon[UserIndex][TillIndex] += Överföra;
                Console.WriteLine($"{AnvändarKonton[UserIndex][FrånIndex]} : {KontoSaldon[UserIndex][FrånIndex]:N2}");
                Console.WriteLine($"{AnvändarKonton[UserIndex][TillIndex]} : {KontoSaldon[UserIndex][TillIndex]:N2}\n");

                break;
            }
        }

        //This method handles taking out money from an account.
        //The method takes 3 paramters, UserIndex which is used to located the right account, AnvändarKonton which is where all the accounts are located and then KontoSaldon which is where all the balancs are stored.
        //The method also needs the PinKodArray so that it can check if the inputed pinkod is correct.
        static void KontoUtdrag(int UserIndex, List<List<String>> AnvändarKonton, List<List<decimal>> KontoSaldon, int[] PinKodArray) 
        {
                int PinKod2;
                decimal Summa;
                int KontoNummer;
                Console.WriteLine("Vilket konto vill du ta ut pengar från?");
                for (int i = 0; i < AnvändarKonton[UserIndex].Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {AnvändarKonton[UserIndex][i]} : {KontoSaldon[UserIndex][i]:N2}");
                }
                
                while(true)
                {
                    Console.Write("Välj: ");
                    try
                    {
                        KontoNummer = Int32.Parse(Console.ReadLine());
                        if(KontoNummer > AnvändarKonton[UserIndex].Count || KontoNummer < 1)
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        break;
                    } 
                    catch (FormatException) { Console.WriteLine("Ogiltigt val, var snäll skriv in ett nummer!"); }
                    catch (ArgumentOutOfRangeException) { Console.WriteLine($"Ogiltigt val, var snäll och skriv in ett nummer mellan 1-{AnvändarKonton[UserIndex].Count}");}
                }
                Console.WriteLine("");
                while (true)
                {
                    Console.Write("Summa: ");
                    try
                    {
                        Summa = decimal.Parse(Console.ReadLine());

                        if (Summa <= 0)
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        if (Summa > KontoSaldon[UserIndex][KontoNummer - 1])
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


                
                while(true)
                {
                    Console.Write("Ange Pinkod: ");
                    try
                    {
                        PinKod2 = Int32.Parse(Console.ReadLine());
                        if(PinKod2 == PinKodArray[UserIndex]) 
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Fel pinkod, Försök igen!");
                        }
                    }
                    catch (FormatException) { Console.WriteLine("Ogiltigt val, var snäll och skriv in ett nummer!"); }
                }
                    KontoSaldon[UserIndex][KontoNummer - 1] -= Summa;
                    Console.WriteLine($"Nytt saldo: {AnvändarKonton[UserIndex][KontoNummer - 1]} : {KontoSaldon[UserIndex][KontoNummer - 1]:N2} \n");
        }



        static void Main(string[] args)
        {

            //Index for remembering which account is currently being used by the user.
            int UserIndex = -1;
            //Temp
            string StringTemp;
            //Integer for the input of the pinkod
            int PinKod = 0;
            //Integer for the input of the menu choice
            int MenyVal = 0;
            //Integer for the amounts of attemps the user has used when trying to login.
            int PinAttempts = 0;
            //Integer for the amount of times the user is allowed to fail when trying to login
            int LockoutTime = 3;
            //Array for the different usernames.
            string[] AnvändarNamnArray = { "Theodor", "Anas", "Shenol", "Zandra", "Albion" };
            //Array for the different pincodes associated to each username.
            int[] PinKodArray = { 1337, 0026, 3337, 4337, 5337 };
            //List for the different accounts.
            List<List<string>> AnvändarKonton = new List<List<string>>
            {
                new List<string> {"LöneKonto", "SparKonto", "MatKonto", "Övrigt"},
                new List<string> {"LöneKonto", "SparKonto", "ReseKonto"},
                new List<string> {"PrivatKonto", "FondKonto", "DjurKonto"},
                new List<string> {"LöneKonto"},
                new List<string> {"LöneKonto", "SparKonto", "MatKonto", "Övrigt", "DjurKonto", "HusKonto"}
            };
            //List for the different balances.
            List<List<decimal>> KontoSaldon = new List<List<decimal>>
            {
                new List<decimal> {23000.54m, 150000.54m, 3000.44m, 1500.45m},
                new List<decimal> {23000.34m, 150000.35m, 15000.35m},
                new List<decimal> {23000, 150000, 2500},
                new List<decimal> {23000, 50000, 200000},
                new List<decimal> { 23000, 150000, 3000, 1500, 2500, 50000}
            };

            Console.WriteLine("Välkommen till TheodorBank skriv in ditt användarnamn och pin-kod för att fortsätta! \n");

            while(true) {
                Console.Write("Användarnamn: ");
                StringTemp = Console.ReadLine();
                Console.Write("Pin-kod (4 siffror): ");
                int.TryParse(Console.ReadLine(), out PinKod);
                Console.WriteLine("");
                if (CheckLogin(StringTemp, PinKod, AnvändarNamnArray, PinKodArray, out UserIndex))
                {
                    MenyVal = 0;
                    while (MenyVal != 4)
                    {
                        Console.WriteLine($"Välkommen {StringTemp}!");
                        Console.WriteLine("1. Se dina konton och saldo");
                        Console.WriteLine("2. Överföring mellan konton");
                        Console.WriteLine("3. Ta ut pengar");
                        Console.WriteLine("4. Logga ut");
                        Console.Write("Välj (1-4): ");
                        if (!(int.TryParse(Console.ReadLine(), out MenyVal) && MenyVal <= 4 && MenyVal >= 1))
                        {
                            Console.WriteLine("Ogiltigt val");
                            continue;
                        }
                        Console.WriteLine("");
                        switch (MenyVal){
                            case 1:
                                VisaKonton(UserIndex, AnvändarKonton, KontoSaldon);
                                Console.WriteLine("Klicka enter för att komma till huvudmenyn");
                                Console.ReadLine();
                                break;
                            case 2:
                                KontoÖverföring(UserIndex, AnvändarKonton, KontoSaldon);
                                Console.WriteLine("Klicka enter för att komma till huvudmenyn");
                                Console.ReadLine();

                                break;
                            case 3:
                                KontoUtdrag(UserIndex, AnvändarKonton, KontoSaldon, PinKodArray);
                                Console.WriteLine("Klicka enter för att komma till huvudmenyn");
                                Console.ReadLine();
                                break;
                            case 4:
                                Console.WriteLine($"Hej då {StringTemp}! \n");
                                Console.WriteLine("Välkommen till TheodorBank skriv in ditt användarnamn och pin-kod för att fortsätta! \n");
                                PinAttempts = 0;
                                StringTemp = "";
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    PinAttempts++;
                    Console.WriteLine("Fel lösenord eller Användarnamn \n");
                    if (PinAttempts == 3)
                    {
                        Console.WriteLine($"Du har använt alla dina försök du måste vänta {LockoutTime} minuter innana du kan försöak igen!");
                        DateTime startTime = DateTime.UtcNow;
                        TimeSpan breakDuration = TimeSpan.FromMinutes(LockoutTime);
                        while (true)
                        {
                            if (DateTime.UtcNow - startTime > breakDuration)
                            {
                                LockoutTime = LockoutTime * 2;
                                PinAttempts = 0;
                                break;
                            }
                        }
                    }
                }
            }
        }
    } }
