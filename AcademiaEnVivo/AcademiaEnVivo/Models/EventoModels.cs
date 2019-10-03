using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcademiaEnVivo.Models
{
    public class EventoModels
    {
        public Guid id_evento { get; set; }
        [DisplayName("Categoria")]
        [Required(ErrorMessage = "Campo Obligatorio")]
        public Guid? id_categoria { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        [DisplayName("Nombre")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        [DisplayName("Descripción")]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [DisplayName("Fecha Evento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public DateTime fecha_evento { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [DisplayName("Hora Desde")]
        public string hora_desde { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        [DisplayName("Hora Hasta")]
        public string hora_hasta { get; set; }
        public int duracion { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        [Required(ErrorMessage = "Campo Obligatorio")]
        [DisplayName("Precio")]
        public decimal precio { get; set; }

        [DisplayName("Estado")]
        public bool estado { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [DisplayName("Invitados")]
        public string invitados { get; set; }

        public string estado_string
        {
            get
            { return estado_; }

            set
            {
                if (estado == true)
                    estado_ = "Activo";
                else
                    estado_ = "Inactivo";
            }
        }

        public string estado_ { get; set; }

        public string nombre_categoria { get; set; }
    }
}