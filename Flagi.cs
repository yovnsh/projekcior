namespace Projekcior.Pamiec
{
    class MagazynFlag
    {
        // 15 14 13 12 11 10  9  8    7  6  5  4  3  2  1  0
        // -- -- -- -- OF DF IF TF   SF ZF -- AF -- PF -- CF
        public UInt16 FlagiSurowe = 0b0000_0000_0000_0000;

        private bool GetBit(int n)
        {
            if(n < 0 || n > 15)
            {
                throw new ArgumentException("nieprawidłowy bit");
            }

            // 1 << n = jedynka na bicie, który pragniemy
            // Flagi | (selection) - zerujemy wszystkie bity poza naszym wybranym
            // (...) > 0 jeżeli nasz wybrany bit to 1 to liczba będzie większa niż zero

            UInt16 selection_number = (UInt16)(1 << n);
            return (FlagiSurowe & selection_number) > 0;
        }

        private void SetBit(int n, bool value)
        {
            if (n < 0 || n > 15)
            {
                throw new ArgumentException("nieprawidłowy bit");
            }

            if(value)
            {
                // ustawiamy bit który potrzebujemy na 1
                FlagiSurowe |= (UInt16)(1 << n);
            } else
            {
                // uuhh
                // 1 << n - jedynka na bicie który potrzebujemy
                // ~(...) - jedynki na wszystkich bitach poza wybranym
                // Flagi &= ~(...) - pozostawiamy wszystkie bity bez zmian, a wybrany przez nas ustawiamy na 0
                FlagiSurowe &= (UInt16)(~(UInt16)(1 << n));
            }
        }

        // overflow flag - kiedy wykroczymy poza limit liczby np pomnożymy dwie bardzo duże liczby i magicznie zrobi się ujemna
        // ----x--- --------
        public bool OF
        {
            get
            {
                return GetBit(11);
            }
            set
            {
                SetBit(11, value);
            }
        }

        // sign flag - kiedy wynik operacji jest ujemny przyjmuje 1 (w praktyce to znaczy że pierwszy bit = 1)
        // -----x-- --------
        public bool DF
        {
            get
            {
                return GetBit(10);
            }
            set
            {
                SetBit(10, value);
            }
        }

        // interrupt flag - kiedy 1 przerwania będą rejestrowane, jeśli 0 nie
        // ------x- --------
        public bool IF
        {
            get
            {
                return GetBit(9);
            }
            set
            {
                SetBit(9, value);
            }
        }

        // trap flag - kiedy 1 instrukcje są wykonywane pojedyńczo (po każdej uruchamiane jest przerwanie)
        // -------x --------
        public bool TF
        {
            get
            {
                return GetBit(8);
            }
            set
            {
                SetBit(8, value);
            }
        }

        // sign flag - kiedy wynik operacji jest ujemny przyjmuje 1 (w praktyce to znaczy że pierwszy bit = 1)
        // -------- x-------
        public bool SF
        {
            get
            {
                return GetBit(7);
            }
            set
            {
                SetBit(7, value);
            }
        }

        // zero flag - kiedy wynik operacji matematycznej jest równy 0 przyjmuje 1
        // -------- -x------
        public bool ZF
        {
            get
            {
                return GetBit(6);
            }
            set
            {
                SetBit(6, value);
            }
        }

        // auxiliary carry flag (cokolwiek to znaczy) - kiedy przy operacjach BCD (czyli chyba w systemie 10) nastąpi przeniesienie do następnej kolumny
        // -------- ---x----
        public bool AF
        {
            get
            {
                return GetBit(4);
            }
            set
            {
                SetBit(4, value);
            }
        }

        // parity flag - kiedy liczba jedynek w binarnej reprezentacji liczby jest kurwa parzysta XD ustawia sie na 1
        // -------- ------x--
        public bool PF
        {
            get
            {
                return GetBit(2);
            }
            set
            {
                SetBit(2, value);
            }
        }

        // carry flag - kiedy przy dodawaniu nastąpi przeniesienie lub przy odejmowaniu pożyczka
        // -------- -------x
        public bool CF
        {
            get
            {
                return GetBit(0);
            }
            set
            {
                SetBit(0, value);
            }
        }
    }
}
