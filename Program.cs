using Projekcior;
using Projekcior.Commands;

namespace Projekcior {
    class Program {

        public static bool ExitRequest = false;
        public static List<CommandGroup> Commands = new List<CommandGroup>();
        public static Hipokamp Pamiec = new Hipokamp();

        public static void Main() {
            RegisterCommands();

            Console.WriteLine("czeć");

            while(!ExitRequest) {
                try {
                    ReadInstruction();
                } catch(EndOfStreamException) {
                    ExitRequest = true;
                }
                catch(NotImplementedException) {
                    Console.WriteLine("kiedyś zadziała");
                }
                catch(ArgumentException) {
                    Console.WriteLine("błędne argumenty");
                    break;
                }
                catch(Exception err) {
                    Console.WriteLine("błąd: " + err.Message);
                    break;
                }
            }
        }

        public static void ReadInstruction() {
            Console.Clear();
            PrintData();
            Console.Write("\n> ");

            string? instruction = Console.ReadLine();
            if(instruction == null)
                throw new EndOfStreamException();

            string[] tmp = instruction.Split(';')[0].Trim().Split(" ", 2);

            string cmd = tmp[0].ToLower();

            string[] args;
            if (tmp.Length == 1) {
                args = new string[0];
            } else {
                args = tmp[1].Replace(" ", "").Split(",");
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
                    break;
                }
            }

            if(!command_found) {
                Console.WriteLine("nieznana komenda");
            }
        }

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
            /*
             * nic tu nigdy nie istniało
            else if (FlagArgument.Contains(argument_name))
            {
                return new FlagArgument(argument_name);
            }
            */
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

        public static void PrintData()
        {
            Console.WriteLine($" ___________________________________________________________________________________________________ ");
            Console.WriteLine($"|                                                                                                   |");
            Console.WriteLine($"|                                              Rejestry                                             |");
            Console.WriteLine($"|___________________________________________________________________________________________________|");
            Console.WriteLine($"|                        |                        |                        |                        |");
            Console.WriteLine($"| {"AX",22} | {"BX",22} | {"CX",22} | {"DX",22} |");
            /*Console.Write($" OF | DF | IF | TF | SF | ZF | AF | PF | CF |");
            Console.Write($" {"IP",6} | {"SP",6} |\n");*/

            // rejestry
            Console.WriteLine($"| uint  {unchecked((UInt16)Program.Pamiec.Rejestry.AX),16} | uint  {unchecked((UInt16)Program.Pamiec.Rejestry.BX),16} | uint  {unchecked((UInt16)Program.Pamiec.Rejestry.CX),16} | uint  {unchecked((UInt16)Program.Pamiec.Rejestry.DX),16} |");
            Console.WriteLine($"| int   {Program.Pamiec.Rejestry.AX.ToString("+#;-#;0"),16} | int   {Program.Pamiec.Rejestry.BX.ToString("+#;-#;0"),16} | int   {Program.Pamiec.Rejestry.CX.ToString("+#;-#;0"),16} | int   {Program.Pamiec.Rejestry.DX.ToString("+#;-#;0"),16} |");
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

            if(Program.Pamiec.Wskazniki["IP"] == 0)
            {
                Console.Write($" {"---",25} |");
            }
            else
            {
                Console.Write($" {Program.Pamiec.PamiecAdresowana[Program.Pamiec.Segmenty["CS"] + Program.Pamiec.Wskazniki["IP"]], 25} |");
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

        public static string ToBCDString(Int16 n)
        {
            string string_value = "";
            while(n > 0)
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

        public static void RegisterCommands() {
            // tutaj dodajemy wszystkie grupy komend jakie chcemy żeby program używał
            Commands.Add(new ExampleCommand());
            Commands.Add(new PrzesylanieDanych());
            Commands.Add(new KomendyArytmetyczne());
            Commands.Add(new KomendyLogiczne());
        }
    }
}