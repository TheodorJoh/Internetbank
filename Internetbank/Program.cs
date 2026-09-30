namespace Internetbank
{
    internal class Program
    {

        static bool CheckLogin(string AnvändarNamn, int PinKod, string[] AnvändarNamnArray, int[] PinKodArray)
        {
            for(int i = 0; i < AnvändarNamnArray.Length;i++)
            {
                if(AnvändarNamn == AnvändarNamnArray[i] && PinKod == PinKodArray[i])
                {
                    return true;
                }
            }
            return false;
        }




        static void Main(string[] args)
        {
            string StringTemp;
            int IntTemp = 0;
            int MenyVal = 0;
            int PinAttempts = 0;
            int LockoutTime = 10;
            string[] AnvnändarNamnArray = { "Theodor", "Leon", "Shenol", "Zandra", "Albion" };
            int[] PinKodArray = { 1337, 2337, 3337, 4337, 5337 };

            Console.WriteLine("Välkommen till TheodorBank skriv in ditt användarnamn och pin-kod för att fortsätta!");

            while(true) {
                Console.Write("Användarnamn: ");
                StringTemp = Console.ReadLine();
                Console.Write("Pin-kod (4 siffror): ");
                int.TryParse(Console.ReadLine(), out IntTemp);

                if (CheckLogin(StringTemp, IntTemp, AnvnändarNamnArray, PinKodArray))
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
                                break;
                            case 2:
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
