﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaBase.DAL.Entity
{
    public class DeviceAlarmEntityConfig : IEntityTypeConfiguration<DeviceAlarmEntity>
    {
        public void Configure(EntityTypeBuilder<DeviceAlarmEntity> builder)
        {
            builder.ToTable("tbDeviceAlarm");
            builder.Property(r => r.DeviceName).HasMaxLength(50).IsRequired(false);
            builder.Property(r => r.AlarmTime);

            builder.Property(r => r.AlarmInfo).HasMaxLength(100).IsRequired(false);


            builder.Property(r => r.AlarmType).HasMaxLength(20).IsRequired(false);
            builder.Property(r => r.AlarmDateStr).HasMaxLength(20).IsRequired(false);


        }
    }
}
