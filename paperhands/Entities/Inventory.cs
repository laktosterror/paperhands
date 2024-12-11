﻿using System;
using System.Collections.Generic;

namespace paperhands.Entities;

public partial class Inventory
{
    public long StoreId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public long Amount { get; set; }

    public virtual Book Isbn13Navigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}