using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiFestivo.Dominio
{
    [Table("Tipo")]
    public class Tipo
    {
        [Column("Id")]

        public int Id { get; set; }
        [Column("Tipo"), StringLength(100)]

        public string tipo { get; set; }
    }
}
