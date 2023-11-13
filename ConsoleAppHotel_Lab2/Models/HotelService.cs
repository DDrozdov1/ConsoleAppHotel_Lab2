using System;
using System.Collections.Generic;

namespace ConsoleAppHotel_Lab2.Models;

public partial class HotelService
{
    public int HotelServiceid { get; set; }

    public string? HotelServiceName { get; set; }

    public string? HotelServiceDescription { get; set; }

    public decimal? HotelServiceCost { get; set; }

    public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();
}
