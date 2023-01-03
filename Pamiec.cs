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
        public Stack<Int16> Stos = new Stack<Int16>(0xffff);
        public Int16[] Pamiec = new Int16[0xffff];
        public Dictionary<string, Int16> Segmenty = new Dictionary<string, Int16>();
        public Dictionary<string, UInt16> Wskazniki = new Dictionary<string, UInt16>();
        public Hipokamp() {
            // Flagi
            Flagi.Add("OF", false);
            Flagi.Add("DF", false);
            Flagi.Add("IF", false);
            Flagi.Add("TF", false);
            Flagi.Add("SF", false);
            Flagi.Add("ZF", false);
            Flagi.Add("AF", false);
            Flagi.Add("PF", false);
            Flagi.Add("CF", false);
            // Segmenty
            Segmenty.Add("CS", 0);
            Segmenty.Add("DS", 0);
            Segmenty.Add("ES", 0);
            Segmenty.Add("SS", 0);
            // Wskazniki
            Wskazniki.Add("SP", Convert.ToUInt16(0xfffe));
            Wskazniki.Add("BP", 0);
            Wskazniki.Add("IP", 0);
            Wskazniki.Add("SI", 0);
            Wskazniki.Add("DI", 0);
        }
    }
}