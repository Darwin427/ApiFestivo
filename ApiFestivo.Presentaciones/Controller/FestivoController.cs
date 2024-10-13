using ApiFestivo.Core.Servicios;
using ApiFestivo.Dominio;
using Microsoft.AspNetCore.Mvc;

namespace ApiFestivo.Presentaciones.Controller
{
    [ApiController]
    [Route("api/Festivos")]
    public class FestivoController : ControllerBase
    {
        private readonly IFestivoServicio servicio;
        public FestivoController(IFestivoServicio servicio)
        {
            this.servicio = servicio;
        }

        [HttpGet("ObtenerTodosLosFestivos")]
        public async Task<ActionResult<IEnumerable<Festivo>>> ObtenerTodosLosFestivos()
        {
            return Ok(await servicio.ObtenerTodosLosFestivos());
        }
        [HttpGet("obtener/{id}")]
        public async Task<ActionResult<Festivo>> Obtener(int id)
        {
            return Ok(await servicio.Obtener(id));
        }

        [HttpPost("AgregarFestivo")]
        public async Task<ActionResult<Festivo>> AgregarFestivo(DateTime fechaFestivo)
        {
            return Ok(await servicio.AgregarFestivo(fechaFestivo));
        }
        [HttpPut("ActualizarFestivo")]
        public async Task<ActionResult<Festivo>> ActualizarFestivo(DateTime fechaFestivo)
        {
            return Ok(await servicio.ActualizarFestivo(fechaFestivo));
        }
        [HttpDelete("EliminarFestivo/{fechaFestivo}")]
        public async Task<ActionResult<bool>> EliminarFestivo(DateTime fechaFestivo)
        {
            return Ok(await servicio.EliminarFestivo(fechaFestivo));
        }
        [HttpGet("EsFestivo/{IndiceDato}/{fecha}")]
        public async Task<ActionResult<IEnumerable<Festivo>>> EsFestivo(int IndiceDato, DateTime fecha)
        {
            return Ok(await servicio.EsFestivo(IndiceDato, fecha));
        }
    }
}
