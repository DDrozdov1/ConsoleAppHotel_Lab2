using System;
using System.Collections.Generic;

namespace ConsoleAppHotel_Lab2.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? RoomType { get; set; }

    public int? RoomCapacity { get; set; }

    public string? RoomDescription { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<RoomPrice> RoomPrices { get; set; } = new List<RoomPrice>();

    public virtual ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();
}
