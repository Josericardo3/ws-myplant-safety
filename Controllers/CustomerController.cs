using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using ejemplo.netcore.DbContext;
using ejemplo.netcore.Entity;
using Microsoft.EntityFrameworkCore;

namespace ejemplo.netcore.Controllers
{
    /// <summary>
    /// Retorna los datos de las ordenes
    /// </summary>        
    /// <remarks>
    /// Sample request:
    ///
    ///     customer/
    ///
    /// </remarks>
    /// <param name="Id">Identificador del Orden</param>
    /// <returns></returns>
    /// <response code="400">Si no se encuentran ordenes</response>
    /// <response code="200">Si encuentra ordenes</response>
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(AppDbContext context, ILogger<CustomerController> logger) 
        {
            this.context = context;
            _logger = logger;
        }

        [HttpGet("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            try
            {
                return Ok(context.Customers.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error - ...");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Retorna los datos de las ordenes por Id
        /// </summary>        
        /// <remarks>
        /// Sample request:
        ///
        ///     customer/1
        ///
        /// </remarks>
        /// <param name="Id">Identificador de la orden</param>
        /// <returns></returns>
        /// <response code="400">Si no se encuentra una orden por la Id</response>
        /// <response code="200">Si encuentra una orden por la Id</response>
        [HttpGet("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<Customer>> Get(string id)
        {
            try
            {
                var customer = context.Customers.FirstOrDefault(p => p.CustomerID == id);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Error - ...");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Agregar Customers
        /// </summary>        
        /// <returns></returns>
        /// <response code="500">Si no se encuentra la orden por la Id</response>
        /// <response code="200">Si se crea la orden</response>        
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Post([FromBody] Customer input)
        {
            try
            {
                context.Customers.Add(input);
                context.SaveChanges();
                return Ok(input);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error - ...");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Actualizar Customers
        /// </summary>        
        /// <returns></returns>
        /// <response code="500">Si no se encuentra una orden por la Id</response>
        /// <response code="200">Si se actualiza la orden</response>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Put(string id, [FromBody] Customer input)
        {
            try
            {
                if (input.CustomerID == id)
                {
                    context.Entry(input).State = EntityState.Modified;
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error - ...");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Eliminar Customers
        /// </summary>        
        /// <returns></returns>
        /// <response code="500">Si no se encuentra una orden por la Id</response>
        /// <response code="200">Si se elimina la orden</response>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Delete(string id)
        {
            try
            {
                var customer = context.Customers.FirstOrDefault(p => p.CustomerID==id);
                if (customer != null)
                {
                    context.Customers.Remove(customer);
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);    
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error - ...");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"{ex.Message}");
            }
        }
    }
}
