using System;

namespace Projekcior.Commands
{
    class KomendyArytmetyczne : CommandGroup
    {
        public bool ExecuteCommand(string cmd, Argument[] args)
        {
            switch (cmd)
            {
                case "add":
                    add(args);
                    break;
                case "sub":
                    sub(args);
                    break;
                case "adc":
                    adc(args);
                    break;
                case "sbb":
                    sbb(args);
                    break;
                case "inc":
                    inc(args);
                    break;
                case "dec":
                    dec(args);
                    break;
                case "aaa":
                    aaa(args);
                    break;
                case "aas":
                    aas(args);
                    break;
                case "daa":
                    daa(args);
                    break;
                case "das":
                    das(args);
                    break;
                case "mul":
                    mul(args);
                    break;
                case "imul":
                    imul(args);
                    break;
                case "div":
                    div(args);
                    break;
                case "idiv":
                    idiv(args);
                    break;
                case "aad":
                    aad(args);
                    break;
                case "aam":
                    aam(args);
                    break;
                case "cbw":
                    cbw(args);
                    break;
                case "cwd":
                    cwd(args);
                    break;
                default:
                    return false;
            }
            return true;
        }

        public static bool is_even_parity(Int16 n)
        {
            bool parzysty = true;
            while (n > 0)
            {
                if ((n | 1) == n)
                {
                    parzysty = !parzysty;
                }
                n >>= 1;
            }
            return parzysty;
        }

        void add(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new Exception("nieprawidłowa liczba argumentów");
            }

            bool overflow = false;
            try
            {
                Int16 overflow_check = checked((Int16)(args[0].Get() + args[1].Get()));
            }
            catch (OverflowException)
            {
                overflow = true;
            }

            bool carry = false;
            try
            {
                UInt16 carry_check = checked((UInt16)(args[0].GetUnsigned() + args[1].GetUnsigned()));
            }
            catch (OverflowException)
            {
                carry = true;
            }


            Int16 suma = unchecked((Int16)(args[0].Get() + args[1].Get()));
            Program.Pamiec.Flagi.SF = (suma < 0);
            Program.Pamiec.Flagi.ZF = (suma == 0);
            Program.Pamiec.Flagi.OF = overflow;
            Program.Pamiec.Flagi.CF = carry;
            Program.Pamiec.Flagi.PF = is_even_parity(suma);
            args[0].Set(suma);

        }

        void sub(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new Exception("nieprawidłowa liczba argumentów");
            }

            bool overflow = false;
            try
            {
                Int16 overflow_check = checked((Int16)(args[0].Get() - args[1].Get()));
            }
            catch (OverflowException)
            {
                overflow = true;
            }

            bool carry = false;
            try
            {
                UInt16 carry_check = checked((UInt16)(args[0].GetUnsigned() - args[1].GetUnsigned()));
            }
            catch (OverflowException)
            {
                carry = true;
            }

            Int16 suma = unchecked((Int16)(args[0].Get() - args[1].Get()));
            Program.Pamiec.Flagi.SF = (suma < 0);
            Program.Pamiec.Flagi.ZF = (suma == 0);
            Program.Pamiec.Flagi.OF = overflow;
            Program.Pamiec.Flagi.CF = carry;
            Program.Pamiec.Flagi.PF = is_even_parity(suma);
            args[0].Set(suma);

        }

        void adc(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new Exception("nieprawidłowa liczba argumentów");
            }

            Int16 carry_value = Convert.ToInt16(Program.Pamiec.Flagi.CF);
            bool overflow = false;
            try
            {
                Int16 overflow_check = checked((Int16)(args[0].Get() + args[1].Get() + carry_value));
            }
            catch (OverflowException)
            {
                overflow = true;
            }

            bool carry = false;
            try
            {
                UInt16 carry_check = checked((UInt16)(args[0].GetUnsigned() + args[1].GetUnsigned() + carry_value));
            }
            catch (OverflowException)
            {
                carry = true;
            }


            Int16 suma = unchecked((Int16)(args[0].Get() + args[1].Get() + carry_value));
            Program.Pamiec.Flagi.SF = (suma < 0);
            Program.Pamiec.Flagi.ZF = (suma == 0);
            Program.Pamiec.Flagi.OF = overflow;
            Program.Pamiec.Flagi.CF = carry;
            Program.Pamiec.Flagi.PF = is_even_parity(suma);
            args[0].Set(suma);
        }

        void sbb(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new Exception("nieprawidłowa liczba argumentów");
            }

            Int16 carry_value = Convert.ToInt16(Program.Pamiec.Flagi.CF);
            bool overflow = false;
            try
            {
                Int16 overflow_check = checked((Int16)(args[0].Get() - args[1].Get() - carry_value));
            }
            catch (OverflowException)
            {
                overflow = true;
            }

            bool carry = false;
            try
            {
                UInt16 carry_check = checked((UInt16)(args[0].GetUnsigned() - args[1].GetUnsigned() - carry_value));
            }
            catch (OverflowException)
            {
                carry = true;
            }


            Int16 suma = unchecked((Int16)(args[0].Get() - args[1].Get() - carry_value));
            Program.Pamiec.Flagi.SF = (suma < 0);
            Program.Pamiec.Flagi.ZF = (suma == 0);
            Program.Pamiec.Flagi.OF = overflow;
            Program.Pamiec.Flagi.CF = carry;
            Program.Pamiec.Flagi.PF = is_even_parity(suma);
            args[0].Set(suma);

        }

        void inc(Argument[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("zła liczba argumentów");
            }

            add(new Argument[] { args[0], new NumericConstant(1) });
        }

        void dec(Argument[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("zła liczba argumentów");
            }

            sub(new Argument[] { args[0], new NumericConstant(1) });
        }

        void aaa(Argument[] args)
        {
            if (args.Length != 0)
            {
                throw new ArgumentException("zła liczba argumentów");
            }

            // zaznaczamy 4 ostatnie bity
            if ((Program.Pamiec.Rejestry.AL & 0x0f) > 9 || Program.Pamiec.Flagi.AF)
            {
                Program.Pamiec.Rejestry.AX += 0x106;
                Program.Pamiec.Flagi.CF = true;
                Program.Pamiec.Flagi.AF = true;
            }
            else
            {
                Program.Pamiec.Flagi.CF = false;
                Program.Pamiec.Flagi.AF = false;
            }
            Program.Pamiec.Rejestry.AL &= 0x0f;
        }

        void aas(Argument[] args)
        {
            if (args.Length != 0)
            {
                throw new ArgumentException("zła liczba argumentów");
            }

            // zaznaczamy 4 ostatnie bity
            //sbyte n = Program.Pamiec.Rejestry.AL;
            if ((Program.Pamiec.Rejestry.AL & 0x0f) > 9 || Program.Pamiec.Flagi.AF)
            {
                Program.Pamiec.Rejestry.AX -= 6;
                Program.Pamiec.Rejestry.AH -= 1;
                Program.Pamiec.Flagi.CF = true;
                Program.Pamiec.Flagi.AF = true;
            }
            else
            {
                Program.Pamiec.Flagi.CF = false;
                Program.Pamiec.Flagi.AF = false;
            }
            Program.Pamiec.Rejestry.AL &= 0x0f;
        }

        void daa(Argument[] args)
        {
            if (args.Length != 0)
            {
                throw new ArgumentException("zła liczba argumentów");
            }

            byte al_tmp = unchecked((byte)Program.Pamiec.Rejestry.AL);
            bool cf_tmp = Program.Pamiec.Flagi.CF;
            Program.Pamiec.Flagi.CF = false;
            if ((Program.Pamiec.Rejestry.AL & 0x0f) > 9 || Program.Pamiec.Flagi.AF)
            {
                // wywołuję komendę dodawanie żeby zobaczyć czy jest carry
                add(new Argument[] { new HalfRegisterArgument("AL"), new NumericConstant(6) });

                Program.Pamiec.Flagi.CF = cf_tmp || Program.Pamiec.Flagi.CF;
                Program.Pamiec.Flagi.AF = true;
            }
            else
            {
                Program.Pamiec.Flagi.AF = false;
            }
            if (al_tmp > 0x99 || cf_tmp)
            {
                Program.Pamiec.Rejestry.AL += 0x60;
                Program.Pamiec.Flagi.CF = true;
            }
            else
            {
                Program.Pamiec.Flagi.CF = false;
            }
        }

        void das(Argument[] args)
        {
            if (args.Length != 0)
            {
                throw new ArgumentException("zła liczba argumentów");
            }

            byte al_tmp = unchecked((byte)Program.Pamiec.Rejestry.AL);
            bool cf_tmp = Program.Pamiec.Flagi.CF;
            Program.Pamiec.Flagi.CF = false;
            if ((Program.Pamiec.Rejestry.AL & 0x0f) > 9 || Program.Pamiec.Flagi.AF)
            {
                // wywołuję komendę dodawanie żeby zobaczyć czy jest carry
                sub(new Argument[] { new HalfRegisterArgument("AL"), new NumericConstant(6) });

                Program.Pamiec.Flagi.CF = cf_tmp || Program.Pamiec.Flagi.CF;
                Program.Pamiec.Flagi.AF = true;
            }
            else
            {
                Program.Pamiec.Flagi.AF = false;
            }
            if (al_tmp > 0x99 || cf_tmp)
            {
                Program.Pamiec.Rejestry.AL -= 0x60;
                Program.Pamiec.Flagi.CF = true;
            }
        }

        void mul(Argument[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            Type argument_type = args[0].GetType();
            if (argument_type != typeof(RegisterArgument)
                && argument_type != typeof(RegisterArgument)
                && argument_type != typeof(MemoryArgument))
            {
                throw new ArgumentException("argumentem może być tylko rejestr lub komórka");
            }

            if (argument_type == typeof(HalfRegisterArgument))
            {
                UInt16 result = unchecked((UInt16)((UInt16)Program.Pamiec.Rejestry.AL * (UInt16)args[0].Get()));
                Program.Pamiec.Rejestry.AX = unchecked((Int16)result);
                if(Program.Pamiec.Rejestry.AH != 0)
                {
                    Program.Pamiec.Flagi.CF = true;
                    Program.Pamiec.Flagi.OF = true;
                }
            } else
            {
                uint result = (uint)((UInt16)Program.Pamiec.Rejestry.AX * args[0].GetUnsigned());
                Program.Pamiec.Rejestry.AX = (Int16)(result & 0x0000ffff);
                Program.Pamiec.Rejestry.DX = (Int16)((result & 0xffff0000) >> 16);
                if (Program.Pamiec.Rejestry.DX != 0)
                {
                    Program.Pamiec.Flagi.CF = true;
                    Program.Pamiec.Flagi.OF = true;
                }
            }
        }

        void imul(Argument[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            Type argument_type = args[0].GetType();
            if (argument_type != typeof(RegisterArgument)
                && argument_type != typeof(RegisterArgument)
                && argument_type != typeof(MemoryArgument))
            {
                throw new ArgumentException("argumentem może być tylko rejestr lub komórka");
            }

            if (argument_type == typeof(HalfRegisterArgument))
            {
                Int16 result = (Int16)(Program.Pamiec.Rejestry.AL * args[0].Get());
                Program.Pamiec.Rejestry.AX = unchecked((Int16)result);
                if (Program.Pamiec.Rejestry.AH != 0)
                {
                    Program.Pamiec.Flagi.CF = true;
                    Program.Pamiec.Flagi.OF = true;
                }
            }
            else
            {
                int result = Program.Pamiec.Rejestry.AX * args[0].Get();
                Program.Pamiec.Rejestry.AX = (Int16)(result & 0x0000ffff);
                Program.Pamiec.Rejestry.DX = (Int16)((result & 0xffff0000) >> 16);
                if (Program.Pamiec.Rejestry.DX != 0)
                {
                    Program.Pamiec.Flagi.CF = true;
                    Program.Pamiec.Flagi.OF = true;
                }
            }
        }

        void div(Argument[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            Type argument_type = args[0].GetType();
            if (argument_type != typeof(RegisterArgument)
                && argument_type != typeof(RegisterArgument)
                && argument_type != typeof(MemoryArgument))
            {
                throw new ArgumentException("argumentem może być tylko rejestr lub komórka");
            }

            if (argument_type == typeof(HalfRegisterArgument))
            {
                byte result = (byte)((UInt16)Program.Pamiec.Rejestry.AX / args[0].GetUnsigned());
                byte remainder = (byte)((UInt16)Program.Pamiec.Rejestry.AX % args[0].GetUnsigned());
                unchecked
                {
                    Program.Pamiec.Rejestry.AH = (sbyte)result;
                    Program.Pamiec.Rejestry.AL = (sbyte)remainder;
                }
            }
            else
            {
                uint dividend = (UInt16)Program.Pamiec.Rejestry.AX;
                dividend |= (uint)((UInt16)Program.Pamiec.Rejestry.DX << 16);

                UInt16 result = (UInt16)(dividend / args[0].GetUnsigned());
                UInt16 remainder = (UInt16)(dividend % args[0].GetUnsigned());
                unchecked
                {
                    Program.Pamiec.Rejestry.AX = (Int16) result;
                    Program.Pamiec.Rejestry.DX = (Int16) remainder;
                }
            }
        }

        void idiv(Argument[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            Type argument_type = args[0].GetType();
            if (argument_type != typeof(RegisterArgument)
                && argument_type != typeof(RegisterArgument)
                && argument_type != typeof(MemoryArgument))
            {
                throw new ArgumentException("argumentem może być tylko rejestr lub komórka");
            }

            if (argument_type == typeof(HalfRegisterArgument))
            {
                Int16 result = (Int16)(Program.Pamiec.Rejestry.AX / args[0].Get());
                Int16 remainder = (Int16)(Program.Pamiec.Rejestry.AX % args[0].Get());
                unchecked
                {
                    Program.Pamiec.Rejestry.AH = (sbyte)result;
                    Program.Pamiec.Rejestry.AL = (sbyte)remainder;
                }
            }
            else
            {
                int dividend = (Int16)Program.Pamiec.Rejestry.AX;
                dividend |= (int)((Int16)Program.Pamiec.Rejestry.DX << 16);

                Int16 result = (Int16)(Program.Pamiec.Rejestry.AX / args[0].Get());
                Int16 remainder = (Int16)(Program.Pamiec.Rejestry.AX % args[0].Get());
                unchecked
                {
                    Program.Pamiec.Rejestry.AX = result;
                    Program.Pamiec.Rejestry.DX = remainder;
                }
            }
        }

        void aad(Argument[] args)
        {
            if(args.Length != 0)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            sbyte al_tmp = Program.Pamiec.Rejestry.AL;
            sbyte ah_tmp = Program.Pamiec.Rejestry.AH;
            unchecked
            {
                Program.Pamiec.Rejestry.AL = (sbyte)((al_tmp + (ah_tmp * 10)) & 0xff);
            }
            Program.Pamiec.Rejestry.AH = 0;

            Program.Pamiec.Flagi.SF = Program.Pamiec.Rejestry.AL < 0;
            Program.Pamiec.Flagi.ZF = Program.Pamiec.Rejestry.AL == 0;
            Program.Pamiec.Flagi.PF = is_even_parity(Program.Pamiec.Rejestry.AL);
        }

        void aam(Argument[] args)
        {
            if (args.Length != 0)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }
            Program.Pamiec.Rejestry.AH /= 10;
            Program.Pamiec.Rejestry.AL %= 10;
        }

        void cbw(Argument[] args)
        {
            if (args.Length != 0)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }
            Program.Pamiec.Rejestry.AX = (Int16)(Program.Pamiec.Rejestry.AL);
        }

        void cwd(Argument[] args)
        {
            if (args.Length != 0)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            if(Program.Pamiec.Rejestry.AX < 0)
            {
                Program.Pamiec.Rejestry.DX = unchecked((Int16)(0xffff));
            } else
            {
                Program.Pamiec.Rejestry.DX = 0;
            }
        }
    }
}