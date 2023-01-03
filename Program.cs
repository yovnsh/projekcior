using Projekcior;
using Projekcior.Commands;

namespace Projekcior {
    class Program {

        public static bool ExitRequest = false;
        public static List<CommandGroup> Commands = new List<CommandGroup>();
        public static Hipokamp Pamiec = new Hipokamp();

        public static void Main() {
            // init
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
                }
                catch(Exception err) {
                    Console.WriteLine("błąd: " + err.Message);
                    break;
                }
            }
        }

        public static void ReadInstruction() {
            Console.Write("> ");
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
                args_converted[n] = ReadArgument1(args[n]);
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
        public static Argument ReadArgument1(string argument_name) {
            if(RegisterArgument.Contains(argument_name))
            {
                Console.WriteLine("rejestr " + argument_name);
                return new RegisterArgument(argument_name);
            }
            else if (HalfRegisterArgument.Contains(argument_name))
            {
                Console.WriteLine("pół rejestr " + argument_name);
                return new HalfRegisterArgument(argument_name);
            }
            else if(SegmentArgument.Contains(argument_name))
            {
                Console.WriteLine("segment " + argument_name);
                return new SegmentArgument(argument_name);
            }
            else if(PointerArgument.Contains(argument_name))
            {
                Console.WriteLine("wskaźnik " + argument_name);
                return new PointerArgument(argument_name);
            }
            else if(FlagArgument.Contains(argument_name))
            {
                Console.WriteLine("flaga " + argument_name);
                return new FlagArgument(argument_name);
            }
            else
            {
                Console.WriteLine("liczba " + argument_name);
                return new NumericConstant(argument_name);
            }
        }

        public static void RegisterCommands() {
            // tutaj dodajemy wszystkie grupy komend jakie chcemy żeby program używał
            Commands.Add(new ExampleCommand());
        }
    }
}