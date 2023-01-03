namespace Projekcior.Pamiec {
    class MagazynRejestrow {
        private static sbyte HighPart(Int16 rejestr) {
            // pobiera z rejestru lewą strone
            // xxxx xxxx ---- ----
            return Convert.ToSByte(rejestr & 0xff00);
        }
        private static sbyte LowPart(Int16 rejestr) {
            // pobiera z rejestru prawą stronę
            // ---- ---- xxxx xxxx
            return Convert.ToSByte(rejestr & 0x00ff);
        }
        private static void SetHighPart(ref Int16 rejestr, sbyte value) {
            rejestr &= (Int16)0x00ffU;
            rejestr |= (Int16)(Convert.ToInt16(value) << 8);
        }

        private static void SetLowPart(ref Int16 rejestr, sbyte value) {
            unchecked
            {
                // : ] działa? działa!
                rejestr &= (Int16)0xff00U;
            }
            rejestr |= (Int16)(Convert.ToInt16(value) & 0x00ffU);
        }

        public Int16 AX;
        public sbyte AH {
            get {
                return HighPart(AX);
            }
            set {
                SetHighPart(ref AX, value);
            }
        }

        public sbyte AL {
            get {
                return LowPart(AX);
            }
            set {
                SetLowPart(ref AX, value);
            }
        }

        public Int16 BX;
        public sbyte BH {
            get {
                return HighPart(BX);
            }
            set {
                SetHighPart(ref BX, value);
            }
        }

        public sbyte BL {
            get {
                return LowPart(BX);
            }
            set {
                SetLowPart(ref BX, value);
            }
        }

        public Int16 CX;
        public sbyte CH {
            get {
                return HighPart(CX);
            }
            set {
                SetHighPart(ref CX, value);
            }
        }

        public sbyte CL {
            get {
                return LowPart(CX);
            }
            set {
                SetLowPart(ref CX, value);
            }
        }

        public Int16 DX;
        public sbyte DH {
            get {
                return HighPart(DX);
            }
            set {
                SetHighPart(ref DX, value);
            }
        }

        public sbyte DL {
            get {
                return LowPart(DX);
            }
            set {
                SetLowPart(ref DX, value);
            }
        }
    }
}