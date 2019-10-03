using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcademiaEnVivo.Models
{
    public class EventoModels:BasicModel
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

        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        [Required(ErrorMessage = "Campo Obligatorio")]
        [DisplayName("Hora Inicio")]
        public DateTime hora_desde { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        [Required(ErrorMessage = "Campo Obligatorio")]
        [DisplayName("Hora Fin")]
        public DateTime hora_hasta { get; set; }
        public int duracion { get; set; }


        //[DisplayFormat(DataFormatString = "##.000,00", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo Obligatorio")]       
        [DisplayName("Precio")]
        public string precio { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double precio_decimal { get; set; }

        [DisplayName("Estado")]
        public bool estado { get; set; }

       // [Required(ErrorMessage = "Campo Obligatorio")]
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
        public string hora_desde_string { get; set; }
        public string hora_hasta_string { get; set; }

        public List<BasicModel> lista_invitados = new List<BasicModel>();
    }
}