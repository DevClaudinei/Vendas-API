﻿using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infraestructure.Data.Context;
using System;

namespace DomainServices.Services;

public abstract class BaseService
{
    protected IUnitOfWork UnitOfWork;
    protected IRepositoryFactory RepositoryFactory;

    public BaseService(IUnitOfWork<ApplicationDbContext> unitOfWork, IRepositoryFactory<ApplicationDbContext> repositoryFactory)
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        RepositoryFactory = repositoryFactory ?? (IRepositoryFactory)UnitOfWork;
    }
}
