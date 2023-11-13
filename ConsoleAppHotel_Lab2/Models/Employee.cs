using System;
using System.Collections.Generic;

namespace ConsoleAppHotel_Lab2.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? EmployeeFullName { get; set; }

    public string? EmployeePosition { get; set; }

    public virtual ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();
}
