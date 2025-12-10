namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Wrappers.Configs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Suspect.TaskPro.AppCore.Domain.WrapperAggregate.Entities;

public class WrapperConfig : IEntityTypeConfiguration<Wrapper>
{
    public void Configure(EntityTypeBuilder<Wrapper> builder)
    {
        builder.ToTable("Wrappers");
    }
}