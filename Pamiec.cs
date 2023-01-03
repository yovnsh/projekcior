using System.Collections.Generic;
using Projekcior.Pamiec;

namespace Projekcior {
    /*
    *
    * hipokamp czyli skupisko szarych komórek w płatach skroniowych, wpuklające się do rogów komór bocznych mózgu.
    *
    */
    class Hipokamp {

        public MagazynRejestrow Rejestry = new MagazynRejestrow();
        public Dictionary<string, bool> Flagi = new Dictionary<string, bool>();
        //public Stack<Int16> Stos = new Stack<Int16>(0xffff);
        //public Int16[] Pamiec = new Int16[0xffff];
        public Dictionary<string, UInt16> Segmenty = new Dictionary<string, UInt16>();
        public Dictionary<string, UInt16> Wskazniki = new Dictionary<string, UInt16>();

        // otóż jak sie okazuje wszystko jest przechowywane w jednej pamięci
        // i stack i kod i jeszcze coś tam innego
        // ze względu na to że mi sie nie chce pisać kompilacji kodu w asemblerze
        // pamięć będzie reprezentowana jako tablica stringów
        // instrukcje będą tam zapisane w całości :]
        // liczby zamienione przez ToString
        public string[] PamiecAdresowana = new string[0xffff];
        public Hipokamp() {
            // Flagi
            Flagi.Add("OF", false); // overflow flag - kiedy wykroczymy poza limit liczby np pomnożymy dwie bardzo duże liczby i magicznie zrobi się ujemna
            Flagi.Add("SF", false); // sign flag - kiedy wynik operacji jest ujemny przyjmuje 1 (w praktyce to znaczy że pierwszy bit = 1)
            Flagi.Add("ZF", false); // zero flag - kiedy wynik operacji matematycznej jest równy 0 przyjmuje 1
            Flagi.Add("AF", false); // auxiliary carry flag (cokolwiek to znaczy) - kiedy przy operacjach BCD (czyli chyba w systemie 10) nastąpi przeniesienie do następnej kolumny
            Flagi.Add("PF", false); // parity flag - kiedy liczba jedynek w binarnej reprezentacji liczby jest kurwa parzysta XD ustawia sie na 1
            Flagi.Add("CF", false); // carry flag - kiedy przy dodawaniu nastąpi przeniesienie lub przy odejmowaniu pożyczka

            Flagi.Add("DF", false); // direction flag - prawdopodobnie sprawia że teksty są czytane od tyłu
            Flagi.Add("IF", false); // interrupt flag - kiedy 1 przerwania będą rejestrowane, jeśli 0 nie
            Flagi.Add("TF", false); // trap flag - kiedy 1 instrukcje są wykonywane pojedyńczo (po każdej uruchamiane jest przerwanie)
            // Segmenty
            Segmenty.Add("CS", 0); // code segment - wskazuje na miejsce gdzie sie zaczyna kod w pamięci
            Segmenty.Add("DS", 0); // data segment - wskazuje na miejsce gdzie są przechowywane jakieś stałe dane w pamięci
            Segmenty.Add("ES", 0); // extra segment - wskazuje na miejsce gdzie jest przechowywane ?więcej danych (ngl chyba tego nie bede używać)
            Segmenty.Add("SS", 0); // stack segment - wskazuje na stos w pamięci
            // Wskazniki
            Wskazniki.Add("SP", 0); // stack pointer - ile elementów znajduje się w stosie ([SS + SP] wskazuje na ostatni element stosu)
            Wskazniki.Add("BP", 0); // base pointer - ???
            Wskazniki.Add("IP", 0); // instruction pointer - numer aktualnie wykonywanego rozkazu ([CS + IP] wskazuje na miejsce w pamięci gdzie jest przechowywana instrukcja)
            Wskazniki.Add("SI", 0); // source index - ???
            Wskazniki.Add("DI", 0); // destination index - ???
        }
    }
}