using System;
using FluentNHibernate;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Conventions
{
    public class ForeignKeyConvention : FluentNHibernate.Conventions.ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            if (property == null)
                return type.Name + "Fk";

            return property.Name + "Fk";
        }
    }
}
