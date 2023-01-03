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
        
        void add(Argument[] args)
        {
            //...
        }

        void sub(Argument[] args)
        {
            //...
        }

        void adc(Argument[] args)
        {
            //...
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