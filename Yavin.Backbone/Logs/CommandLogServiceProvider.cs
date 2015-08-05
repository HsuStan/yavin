using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yavin.Meta.Logs;
using Yavin.Model.Common;
using Yavin.Model.Logs;
using Yavin.ORM;
using System.Linq.Dynamic;
using Yavin.Core;
using AutoMapper;

namespace Yavin.Backbone.Logs
{
	/// <summary>
	/// 指令基础数据服务接口实现
	/// </summary>
	public class CommandLogServiceProvider : ICommandLogService
	{
		#region 字段
		private readonly IDataAccess<CommandLogMeta> _logData;
		#endregion

		public CommandLogServiceProvider(IDataAccess<CommandLogMeta> logData)
		{
			this._logData = logData;
		}

		#region protected方法
		/// <summary>
		/// 转换器
		/// </summary>
		/// <param name="log"></param>
		/// <param name="meta"></param>
		/// <returns></returns>
		protected CommandLogMeta Converter(CommandLog log, CommandLogMeta meta = null)
		{
			if (meta == null)
			{
				meta = new CommandLogMeta();
				meta.CreateTime = DateTime.Now;
			}
			meta.CreatorId = log.Creator != null ? log.Creator.Id : string.Empty;
			meta.CreatorJson = JsonConvert.SerializeObject(log.Creator);
			meta.CreatorType = log.Creator != null ? log.Creator.Type : string.Empty;
			meta.DeviceId = log.Device != null ? log.Device.Id : string.Empty;
			meta.DeviceJson = JsonConvert.SerializeObject(log.Device);
			meta.DeviceType = log.Device != null ? log.Device.Type : string.Empty;
			meta.Enabled = log.Enabled;
			meta.Statement = log.Statement;
			meta.Status = log.Status;
			meta.UpdateTime = DateTime.Now;
			return meta;
		}

		/// <summary>
		/// 根据搜索条件取得查询
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		protected IQueryable<CommandLogMeta> GetQuery(Search search)
		{
			var query = this._logData.Table;
			//主键条件
			if (search.Ids != null && search.Ids.Length > 0)
				query = query.Where(l => search.Ids.Contains(l.Id));
			//时间段开始条件
			if (search.StartTime.HasValue)
				query = query.Where(l => l.CreateTime >= search.StartTime.Value);
			//时间段结束条件
			if (search.EndTime.HasValue)
				query = query.Where(l => l.CreateTime <= search.EndTime.Value);
			//是否可用条件
			if (search.Enabled.HasValue)
				query = query.Where(l => l.Enabled == search.Enabled.Value);
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
			//设备条件
			if (search.Devices != null && search.Devices.Length > 0)
			{
				var whereIndex = -1;
				var selector = new List<string>();
				var arr = new object[search.Devices.Length * 2];
				var arrIndex = -1;
				foreach (var device in search.Devices)
				{
					selector.Add(string.Format("(DeviceId=@{0} AND DeviceType=@{1})", ++whereIndex, ++whereIndex));
					arr[++arrIndex] = device.Id;
					arr[++arrIndex] = device.Type;
				}
				query = query.Where(string.Join(" OR ", selector.ToArray()), arr);
			}
			//创建者条件
			if (search.Users != null && search.Users.Length > 0)
			{
				var whereIndex = -1;
				var selector = new List<string>();
				var arr = new object[search.Users.Length * 2];
				var arrIndex = -1;
				foreach (var user in search.Users)
				{
					selector.Add(string.Format("(CreatorId=@{0} AND CreatorType=@{1})", ++whereIndex, ++whereIndex));
					arr[++arrIndex] = user.Id;
					arr[++arrIndex] = user.Type;
				}
				query = query.Where(string.Join(" OR ", selector.ToArray()), arr);
			}
			return query;
		}
		#endregion

		public virtual void Insert(CommandLog log)
		{
			if (log == null)
				throw new ArgumentNullException("log");
			var meta = this.Converter(log);
			this._logData.Insert(meta);
			log.Id = meta.Id;
		}

		public virtual void Update(long id, CommandLog log)
		{
			if (log == null)
				throw new ArgumentNullException("log");
			var meta = this._logData.GetByID(id);
			if (meta == null)
				throw new BaseException("没有这个指令数据");
			meta = this.Converter(log, meta);
			this._logData.Update(meta);
		}

		public virtual CommandLog Select(long id)
		{
			var meta = this._logData.GetByID(id);
			if (meta == null)
				return null;
			var log = Mapper.Map<CommandLogMeta, CommandLog>(meta);
			return log;
		}

		public virtual int Count(Search search)
		{
			if (search == null)
				throw new ArgumentNullException("search");
			var query = this.GetQuery(search);
			return query.Count();
		}

		public virtual Paging<CommandLog> Select(Search search, int page, int size)
		{
			if (search == null)
				throw new ArgumentNullException("search");
			var query = this.GetQuery(search);
			query = query.OrderByDescending(l => l.CreateTime);
			var metas = new Paging<CommandLogMeta>(query, page, size);
			var logs = Mapper.Map<CommandLogMeta[], CommandLog[]>(metas.Data);
			return new Paging<CommandLog>(logs, metas.PageCount, metas.TotalCount);
		}
	}
}
