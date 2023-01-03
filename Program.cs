using Projekcior;
using Projekcior.Commands;

namespace Projekcior {
    class Program {

        public static bool ExitRequest = false;
        public static List<CommandGroup> Commands = new List<CommandGroup>();
        public static Hipokamp Pamiec = new Hipokamp();

        public static void Main() {
            RegisterCommands();

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
        public static Argument ReadArgument(string argument_name) {
            if(RegisterArgument.Contains(argument_name))
            {
                return new RegisterArgument(argument_name);
            }
            else if (HalfRegisterArgument.Contains(argument_name))
            {
                return new HalfRegisterArgument(argument_name);
            }
            else if(SegmentArgument.Contains(argument_name))
            {
                return new SegmentArgument(argument_name);
            }
            else if(PointerArgument.Contains(argument_name))
            {
                return new PointerArgument(argument_name);
            }
            else if(FlagArgument.Contains(argument_name))
            {
                return new FlagArgument(argument_name);
            }
            else if (NumericConstant.Contains(argument_name))
            {
                return new NumericConstant(argument_name);
            } 
            else if (MemoryArgument.Contains(argument_name))
            {
                return new MemoryArgument(argument_name);
            }
            else
            {
                throw new ArgumentException("nieznany argument", argument_name);
            }
        }

        public static void PrintData()
        {
            Console.WriteLine($" ___________________________________ ____________________________________________ ___________________________________");
            Console.WriteLine($"|                                   |                                            |                                   |");
            Console.WriteLine($"|              Rejestry             |                    Flagi                   |              Segmenty             |");
            Console.WriteLine($"|___________________________________|____________________________________________|___________________________________|");
            Console.WriteLine($"|        |        |        |        |    |    |    |    |    |    |    |    |    |        |        |        |        |");

            Console.Write($"| {"AX", 6} | {"BX", 6} | {"CX", 6} | {"DX", 6} |");
            Console.Write($" OF | DF | IF | TF | SF | ZF | AF | PF | CF |");
            Console.Write($" {"CS", 6} | {"DS", 6} | {"ES", 6} | {"SS", 6} |\n");

            // rejestry
            Console.Write($"| {Program.Pamiec.Rejestry.AX, 6} | {Program.Pamiec.Rejestry.BX, 6} | {Program.Pamiec.Rejestry.CX, 6} | {Program.Pamiec.Rejestry.DX, 6} |");
            // flagi
            Console.Write($" {Convert.ToInt16(Program.Pamiec.Flagi["OF"]), 2} | {Convert.ToInt16(Program.Pamiec.Flagi["DF"]),2} | {Convert.ToInt16(Program.Pamiec.Flagi["IF"]),2} | {Convert.ToInt16(Program.Pamiec.Flagi["TF"]),2} |");
            Console.Write($" {Convert.ToInt16(Program.Pamiec.Flagi["SF"]),2} | {Convert.ToInt16(Program.Pamiec.Flagi["ZF"]),2} | {Convert.ToInt16(Program.Pamiec.Flagi["AF"]),2} | {Convert.ToInt16(Program.Pamiec.Flagi["PF"]),2} |");
            Console.Write($" {Convert.ToInt16(Program.Pamiec.Flagi["CF"]),2} |");
            // segmenty
            Console.Write($" {Program.Pamiec.Segmenty["CS"],6} | {Program.Pamiec.Segmenty["DS"],6} | {Program.Pamiec.Segmenty["ES"],6} | {Program.Pamiec.Segmenty["SS"],6} |\n");

            Console.WriteLine($"|________|________|________|________|____|____|____|____|____|____|____|____|____|________|________|________|________|");
        }

        public static void RegisterCommands() {
            // tutaj dodajemy wszystkie grupy komend jakie chcemy żeby program używał
            Commands.Add(new ExampleCommand());
            Commands.Add(new PrzesylanieDanych());
        }
    }
}