using System.Runtime;

namespace Internetbank
{
    internal class Program
    {

        static bool CheckLogin(string AnvändarNamn, int PinKod, List<string> AnvändarNamnArray, List<int> PinKodArray, out int UserIndex)
        {
            for(int i = 0; i < AnvändarNamnArray.Count;i++)
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




        static void Main(string[] args)
        {
            int UserIndex = -1;
            string StringTemp;
            int IntTemp = 0;
            int MenyVal = 0;
            int PinAttempts = 0;
            int LockoutTime = 10;
            List<string> AnvnändarNamnArray = new List<string>{ "Theodor", "Leon", "Shenol", "Zandra", "Albion" };
            List<int> PinKodArray = new List<int>{ 1337, 2337, 3337, 4337, 5337 };
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

            Console.WriteLine("Välkommen till TheodorBank skriv in ditt användarnamn och pin-kod för att fortsätta!");

            while(true) {
                Console.Write("Användarnamn: ");
                StringTemp = Console.ReadLine();
                Console.Write("Pin-kod (4 siffror): ");
                int.TryParse(Console.ReadLine(), out IntTemp);

                if (CheckLogin(StringTemp, IntTemp, AnvnändarNamnArray, PinKodArray, out UserIndex))
                {
                    MenyVal = 0;
                    while (MenyVal != 4)
                    {
                        Console.WriteLine($"Välkommen {StringTemp}!");
                        Console.WriteLine("1. Se dina konton och saldo");
                        Console.WriteLine("2. Överföring mellan konton");
                        Console.WriteLine("3. Ta ut pengar");
                        Console.WriteLine("4. Logga ut");
                        Console.Write("Val (1-4): ");
                        if (!(int.TryParse(Console.ReadLine(), out MenyVal) && MenyVal <= 4 && MenyVal >= 1))
                        {
                            Console.WriteLine("Ogiltigt val");
                            continue;
                        }
                        switch (MenyVal){
                            case 1:
                                for(int i = 0;i < AnvändarKonton[UserIndex].Count;i++)
                                {
                                    Console.WriteLine($"{AnvändarKonton[UserIndex][i]} : {KontoSaldon[UserIndex][i]}");
                                }
                                break;
                            case 2:
                                Console.WriteLine("Ta ut pengar!");
                                for(int i = 0; i < AnvändarKonton[UserIndex].Count; i++)
                                {
                                    Console.WriteLine($"{i+1}. {AnvändarKonton[UserIndex][i]}");
                                }
                                Console.WriteLine("Välj:");
                                int KontoNummer = int.Parse(Console.ReadLine());
                                int DisplayNummer = 1;
                                for (int i = 0; i < AnvändarKonton[UserIndex].Count; i++)
                                {
                                    if(i == (KontoNummer-1))
                                    {
                                        continue;
                                    }
                                    Console.WriteLine($"{DisplayNummer}. {AnvändarKonton[UserIndex][i]}");
                                    DisplayNummer++;
                                }


                                break;
                            case 3:
                                break;
                            case 4:
                                Console.WriteLine($"Hej då {StringTemp}");
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
                    Console.WriteLine("Fel lösenord eller Användarnamn");
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
