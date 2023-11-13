using System;
using System.Collections.Generic;

namespace ConsoleAppHotel_Lab2.Models;

public partial class RoomPrice
{
    public int RoomPriceId { get; set; }

    public int? RoomId { get; set; }

    public decimal? RoomCost { get; set; }

    public virtual Room? Room { get; set; }
}
