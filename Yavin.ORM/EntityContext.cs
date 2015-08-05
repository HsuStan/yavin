using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace Yavin.ORM
{
	/// <summary>
	/// EF框架数据访问封装
	/// </summary>
	public class EntityContext : DbContext
	{
		private readonly string _assemblyName;

		public EntityContext()
			: base()
		{
			//需要应用程序提供EntityTypeConfiguration泛型所在的程序集全名，只能有一个
			this._assemblyName = ConfigurationManager.AppSettings["EntityTypeConfigurationAssemblyName"];
		}

		public EntityContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			this._assemblyName = ConfigurationManager.AppSettings["EntityTypeConfigurationAssemblyName"];
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			var typesToRegister = Assembly.Load(this._assemblyName).GetTypes()
			.Where(type => !String.IsNullOrEmpty(type.Namespace))
			.Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
			foreach (var type in typesToRegister)
			{
				dynamic configurationInstance = Activator.CreateInstance(type);
				modelBuilder.Configurations.Add(configurationInstance);
			}
			base.OnModelCreating(modelBuilder);
		}
	}
}
