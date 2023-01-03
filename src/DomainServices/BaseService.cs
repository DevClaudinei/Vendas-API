using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infraestructure.Data.Context;

namespace DomainServices;

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
