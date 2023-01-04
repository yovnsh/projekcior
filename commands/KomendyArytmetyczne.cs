using System.Reflection.Metadata.Ecma335;

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
                case "cwb":
                    cwb(args);
                    break;
                default:
                    return false;
            }
            return true;
        }

        bool is_even_parity(Int16 n)
        {
            bool parzysty = true;
            while(n > 0)
            {
                if((n | 1) == n)
                {
                    parzysty = !parzysty;
                }
                n >>= 1;
            }
            return parzysty;
        }
        
        void add(Argument[] args)
        {
            if(args.Length != 2)
            {
                throw new Exception("nieprawidłowa liczba argumentów");
            }

            bool overflow = false;
            try
            {
                Int16 overflow_check = checked((Int16)(args[0].Get() + args[1].Get()));
            }
            catch(OverflowException)
            {
                overflow = true;
            }

            bool carry = false;
            try
            {
                UInt16 carry_check = checked((UInt16)(args[0].GetUnsigned() + args[1].GetUnsigned()));
            }
            catch(OverflowException)
            {
                carry = true;
            }


            Int16 suma = unchecked((Int16)(args[0].Get() + args[1].Get()));
            Program.Pamiec.Flagi.OF = overflow;
            Program.Pamiec.Flagi.CF = carry;
            Program.Pamiec.Flagi.PF = is_even_parity(suma);
            args[0].Set(suma);

        }

        void sub(Argument[] args)
        {
            //...
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
            Program.Pamiec.Flagi.OF = overflow;
            Program.Pamiec.Flagi.CF = carry;
            Program.Pamiec.Flagi.PF = is_even_parity(suma);
            args[0].Set(suma);
        }

        void sbb(Argument[] args)
        {
            //...
        }

        void inc(Argument[] args)
        {
            //...
        }

        void dec(Argument[] args)
        {
            //...
        }

        void aaa(Argument[] args)
        {
            //...
        }

        void aas(Argument[] args)
        {
            //...
        }

        void daa(Argument[] args)
        {
            //...
        }

        void das(Argument[] args)
        {
            //...
        }

        void mul(Argument[] args)
        {
            //...
        }

        void imul(Argument[] args)
        {
            //...
        }

        void div(Argument[] args)
        {
            //...
        }

        void idiv(Argument[] args)
        {
            //...
        }

        void aad(Argument[] args)
        {
            //...
        }

        void aam(Argument[] args)
        {
            //...
        }

        void cbw(Argument[] args)
        {
            //...
        }

        void cwb(Argument[] args)
        {
            //...
        }
    }
}