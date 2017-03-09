using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZarzadzanieProjektem.Models
{
    public class ProjectViewModel
    {
        public IEnumerable<Project> Projekty { get; set; }
        public IEnumerable<Osoba> Osoby { get; set; }
        public Osoba LoggedUser { get; set; }
    }
}