using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.Data.MySqlClient;
using Wuther.Bussiness.Interface;

namespace Wuther.Bussiness.Service {

    public class Repository<T> : IRepository<T> where T : class {
        private DbContext _context;
        public Repository (DbContext context) {
            _context = context;
        }

        public void Commit () {
            _context.SaveChanges ();
        }

        public async Task<bool> Delete (T t) {
            _context.Set<T> ().Remove (t);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool Delete (object id) {
            var entity = _context.Set<T> ().Find (id);
            if (entity != null) {
                _context.Set<T> ().Remove (entity);
                Commit ();
                return true;
            }
            return false;
        }

        public bool DeleteList (List<T> list) {
            _context.Set<T> ().RemoveRange (list);
            Commit ();
            return true;
        }

        public async Task<int> DeleteListAsync (List<T> list) {
            _context.Set<T> ().RemoveRange (list);
            return await _context.SaveChangesAsync ();
        }

        public IQueryable<T> FindAll () {
            return _context.Set<T> ();
        }

        public async Task<T> FindAsync (object id) {
            return await _context.Set<T> ().FindAsync (id);
        }

        public async Task<IList<T>> FindAsync (Expression<Func<T, bool>> exp = null) {
            return await _context.Set<T> ().Where (exp).ToListAsync ();
        }

        public IQueryable<T> GetTableData (int pageindex, int pagesize, out int count, Expression<Func<T, bool>> exp = null, Expression<Func<T, bool>> orderby = null) {
            var list = _context.Set<T> ().Where (exp);
            count = list.Count ();
            return list.Skip ((pageindex - 1) * pagesize).Take (pagesize).OrderBy (orderby);
        }

        public async Task<T> InsertAsync (T t) {
             EntityEntry entity = await _context.Set<T> ().AddAsync (t);
            _context.SaveChanges ();
            return (T)entity.Entity;
        }

        public async Task<int> UpdateAsync (T t) {
            _context.Set<T> ().Update (t);
            return await _context.SaveChangesAsync ();
        }

        public async Task<int> UpdateAsync (Expression<Func<T, bool>> where, List<string> properties) {
            var list = _context.Set<T> ().Where (where);
            foreach (var item in list) {
                foreach (var prop in properties) {
                    _context.Entry (item).Property (prop).IsModified = true;
                }
            }
            return await _context.SaveChangesAsync ();
        }

        public async Task<int> UpdateRangeAsync (IList<T> list) {
            _context.Set<T> ().UpdateRange (list);
            return await _context.SaveChangesAsync ();
        }
    }
}