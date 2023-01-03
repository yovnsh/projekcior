namespace Projekcior.Commands
{
    class PrzesylanieDanych : CommandGroup
    {
        public bool ExecuteCommand(string cmd, Argument[] args)
        {
            switch (cmd)
            {
                case "mov":
                    mov(args);
                    break;
                case "push":
                    push(args);
                    break;
                case "pop":
                    pop(args);
                    break;
                case "xchg":
                    xchg(args);
                    break;
                case "lea":
                    lea(args);
                    break;
                case "lds":
                    lds(args);
                    break;
                case "les":
                    les(args);
                    break;
                case "lahf":
                    lahf(args);
                    break;
                case "sahf":
                    sahf(args);
                    break;
                case "popf":
                    popf(args);
                    break;
                case "pushf":
                    pushf(args);
                    break;
                case "stc":
                    stc(args);
                    break;
                case "clc":
                    clc(args);
                    break;
                default:
                    return false;
            }
            return true;
        }

        // kopiuje dane z argumentu 1 do argumentu 0
        // arg[0] = arg[1]
        void mov(Argument[] args)
        {
            // typy sprawdzić
            if(args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            var flag_type = typeof(FlagArgument);
            if (args[0].GetType() == flag_type || args[1].GetType() == flag_type) {
                throw new ArgumentException("nie można używać flag w instrukcji mov");
            }

            args[0].Set(args[1]);
        }


        void push(Argument[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }
            UInt16 stack_segment = Program.Pamiec.Segmenty["SS"];
            UInt16 stack_pointer = Program.Pamiec.Wskazniki["SP"];
            stack_pointer += 1;
            Program.Pamiec.Wskazniki["SP"] = stack_pointer;
            Program.Pamiec.PamiecAdresowana[stack_segment + stack_pointer] = args[0].Get().ToString();

        }

        void pop(Argument[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }
            UInt16 stack_segment = Program.Pamiec.Segmenty["SS"];
            UInt16 stack_pointer = Program.Pamiec.Wskazniki["SP"];
            args[0].Set(Convert.ToInt16(Program.Pamiec.PamiecAdresowana[stack_segment + stack_pointer]));
            stack_pointer -= 1;
            Program.Pamiec.Wskazniki["SP"] = stack_pointer;
        }

        void xchg(Argument[] args)
        {
            // typy
            if(args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }
            Int16 tmp = args[0].Get();
            args[0].Set(args[1]);
            args[1].Set(tmp);
        }

        void lea(Argument[] args)
        {
            //...
        }

        void lds(Argument[] args)
        {
            //...
        }

        void les(Argument[] args)
        {
            //...
        }

        void lahf(Argument[] args)
        {
            //...
        }

        void sahf(Argument[] args)
        {
            //...
        }

        void popf(Argument[] args)
        {
            //...
        }

        void pushf(Argument[] args)
        {
            //...
        }

        void stc(Argument[] args)
        {
            Program.Pamiec.Flagi["CF"] = true;
        }

        void clc(Argument[] args)
        {
            Program.Pamiec.Flagi["CF"] = false;
        }
    }
}
