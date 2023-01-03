using DomainModels.Entities;
using DomainModels.Enums;
using System;
using System.Collections.Generic;

namespace Application.Models;

public class CreateVendaRequest
{
    public CreateVendaRequest(DateTime dataVenda, Vendedor vendedor, ICollection<Item> itens, Status status)
    {
        DataVenda = dataVenda;
        Vendedor = vendedor;
        Itens = itens;
        Status = status;
    }

    public DateTime DataVenda { get; set; }
    public virtual Vendedor Vendedor { get; set; }
    public virtual ICollection<Item> Itens { get; set; }
    public Status Status { get; set; }
}