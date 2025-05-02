using Quarter.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Repositories.Contract
{
    public interface IGenericRepository<TEntity,Tkey> where TEntity : BaseEntity <Tkey>
    {
       Task< IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
      
        Task<TEntity> GetAsync(Tkey id);
    }
}
