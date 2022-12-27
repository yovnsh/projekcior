using Projekcior;
using Projekcior.Commands;

namespace Projekcior {
    class Program {

        public static bool ExitRequest = false;
        public static List<CommandGroup> Commands = new List<CommandGroup>();
        private static Hipokamp Pamiec = new Hipokamp();

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
            
            bool command_found = false;
            foreach(CommandGroup group in Commands) {
                if(group.ExecuteCommand(cmd, args)) {
                    command_found = true;
                    break;
                }
            }

            if(!command_found) {
                Console.WriteLine("nieznana komenda");
            }
        }

        public static void RegisterCommands() {
            // tutaj dodajemy wszystkie grupy komend jakie chcemy żeby program używał
            Commands.Add(new ExampleCommand());
        }
    }
}