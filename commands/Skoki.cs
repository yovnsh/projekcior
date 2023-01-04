namespace Projekcior.Commands
{
    class Skoki : CommandGroup
    {
        public bool ExecuteCommand(string cmd, Argument[] args)
        {
            if(cmd != "ret" && args.Length != 1)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }

            switch (cmd)
            {
                case "jmp":
                    jmp(args);
                    break;
                case "call":
                    call(args);
                    break;
                case "ret":
                    ret(args);
                    break;
                case "je":
                case "jz":
                    je(args);
                    break;
                case "jne":
                case "jnz":
                    jne(args);
                    break;
                case "jo":
                    jo(args);
                    break;
                case "jno":
                    jno(args);
                    break;
                case "jc":
                case "jb":
                case "jnae":
                    jc(args);
                    break;
                case "jnc":
                case "jae":
                case "jnb":
                    jnc(args);
                    break;
                case "jp":
                case "jpe":
                    jp(args);
                    break;
                case "jnp":
                case "jpo":
                    jnp(args);
                    break;
                case "js":
                    js(args);
                    break;
                case "jns":
                    jns(args);
                    break;
                case "jbe":
                case "jna":
                    jbe(args);
                    break;
                case "ja":
                case "jnbe":
                    ja(args);
                    break;
                case "jl":
                case "jnge":
                    jl(args);
                    break;
                case "jle":
                case "jng":
                    jle(args);
                    break;
                case "jge":
                case "jnl":
                    jge(args);
                    break;
                case "jnle":
                case "jg":
                    jnle(args);
                    break;
                case "jcxz":
                    jcxz(args);
                    break;
                default:
                    return false;
            }
            return true;
        }

        void call(Argument[] args)
        {
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
            catch(OverflowException)
            {
                throw new Exception("stack overflow");
            }

            Program.Pamiec.Wskazniki["SP"] = stack_pointer;
            Program.Pamiec.PamiecAdresowana[stack_segment + stack_pointer] = Program.Pamiec.Wskazniki["IP"].ToString();
            jmp(args);
        }

        void ret(Argument[] args)
        {
            if(args.Length == 0)
            {
                throw new ArgumentException("nieprawidłowa liczba argumentów");
            }
            UInt16 stack_segment = Program.Pamiec.Segmenty["SS"];
            UInt16 stack_pointer = Program.Pamiec.Wskazniki["SP"];
            Program.Pamiec.Wskazniki["IP"] = Convert.ToUInt16(Program.Pamiec.PamiecAdresowana[stack_segment + stack_pointer]);

            try
            {
                checked
                {
                    stack_pointer -= 1;
                }
            }
            catch(OverflowException)
            {
                throw new Exception("stack underflow");
            }


            Program.Pamiec.Wskazniki["SP"] = stack_pointer;
        }

        void jmp(Argument[] args)
        {
            Program.Pamiec.Wskazniki["IP"] = (UInt16) args[0].Get();
        }

        void je(Argument[] args)
        {
            if(Program.Pamiec.Flagi.ZF)
            {
                jmp(args);
            }
        }

        void jne(Argument[] args)
        {
            if(!Program.Pamiec.Flagi.ZF)
            {
                jmp(args);
            }
        }

        void jo(Argument[] args)
        {
            if(Program.Pamiec.Flagi.OF)
            {
                jmp(args);
            }
        }

        void jno(Argument[] args)
        {
            if (!Program.Pamiec.Flagi.OF)
            {
                jmp(args);
            }
        }

        void jc(Argument[] args)
        {
            if (Program.Pamiec.Flagi.CF)
            {
                jmp(args);
            }
        }

        void jnc(Argument[] args)
        {
            if (!Program.Pamiec.Flagi.CF)
            {
                jmp(args);
            }
        }

        void jp(Argument[] args)
        {
            if (Program.Pamiec.Flagi.PF)
            {
                jmp(args);
            }
        }

        void jnp(Argument[] args)
        {
            if (!Program.Pamiec.Flagi.PF)
            {
                jmp(args);
            }
        }

        void js(Argument[] args)
        {
            if (Program.Pamiec.Flagi.SF)
            {
                jmp(args);
            }
        }

        void jns(Argument[] args)
        {
            if (!Program.Pamiec.Flagi.SF)
            {
                jmp(args);
            }
        }

        void jbe(Argument[] args)
        {
            if (Program.Pamiec.Flagi.ZF || Program.Pamiec.Flagi.CF)
            {
                jmp(args);
            }
        }

        void ja(Argument[] args)
        {
            if (!Program.Pamiec.Flagi.ZF && !Program.Pamiec.Flagi.CF)
            {
                jmp(args);
            }
        }

        void jl(Argument[] args)
        {
            if(Program.Pamiec.Flagi.SF != Program.Pamiec.Flagi.OF)
            {
                jmp(args);
            }
        }

        void jle(Argument[] args)
        {
            if (Program.Pamiec.Flagi.ZF || (Program.Pamiec.Flagi.SF != Program.Pamiec.Flagi.OF))
            {
                jmp(args);
            }
        }

        void jge(Argument[] args)
        {
            if (Program.Pamiec.Flagi.SF == Program.Pamiec.Flagi.OF)
            {
                jmp(args);
            }
        }

        void jnle(Argument[] args)
        {
            if (!Program.Pamiec.Flagi.ZF && (Program.Pamiec.Flagi.SF == Program.Pamiec.Flagi.OF))
            {
                jmp(args);
            }
        }

        void jcxz(Argument[] args)
        {
            if(Program.Pamiec.Rejestry.CX == 0)
            {
                jmp(args);
            }
        }
    }
}