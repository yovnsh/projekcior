namespace Projekcior {
    interface Argument
    {
        Int16 Get();
        UInt16 GetUnsigned();
        bool Get_Bool();
        sbyte Get_Sbyte();

        void Set(Argument value);
        void Set(Int16 value);

        // sprawdza czy nazwa jest prawidłowa dla konkretnego typu argumentu
        static bool Contains(string name)
        {
            return false;
        }
    }

    class RegisterArgument : Argument
    {
        public readonly string RegisterName;
        public Int16 Register
        {
            get
            {
                return RecognizeRegister(RegisterName);
            }
            set
            {
                ref Int16 rejestr = ref RecognizeRegister(RegisterName);
                rejestr = value;
            }
        }

        public RegisterArgument(string name)
        {
            if (!Contains(name))
            {
                throw new ArgumentException("podana nazwa nie jest rejestrem");
            }

            RegisterName = name;
        }

        public Int16 Get()
        {
            return Register;
        }

        public UInt16 GetUnsigned()
        {
            unchecked
            {
                return (UInt16) Register;
            }
        }

        public sbyte Get_Sbyte()
        {
            throw new Exception("rejestr nie może zostac przekonwertowany na połówkę rejestru");
        }

        public bool Get_Bool()
        {
            throw new Exception("rejestr nie może zostać przekonwertowany na flagę");
        }

        public void Set(Argument other)
        {
            Register = other.Get();
        }

        public void Set(Int16 value)
        {
            Register = value;
        }

        public static bool Contains(string name)
        {
            switch (name)
            {
                case "AX":
                case "BX":
                case "CX":
                case "DX":
                    return true;
                default:
                    return false;
            }
        }

        public static ref Int16 RecognizeRegister(string name)
        {
            switch (name)
            {
                case "AX":
                    return ref Program.Pamiec.Rejestry.AX;
                case "BX":
                    return ref Program.Pamiec.Rejestry.BX;
                case "CX":
                    return ref Program.Pamiec.Rejestry.CX;
                case "DX":
                    return ref Program.Pamiec.Rejestry.DX;
                default:
                    throw new ArgumentException("podana nazwa nie jest rejestrem");
            }
        }
    }

    class SegmentArgument : Argument
    {
        public readonly string SegmentName;
        public UInt16 Segment
        {
            get
            {
                return Program.Pamiec.Segmenty[SegmentName];
            }
            set
            {
                Program.Pamiec.Segmenty[SegmentName] = value;
            }
        }

        public SegmentArgument(string name)
        {
            if (!Contains(name))
            {
                throw new ArgumentException("podana nazwa nie jest segmentem");
            }

            SegmentName = name;
        }

        public Int16 Get()
        {
            unchecked
            {
                return (Int16)Segment;
            }
        }

        public UInt16 GetUnsigned()
        {
            return Segment;
        }

        public sbyte Get_Sbyte()
        {
            throw new Exception("segment nie może zostac przekonwertowany na połówkę rejestru");
        }

        public bool Get_Bool()
        {
            throw new Exception("segment nie może zostać przekonwertowany na flagę");
        }

        public void Set(Argument other)
        {
            throw new Exception("nie wolno zmieniać segmentów");
        }

        public void Set(Int16 value)
        {
            throw new Exception("nie wolno zmieniać segmentów");
        }


        public static bool Contains(string name)
        {
            return Program.Pamiec.Segmenty.ContainsKey(name);
        }
    }

    class PointerArgument : Argument
    {
        public readonly string PointerName;
        public UInt16 Pointer
        {
            get
            {
                return Program.Pamiec.Wskazniki[PointerName];
            }
            set
            {
                Program.Pamiec.Wskazniki[PointerName] = value;
            }
        }

        public PointerArgument(string name)
        {
            if (!Contains(name))
            {
                throw new ArgumentException("podana nazwa nie jest segmentem");
            }

            PointerName = name;
        }

        public Int16 Get()
        {
            unchecked
            {
                return (Int16)Pointer;
            }
        }

        public UInt16 GetUnsigned()
        {
            return Pointer;
        }

        public sbyte Get_Sbyte()
        {
            throw new Exception("segment nie może zostac przekonwertowany na połówkę rejestru");
        }

        public bool Get_Bool()
        {
            throw new Exception("segment nie może zostać przekonwertowany na flagę");
        }

        public void Set(Argument other)
        {
            throw new Exception("nie wolno zmieniać wskaźników");
        }

        public void Set(Int16 value)
        {
            throw new Exception("nie wolno zmieniać wskaźników");
        }


        public static bool Contains(string name)
        {
            return Program.Pamiec.Wskazniki.ContainsKey(name);
        }
    }

    class HalfRegisterArgument : Argument
    {
        public readonly string HalfRegisterName;
        public sbyte HalfRegister
        {
            get
            {
                switch (HalfRegisterName)
                {
                    case "AH":
                        return Program.Pamiec.Rejestry.AH;
                    case "AL":
                        return Program.Pamiec.Rejestry.AL;
                    case "BH":
                        return Program.Pamiec.Rejestry.BH;
                    case "BL":
                        return Program.Pamiec.Rejestry.BL;
                    case "CH":
                        return Program.Pamiec.Rejestry.CH;
                    case "CL":
                        return Program.Pamiec.Rejestry.CL;
                    case "DH":
                        return Program.Pamiec.Rejestry.DH;
                    case "DL":
                        return Program.Pamiec.Rejestry.DL;
                    default:
                        throw new Exception("podana nazwa nie jest połówką rejestru");
                }
            }
            set
            {
                switch (HalfRegisterName)
                {
                    case "AH":
                        Program.Pamiec.Rejestry.AH = value;
                        break;
                    case "AL":
                        Program.Pamiec.Rejestry.AL = value;
                        break;
                    case "BH":
                        Program.Pamiec.Rejestry.BH = value;
                        break;
                    case "BL":
                        Program.Pamiec.Rejestry.BL = value;
                        break;
                    case "CH":
                        Program.Pamiec.Rejestry.CH = value;
                        break;
                    case "CL":
                        Program.Pamiec.Rejestry.CL = value;
                        break;
                    case "DH":
                        Program.Pamiec.Rejestry.DH = value;
                        break;
                    case "DL":
                        Program.Pamiec.Rejestry.DL = value;
                        break;
                    default:
                        throw new Exception("podana nazwa nie jest połówką rejestru");
                }
            }
        }

        public HalfRegisterArgument(string name)
        {
            if (!Contains(name))
            {
                throw new ArgumentException("podana nazwa nie jest segmentem");
            }

            HalfRegisterName = name;
        }

        public Int16 Get()
        {
            return (Int16) HalfRegister;
        }

        public UInt16 GetUnsigned()
        {
            unchecked {
                return (UInt16)HalfRegister;
            }
        }

        public sbyte Get_Sbyte()
        {
            return HalfRegister;
        }

        public bool Get_Bool()
        {
            throw new Exception("nie można przekonwertować połówkki rejestru na flagę");
        }

        public void Set(Argument other)
        {
            HalfRegister = other.Get_Sbyte();
        }

        public void Set(Int16 value)
        {
            unchecked
            {
                HalfRegister = (sbyte)(value);
            }
        }


        public static bool Contains(string name)
        {
            switch (name)
            {
                case "AH":
                case "AL":
                case "BH":
                case "BL":
                case "CH":
                case "CL":
                case "DH":
                case "DL":
                    return true;
                default:
                    return false;
            }
        }
    }

    class NumericConstant : Argument {
        public readonly Int16 value;

        public NumericConstant(string number_string)
        {
            if(number_string == "0")
            {
                value = 0;
            }

            // jeśli sie zaczyna plusem to go usuwamy
            if(number_string.StartsWith("+"))
            {
                number_string = number_string.Substring(1);
            }

            Int16 sign = 1;
            if(number_string.StartsWith("-"))
            {
                sign = -1;
                number_string = number_string.Substring(1);
            }

            if(number_string.StartsWith("0X"))
            {
                value = (Int16)(sign * Convert.ToInt16(number_string.Substring(2), 16));
            }
            else if (number_string.EndsWith("B"))
            {
                value = (Int16)(sign * Convert.ToInt16(number_string.Substring(0, number_string.Length - 1), 2));
            }
            else if(number_string.StartsWith("0"))
            {
                value = (Int16)(sign * Convert.ToInt16(number_string, 8));
            }
            else if(number_string.All(char.IsDigit))
            {
                value = (Int16)(sign * Convert.ToInt16(number_string, 10));
            }
            else
            {
                throw new ArgumentException("to nie jest liczba");
            }
        }

        public NumericConstant(Int16 number)
        {
            value = number;
        }

        public Int16 Get()
        {
            return value;
        }

        public UInt16 GetUnsigned()
        {
            unchecked
            {
                return (UInt16) value;
            }
        }

        public bool Get_Bool()
        {
            if(value != 0 && value != 1)
            {
                throw new Exception("liczba nierówna 0 nie może zostać przekonwertowana na flagę");
            }
            return Convert.ToBoolean(value);
        }
        public sbyte Get_Sbyte()
        {
            unchecked
            {
                // chce żeby 0b11111111 też sie dało tak zapisać
                return (sbyte)((byte)(value));
            }
        }

        public void Set(Argument value)
        {
            throw new Exception("constant cannot be modified");
        }

        public void Set(Int16 value)
        {
            throw new Exception("constant cannot be modified");
        }


        public static bool Contains(string name)
        {
            string copy = name;
            if(copy.StartsWith("-") || copy.StartsWith("+"))
            {
                copy = copy.Substring(1);
            }
            return (copy == "0" || copy.StartsWith("0X") || copy.EndsWith("B") || copy.StartsWith("0") || copy.All(char.IsDigit));
        }
    }

    class MemoryArgument : Argument
    {
        public readonly UInt16 adress;

        public MemoryArgument(string adress_string)
        {
            string raw_adress = adress_string.Substring(1, adress_string.Length - 2);
            adress = (UInt16)Program.ReadArgument(raw_adress).Get();
        }

        public Int16 Get()
        {
            return Convert.ToInt16(Program.Pamiec.PamiecAdresowana[adress]);
        }

        public UInt16 GetUnsigned()
        {
            return Convert.ToUInt16(Program.Pamiec.PamiecAdresowana[adress]);
        }

        public UInt16 GetAddress()
        {
            return adress;
        }

        public bool Get_Bool()
        {
            throw new Exception("z pamięci można tylko int16 z pamięci");
        }
        public sbyte Get_Sbyte()
        {
            throw new Exception("można wczytać tylko int16 z pamięci");
        }

        public void Set(Argument value)
        {
            Program.Pamiec.PamiecAdresowana[adress] = value.Get().ToString();
        }

        public void Set(Int16 value)
        {
            Program.Pamiec.PamiecAdresowana[adress] = value.ToString();
        }


        public static bool Contains(string name)
        {
            if(name.StartsWith("[") && name.EndsWith("]"))
            {
                return true;
            }
            return false;
        }
    }
}