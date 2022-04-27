using System;
using System.Collections.Generic;
using System.Data;

namespace DapperGenericRepository.Helpers
{
    public static class BulkInsertHelpers
    {
        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            var type = typeof(T);
            var properties = type.GetProperties();

            var dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;

            foreach (var info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name,
                    Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (var entity in list)
            {
                var values = new object?[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}