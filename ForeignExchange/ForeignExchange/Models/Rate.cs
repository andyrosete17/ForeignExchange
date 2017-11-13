
namespace ForeignExchange.Models
{
    using SQLite.Net.Attributes;

    public class Rate
    {
        //To work with sql lite it's necessary to define  a primary key
        [PrimaryKey]
        public int RateId { get; set; }

        public string Code { get; set; }

        public double TaxRate { get; set; }

        public string Name { get; set; }

        //To work with sql lite it's necessary to make search
        public override int GetHashCode()
        {
            return RateId;
        }
    }
}
