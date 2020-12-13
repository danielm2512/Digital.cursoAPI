using Digital.curso.api.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digital.curso.api.Infraestruture.Data.Mappings
{
    public class UusarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("TB_USUARIO");
            builder.HasKey(p => p.Codigo);
            builder.Property(p => p.Codigo).ValueGeneratedOnAdd();
            builder.Property(p => p.Login);
            builder.Property(p => p.Senha);
            builder.Property(p => p.Email);
        }
    }
}
