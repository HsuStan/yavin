using System;
using System.Collections.Generic;
using System.Linq;

namespace Yavin.Model.Common
{
    /// <summary>
    /// 分页数据泛型容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Paging<T>
    {
        protected int _pageSize;
        protected int _totalCount;
        protected int _pageCount;
        protected List<T> _dataList;
        protected T[] _dataArray;

        public Paging(IQueryable<T> data, int page, int size)
        {
            this._totalCount = data.Count();
            this._pageSize = size;
            if (page < 1)
                page = 1;
            this._dataList = data.Skip((page - 1) * size).Take(size).ToList();
            this._pageCount = (int)Math.Ceiling(this._totalCount / (double)this._pageSize);
        }

        public Paging(IList<T> data, int page, int size)
        {
            this._totalCount = data.Count;
            this._pageSize = size;
            if (page < 1)
                page = 1;
            this._dataList = data.Skip((page - 1) * size).Take(size).ToList();
            this._pageCount = (int)Math.Ceiling(this._totalCount / (double)this._pageSize);
        }

        public Paging(IEnumerable<T> data, int page, int size)
        {
            this._totalCount = data.Count();
            this._pageSize = size;
            if (page < 1)
                page = 1;
            this._dataList = data.Skip((page - 1) * size).Take(size).ToList();
            this._pageCount = (int)Math.Ceiling(this._totalCount / (double)this._pageSize);
        }

        public Paging(T[] data, int pageCount, int totalCount)
        {
            this._dataArray = data;
            this._pageCount = pageCount;
            this._totalCount = totalCount;
        }

        /// <summary>
        /// 当前页数据
        /// </summary>
        public T[] Data
        {
            get { return this._dataArray != null ? this._dataArray : this._dataList.ToArray(); }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return this._pageCount; }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get { return this._totalCount; }
        }
    }
}
