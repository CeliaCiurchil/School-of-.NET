using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole> //configuratiosn for tis table
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d",
                    Name = "Staff",
                    NormalizedName = "STAFF",
                    ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"
                },
                new IdentityRole
                {
                    Id = "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e",
                    Name = "Client",
                    NormalizedName = "CLIENT",
                    ConcurrencyStamp = "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"
                }
            );
        }
    }
}
