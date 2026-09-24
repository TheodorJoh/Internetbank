namespace Internetbank
{
    internal class Program
    {

        static bool CheckPinKod(int PinKod, int[] PinKodArray)
        {
            foreach (int pin in PinKodArray)
            {
                if (PinKod == pin) return true;
            }
            return false;
        }

        static bool CheckAnvändarNamn(string AnvändarNamn, string[] AnvändarNamnArray)
        {
            foreach (string Namn in AnvändarNamnArray)
            {
                if (AnvändarNamn == Namn) return true;
            }
            return false;
        }




        static void Main(string[] args)
        {
            string StringTemp;
            int IntTemp;
            int PinAttempts = 0;
            int LockoutTime = 10;
            string[] AnvnändarNamnArray = { "Theodor", "Leon", "Shenol", "Zandra", "Albion" };
            int[] PinKodArray = { 1337, 2337, 3337, 4337, 5337 };

            Console.WriteLine("Välkommen till TheodorBank skriv in ditt användarnamn och pin-kod för att fortsätta!");
            Console.Write("Användarnamn: ");
            StringTemp = Console.ReadLine();
            Console.Write("Pin-kod (4 siffror): ");
            int.TryParse(Console.ReadLine(), out IntTemp);
            PinAttempts++;
            while(true) {
                while (!(CheckAnvändarNamn(StringTemp, AnvnändarNamnArray) && CheckPinKod(IntTemp, PinKodArray)))
                {
                    Console.WriteLine("Fel lösenord eller Användarnamn");
                    Console.Write("Användarnamn: ");
                    StringTemp = Console.ReadLine();
                    Console.Write("Pin-kod (4 siffror): ");
                    int.TryParse(Console.ReadLine(), out IntTemp);
                    PinAttempts++;
                    if (PinAttempts == 3)
                    {
                        Console.WriteLine("Du har använt alla dina försök du måste vänta 10 minuter innana du kan försöak igen!");
                        DateTime startTime = DateTime.UtcNow;
                        TimeSpan breakDuration = TimeSpan.FromMinutes(LockoutTime);
                        while (true)
                        {
                            if (DateTime.UtcNow - startTime > breakDuration)
                            {
                                LockoutTime = LockoutTime * 2;
                                PinAttempts--;
                                break;
                            }
                        }
                    }
                }

                if (CheckAnvändarNamn(StringTemp, AnvnändarNamnArray) && CheckPinKod(IntTemp, PinKodArray))
                {
                    while (true)
                    {
                        Console.WriteLine($"Välkommen {StringTemp}!");
                        Console.WriteLine("1. Se dina konton och saldo");
                        Console.WriteLine("2. Överföring mellan konton");
                        Console.WriteLine("3. Ta ut pengar");
                        Console.WriteLine("4. Logga ut");
                        Console.Write("Val (1-4): ");
                        if (!(int.TryParse(Console.ReadLine(), out IntTemp) && IntTemp <= 4 && IntTemp >= 1))
                        {
                            Console.WriteLine("Ogiltigt val");
                            continue;
                        }
                        switch (IntTemp){
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                Console.WriteLine($"Hej då {StringTemp}");
                                IntTemp = 0;
                                StringTemp = "";
                                break;
                            default:
                                break;
                        }
                    }


                }
            }
        }
    } }
