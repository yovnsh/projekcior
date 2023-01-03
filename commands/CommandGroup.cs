namespace Projekcior.Commands {
    interface CommandGroup {
        bool ExecuteCommand(string command, Argument[] args);
    }
}