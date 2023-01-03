using DomainModels.Entities;
using DomainModels.Enums;
using System;
using System.Collections.Generic;

namespace Application.Models;

public class VendaResult
{
    protected VendaResult() { }

    public VendaResult(long idVenda, DateTime dataVenda, ICollection<Item> itens, Status status)
    {
        IdVenda = idVenda;
        DataVenda = dataVenda;
        Itens = itens;
        Status = status;
    }

    public long IdVenda { get; set; }
    public DateTime DataVenda { get; set; }
    public virtual ICollection<Item>? Itens { get; set; }
    public Status Status { get; set; }
}