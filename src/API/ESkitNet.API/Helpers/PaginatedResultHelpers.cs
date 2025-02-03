using ESkitNet.Core.Abstractions;
using ESkitNet.Core.Specifications;

namespace ESkitNet.API.Helpers;

public static class PaginatedResultHelpers
{
    public async static Task<PaginatedResult<TDto>> GetPageDetails<TDto, TEntity, TEntityKey>(
        IUnitOfWork unitOfWork, ISpecification<TEntity> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
        where TEntity : Entity<TEntityKey>
        where TEntityKey : class
        where TDto : class
    {
        //var spec = new ProductSpecification(specParams);
        var entities = await unitOfWork.Repository<TEntity, TEntityKey>().GetAllWithSpecAsync(spec, cancellationToken);
        var count = await unitOfWork.Repository<TEntity, TEntityKey>().CountAsync(spec, cancellationToken);
        var dtos = entities.Adapt<List<TDto>>();
        var paginatedResult = new PaginatedResult<TDto>(pageNumber, pageSize, count, dtos);

        return paginatedResult;
    }
}
