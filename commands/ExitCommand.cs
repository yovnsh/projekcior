namespace Projekcior.Commands {
    class ExitCommand : CommandGroup {
        public bool ExecuteCommand(string cmd, Argument[] args) {
            switch(cmd) {
                case "exit":
                    if(Program.ShellMode)
                    {
                        exit(args);
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }

        void exit(Argument[] args) {
            if(args.Length > 0) {
                throw new ArgumentException();
            }
            Program.ExitRequest = true;
            Console.WriteLine("wychodzimy stąd");
        }
    }
}