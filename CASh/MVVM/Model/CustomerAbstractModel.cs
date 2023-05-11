using CASh.Core.DBClient;
using System;

namespace CASh.MVVM.Model
{
    public abstract class CustomerAbstractModel : AbstractModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public double TotalMoneySpend { get; set; }
        public int OrderCounter { get; set; }
        public int RmaCounter { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
