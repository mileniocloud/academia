using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcademiaEnVivo.Models
{
    public class BasicModel
    {
        public Guid id { get; set; }
        public string value { get; set; }
        public bool ShowError { get; set; }
        public string ErrorMessage { get; set; }

        public Titles title = new Titles();

        public TypeError type = new TypeError();
        public bool Clean { get; set; }
    }

    public enum TypeError
    {
        error,
        success,
        warning
    }
    public enum Titles {
        Exito,
        Error,
        Advertencia,
        Atención
    }
}