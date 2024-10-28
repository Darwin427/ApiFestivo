using ApiFestivo.Core.Servicios;
using ApiFestivo.Dominio;
using ApiFestivo.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace ApiFestivo.Presentaciones.Controller
{
    [ApiController]
    [Route("api/Festivo")]
    public class FestivoController : ControllerBase
    {
        private readonly FestivoServicio servicio;
        public FestivoController(FestivoServicio servicio)
        {
            this.servicio = servicio;
        }
        [HttpHead("EsFestivo/{fecha}")]
        public async Task<ActionResult<IEnumerable<Festivo>>> EsFestivo(DateTime fecha)
        {
            return Ok(await servicio.EsFestivo(fecha));
        }
    }
}
