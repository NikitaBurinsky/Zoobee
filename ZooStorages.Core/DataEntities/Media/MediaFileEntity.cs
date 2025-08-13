using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStorages.Domain.DataEntities.Media
{
	public enum MediaType { Image, Video, Document, Other , NONE}
	public class MediaFileEntity : BaseEntity
	{
		public Guid Id { get; set; }
		/// <summary>
		/// Уникальное имя файла, включая расширение (dweg.png). Выдается сервером
		/// </summary>
		public string FileName { get; set; }
		/// <summary>
		/// Локальный путь, включая имя.
		/// </summary>
		public string FileLocalURL { get; set; } 
		public MediaType Type { get; set; }
		public long Size { get; set; } //Размер файла в байтах
	
	}

	public class MediaFileEntityConfigurator : IEntityTypeConfiguration<MediaFileEntity>
	{
		public void Configure(EntityTypeBuilder<MediaFileEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.HasIndex(e => e.FileLocalURL).IsUnique(true);
		}
	}

}
