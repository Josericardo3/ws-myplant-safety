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
    ///     product/
    ///
    /// </remarks>
    /// <param name="Id">Identificador del Orden</param>
    /// <returns></returns>
    /// <response code="400">Si no se encuentran ordenes</response>
    /// <response code="200">Si encuentra ordenes</response>
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger<ProductController> _logger;
        public ProductController(AppDbContext context, ILogger<ProductController> logger) 
        {
            this.context = context;
            _logger = logger;
        }

        [HttpGet("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<ProductController>> Get()
        {
            try
            {
                return Ok(context.Products.ToList());
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
        ///     product/1
        ///
        /// </remarks>
        /// <param name="Id">Identificador de la orden</param>
        /// <returns></returns>
        /// <response code="400">Si no se encuentra una orden por la Id</response>
        /// <response code="200">Si encuentra una orden por la Id</response>
        [HttpGet("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<Product>> Get(int id)
        {
            try
            {
                var product = context.Products.FirstOrDefault(p => p.ProductID == id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Error - ...");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Agregar products
        /// </summary>        
        /// <returns></returns>
        /// <response code="500">Si no se encuentra la orden por la Id</response>
        /// <response code="200">Si se crea la orden</response>        
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Post([FromBody] Product input)
        {
            try
            {
                context.Products.Add(input);
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
        /// Actualizar products
        /// </summary>        
        /// <returns></returns>
        /// <response code="500">Si no se encuentra una orden por la Id</response>
        /// <response code="200">Si se actualiza la orden</response>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Put(int id, [FromBody] Product input)
        {
            try
            {
                if (input.ProductID == id)
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
        /// Eliminar products
        /// </summary>        
        /// <returns></returns>
        /// <response code="500">Si no se encuentra una orden por la Id</response>
        /// <response code="200">Si se elimina la orden</response>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Delete(int id)
        {
            try
            {
                var product = context.Products.FirstOrDefault(p => p.ProductID==id);
                if (product != null)
                {
                    context.Products.Remove(product);
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
