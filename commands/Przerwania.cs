namespace Projekcior.Commands
{
    class Przerwania : CommandGroup
    {
        public bool ExecuteCommand(string cmd, Argument[] args)
        {
            switch (cmd)
            {
                case "int":
                    interrupt(args);
                    break;
                case "into":

                default:
                    return false;
            }
            return true;
        }

        void interrupt(Argument[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            if (args[0].GetType() != typeof(NumericConstant))
            {
                throw new ArgumentException("numer przerwania musi być liczbą");
            }

            Program.Pamiec.Flagi.IF = true;
            Program.InterruptCode = args[0].Get();
        }

        void into(Argument[] args)
        {
            if (Program.Pamiec.Flagi.OF) {
                interrupt(new Argument[] { new NumericConstant(4) });
            }
        }
    }
}