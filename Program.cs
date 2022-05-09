using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;

namespace ConsoleApp1
{

    public class Program
    {
        public string Name;
        public string Currency = "$";
        static Screen screen;

        public static void Main(string[] args)
        {
            Screen screen = new Screen();
            Program program = new Program();

            CinemaConsoleApp(program);
            Menu(program);
            UserSel(program);
        }

        public static void CinemaConsoleApp(Program program)
        {

        Console.WriteLine("İşlem yapmak istediğiniz salonu tanımlayın: ");
            Console.WriteLine();
            Console.Write("Salonun adı: ");
            string scName = Console.ReadLine();

        Console.Write("Salonun izleyici kapasitesi: ");
            string input = Console.ReadLine();
        int capacity = IntInputValidation(input);
        int emptySeats = capacity;

        Console.Write("Mevcut bakiyeniz: {0}", program.Currency);
            input = Console.ReadLine();
            float balance = InputValidation(input);

        Console.Write("Tam bilet fiyatı: {0}", program.Currency);
            input = Console.ReadLine();
            float priceF = InputValidation(input);

        Console.Write("İndirimli bilet fiyatının indirim oranı: %");
            input = Console.ReadLine();
            float discount = InputValidation(input);
        float priceH = Screen.ReducedPrice(priceF, discount);

        screen = new Screen(scName, capacity, emptySeats, balance, priceF, priceH);

    }

    static void Menu(Program program) // Main Menu
    {
        AddingNonsenseSides( "_");
        Console.Write("______________Ana Menu_______________");
        AddingNonsenseSides( "_");
        Console.WriteLine(); Console.WriteLine();
        AddingNonsenseSides( " "); Console.WriteLine("           D  Son durum");
        AddingNonsenseSides( " "); Console.WriteLine("           S  Bilet satışı");
        AddingNonsenseSides( " "); Console.WriteLine("           I  Bilet iadesi");
        AddingNonsenseSides( " "); Console.WriteLine("           B  Detaylı bilgi");
        AddingNonsenseSides( " "); Console.WriteLine("           X  Çıkış");
        AddingNonsenseSides( " "); Console.WriteLine();
        Console.Write("Yapmak istediğiniz işlem harfini girin: ");

        UserSel( program);
    }

    #region Main Menu Methods
    public static void LastState(Program program) // D Last State
    {
        program.Name = "LastState";
        AddingNonsense("_");

        Console.WriteLine("________Salon {0} için son durum________", screen.ScName);
        Console.WriteLine();
        Console.WriteLine("Boş koltuk adedi: " + screen.EmptySeats);
        Console.WriteLine("Satılan tam bilet adedi: " + screen.SoldSeatsTotF);
        Console.WriteLine("Satılan yarım bilet adedi: " + screen.SoldSeatsTotH);

        AddingNonsense("_");
        SubMenu( program);
    }

    public static void TicketOfficeSale(Program program) // S Sale 
    {
        program.Name = "TicketOfficeSale";
        AddingNonsense("_");

        Console.WriteLine("_______Salon {0} için bilet satışı_______", screen.ScName);
        Console.WriteLine();

        Console.Write("Satılacak tam bilet adedi: ");
        string input = Console.ReadLine();
        screen.SoldSeatsF = IntInputValidation(input);

        Console.Write("Satılacak yarım bilet adedi: ");
        input = Console.ReadLine();
        screen.SoldSeatsH = IntInputValidation(input);
        Console.WriteLine();

        if ((screen.SoldSeatsH + screen.SoldSeatsF) <= screen.EmptySeats && (screen.SoldSeatsTotF + screen.SoldSeatsTotH) <= screen.Capacity)
        {
            // Num of Seats
            screen.SoldSeatsTotF = Screen.NumSeats(screen.SoldSeatsF, screen.SoldSeatsTotF);
            screen.SoldSeatsTotH = Screen.NumSeats(screen.SoldSeatsH, screen.SoldSeatsTotH);
            screen.TotSeatSold = Screen.NumTotSeat(screen.SoldSeatsTotF, screen.SoldSeatsTotH);
            screen.EmptySeats = Screen.NumEmptySeats(screen.Capacity, screen.TotSeatSold, screen.EmptySeats);

            // Price calculation
            screen.RevF = Screen.Cost(screen.PriceF, screen.SoldSeatsF);
            screen.RevTotF += screen.RevF;
            screen.RevH = Screen.Cost(screen.PriceH, screen.SoldSeatsH);
            screen.RevTotH += screen.RevH;
            screen.RevTot = Screen.Sum(screen.RevF, screen.RevH);
            screen.Balance += Screen.Sum(screen.RevF, screen.RevH);

            Console.WriteLine("Satış işleminiz başarıyla tamamlanmıştır.");
            Console.WriteLine("Satış işleminden kazancınız: {0} ", program.Currency + screen.RevTot);
        }
        else
        {
            Console.WriteLine("Girdiğiniz bilet sayısı kapasitenin üzerindedir.");
            Console.WriteLine("Alabileceğiniz toplam boş koltuk adedi: " + screen.EmptySeats);
            Console.WriteLine();
            SubMenu(program);
        }
        AddingNonsense("_");
        SubMenu(program);
    }

    public static void TicketOfficeRefund(Program program) // I Refund 
    {
        program.Name = "TicketOfficeRefund";
        AddingNonsense( "_");

        Console.WriteLine("_______Salon {0} için bilet iadesi_______", screen.ScName);
        Console.WriteLine();

        Console.Write("İade edilecek tam bilet adedi: ");
        string input = Console.ReadLine();
        screen.RefSeatsF = IntInputValidation(input);

        Console.Write("İade edilecek yarım bilet adedi: ");
        input = Console.ReadLine();
        screen.RefSeatsH = IntInputValidation(input);
        Console.WriteLine();

        if (screen.RefSeatsF <= screen.SoldSeatsTotF && screen.RefSeatsH <= screen.SoldSeatsTotH && (screen.RefSeatsF + screen.RefSeatsH) <= screen.TotSeatSold)
        {
            // Num of Seats
            screen.RefSeatsTotF = Screen.NumSeats(screen.RefSeatsF, screen.RefSeatsTotF);
            screen.SoldSeatsTotF -= screen.RefSeatsF;
            screen.RefSeatsTotH = Screen.NumSeats(screen.RefSeatsH, screen.RefSeatsTotH);
            screen.SoldSeatsTotH -= screen.RefSeatsH;
            screen.TotSeatSold -= Screen.NumTotSeat(screen.RefSeatsF, screen.RefSeatsH);
            screen.EmptySeats = Screen.NumEmptySeats(screen.Capacity, screen.TotSeatSold, screen.EmptySeats);

            // Price calculation
            screen.RefRevF = Screen.Cost(screen.PriceF, screen.RefSeatsF);
            screen.RevTotF -= screen.RefRevF;
            screen.RefRevH = Screen.Cost(screen.PriceH, screen.RefSeatsH);
            screen.RevTotH -= screen.RefRevH;
            screen.RefRevTotal = Screen.Sum(screen.RefRevF, screen.RefRevH);
            screen.Balance = Screen.Refund(screen.RefRevF, screen.RefRevH, screen.Balance);

            Console.WriteLine("İade işleminiz başarıyla tamamlanmıştır.");
            Console.WriteLine("İade işleminden kaybınız: {0} ", program.Currency + screen.RefRevTotal);
        }
        else
        {
            Console.WriteLine("Girdiğiniz bilet sayısı satılan bilet sayısının üzerindedir.");
            Console.WriteLine("İade edilebilecek tam bilet adedi: " + screen.SoldSeatsTotF);
            Console.WriteLine("İade edilebilecek indirimli bilet adedi: " + screen.SoldSeatsTotH);
            Console.WriteLine("İade edilebilecek toplam satılmış bilet adedi: " + screen.TotSeatSold);
            Console.WriteLine();
            SubMenu( program);
        }
        AddingNonsense( "_");
        SubMenu( program);
    }

    public static void DetailedState(Program program) // B Detailed State 
    {
        program.Name = "DetailedState";
        AddingNonsense( "_");

        Console.WriteLine("____Salon {0} için detaylı son durum_____", screen.ScName);
        Console.WriteLine();

        Console.WriteLine("İzleyici kapasitesi: " + screen.Capacity);
        Console.WriteLine("Boş koltuk adedi: " + screen.EmptySeats);
        Console.WriteLine("Satılan tam bilet adedi: " + screen.SoldSeatsTotF);
        Console.WriteLine("Satılan indirimli bilet adedi: " + screen.SoldSeatsTotH);
        Console.WriteLine("Satılan tüm biletlerin adedi: " + screen.TotSeatSold);
        Console.WriteLine();

        Console.WriteLine("Güncel bakiye: {0} ", program.Currency + screen.Balance);
        Console.WriteLine("Tam bilet fiyatı {0} ", program.Currency + screen.PriceF);
        Console.WriteLine("İndirimli bilet fiyatı {0} ", program.Currency + screen.PriceH);
        Console.WriteLine("Satılan tam biletlerin geliri: {0} ", program.Currency + screen.RevTotF);
        Console.WriteLine("Satılan indirimli biletlerin geliri: {0} ", program.Currency + screen.RevTotH);
        Console.WriteLine("Satılan tüm biletlerin geliri: {0} ", program.Currency + (screen.RevTotH + screen.RevTotF));

        AddingNonsense( "_");
        SubMenu( program);
    }
    public static void TheEnd() // X
    {
        Console.WriteLine();
        Console.WriteLine("Program sonlandırılmıştır. ");
        Environment.Exit(0);
    }
    #endregion

    #region Input Recognition and Switch 
    static void UserSel(Program program) // Menu branch selection according to user Input
    {
        string userInput = Console.ReadLine();
        string userInputUpper = userInput.ToUpper();
        switch (userInputUpper)
        {
            case "D":
                LastState(program);
                break;

            case "S":
                TicketOfficeSale( program);
                break;

            case "I":
                TicketOfficeRefund( program);
                break;

            case "B":
                DetailedState( program);
                break;

            case "X":
                TheEnd();
                break;

            default:
                Console.WriteLine();
                Console.Write("Lütfen geçerli bir harf girin: ");
                UserSel( program);
                break;
        }
    }
    public static void SubMenu(Program program) // Sub Menu selection according to user Input 
    {
        Console.WriteLine();
        Console.Write("Ana menuye dönmek için L, işleme devam etmek için C, programdan çıkmak için X yazın: ");
        string userInput = Console.ReadLine();
        string userInputUpper = userInput.ToUpper();
        Console.WriteLine();

        if (userInputUpper == "L")
            Menu( program);

        else if (userInputUpper == "C")
            MenuSwitcher(program);

        else if (userInputUpper == "X")
            TheEnd();

        else
        {
            Console.WriteLine();
            Console.Write("Lütfen geçerli bir harf girin: ");
            SubMenu( program);
        }
    }

    public static void MenuSwitcher(Program program) // Sub menu switch method according to string
    {
        switch (program.Name)
        {
            case "TicketOfficeRefund":
                TicketOfficeRefund( program);
                break;

            case "TicketOfficeSale":
                TicketOfficeSale(program);
                break;

            case "LastState":
                LastState(program);
                break;

            case "DetailedState":
                DetailedState(program);
                break;

            default:
                break;
        }
    }
    #endregion

    #region Tools
    public static float InputValidation(string input) // Float parse validation
    {
        float check;
        while (!float.TryParse(input, out check))
        {
            Console.Write("Hatalı giriş yaptınız, lütfen sadece sayi girişi yapın: ");
            input = Console.ReadLine();
            Console.WriteLine();
        }
        return check;
    }

    public static int IntInputValidation(string input) // Int parse validation
    {
        int check;
        while (!int.TryParse(input, out check))
        {
            Console.Write("Hatalı giriş yaptınız, lütfen sadece sayi girişi yapın: ");
            input = Console.ReadLine();
            Console.WriteLine();
        }
        return check;
    }

    public static void AddingNonsense(string input)  // Adding string according to name length 
    {
        int repeat = 36;
        Console.WriteLine();
        for (int i = 0; i <= screen.ScName.Length; i++)
        {
            Console.Write((input));
        }

        for (int i = 0; i <= repeat; i++)
        {
            Console.Write((input));
        }

        Console.WriteLine(); Console.WriteLine();
    }

    public static void AddingNonsenseSides(string input)  // Adding half of a string according to name length 
    {
        for (int i = 0; i <= screen.ScName.Length / 2; i++)
        {
            Console.Write(input);
        }
    }


    #endregion

}

}
