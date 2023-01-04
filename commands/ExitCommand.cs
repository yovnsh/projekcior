namespace Projekcior.Commands {
    class ExitCommand : CommandGroup {
        public bool ExecuteCommand(string cmd, Argument[] args) {
            switch(cmd) {
                case "exit":
                    if(Program.ShellMode)
                    {
                        exit(args);
                        return true;
                    } else
                    {
                        return false;
                    }
                default:
                    return false;
            }
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