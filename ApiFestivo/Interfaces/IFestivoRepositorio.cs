using ApiFestivo.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiFestivo.Core.Interfaces
{
    public interface IFestivoRepositorio
    {
        Task<IEnumerable<Festivo>> ObtenerTodosLosFestivos();

        Task<Festivo> Obtener(int Id);

        Task<bool> EsFestivo(Festivo fecha);

        Task<Festivo> AgregarFestivo(Festivo fechaFestivo);

        Task<bool> EliminarFestivo(Festivo fechaFestivo);

        Task<Festivo> ActualizarFestivo(Festivo fechaFestivo);
    }
}
