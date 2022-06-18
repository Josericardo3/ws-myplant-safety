using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using ejemplo.netcore.DbContext;
using ejemplo.netcore.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace ejemplo.netcore.Controllers
{
    /// <summary>
    /// Retorna los datos de las ordenes
    /// </summary>        
    /// <remarks>
    /// Sample request:
    ///
    ///     Orders/
    ///
    /// </remarks>
    /// <param name="Id">Identificador del Orden</param>
    /// <returns></returns>
    /// <response code="400">Si no se encuentran ordenes</response>
    /// <response code="200">Si encuentra ordenes</response>
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private db dbop = new db();
        string msg = String.Empty;
        private readonly AppDbContext context;
        private readonly ILogger<OrderController> _logger;
        
        public OrderController(AppDbContext context, ILogger<OrderController> logger) 
        
        {
            this.context = context;
            _logger = logger;
           
        }
        
        [HttpGet("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        
        public ActionResult<IEnumerable<List<Object>>> Get()
        {
            try
            {
                Order ord = new Order();
                DataSet ds = dbop.OrderListGet(ord,out msg);
                List<OrderList> orderList = new List<OrderList>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    orderList.Add(new OrderList()
                    {
                        OrderId = Convert.ToInt32(dr["OrderID"]),
                        OrderDate = Convert.ToDateTime(dr["OrderDate"]),
                        ContactName = dr["ContactName"].ToString(),
                        Phone = dr["OrderDate"].ToString()
                    });
                }
                return Ok(orderList);
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
        ///     Orders/1
        ///
        /// </remarks>
        /// <param name="Id">Identificador de la orden</param>
        /// <returns></returns>
        /// <response code="400">Si no se encuentra una orden por la Id</response>
        /// <response code="200">Si encuentra una orden por la Id</response>
        [HttpGet("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<Order>> Get(int id)
        {
            try
            {
                var order = context.Orders.FirstOrDefault(p => p.OrderID == id);
                return Ok(order);
                //return StatusCode((int) HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Error - ...");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Agregar Orders
        /// </summary>        
        /// <returns></returns>
        /// <response code="500">Si no se encuentra la orden por la Id</response>
        /// <response code="200">Si se crea la orden</response>        
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Post([FromBody] Order input)
        {
            try
            {
                context.Orders.Add(input);
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
        /// Actualizar Orders
        /// </summary>        
        /// <returns></returns>
        /// <response code="500">Si no se encuentra una orden por la Id</response>
        /// <response code="200">Si se actualiza la orden</response>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Put(int id, [FromBody] Order input)
        {
            try
            {
                if (input.OrderID == id)
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
        /// Eliminar Orders
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
                var order = context.Orders.FirstOrDefault(p => p.OrderID==id);
                if (order != null)
                {
                    context.Orders.Remove(order);
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
