using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ejemplo.netcore.Entity
{
    public class OrderList
    {
        [Key]
        public int ProductID { get; set; }
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? ProductName { get; set; }
        public double? UnitPrice { get; set; }
        public Int16? Quantity { get; set; }
        public string? ContactName { get; set; }
        public string? Phone { get; set; }
    }
}