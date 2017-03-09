using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

public static class StringExtensions
{
    public static string SubStringTo(this string thatString, int limit)
    {
        if (!String.IsNullOrEmpty(thatString))
        {
            if (thatString.Length > limit)
            {
                return thatString.Substring(0, limit - 3) + "...";
            }
            return thatString;
        }
        return string.Empty;
    }
}

namespace ZarzadzanieProjektem.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string Tytul { get; set; }
        public string Opis { get; set; }
        public string Wymagania { get; set; }
        public virtual Dokumentacja Zalozenia { get; set; }
        public virtual List<Osoba> Wykonawcy { get; set; }
    }
    public class Osoba
    {
        [Key]
        public int OsobaId { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public virtual ProjectDetails ProjektClaimed { get; set; }
        public virtual List<Project> NonClaimedProjekts { get; set; }
    }
    public class ProjectDetails
    {
        [Key]
        public int ProjectDetailsId { get; set; }
        public virtual Project Projekt { get; set; }
        public string GitHubLink { get; set; }
        public virtual Dokumentacja Dokumentacja { get; set; }
        public virtual Dokumentacja DokumentacjaZalozen { get; set; }
    }
    public class Dokumentacja
    {
        [Key]
        public int DokumentacjaId { get; set; }
        public string NazwaPliku { get; set; }
        public string Rozszerzenie { get; set; }
        public BinaryData Plik { get; set; }  
    }
    public class BinaryData
    {
        [Key]
        public int BinaryDataId { get; set; }
        public byte[] Plik { get; set; }
    }
}