using SHCA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.Core.Interface
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        IEnumerable<TEntity> GetAll();
        bool Save (TEntity entity);
        bool Delete (TEntity entity);
        IEnumerable<TEntity> Get (int id);
        IEnumerable<TEntity> Get (string id);
    }
}
