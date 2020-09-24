namespace Core.Entities.Domain
{
    public class LodgingAddress : BaseEntity
    {
        public string County { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Floor { get; set; }
        public string DoorNumber { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
        public int LodgingId { get; set; }
        public Lodging Lodging { get; set; }
    }
}