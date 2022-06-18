using System;

namespace ejemplo.netcore.Entity
{
    public class OrderModify
    {
        public int OrderID { get; set; }
        public string ContactTitle { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? Confirm { get; set; }
        public string? Comment { get; set; }
    }
}