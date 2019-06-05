using System;

namespace GUIproject
{
    public class RootObject
    {
        public DateTime Data { get; set; }
        public int Identyfikator { get; set; }
        public int IdentyfikatorArt { get; set; }
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public double Oceny { get; set; }
        public int Wyswietlenia { get; set; }

        public void Convert(Gazeta gazeta)
        {
            this.Identyfikator = gazeta.redaktorzy.id; 
            this.imie = gazeta.redaktorzy.dane.imie;
            this.nazwisko = gazeta.redaktorzy.dane.nazwisko;
            this.Oceny = gazeta.artykuly.stats.oceny;
            this.Wyswietlenia = gazeta.artykuly.stats.wyswietlenia;
            this.Data = gazeta.artykuly.data;
            this.IdentyfikatorArt = gazeta.artykuly.id;
        }

    }
}
