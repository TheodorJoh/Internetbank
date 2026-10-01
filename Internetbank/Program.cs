namespace Internetbank
{
    internal class Program
    {

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

        static void VisaKonton(int UserIndex, List<List<String>> AnvändarKonton, List<List<decimal>> KontoSaldon)
        {
            for (int i = 0; i < AnvändarKonton[UserIndex].Count; i++)
            {
                Console.WriteLine($"{AnvändarKonton[UserIndex][i]} : {KontoSaldon[UserIndex][i]}");
            }
            Console.WriteLine("Enter för att komma tillbaka till huvudmenyn");
            Console.ReadLine();
        }

        static void KontoÖverföring(int UserIndex, List<List<String>> AnvändarKonton, List<List<decimal>> KontoSaldon)
        {
            while (true)
            {
                Console.WriteLine("Vilket konto vill du överföra pengar från?");
                for (int i = 0; i < AnvändarKonton[UserIndex].Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {AnvändarKonton[UserIndex][i]}");
                }

                Console.Write("Välj: ");
                if (!(int.TryParse(Console.ReadLine(), out int KontoNummer) && KontoNummer <= AnvändarKonton[UserIndex].Count && KontoNummer >= 1))
                {
                    Console.WriteLine("Ogiltigt val");
                    continue;
                }
                Console.WriteLine("");


                Console.WriteLine("Vilket konto vill du överföra pengar till?");
                for (int i = 0; i < AnvändarKonton[UserIndex].Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {AnvändarKonton[UserIndex][i]}");
                }
                Console.Write("Välj: ");
                if (!(int.TryParse(Console.ReadLine(), out int KontoNummer2) && KontoNummer2 <= AnvändarKonton[UserIndex].Count && KontoNummer2 >= 1))
                {
                    Console.WriteLine("Ogiltigt val");
                    continue;
                }
                Console.WriteLine("");
                if (KontoNummer == KontoNummer2)
                {
                    Console.WriteLine("Du kan inte överföra pengar till samma konto.");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine("Hur mycket vill du överföra? \n");
                Console.Write("Summa: ");

                if (!(decimal.TryParse(Console.ReadLine(), out decimal Överföra) && Överföra > 0 && Överföra <= KontoSaldon[UserIndex][KontoNummer - 1]))
                {
                    Console.WriteLine("Ogiltigt val");
                    continue;
                }
                Console.WriteLine("");
                KontoSaldon[UserIndex][KontoNummer - 1] -= Överföra;
                KontoSaldon[UserIndex][KontoNummer2 - 1] += Överföra;
                Console.WriteLine($"{AnvändarKonton[UserIndex][KontoNummer - 1]} : {KontoSaldon[UserIndex][KontoNummer - 1]}");
                Console.WriteLine($"{AnvändarKonton[UserIndex][KontoNummer2 - 1]} : {KontoSaldon[UserIndex][KontoNummer2 - 1]}\n");

                break;
            }
        }

        static void KontoUtdrag(int UserIndex, List<List<String>> AnvändarKonton, List<List<decimal>> KontoSaldon, int[] PinKodArray) 
        {
            while (true)
            {
                Console.WriteLine("Vilket konto vill du ta ut pengar från?");
                for (int i = 0; i < AnvändarKonton[UserIndex].Count; i++)
                {
                    Console.WriteLine($"{i + 1} : {AnvändarKonton[UserIndex][i]} : {KontoSaldon[UserIndex][i]}");
                }
                Console.Write("Välj: ");
                if (!(int.TryParse(Console.ReadLine(), out int KontoNummer) && KontoNummer <= AnvändarKonton[UserIndex].Count && KontoNummer >= 1))
                {
                    Console.WriteLine("Ogiltigt val");
                    continue;
                }
                Console.WriteLine("");
                Console.WriteLine("Hur mycket vill du ta ut?");
                if (!(decimal.TryParse(Console.ReadLine(), out decimal Summa) && Summa > 0 && Summa <= KontoSaldon[UserIndex][KontoNummer - 1]))
                {
                    Console.WriteLine("Ogiltigt val");
                    continue;
                }
                Console.WriteLine("");


                Console.Write("Ange Pinkod: ");
                if (!(int.TryParse(Console.ReadLine(), out int PinKod)))
                {
                    Console.WriteLine("Ogiltigt val");
                    continue;
                }
                Console.WriteLine("");

                if (PinKod != PinKodArray[UserIndex])
                {
                    Console.WriteLine("Fel pinkod");
                }
                else
                {
                    KontoSaldon[UserIndex][KontoNummer - 1] -= Summa;
                    Console.WriteLine($"Nytt saldo: {AnvändarKonton[UserIndex][KontoNummer - 1]} : {KontoSaldon[UserIndex][KontoNummer - 1]} \n");
                }
                break;
            }
        }



        static void Main(string[] args)
        {
            int UserIndex = -1;
            string StringTemp;
            int PinKod = 0;
            int MenyVal = 0;
            int PinAttempts = 0;
            int LockoutTime = 3;
            string[] AnvändarNamnArray = { "Theodor", "Anas", "Shenol", "Zandra", "Albion" };
            int[] PinKodArray = { 1337, 2026, 3337, 4337, 5337 };
            List<List<string>> AnvändarKonton = new List<List<string>>
            {
                new List<string> {"LöneKonto", "SparKonto", "MatKonto", "Övrigt"},
                new List<string> {"LöneKonto", "SparKonto", "ReseKonto"},
                new List<string> {"PrivatKonto", "FondKonto", "DjurKonto"},
                new List<string> {"LöneKonto", "HusKonto", "FondKonto"},
                new List<string> {"LöneKonto", "SparKonto", "MatKonto", "Övrigt", "DjurKonto", "HusKonto"}
            };

            List<List<decimal>> KontoSaldon = new List<List<decimal>>
            {
                new List<decimal> {23000, 150000, 3000, 1500},
                new List<decimal> {23000, 150000, 15000},
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
                                break;
                            case 2:
                                KontoÖverföring(UserIndex, AnvändarKonton, KontoSaldon);
                                Console.WriteLine("Enter för att komma tillbaka till huvudmenyn");
                                Console.ReadLine();

                                break;
                            case 3:
                                KontoUtdrag(UserIndex, AnvändarKonton, KontoSaldon, PinKodArray);
                                Console.WriteLine("Enter för att komma tillbaka till huvudmenyn");
                                Console.ReadLine();
                                break;
                            case 4:
                                Console.WriteLine($"Hej då {StringTemp}! \n");
                                Console.WriteLine("Välkommen till TheodorBank skriv in ditt användarnamn och pin-kod för att fortsätta! \n");
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
