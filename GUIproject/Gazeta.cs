using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GUIproject
{
    public class Gazeta
    {
        public struct DaneOsobowe
        {
            public string imie { get; set; }
            public string nazwisko { get; set; }
        }
        public struct Statystyki
        {
            public int wyswietlenia { get; set; }
            public double oceny { get; set; }
        }
        public struct Redaktorzy
        {
            public DaneOsobowe dane;
            public int id { get; set; }
        }
        public struct Artykuly
        {
            public int id { get; set; }
            public DateTime data { get; set; }
            public Statystyki stats;
        }

        public DaneOsobowe dane;
        public Statystyki stats;
        public Redaktorzy redaktorzy;
        public Artykuly artykuly;

        public void Convert(RootObject obj)
        {
            this.redaktorzy.id = obj.Identyfikator;
            this.redaktorzy.dane.imie = obj.imie;
            this.redaktorzy.dane.nazwisko = obj.nazwisko;
            this.artykuly.stats.oceny = obj.Oceny;
            this.artykuly.stats.wyswietlenia = obj.Wyswietlenia;
            this.artykuly.data = obj.Data;
            this.artykuly.id = obj.IdentyfikatorArt;
        }
    }
}
