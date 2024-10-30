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
        private readonly IFestivoServicio servicio;
        public FestivoController(IFestivoServicio servicio)
        {
            this.servicio = servicio;
        }
        [HttpGet("EsFestivo/{fecha}")]
        public async Task<ActionResult<IEnumerable<Festivo>>> EsFestivo(DateTime fecha)
        {
            return Ok(await servicio.EsFestivo(fecha));
        }
    }
}
