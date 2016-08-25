using EF_Web_Test.Expressions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using EntityFramework.Extensions;

namespace EF_Web_Test.Repository
{
    public class Repository<T> where T : class
    {
        private EFDBContext context;

        private DbSet<T> entities;

        private string errorMessage = string.Empty;

        public Repository(EFDBContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// 实体集合
        /// </summary>
        public DbSet<T> Entities
        {
            get
            {
                if (entities == null)
                {
                    entities = context.Set<T>();
                }
                return entities;
            }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Add(T entity)
        {
            this.Entities.Add(entity);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEnumerable<T> entities)
        {
            Entities.AddRange(entities);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return Entities
                .AsQueryable<T>()
                .Count<T>(predicate);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Delete(T entity)
        {
            if (entity != null)
            {
                this.Entities.Remove(entity);
            }
        }

        //    return query;
        //}
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(Expression<Func<T, bool>> predicate)
        {
            Entities.Where(predicate).Delete();
        }

        /// <summary>
        /// 根据id 查询实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T Get(int Id)
        {
            return this.Entities.Find(Id);
        }

        /// <summary>
        /// 查询所有的
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            //  throw new NotImplementedException();

            IEnumerable<T> query = this.Entities;

            return query;
        }

        /// <summary>
        /// 查询所有带条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IQueryable<T> GetAll(Expression<Func<T, bool>> where)
        {
            IQueryable<T> query = this.Entities.Where(where);
            return query;
        }

        /// <summary>
        /// 这里对Set<T>是带条件的操作
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T GetByWhere(Expression<Func<T, bool>> where)
        {
            return this.Entities.Where(where).FirstOrDefault<T>();
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<T> GetPage(int pageIndex, int pageSize, string orderBy, bool ascending, out int totalRecord)
        {
            totalRecord = 0;
            List<T> list = Entities.OrderBy(orderBy, ascending).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            totalRecord = Entities.Count();
            if (totalRecord <= 0) return new List<T>();
            return list;
        }

        /// </summary>
        /// 根据条件分页获得记录
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="ascending">是否升序</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="totalRecord">总记录数</param>
        /// <returns>记录列表</returns>
        public virtual List<T> GetPage(Expression<Func<T, bool>> where, string orderBy, bool ascending, int pageIndex, int pageSize, out int totalRecord)
        {
            totalRecord = 0;
            var list = Entities.Where(where);
            totalRecord = list.Count();
            if (totalRecord <= 0) return new List<T>();

            list = list.OrderBy(orderBy, ascending).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return list.ToList();
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Update(T entity)
        {
            Entities.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 根据条件更新实体
        /// </summary>
        /// <param name="expression"></param>
        public void Update(Expression<Action<T>> expression)
        {
            T newEntity = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null) as T;//建立指定类型的实例
            List<string> propertyNameList = new List<string>();
            MemberInitExpression param = expression.Body as MemberInitExpression;
            foreach (var item in param.Bindings)
            {
                string propertyName = item.Member.Name;
                object propertyValue;
                var memberAssignment = item as MemberAssignment;
                if (memberAssignment.Expression.NodeType == ExpressionType.Constant)
                {
                    propertyValue = (memberAssignment.Expression as ConstantExpression).Value;
                }
                else
                {
                    propertyValue = Expression.Lambda(memberAssignment.Expression, null).Compile().DynamicInvoke();
                }
                typeof(T).GetProperty(propertyName).SetValue(newEntity, propertyValue, null);
                propertyNameList.Add(propertyName);
            }
            Entities.Attach(newEntity);
            context.Configuration.ValidateOnSaveEnabled = false;
            var ObjectStateEntry = ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.GetObjectStateEntry(newEntity);
            propertyNameList.ForEach(x => ObjectStateEntry.SetModifiedProperty(x.Trim()));
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        public void UpdateRange(Expression<Func<T, bool>> where, Expression<Func<T, T>> predicate)
        {
            Entities.Where(where).Update(predicate);
        }
        public virtual RepositoryQuery<T> Query()
        {
            var repositoryGetFluentHelper = new RepositoryQuery<T>(this);

            return repositoryGetFluentHelper;
        }

        internal IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<T> query = Entities;

            if (includeProperties != null)
            {
                includeProperties.ForEach(i => { query = query.Include(i); });
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);

            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        
    }

}