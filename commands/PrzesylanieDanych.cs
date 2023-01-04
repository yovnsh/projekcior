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
                case "in":
                    input(args);
                    break;
                case "out":
                    output(args);
                    break;
                default:
                    return false;
            }
            return true;
        }

        // kopiuje dane z argumentu[1] do argumentu [0]
        // arg[0] = arg[1]
        // 2 argumenty
        void mov(Argument[] args)
        {
            // typy sprawdzić
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            args[0].Set(args[1]);
        }

        // wstawia argument[0] na stack
        // 1 argument
        void push(Argument[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }
            UInt16 stack_segment = Program.Pamiec.Segmenty["SS"];
            UInt16 stack_pointer = Program.Pamiec.Wskazniki["SP"];

            try
            {
                checked
                {
                    stack_pointer += 1;
                    UInt16 stack_overflow_check = (UInt16)(stack_pointer + stack_segment);
                }
            }
            catch (OverflowException)
            {
                throw new Exception("stack overflow");
            }

            Program.Pamiec.Wskazniki["SP"] = stack_pointer;
            Program.Pamiec.PamiecAdresowana[stack_segment + stack_pointer] = args[0].Get().ToString();
        }

        // ściąga ostatnią liczbę ze stack'u i zapisuje ją w argumencie[0]
        // 1 argument
        void pop(Argument[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }
            UInt16 stack_segment = Program.Pamiec.Segmenty["SS"];
            UInt16 stack_pointer = Program.Pamiec.Wskazniki["SP"];
            args[0].Set(Convert.ToInt16(Program.Pamiec.PamiecAdresowana[stack_segment + stack_pointer]));

            try
            {
                checked
                {
                    stack_pointer -= 1;
                }
            }
            catch (OverflowException)
            {
                throw new Exception("stack underflow");
            }

            Program.Pamiec.Wskazniki["SP"] = stack_pointer;
        }

        // podmienia wartości argumentu[0] i argumentu[1]
        // 2 argumenty
        void xchg(Argument[] args)
        {
            // typy
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }
            Int16 tmp = args[0].Get();
            args[0].Set(args[1]);
            args[1].Set(tmp);
        }

        // zapisuje w argumencie[0] adres pamięci podanej w argumencie[1]
        // argument[1] musi być pamięcią
        // 2 argumenty
        void lea(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            if (args[1].GetType() != typeof(MemoryArgument))
            {
                throw new ArgumentException("drugi argument musi być wskaźnikiem pamięci");
            }

            MemoryArgument mem = (MemoryArgument)args[1];

            args[0].Set((Int16)mem.GetAddress());
        }

        // zapisuje w argumenci[0] wartość rejestru DS + adres pamięci podanej w argumencie[1]
        // argument[1] musi być pamięcią
        // 2 argumenty
        void lds(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            if (args[1].GetType() != typeof(MemoryArgument))
            {
                throw new ArgumentException("drugi argument musi być wskaźnikiem pamięci");
            }

            MemoryArgument mem = (MemoryArgument)args[1];
            args[0].Set((Int16)(Program.Pamiec.Segmenty["DS"] + mem.GetAddress()));
        }

        // zapisuje w argumencie[0] wartość rejestru ES + adres pamięci podanej w argumencie [1]
        // argument 1 musi być pamięcią
        void les(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            if (args[1].GetType() != typeof(MemoryArgument))
            {
                throw new ArgumentException("drugi argument musi być wskaźnikiem pamięci");
            }

            MemoryArgument mem = (MemoryArgument)args[1];
            args[0].Set((Int16)(Program.Pamiec.Segmenty["ES"] + mem.GetAddress()));
        }

        // kopiuje prawe 8 bitów (młodsze albo mniej znaczące) z rejestru statusu/flagi do rejestru AH
        // zapisane dane w AH będą miały format
        //  7  6  5  4  3  2  1  0
        // SF ZF -- AF -- PF -- CF
        // gdzie -- oznacza dowolną wartość
        // 0 argumentów
        void lahf(Argument[] args)
        {
            if (args.Length > 0)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            unchecked
            {
                Program.Pamiec.Rejestry.AH = (sbyte)(Program.Pamiec.Flagi.FlagiSurowe & 0x00ff);
            }
        }

        // kopiuje lewe 8 bitów (starsze albo bardziej znaczące) z rejestru statusu/flagi do rejestru AH
        // zapisane dane w AH będą miały format
        //  7  6  5  4  3  2  1  0
        // SF ZF -- AF -- PF -- CF
        // gdzie -- oznacza dowolną wartość
        // 0 argumentów
        void sahf(Argument[] args)
        {
            if (args.Length > 0)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            unchecked
            {
                Program.Pamiec.Rejestry.AH = (sbyte)((Program.Pamiec.Flagi.FlagiSurowe & 0xff00) >> 8);
            }
        }


        void popf(Argument[] args)
        {
            if (args.Length != 0)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            UInt16 stack_segment = Program.Pamiec.Segmenty["SS"];
            UInt16 stack_pointer = Program.Pamiec.Wskazniki["SP"];
            Int16 wartosc_na_stacku = Convert.ToInt16(Program.Pamiec.PamiecAdresowana[stack_segment + stack_pointer]);
            unchecked
            {
                Program.Pamiec.Flagi.FlagiSurowe = (UInt16)(wartosc_na_stacku);
            }
            stack_pointer -= 1;
            Program.Pamiec.Wskazniki["SP"] = stack_pointer;

        }

        void pushf(Argument[] args)
        {
            if (args.Length != 0)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            UInt16 stack_segment = Program.Pamiec.Segmenty["SS"];
            UInt16 stack_pointer = Program.Pamiec.Wskazniki["SP"];
            stack_pointer += 1;

            Int16 to_wkladamy_na_stack;
            unchecked
            {
                to_wkladamy_na_stack = (Int16)Program.Pamiec.Flagi.FlagiSurowe;
            }
            Program.Pamiec.PamiecAdresowana[stack_segment + stack_pointer] = to_wkladamy_na_stack.ToString();

            Program.Pamiec.Wskazniki["SP"] = stack_pointer;
        }

        void stc(Argument[] args)
        {
            Program.Pamiec.Flagi.CF = true;
        }

        void clc(Argument[] args)
        {
            Program.Pamiec.Flagi.CF = false;
        }

        void input(Argument[] args) {
            if(args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            if (args[1].GetType() != typeof(NumericConstant) && args[1].GetType() != typeof(RegisterArgument))
            {
                throw new ArgumentException("drugi argument może być tylko liczbą lub rejestrem DX");
            }

            if (args[1].GetType() == typeof(RegisterArgument) && ((RegisterArgument)args[1]).RegisterName != "DX")
            {
                throw new ArgumentException("drugi argument może być tylko rejestrem DX");
            }

            Console.Write("Podaj liczbę: ");
            string? input_number = Console.ReadLine();

            if(input_number == null) {
                input_number = "";
            }
            
            if(!NumericConstant.Contains(input_number))
            {
                throw new Exception("podana wartość nie jest prawidłową liczbą");
            }

            args[0].Set(new NumericConstant(input_number));
        }

        void output(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            if (args[1].GetType() != typeof(NumericConstant) && args[1].GetType() != typeof(RegisterArgument) && args[1].GetType() != typeof(HalfRegisterArgument))
            {
                throw new ArgumentException("drugi argument może być tylko liczbą lub rejestrem AX lub AL");
            }

            if (args[1].GetType() == typeof(HalfRegisterArgument) && ((HalfRegisterArgument)args[1]).HalfRegisterName != "AL")
            {
                throw new ArgumentException("drugi argument może być tylko rejestrem AX lub AL");
            }
            else if (args[1].GetType() == typeof(RegisterArgument) && ((RegisterArgument)args[1]).RegisterName != "AX")
            {
                throw new ArgumentException("drugi argument może być tylko rejestrem AX lub AL");
            }

            Console.WriteLine("Wyjscie: " + args[0].Get());
            Console.WriteLine("[wcisnij cokolwiek aby kontynuowac]");
            Console.ReadKey();
        }
    }
}
