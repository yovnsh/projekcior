using Projekcior;
using Projekcior.Commands;

namespace Projekcior {
    class Program {

        public static bool ExitRequest = false;
        public static List<CommandGroup> Commands = new List<CommandGroup>();
        public static Hipokamp Pamiec = new Hipokamp();
        public static string OstatniaInstrukcja = "";
        public static bool ShellMode = false;
        public static Int16 InterruptCode = 0;


        // punkt wejściowy
        // najpierw sprawdza argumenty - jeśli ich nie ma to uruchamia wybor trybu
        //
        // argumenty:
        // --file [nazwa] [--debug] - uruchamia tryb wczytywania z pliku
        // --shell                  - uruchamia tryb wczytywania komend z konsoli
        public static void Main(string[] args) {
            RegisterCommands(); // podpina wszystkie moduły z komendami

            // jeśli podano w argument w cli to nie wyświetli się wybór trybu programu
            if (args.Length > 0)
            {
                if (args.Contains("--file") && args.Contains("--shell"))
                {
                    Console.WriteLine("nie mozna uruchomic jednoczesnie file i shell");
                    return;
                }

                switch (args[0])
                {
                    case "--file":
                        if (!HandleFileCommand(args))
                        {
                            return;
                        }
                        break;
                    case "--shell":
                        RunShell();
                        break;
                    default:
                        Console.WriteLine("zla komenda");
                        return;
                }

                return;
            }

            // jeśli natomiast nie ma nic w argumentach to lecimy z wyborem trybu
            while (!ExitRequest)
            {
                Console.Clear();
                Console.WriteLine("Komendy:\n");
                Console.WriteLine("file [filename] [--debug]  - wczytuje plik z instrukcjami asemblera i go uruchamia");
                Console.WriteLine("shell                                 - uruchamia konsole, w ktorej mozesz wpisywac pojedynczo komendy");
                Console.WriteLine("exit                                  - wychodzi z programu");
                Console.Write("\n> ");
                
                // pobieramy i rozdzielamy komendę urzytkownika na kawałki
                string[] command = ParseCLIArguments(Console.ReadLine());

                if (command.Length < 1)
                {
                    continue;
                }

                if (command.Contains("file") && command.Contains("shell"))
                {
                    Console.WriteLine("nie mozna uruchomic jednoczesnie file i shell");
                    Console.ReadKey();
                    continue;
                }

                switch (command[0])
                {
                    case "file":
                        if(!HandleFileCommand(command))
                        {
                            Console.ReadKey();
                            continue;
                        }
                        break;
                    case "shell":
                        RunShell();
                        break;
                    case "exit":
                        ExitRequest = true;
                        break;
                    default:
                        Console.WriteLine("zla komenda");
                        Console.ReadKey();
                        continue;
                }
            }
        }

        /// <summary>
        /// wywoływany jeśli wybrano tryb odczytywania pliku
        /// uruchamia tryb odczytywania z pliku z podnymi parametrami w komendzie
        /// </summary>
        /// <param name="command">komenda wywołania trybu pliku</param>
        /// <returns>bool wskazujący czy komenda wywołania trybu pliku jest poprawna</returns>
        /// <example>
        /// static void Main(args[]) {
        ///     // use case 1
        ///     if(args[0] == "--file") {
        ///         HandleFileCommand(args);
        ///     }
        ///     // use case 2
        ///     string[] command = ParseCLIArguments(Console.ReadLine());
        ///     if(command[0] == "file") {
        ///         HandleFileCommand(command);
        ///     }
        /// }
        /// </example>
        static bool HandleFileCommand(string[] command)
        {
            if (command.Length < 2)
            {
                Console.WriteLine("komenda file potrzebuje podania pliku");
                return false;
            }

            bool debug = false;
            if (command.Length > 2 && command.Contains("--debug"))
            {
                debug = true;
            }

            RunFile(command[1], debug);
            return true;
        }

        /// <summary>
        /// rozdziela komendę na oddzielne człony
        /// człony są rozdzielane spacjami
        /// w fragmentach ograniczonych cudzysłowiami spacja nie jest traktowana jako separator
        /// </summary>
        /// <param name="commandLine">komenda do przetworzenia</param>
        /// <returns>tablica z członami komendy</returns>
        /// <example>
        /// var cmd = "1 2 3";
        /// Console.WriteLine(ParseCLIArguments(cmd).ToString()); // ["1", "2", "3"]
        /// 
        /// var cmd2 = "file \"nazwa ze spacjami\" --debug";
        /// Console.WriteLine(ParseCLIArguments(cmd2).ToString()); // ["file", "nazwa ze spacjami", "--debug"]
        /// </example>
        static string[] ParseCLIArguments(string? commandLine)
        {
            if(commandLine == null || commandLine.Length == 0) {
                return new string[] {};
            }

            char[] parmChars = commandLine.ToCharArray();
            bool inQuote = false;
            for (int index = 0; index < parmChars.Length; index++)
            {
                if (parmChars[index] == '"')
                    inQuote = !inQuote;
                if (!inQuote && parmChars[index] == ' ')
                    parmChars[index] = '\n';
            }
            return (new string(parmChars)).Split('\n');
        }

        /// <summary>
        /// logika trybu pliku
        /// </summary>
        /// <param name="filename">plik z programem do uruchomienia</param>
        /// <param name="debug">
        /// bool czy flaga TF powinna być ustawiona
        /// sprawia ona że po każdej instrukcji nastąpi przerwanie
        /// pozwala ona to na uruchamianie programu linijka po linijce
        /// </param>
        public static void RunFile(string filename, bool debug)
        {
            Program.Pamiec.Flagi.TF = debug;
            string[] lines;

            try
            {
                lines = File.ReadAllLines(filename);
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("plik nie istnieje");
                return;
            }

            UInt16 StackPointer;
            try
            {
                // program może mieć co najwyżej tyle instrukcji
                // na ile jest w stanie pozwolić typ UInt16
                // czyli 65535 instrukcji
                StackPointer = Convert.ToUInt16(lines.Length);
            }
            catch(OverflowException)
            {
                Console.WriteLine("za duży program");
                return;
            }

            // pamięć ma taką strukturę
            // [Kod...Stack...]
            // ze względu na to że wszystko znajduje się w jednej pamięci
            // używamy segmentu CS, który wskazuje na indeks pierwszej instrukcji
            // segment SS wskazuje na pierwszy indeks stacku
            // dlatego ustawiamy segment SS na indeks zaraz za ostatnią instrukcją kodu
            Program.Pamiec.Segmenty["SS"] = StackPointer;

            for(int i = 0; i < lines.Length; i++)
            {
                Program.Pamiec.PamiecAdresowana[i] = lines[i];
            }

            UInt16 AdresInstrukcji = Convert.ToUInt16(Program.Pamiec.Segmenty["CS"] + Program.Pamiec.Wskazniki["IP"]);

            // wykonujemy instrukcje dopóki nie dotrzemy do ich końca czyli segmentu stacku
            bool error = false;
            while (AdresInstrukcji < Program.Pamiec.Segmenty["SS"])
            {
                try
                {
                    if (Program.Pamiec.Flagi.TF)
                    {
                        Console.Clear();
                        PrintData();
                        Console.WriteLine("\n");
                    }

                    ReadInstruction(Program.Pamiec.PamiecAdresowana[AdresInstrukcji]);
                    AdresInstrukcji = Convert.ToUInt16(Program.Pamiec.Segmenty["CS"] + Program.Pamiec.Wskazniki["IP"]);
                }
                catch(NotImplementedException)
                {
                    Console.Clear();
                    PrintData();
                    Console.WriteLine("\n");
                    error = true;

                    Console.WriteLine("błąd w linijce " + Program.Pamiec.Wskazniki["IP"]);
                    Console.WriteLine(Program.Pamiec.PamiecAdresowana[AdresInstrukcji]);
                    Console.WriteLine("komenda niezaimplementowana");
                    break;
                }
                catch(ArgumentException e)
                {
                    Console.Clear();
                    PrintData();
                    Console.WriteLine("\n");
                    error = true;

                    Console.WriteLine("błąd w linijce " + Program.Pamiec.Wskazniki["IP"]);
                    Console.WriteLine(Program.Pamiec.PamiecAdresowana[AdresInstrukcji]);
                    Console.WriteLine(e.Message);
                    break;
                }
                catch(Exception e)
                {
                    Console.Clear();
                    PrintData();
                    Console.WriteLine("\n");
                    error = true;

                    Console.WriteLine("zgłoszono wyjątek podczas wykonywania linijki " + Program.Pamiec.Wskazniki["IP"]);
                    Console.WriteLine(Program.Pamiec.PamiecAdresowana[AdresInstrukcji]);
                    Console.WriteLine(e.Message);
                    break;
                }
            }

            if(!error)
            {
                Console.Clear();
                PrintData();
                Console.WriteLine("\n");
            }

            Console.WriteLine("\nKONIEC WYKONYWANIA PROGRAMU");
            Console.ReadKey();
        }

        /// <summary>
        /// logika trybu konsolowego
        /// pozwala on wpisywać na bieżąco komendy
        /// oraz natychmiast zobaczyć efekt ich wykonania
        /// UWAGA:
        /// tryb konsolowy nie obsługuje skoków ani pętl
        /// obsługuje natomiast dodatkową komendę exit która pozwala opuścić tryb konsolowy
        /// (ctr-c też działa)
        /// </summary>
        public static void RunShell()
        {
            // flaga która pozwala zmienić zachowanie niektórych komend
            // wyłącza skoki i włącza komendę exit
            Program.ShellMode = true; 
            while (!ExitRequest)
            {
                try
                {
                    Console.Clear();
                    PrintData();
                    Console.Write("\n> ");

                    string? instruction = Console.ReadLine();
                    if (instruction == null)
                        throw new EndOfStreamException();

                    ReadInstruction(instruction);
                }
                catch (EndOfStreamException)
                {
                    ExitRequest = true;
                }
                catch (NotImplementedException)
                {
                    Console.WriteLine("\nkiedyś zadziała");
                    Console.WriteLine("[wcisnij cokolwiek aby kontynuowac]");
                    Console.ReadKey();
                    continue;
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine("\nbłędne argumenty");
                    Console.WriteLine(exception.Message);
                    Console.WriteLine("[wcisnij cokolwiek aby kontynuowac]");
                    Console.ReadKey();
                    continue;
                }
                catch (Exception err)
                {
                    Console.WriteLine("\nbłąd: " + err.Message);
                    Console.WriteLine("[wcisnij cokolwiek aby kontynuowac]");
                    Console.ReadKey();
                    continue;
                }
            }
        }

        /// <summary>
        /// interpretuje i uruchamia instrukcję assemblera
        /// </summary>
        /// <param name="instruction">instrukcja Asemblera</param>
        /// <exception cref="Exception">
        /// wewnętrzny błąd wykonywania instrukcji lub komenda nie istnieje
        /// </exception>
        /// <exception cref="NotImplementedException">
        /// Instrukcja nie została zaimplementowana w tym emulatorze
        /// </exception>
        /// <exception cref="ArgumentException">błąd w argumentach instrukcji</exception>
        public static void ReadInstruction(string instruction) {
            string[] tmp = instruction.Split(';')[0].Trim().Split(" ", 2);

            string cmd = tmp[0].ToLower();

            string[] args;
            if (tmp.Length == 1) {
                args = new string[0];
            } else {
                args = tmp[1].Replace(" ", "").ToUpper().Split(",");
            }

            Argument[] args_converted = new Argument[args.Length];
            for(int n = 0; n < args.Length; n++)
            {
                args_converted[n] = ReadArgument(args[n]);
            }
            
            bool command_found = false;
            foreach(CommandGroup group in Commands) {
                if(group.ExecuteCommand(cmd, args_converted)) {
                    command_found = true;

                    if(Program.Pamiec.Flagi.TF)
                    {
                        Program.Pamiec.Flagi.IF = true;
                        Program.InterruptCode = 0;
                    }

                    try
                    {
                        checked
                        {
                            if (group.GetType() != typeof(Skoki))
                            {
                                Program.Pamiec.Wskazniki["IP"]++;
                            }
                            Program.OstatniaInstrukcja = instruction;
                        }
                    }
                    catch(OverflowException)
                    {
                        Console.WriteLine("limit instrukcji");
                        ExitRequest = true;
                        return;
                    }

                    if(Program.Pamiec.Flagi.IF)
                    {
                        Console.WriteLine("INTERRUPT " + Program.InterruptCode);
                        Console.WriteLine("[kliknij cokolwiek aby kontynuuować]");
                        Console.ReadKey();
                        Program.Pamiec.Flagi.IF = false;
                    }

                    break;
                }
            }

            if(!command_found) {
                throw new Exception("nieznana komenda");
            }
        }

        /// <summary>
        /// interpretuje argument
        /// </summary>
        /// <param name="argument_name">tekst argumentu</param>
        /// <returns>
        /// obiekt implementujący interface Argument
        /// pozwala on bezpośrednio ustawiać i pobierać wartość z odpowiednich miejsc w pamięci
        /// taka trochę referencja
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static Argument ReadArgument(string argument_name)
        {
            if (RegisterArgument.Contains(argument_name))
            {
                return new RegisterArgument(argument_name);
            }
            else if (HalfRegisterArgument.Contains(argument_name))
            {
                return new HalfRegisterArgument(argument_name);
            }
            else if (SegmentArgument.Contains(argument_name))
            {
                return new SegmentArgument(argument_name);
            }
            else if (PointerArgument.Contains(argument_name))
            {
                return new PointerArgument(argument_name);
            }
            else if (NumericConstant.Contains(argument_name))
            {
                return new NumericConstant(argument_name);
            } else if(MemoryArgument.Contains(argument_name))
            {
                return new MemoryArgument(argument_name);
            }
            else
            {
                throw new ArgumentException("nieznany argumenty", argument_name);
            }
        }

        /// <summary>
        /// prezentuje pamięć procesora
        /// wypisuje rejestry flagi ostatnią ostatnią rzecz na stacku i ostatnią instrukcję
        /// </summary>
        public static void PrintData()
        {
            Console.WriteLine($" ___________________________________________________________________________________________________ ");
            Console.WriteLine($"|                                                                                                   |");
            Console.WriteLine($"|                                              Rejestry                                             |");
            Console.WriteLine($"|___________________________________________________________________________________________________|");
            Console.WriteLine($"|                        |                        |                        |                        |");
            Console.WriteLine($"| {"AX",22} | {"BX",22} | {"CX",22} | {"DX",22} |");
            // rejestry
            Console.WriteLine($"| uint  {unchecked((UInt16)Program.Pamiec.Rejestry.AX),16} | uint  {unchecked((UInt16)Program.Pamiec.Rejestry.BX),16} | uint  {unchecked((UInt16)Program.Pamiec.Rejestry.CX),16} | uint  {unchecked((UInt16)Program.Pamiec.Rejestry.DX),16} |");
            Console.WriteLine($"| int   {Program.Pamiec.Rejestry.AX.ToString("+ #;- #;0"),16} | int   {Program.Pamiec.Rejestry.BX.ToString("+ #;- #;0"),16} | int   {Program.Pamiec.Rejestry.CX.ToString("+ #;- #;0"),16} | int   {Program.Pamiec.Rejestry.DX.ToString("+ #;- #;0"),16} |");
            Console.WriteLine($"| bcd   {ToBCDString(Program.Pamiec.Rejestry.AX),16} | bcd   {ToBCDString(Program.Pamiec.Rejestry.BX),16} | bcd   {ToBCDString(Program.Pamiec.Rejestry.CX),16} | bcd   {ToBCDString(Program.Pamiec.Rejestry.DX),16} |");
            Console.WriteLine($"| bin   {Convert.ToString(Program.Pamiec.Rejestry.AX, 2).PadLeft(16, '0')} | bin   {Convert.ToString(Program.Pamiec.Rejestry.BX, 2).PadLeft(16, '0')} | bin   {Convert.ToString(Program.Pamiec.Rejestry.CX, 2).PadLeft(16, '0')} | bin   {Convert.ToString(Program.Pamiec.Rejestry.DX, 2).PadLeft(16, '0')} |");

            Console.WriteLine($"|________________________|________________________|________________________|________________________|");
            Console.WriteLine($"|                                            |                           |                          |");
            Console.WriteLine($"|                    Flagi                   |         Instrukcja        |           Stack          |");
            Console.WriteLine($"|____________________________________________|___________________________|__________________________|");
            Console.WriteLine($"|    |    |    |    |    |    |    |    |    |                           |                          |");

            Console.Write($"| OF | DF | IF | TF | SF | ZF | AF | PF | CF |");
            Console.Write($" IP {Program.Pamiec.Wskazniki["IP"], 22} |");
            Console.Write($" SP {Program.Pamiec.Wskazniki["SP"], 21} |\n");

            // flagi
            Console.Write($"| {Convert.ToInt16(Program.Pamiec.Flagi.OF),2} | {Convert.ToInt16(Program.Pamiec.Flagi.DF),2} | {Convert.ToInt16(Program.Pamiec.Flagi.IF),2} | {Convert.ToInt16(Program.Pamiec.Flagi.TF),2} |");
            Console.Write($" {Convert.ToInt16(Program.Pamiec.Flagi.SF),2} | {Convert.ToInt16(Program.Pamiec.Flagi.ZF),2} | {Convert.ToInt16(Program.Pamiec.Flagi.AF),2} | {Convert.ToInt16(Program.Pamiec.Flagi.PF),2} |");
            Console.Write($" {Convert.ToInt16(Program.Pamiec.Flagi.CF),2} |");

            if(Program.OstatniaInstrukcja == "")
            {
                Console.Write($" {"---",25} |");
            }
            else
            {
                Console.Write($" {Program.OstatniaInstrukcja, 25} |");
            }

            if(Program.Pamiec.Wskazniki["SP"] == 0)
            {
                Console.Write($" {"---",24} |\n");
            } else
            {
                Console.Write($" {Program.Pamiec.PamiecAdresowana[Program.Pamiec.Segmenty["SS"] + Program.Pamiec.Wskazniki["SP"]], 24} |\n");
            }

            Console.WriteLine("|____|____|____|____|____|____|____|____|____|___________________________|__________________________|");
        }

        /// <summary>
        /// zamienia liczbę w kodowaniu BCD na tekst
        /// jeśli liczba nie może zostać w ten sposób przedstawiona to zwraca --
        /// </summary>
        /// <param name="n">liczba do przekonwertowania</param>
        /// <returns>zamieniona liczba lub --</returns>
        public static string ToBCDString(Int16 n)
        {
            string string_value = "";
            while(n != 0)
            {
                int tmp = n & 0b1111; // sprwadzamy ostatnie 4 bity
                if (tmp > 9)
                {
                    return "---";
                }
                string_value = tmp.ToString() + string_value;
                n >>= 4;
            }

            if(string_value == "")
            {
                return "0";
            }
            return string_value;
        }

        /// <summary>
        /// tutaj dodajemy wszystkie grupy komend jakie chcemy żeby program używał
        /// </summary>
        public static void RegisterCommands() {

            Commands.Add(new ExitCommand());
            Commands.Add(new PrzesylanieDanych());
            Commands.Add(new KomendyArytmetyczne());
            Commands.Add(new KomendyLogiczne());
            Commands.Add(new Skoki());
            Commands.Add(new Przerwania());
        }
    }
}