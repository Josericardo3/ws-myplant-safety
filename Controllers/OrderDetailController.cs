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
using Microsoft.EntityFrameworkCore;

namespace ejemplo.netcore.Controllers
{
    /// <summary>
    /// Retorna los datos de las ordenes
    /// </summary>        
    /// <remarks>
    /// Sample request:
    ///
    ///     orderDetail/
    ///
    /// </remarks>
    /// <param name="Id">Identificador del Orden</param>
    /// <returns></returns>
    /// <response code="400">Si no se encuentran ordenes</response>
    /// <response code="200">Si encuentra ordenes</response>
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private db dbop = new db();
        string msg = String.Empty;
        private readonly AppDbContext context;
        private readonly ILogger<OrderDetailController> _logger;
        public OrderDetailController(AppDbContext context, ILogger<OrderDetailController> logger) 
        {
            this.context = context;
            _logger = logger;
        }

        [HttpGet("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<OrderDetail>> Get()
        {
            try
            {
                return Ok(context.OrderDetails.ToList());
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
        /// <response code="400">Si no se encuentran los detalles de la  orden por la Id de la orden</response>
        /// <response code="200">Si encuentra los detalles de la orden por la Id de la orden</response>
        [HttpGet("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<List<List<Object>>> Get(int id)
        {
            try
            {
                Order ord = new Order();
                ord.OrderID = id;
                DataSet ds = dbop.OrderGet(ord,out msg);
                List<OrderList> orderList = new List<OrderList>();
                List<OrderModify> orderModify = new List<OrderModify>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    orderList.Add(new OrderList()
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = (dr["ProductName"]).ToString(),
                        UnitPrice = Convert.ToDouble(dr["UnitPrice"]),
                        Quantity = Convert.ToInt16(dr["Quantity"])
                    });
                }
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    orderModify.Add(new OrderModify()
                    {
                        OrderID = Convert.ToInt32(dr["OrderID"]),
                        OrderDate = Convert.ToDateTime(dr["OrderDate"]),
                        Confirm = Convert.ToInt16(dr["Confirm"]),
                        Comment = (dr["Comment"]).ToString(),
                        ContactTitle = (dr["ContactTitle"]).ToString(),
                    });
                }
                List<Object> response = new List<object>();
                response.Add(orderList);
                response.Add(orderModify);
                return Ok(response);
                //return StatusCode((int) HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Error - ...");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Agregar OrderDetail
        /// </summary>        
        /// <returns></returns>
        /// <response code="500">Si no se encuentra la orden por la Id</response>
        /// <response code="200">Si se crea los detalles de la orden</response>        
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Post([FromBody] OrderDetail input)
        {
            try
            {
                context.OrderDetails.Add(input);
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
        /// Actualizar OrderDetail
        /// </summary>        
        /// <returns></returns>
        /// <response code="500">Si no se encuentra una orden por la Id de la orden</response>
        /// <response code="200">Si se actualizan los detalles de la orden</response>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Put(int id, [FromBody] OrderDetail input)
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
        /// Eliminar OrderDetail
        /// </summary>        
        /// <returns></returns>
        /// <response code="500">Si no se encuentra una orden por la Id de la orden</response>
        /// <response code="200">Si se elimina los detalles de la orden</response>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Delete(int id)
        {
            try
            {
                var orderDetail = context.OrderDetails.FirstOrDefault(p => p.OrderID==id);
                if (orderDetail != null)
                {
                    context.OrderDetails.Remove(orderDetail);
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
