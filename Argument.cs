namespace Projekcior {
    interface Argument
    {
        Int16 Get();
        bool Get_Bool();
        sbyte Get_Sbyte();

        void Set(Argument value);
        void Set(Int16 value);

        /// <summary>
        /// sprawdza czy nazwa jest prawidłowa dla konkretnego typu
        /// </summary>
        static bool Contains(string name)
        {
            return false;
        }
    }

    class RegisterArgument : Argument
    {
        private readonly string RegisterName;
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
        private readonly string SegmentName;
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
            unchecked
            {
                Segment = (UInt16)other.Get();
            }
        }

        public void Set(Int16 value)
        {
            unchecked
            {
                Segment = (UInt16)value;
            }
        }


        public static bool Contains(string name)
        {
            return Program.Pamiec.Segmenty.ContainsKey(name);
        }
    }

    class PointerArgument : Argument
    {
        private readonly string PointerName;
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
            return (Int16)Pointer;
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
            Pointer = (UInt16)other.Get();
        }

        public void Set(Int16 value)
        {
            Pointer = (UInt16)value;
        }


        public static bool Contains(string name)
        {
            return Program.Pamiec.Wskazniki.ContainsKey(name);
        }
    }

    /*
     * nie wiem czy to bedzie potrzebe a nie chce mi sie przerabiać tego kodu po ostatniej zmianie
     * 
    class FlagArgument : Argument
    {
        private readonly string FlagName;
        public bool Flag
        {
            get
            {
                return Program.Pamiec.Flagi[FlagName];
            }
            set
            {
                Program.Pamiec.Flagi[FlagName] = value;
            }
        }

        public FlagArgument(string name)
        {
            if (!Contains(name))
            {
                throw new ArgumentException("podana nazwa nie jest segmentem");
            }

            FlagName = name;
        }

        public Int16 Get()
        {
            return Convert.ToInt16(Flag);
        }

        public sbyte Get_Sbyte()
        {
            return Convert.ToSByte(Flag);
        }

        public bool Get_Bool()
        {
            return Flag;
        }

        public void Set(Argument other)
        {
            Flag = other.Get_Bool();
        }

        public void Set(Int16 value)
        {
            if(value != 0 && value != 1)
            {
                throw new ArgumentException("nie można przypisać liczby do flagi");
            }
            Flag = Convert.ToBoolean(value);
        }


        public static bool Contains(string name)
        {
            return Program.Pamiec.Flagi.ContainsKey(name);
        }
    }
    */

    class HalfRegisterArgument : Argument
    {
        private readonly string HalfRegisterName;
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
            throw new Exception("nie można przenkwertować połówki rejestru na Int16");
            //return Convert.ToInt16(HalfRegister);
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
            HalfRegister = Convert.ToSByte(value);
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
        private Int16 value;

        public NumericConstant(string number_string)
        {
            if(number_string == "0")
            {
                value = 0;
            }
            if(number_string.StartsWith("0x"))
            {
                value = Convert.ToInt16(number_string.Substring(2), 16);
            }
            else if (number_string.EndsWith("b"))
            {
                value = Convert.ToInt16(number_string.Substring(0, number_string.Length - 1), 2);
            }
            else if(number_string.StartsWith("0"))
            {
                value = Convert.ToInt16(number_string, 8);
            }
            else
            {
                value = Convert.ToInt16(number_string, 10);
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
            return Convert.ToSByte(value);
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
            return (name == "0" || name.StartsWith("0x") || name.EndsWith("b") || name.StartsWith("0") || name.All(char.IsDigit));
        }
    }

    class MemoryArgument : Argument
    {
        private readonly UInt16 adress;

        public MemoryArgument(string adress_string)
        {
            // to można poprawić ---
            // moge to jakoś zrobić żeby korzystało z faktycznego sposobu adresowania a nie tego chujostwa

            string raw_adress = adress_string.Substring(1, adress_string.Length - 2);
            adress = (UInt16)Program.ReadArgument(raw_adress).Get();
        }

        public Int16 Get()
        {
            return Convert.ToInt16(Program.Pamiec.PamiecAdresowana[adress]);
        }

        public UInt16 GetAddress()
        {
            return adress;
        }

        public bool Get_Bool()
        {
            throw new Exception("chyba powinienem móc wczytać tylko int16 z pamięci");
        }
        public sbyte Get_Sbyte()
        {
            throw new Exception("chyba powinienem móc wczytać tylko int16 z pamięci");
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