using System;
using System.Collections.Generic;

namespace ConsoleAppHotel_Lab2.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string? ClientFullName { get; set; }

    public string? ClientPassportDetails { get; set; }

    public DateTime? CheckInDate { get; set; }

    public DateTime? CheckOutDate { get; set; }

    public int? RoomId { get; set; }

    public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();

    public virtual Room? Room { get; set; }
}
