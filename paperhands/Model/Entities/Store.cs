﻿namespace paperhands.Model.Entities;

public class Store
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}