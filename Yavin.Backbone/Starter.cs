using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yavin.Meta.Device;
using Yavin.Model.Device;
using Yavin.Model.Logs;

namespace Yavin.Backbone
{
	/// <summary>
	/// 程序启动初始化工作封装
	/// </summary>
	public sealed class Starter
	{
		/// <summary>
		/// AutoMapper组件初始化器
		/// </summary>
		public static void AutoMapperInit()
		{
			//屏幕数据映射
			Mapper.CreateMap<ScreenMeta, Screen>()
				.ForMember(screen => screen.Address, meta => meta.MapFrom(p => new Address()
				{
					Code = p.LocationCode,
					Description = p.Address,
					Name = p.LocationName,
					Point = new Point()
					{
						CoordinateType = p.CoordinateType,
						Geohash = p.Geohash,
						Latitude = p.Latitude,
						Longitude = p.Longitude
					}
				}))
				.ForMember(screen => screen.Code, meta => meta.MapFrom(p => p.Code))
				.ForMember(screen => screen.Enabled, meta => meta.MapFrom(p => p.Enabled))
				.ForMember(screen => screen.Height, meta => meta.MapFrom(p => p.Height))
				.ForMember(screen => screen.Id, meta => meta.MapFrom(p => p.Id))
				.ForMember(screen => screen.Status, meta => meta.MapFrom(p => p.Status))
				.ForMember(screen => screen.Width, meta => meta.MapFrom(p => p.Width));
			//设备日志数据映射
			Mapper.CreateMap<DeviceLogMeta, DeviceLog>()
				.ForMember(log => log.CreateTime, meta => meta.MapFrom(p => p.CreateTime))
				.ForMember(log => log.Description, meta => meta.MapFrom(p => p.Content))
				.ForMember(log => log.Device, meta => meta.MapFrom(p => new Yavin.Model.Logs.Device()
				{
					Id = p.DeviceId,
					Type = p.DeviceType
				}))
				.ForMember(log => log.Enabled, meta => meta.MapFrom(p => p.Enabled))
				.ForMember(log => log.Id, meta => meta.MapFrom(p => p.Id))
				.ForMember(log => log.Name, meta => meta.MapFrom(p => p.Name))
				.ForMember(log => log.Status, meta => meta.MapFrom(p => p.Status))
				.ForMember(log => log.Type, meta => meta.MapFrom(p => p.Type));
		}
	}
}
