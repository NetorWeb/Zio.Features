using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zio.Features.Auditing.Contracts;
using Zio.Features.Core;
using Zio.Features.Data.Entities;
using Zio.Features.Core.Extensions;
using Zio.Features.Ddd.Domain.Entities;
using Zio.Features.EntityFrameworkCore.ObjectExtending;
using Zio.Features.EntityFrameworkCore.ValueComparers;
using Zio.Features.EntityFrameworkCore.ValueConverters;
using Zio.Features.ObjectExtending;
using Zio.Features.ObjectExtending.Data;
using Zio.Features.ObjectExtending.ObjectExtending;

namespace Zio.Features.EntityFrameworkCore.Modeling;

public static class ZioEntityTypeBuilderExtensions
{
    public static void ConfigureByConvention(this EntityTypeBuilder b)
    {
        b.TryConfigureConcurrencyStamp();
        b.TryConfigureExtraProperties();
        b.TryConfigureObjectExtensions();
        b.TryConfigureMayHaveCreator();
        b.TryConfigureMustHaveCreator();
        b.TryConfigureSoftDelete();
        b.TryConfigureDeletionTime();
        b.TryConfigureDeletionAudited();
        b.TryConfigureCreationTime();
        b.TryConfigureLastModificationTime();
        b.TryConfigureModificationAudited();
        b.TryConfigureMultiTenant();
    }
    
    public static void TryConfigureConcurrencyStamp(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasConcurrencyStamp>())
        {
            b.Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                .IsConcurrencyToken()
                .HasMaxLength(ConcurrencyStampConsts.MaxLength)
                .HasColumnName(nameof(IHasConcurrencyStamp.ConcurrencyStamp));
        }
    }
    
    public static void TryConfigureExtraProperties(this EntityTypeBuilder b)
    {
        if (!b.Metadata.ClrType.IsAssignableTo<IHasExtraProperties>())
        {
            return;
        }

        b.Property<ExtraPropertyDictionary>(nameof(IHasExtraProperties.ExtraProperties))
            .HasColumnName(nameof(IHasExtraProperties.ExtraProperties))
            .HasConversion(new ExtraPropertiesValueConverter(b.Metadata.ClrType))
            .Metadata.SetValueComparer(new ExtraPropertyDictionaryValueComparer());

        b.TryConfigureObjectExtensions();
    }
    
    public static void TryConfigureObjectExtensions(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasExtraProperties>())
        {
            ObjectExtensionManager.Instance.ConfigureEfCoreEntity(b);
        }
    }
    
    public static void TryConfigureMayHaveCreator(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IMayHaveCreator>())
        {
            b.Property(nameof(IMayHaveCreator.CreatorId))
                .IsRequired(false)
                .HasColumnName(nameof(IMayHaveCreator.CreatorId));
        }
    }
    
    public static void TryConfigureMustHaveCreator(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IMustHaveCreator>())
        {
            b.Property(nameof(IMustHaveCreator.CreatorId))
                .IsRequired()
                .HasColumnName(nameof(IMustHaveCreator.CreatorId));
        }
    }
    
    public static void TryConfigureSoftDelete(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<ISoftDelete>())
        {
            b.Property(nameof(ISoftDelete.IsDeleted))
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName(nameof(ISoftDelete.IsDeleted));
        }
    }

}