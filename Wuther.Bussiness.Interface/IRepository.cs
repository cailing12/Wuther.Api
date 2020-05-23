using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Wuther.Entities.Models;

namespace Wuther.Bussiness.Interface {
    public interface IRepository<T> where T : class {

        /// <summary>
        /// 根据id查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindAsync (object id);

        /// <summary>
        /// 根据过滤条件，获取记录
        /// </summary>
        /// <param name="exp">The exp.</param>
        Task<IList<T>> FindAsync (Expression<Func<T, bool>> exp = null);

        /// <summary>
        /// 一次性查出全部数据
        /// </summary>
        /// <returns></returns>
        IQueryable<T> FindAll ();

        /// <summary>
        /// 得到分页记录
        /// </summary>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="orderby">排序，格式如："Id"/"Id descending"</param>
        IQueryable<T> GetTableData (int pageindex, int pagesize, out int count, Expression<Func<T, bool>> exp = null, Expression<Func<T, bool>> orderby = null);

        /// <summary>
        /// 新增数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        Task<T> InsertAsync (T t);

        /// <summary>
        /// 更新数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        Task<int> UpdateAsync (T t);

        /// <summary>
        /// 更新数据，即时Commit
        /// </summary>
        /// <param name="list"></param>
        Task<int> UpdateRangeAsync (IList<T> list);

        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="entity">The entity.</param>
        Task<int> UpdateAsync (Expression<Func<T, bool>> where, List<string> properties);

        /// <summary>
        /// 删除数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        bool Delete (T t);

        /// <summary>
        /// 根据主键删除数据，即时Commit
        /// </summary>
        /// <param name="t"></param>

        bool Delete (object id);

        /// <summary>
        /// 多行删除
        /// </summary>
        /// <param name="t"></param>
        Task<int> DeleteListAsync (List<T> list);

        /// <summary>
        /// 立即保存全部修改
        /// </summary>
        void Commit ();

        // /// <summary>
        // /// 执行sql 返回集合
        // /// </summary>
        // /// <param name="sql"></param>
        // /// <param name="parameters"></param>
        // /// <returns></returns>
        // IQueryable<T> ExcuteQuery (string sql, params object[] parameters);

        // /// <summary>
        // /// 执行sql，无返回
        // /// </summary>
        // /// <param name="sql"></param>
        // /// <param name="parameters"></param>
        // void Excute (string sql, MySqlParameter[] parameters);

        // /// <summary>
        // /// 执行sql,返回字典类型的数据
        // /// </summary>
        // /// <param name="sql"></param>
        // /// <param name="parameters"></param>
        // IQueryable<Dictionary<string, object>> ExcuteSqlQuery (string sql, params object[] parameters);

    }
}