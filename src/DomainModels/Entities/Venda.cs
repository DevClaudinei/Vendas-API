using System;
using System.Collections.Generic;
using DomainModels.Enums;
using DomainModels.Interfaces;

namespace DomainModels.Entities;

public class Venda : IIdentifiable, ICreatable
{
    protected Venda() { }

    public Venda(DateTime dataVenda, Vendedor vendedor, ICollection<Item> itens, Status status)
    {
        DataVenda = dataVenda;
        Vendedor = vendedor;
        Itens = itens;
        Status = status;
    }

    public long Id { get; set; }
    public DateTime DataVenda { get; set; }
    public virtual Vendedor Vendedor { get; set; }
    public virtual ICollection<Item> Itens { get; set; }
    public Status Status { get; set; }
}