namespace Projekcior.Commands
{
    class KomendyLogiczne : CommandGroup
    {
        public bool ExecuteCommand(string cmd, Argument[] args)
        {
            switch (cmd)
            {
                case "not":
                    not(args);
                    break;
                case "shl":
                    shl(args);
                    break;
                case "shr":
                    shr(args);
                    break;
                case "sal":
                    sal(args);
                    break;
                case "sar":
                    sar(args);
                    break;
                case "rol":
                    rol(args);
                    break;
                case "ror":
                    ror(args);
                    break;
                case "rcl":
                    rcl(args);
                    break;
                case "rcr":
                    rcr(args);
                    break;
                case "and":
                    and(args);
                    break;
                case "test":
                    test(args);
                    break;
                case "or":
                    or(args);
                    break;
                case "xor":
                    xor(args);
                    break;
                default:
                    return false;
            }
            return true;
        }

        void not(Argument[] args)
        {
            if(args.Length != 1)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            args[0].Set((Int16) ~args[0].Get());
        }

        void shl(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            Type argument1_type = args[1].GetType();

            if (argument1_type != typeof(NumericConstant) 
                && (argument1_type != typeof(HalfRegisterArgument)))
            {
                throw new ArgumentException("nieprawidłowy typ drugiego argumentu");
            }

            if(argument1_type == typeof(HalfRegisterArgument))
            {
                if(((HalfRegisterArgument)args[1]).HalfRegisterName != "CL")
                {
                    throw new ArgumentException("jako rejestr do komendy shl można podać tylko cl");
                }
            }

            Int16 move_count = args[1].Get();
            Int16 value = args[0].Get();

            Program.Pamiec.Flagi.CF = (value & 0b1000000000000000) > 0;

            value <<= move_count;

            if (move_count == 1)
            {
                Program.Pamiec.Flagi.OF = ((value & 0b1000000000000000) > 0) == Program.Pamiec.Flagi.CF;
            }

            args[0].Set(value);
        }

        void shr(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            Type argument1_type = args[1].GetType();

            if (argument1_type != typeof(NumericConstant)
                && (argument1_type != typeof(HalfRegisterArgument)))
            {
                throw new ArgumentException("nieprawidłowy typ drugiego argumentu");
            }

            if (argument1_type == typeof(HalfRegisterArgument))
            {
                if (((HalfRegisterArgument)args[1]).HalfRegisterName != "CL")
                {
                    throw new ArgumentException("jako rejestr do komendy shl można podać tylko cl");
                }
            }

            UInt16 value = args[0].GetUnsigned();

            Program.Pamiec.Flagi.CF = (value & 1) > 0;
            Program.Pamiec.Flagi.OF = (value & 0b1000000000000000) > 0;

            value >>= args[1].Get();


            args[0].Set((Int16) value);
        }

        // to jest to samo
        void sal(Argument[] args)
        {
            shl(args);
        }

        void sar(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            Type argument1_type = args[1].GetType();

            if (argument1_type != typeof(NumericConstant)
                && (argument1_type != typeof(HalfRegisterArgument)))
            {
                throw new ArgumentException("nieprawidłowy typ drugiego argumentu");
            }

            if (argument1_type == typeof(HalfRegisterArgument))
            {
                if (((HalfRegisterArgument)args[1]).HalfRegisterName != "CL")
                {
                    throw new ArgumentException("jako rejestr do komendy shl można podać tylko cl");
                }
            }

            Int16 move_count = args[1].Get();
            Int16 value = args[0].Get();

            Program.Pamiec.Flagi.CF = (value & 1) > 0;
            Program.Pamiec.Flagi.OF = move_count == 0;

            value >>= move_count;

            args[0].Set((Int16)value);
        }

        void rol(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            Type argument1_type = args[1].GetType();

            if (argument1_type != typeof(NumericConstant)
                && (argument1_type != typeof(HalfRegisterArgument)))
            {
                throw new ArgumentException("nieprawidłowy typ drugiego argumentu");
            }

            if (argument1_type == typeof(HalfRegisterArgument))
            {
                if (((HalfRegisterArgument)args[1]).HalfRegisterName != "CL")
                {
                    throw new ArgumentException("jako rejestr do komendy shl można podać tylko cl");
                }
            }

            UInt16 value = args[0].GetUnsigned();
            for (int n = 0; n < args[1].Get(); n++)
            {
                Program.Pamiec.Flagi.CF = ((value & 0b1000000000000000) > 0);
                value <<= 1;
                value |= Convert.ToUInt16(Program.Pamiec.Flagi.CF);
            }
            
            args[0].Set(unchecked((Int16)value));
            Program.Pamiec.Flagi.CF = (Convert.ToUInt16(Program.Pamiec.Flagi.CF) ^ Convert.ToUInt16((value & 0b1000000000000000) > 0)) == 1;
        }

        void ror(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            Type argument1_type = args[1].GetType();

            if (argument1_type != typeof(NumericConstant)
                && (argument1_type != typeof(HalfRegisterArgument)))
            {
                throw new ArgumentException("nieprawidłowy typ drugiego argumentu");
            }

            if (argument1_type == typeof(HalfRegisterArgument))
            {
                if (((HalfRegisterArgument)args[1]).HalfRegisterName != "CL")
                {
                    throw new ArgumentException("jako rejestr do komendy shl można podać tylko cl");
                }
            }

            UInt16 value = args[0].GetUnsigned();
            for (int n = 0; n < args[1].Get(); n++)
            {
                Program.Pamiec.Flagi.CF = ((value & 1) > 0);
                value >>= 1;
                value |= (UInt16)(Convert.ToUInt16(Program.Pamiec.Flagi.CF) << 15);
            }

            args[0].Set(unchecked((Int16)value));
            Program.Pamiec.Flagi.CF = (Convert.ToUInt16(Program.Pamiec.Flagi.CF) ^ Convert.ToUInt16((value & 0b1000000000000000) > 0)) == 1;

        }

        void rcl(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            Type argument1_type = args[1].GetType();

            if (argument1_type != typeof(NumericConstant)
                && (argument1_type != typeof(HalfRegisterArgument)))
            {
                throw new ArgumentException("nieprawidłowy typ drugiego argumentu");
            }

            if (argument1_type == typeof(HalfRegisterArgument))
            {
                if (((HalfRegisterArgument)args[1]).HalfRegisterName != "CL")
                {
                    throw new ArgumentException("jako rejestr do komendy shl można podać tylko cl");
                }
            }

            UInt16 value = args[0].GetUnsigned();
            for (int n = 0; n < args[1].Get(); n++)
            {
                bool cf_after = ((value & 0b1000000000000000) > 0);
                value <<= 1;
                value |= Convert.ToUInt16(Program.Pamiec.Flagi.CF);
                Program.Pamiec.Flagi.CF = cf_after;
            }

            args[0].Set(unchecked((Int16)value));
            Program.Pamiec.Flagi.CF = (Convert.ToUInt16(Program.Pamiec.Flagi.CF) ^ Convert.ToUInt16((value & 0b1000000000000000) > 0)) == 1;

        }

        void rcr(Argument[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            Type argument1_type = args[1].GetType();

            if (argument1_type != typeof(NumericConstant)
                && (argument1_type != typeof(HalfRegisterArgument)))
            {
                throw new ArgumentException("nieprawidłowy typ drugiego argumentu");
            }

            if (argument1_type == typeof(HalfRegisterArgument))
            {
                if (((HalfRegisterArgument)args[1]).HalfRegisterName != "CL")
                {
                    throw new ArgumentException("jako rejestr do komendy shl można podać tylko cl");
                }
            }

            UInt16 value = args[0].GetUnsigned();
            for (int n = 0; n < args[1].Get(); n++)
            {
                bool cf_after = ((value & 1) > 0);
                value >>= 1;
                value |= (UInt16)(Convert.ToUInt16(Program.Pamiec.Flagi.CF) << 15);
                Program.Pamiec.Flagi.CF = cf_after;
            }

            args[0].Set(unchecked((Int16)value));
            Program.Pamiec.Flagi.CF = (Convert.ToUInt16(Program.Pamiec.Flagi.CF) ^ Convert.ToUInt16((value & 0b1000000000000000) > 0)) == 1;

        }

        void and(Argument[] args)
        {
            Program.Pamiec.Flagi.CF = false;
            Program.Pamiec.Flagi.OF = false;

            Int16 value = (Int16)(args[0].Get() & args[1].Get());
            Program.Pamiec.Flagi.SF = value < 0;
            Program.Pamiec.Flagi.ZF = value == 0;
            Program.Pamiec.Flagi.PF = KomendyArytmetyczne.is_even_parity(value);
            args[0].Set(value);
        }

        // and tylko bez flag
        void test(Argument[] args)
        {
            Program.Pamiec.Flagi.CF = false;
            Program.Pamiec.Flagi.OF = false;

            Int16 value = (Int16)(args[0].Get() & args[1].Get());
            Program.Pamiec.Flagi.SF = value < 0;
            Program.Pamiec.Flagi.ZF = value == 0;
            Program.Pamiec.Flagi.PF = KomendyArytmetyczne.is_even_parity(value);
        }

        void or(Argument[] args)
        {
            Program.Pamiec.Flagi.CF = false;
            Program.Pamiec.Flagi.OF = false;

            Int16 value = (Int16)(args[0].Get() | args[1].Get());
            Program.Pamiec.Flagi.SF = value < 0;
            Program.Pamiec.Flagi.ZF = value == 0;
            Program.Pamiec.Flagi.PF = KomendyArytmetyczne.is_even_parity(value);
            args[0].Set(value);
        }

        void xor(Argument[] args)
        {
            Program.Pamiec.Flagi.CF = false;
            Program.Pamiec.Flagi.OF = false;

            Int16 value = (Int16)(args[0].Get() ^ args[1].Get());
            Program.Pamiec.Flagi.SF = value < 0;
            Program.Pamiec.Flagi.ZF = value == 0;
            Program.Pamiec.Flagi.PF = KomendyArytmetyczne.is_even_parity(value);
            args[0].Set(value);
        }
    }
}