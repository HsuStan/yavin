using System;
using System.Data.Entity;
using System.Linq;

namespace Yavin.ORM
{
	public class EFDataAccess<TEntity> : IDataAccess<TEntity> where TEntity : BaseMeta
	{
		private readonly EntityContext _context;
		private IDbSet<TEntity> _entities;

		public EFDataAccess(EntityContext context)
		{
			this._context = context;
		}

		protected IDbSet<TEntity> Entities
		{
			get
			{
				if (this._entities == null)
					this._entities = this._context.Set<TEntity>();
				return this._entities;
			}
		}

		protected string GetCacheKey(Type type, object id)
		{
			return string.Format("{0}.{1}", type.FullName, id);
		}

		#region IDataAccess<TEntity> 成员

		public void Insert(TEntity entity)
		{
			this.Entities.Add(entity);
			this._context.SaveChanges();
		}

		public void Delete(TEntity entity)
		{
			this.Entities.Remove(entity);
			this._context.SaveChanges();
		}

		public void Update(TEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");
			this._context.SaveChanges();
		}

		public TEntity GetByID(object id)
		{
			return this.Entities.Find(id);
		}

		public IQueryable<TEntity> Table
		{
			get { return this.Entities; }
		}

		#endregion
	}
}
