using System;
using System.IO;
using System.Text;
using System.Threading;
using Yavin.Core.Infrastructure;

namespace Yavin.Core.File
{
	/// <summary>
	/// 日志文件操作类
	/// </summary>
	public class LogFile : Singleton<LogFile>
	{
		#region 字段
		/// <summary>
		/// 文件目录
		/// </summary>
		protected string _filePath;

		/// <summary>
		/// 文件名称基准
		/// </summary>
		protected DateTime _fileSeed;

		/// <summary>
		/// 线程锁定对象
		/// </summary>
		private ReaderWriterLockSlim _syncLock = new ReaderWriterLockSlim();
		#endregion

		#region 构造
		/// <summary>
		/// 以默认文件名与默认路径创建实例
		/// </summary>
		public LogFile() : this(null, null) { }

		/// <summary>
		/// 以指定的文件名创建实例，文件将默认保存在应用程序根目录
		/// </summary>
		/// <param name="fileName"></param>
		public LogFile(string fileName) : this(fileName, null) { }

		/// <summary>
		/// 以指定的文件名与目录创建实例
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="filePath"></param>
		public LogFile(string fileName, string filePath)
		{
			this._fileSeed = DateTime.Today;
			if (string.IsNullOrEmpty(filePath))
			{
				var path = AppDomain.CurrentDomain.BaseDirectory;
				path = path.Remove(path.Length - 1);
				this._filePath = path;
			}
			else
			{
				this._filePath = filePath;
			}
		}
		#endregion

		/// <summary>
		/// 写入内容
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public bool Write(string content)
		{
			if (DateTime.Today > this._fileSeed)
				this._fileSeed = DateTime.Today;
			var fileName = string.Format(@"{0}\LogFile_{1}.log", this._filePath, this._fileSeed.ToString("yyyyMMdd"));
			var file = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
			var sb = new StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.mmm"));
			sb.Append(Environment.NewLine);
			sb.Append("----------------------------------------------------------------");
			sb.Append(Environment.NewLine);
			sb.Append(content);
			sb.Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			var data = Encoding.UTF8.GetBytes(sb.ToString());
			file.Position = file.Length;
			file.Write(data, 0, data.Length);
			file.Close();
			return true;
		}
	}
}
