using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Yavin.Core;
using Yavin.Core.GPS;
using Yavin.Meta.Device;
using Yavin.Model.Common;
using Yavin.Model.Device;
using Yavin.ORM;

namespace Yavin.Backbone.Device
{
	/// <summary>
	/// 显示屏基础数据服务接口实现
	/// </summary>
	public class ScreenServiceProvider : IScreenService
	{
		#region 字段
		private readonly IDataAccess<ScreenMeta> _screenData;
		#endregion

		public ScreenServiceProvider(IDataAccess<ScreenMeta> screenData)
		{
			this._screenData = screenData;
		}

		#region protected方法
		/// <summary>
		/// 转换器
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		protected ScreenMeta Converter(Screen screen, ScreenMeta meta = null)
		{
			if (meta == null)
			{
				meta = new ScreenMeta();
				meta.CreateTime = DateTime.Now;
			}
			meta.Address = screen.Address != null ? screen.Address.Description : string.Empty;
			meta.Code = screen.Code;
			meta.CoordinateType = screen.Address != null && screen.Address.Point != null ? screen.Address.Point.CoordinateType : string.Empty;
			meta.Enabled = screen.Enabled;
			meta.Geohash = screen.Address != null && screen.Address.Point != null ? screen.Address.Point.Geohash : string.Empty;
			meta.Height = screen.Height;
			meta.Latitude = screen.Address != null && screen.Address.Point != null ? screen.Address.Point.Latitude : 0;
			meta.LocationCode = screen.Address != null ? screen.Address.Code : string.Empty;
			meta.LocationName = screen.Address != null ? screen.Address.Name : string.Empty;
			meta.Longitude = screen.Address != null && screen.Address.Point != null ? screen.Address.Point.Longitude : 0;
			meta.Status = screen.Status;
			meta.UpdateTime = DateTime.Now;
			meta.Width = screen.Width;
			return meta;
		}
		
		/// <summary>
		/// 根据搜索条件生成查询
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		protected IQueryable<ScreenMeta> GetQuery(Search search)
		{
			var query = this._screenData.Table;
			//主键条件
			if (search.Ids != null && search.Ids.Length > 0)
				query = query.Where(s => search.Ids.Contains(s.Id));
			//编号条件
			if (search.Codes != null && search.Codes.Length > 0)
			{
				var selector = new List<string>();
				var arr = new object[search.Codes.Length];
				for (var i = 0; i < search.Codes.Length; i++)
				{
					selector.Add(string.Format("Code=@{0}", i));
					arr[i] = search.Codes[i];
				}
				query = query.Where(string.Join(" OR ", selector.ToArray()), arr);
			}
			//状态条件
			if (search.Status != null && search.Status.Length > 0)
			{
				var selector = new List<string>();
				var arr = new object[search.Status.Length];
				for (var i = 0; i < search.Status.Length; i++)
				{
					selector.Add(string.Format("Status=@{0}", i));
					arr[i] = search.Status[i];
				}
				query = query.Where(string.Join(" OR ", selector.ToArray()), arr);
			}
			//是否可用条件
			if (search.Enabled.HasValue)
				query = query.Where(s => s.Enabled == search.Enabled.Value);
			return query;
		}
		#endregion

		public virtual void Insert(Screen screen)
		{
			if (screen == null)
				throw new ArgumentNullException("screen");
			var meta = this.Converter(screen);
			this._screenData.Insert(meta);
			screen.Id = meta.Id;
		}

		public virtual void Update(long id, Screen screen)
		{
			if (screen == null)
				throw new ArgumentNullException("screen");
			var meta = this._screenData.GetByID(id);
			if (meta == null)
				throw new BaseException("没有这个屏幕数据");
			meta=this.Converter(screen,meta);
			this._screenData.Update(meta);
		}

		public virtual Screen Select(long id)
		{
			var meta = this._screenData.GetByID(id);
			if (meta == null)
				return null;
			var screen = Mapper.Map<ScreenMeta, Screen>(meta);
			return screen;
		}

		public virtual Screen Select(string code)
		{
			var meta = this._screenData.Table.FirstOrDefault(s => s.Code == code);
			if (meta == null)
				return null;
			var screen = Mapper.Map<ScreenMeta, Screen>(meta);
			return screen;
		}

		public virtual int Count(Search search)
		{
			if (search == null)
				throw new ArgumentNullException("search");
			var query = this.GetQuery(search);
			return query.Count();
		}

		public virtual Paging<Screen> Select(Search search, int page, int size)
		{
			if (search == null)
				throw new ArgumentNullException("search");
			var query = this.GetQuery(search);
			query = query.OrderByDescending(s => s.CreateTime);
			var metas = new Paging<ScreenMeta>(query, page, size);
			var screens = Mapper.Map<ScreenMeta[], Screen[]>(metas.Data);
			return new Paging<Screen>(screens, metas.PageCount, metas.TotalCount);
		}

		public virtual Screen[] Select(Point center, float radius)
		{
			if (center == null)
				throw new ArgumentNullException("center");
			if (radius <= 0)
				throw new ArgumentOutOfRangeException("radius");
			var length = Geohash.GetQueryLength(radius);
			var geohash = Geohash.Encode(center.Latitude, center.Longitude).Substring(0, length);
			var nearby = Geohash.GetNearbyRange(geohash);
			var selector = new List<string>();
			var arr = new object[nearby.Length];
			for (var i = 0; i < nearby.Length; i++)
			{
				selector.Add(string.Format("Geohash.StartsWith(@{0})", i));
				arr[i] = nearby[i];
			}
			var query = this._screenData.Table.Where(string.Join(" OR ", selector.ToArray()), arr).ToList();
			query = query.FindAll(x => Calculator.GetDistance(center.Latitude, center.Longitude, x.Latitude, x.Longitude) <= radius);
			var screens = Mapper.Map<ScreenMeta[], Screen[]>(query.ToArray());
			return screens;
		}
	}
}
