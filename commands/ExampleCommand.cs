namespace Projekcior.Commands {
    class ExampleCommand : CommandGroup {
        public bool ExecuteCommand(string cmd, string[] args) {
            switch(cmd) {
                case "exit":
                    exit(args);
                    break;
                case "test":
                    mov(args);
                    break;
                default:
                    return false;
            }
            return true;
        }

        void exit(string[] args) {
            if(args.Length > 0) {
                throw new ArgumentException();
            }
            Program.ExitRequest = true;
            Console.WriteLine("wychodzimy stąd");
        }

        void mov(string[] args) {
            
        }
    }
}