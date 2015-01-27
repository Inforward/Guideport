using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using Portal.Model.Report;

namespace Portal.Data.Sql.EntityFramework.Report
{
    public class ReportRepository : EntityRepository<MasterContext>, IReportRepository
    {
        public ExecuteResult Execute(View view, Pager pager = null)
        {
            var result = new ExecuteResult();

            var cmd = new SqlCommand()
            {
                CommandText = view.StoredProcedureName,
                CommandTimeout = 1800,
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddRange(AddParameters(view, pager));

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString))
            {
                conn.Open();

                cmd.Connection = conn;

                using (var dr = cmd.ExecuteReader())
                {
                    result.Columns = GetReaderSchema(dr);

                    while (dr.Read())
                    {
                        result.Rows.Add(GetRow(dr, result.Columns));
                    }

                    dr.Close();
                }

                if (cmd.Parameters["@totalRowCount"].Value != null && cmd.Parameters["@totalRowCount"].Value != DBNull.Value)
                    result.TotalRowCount = (int)cmd.Parameters["@totalRowCount"].Value;
            }

            return result;            
        }

        private static SqlParameter[] AddParameters(View view, Pager pager)
        {
            var parms = new List<SqlParameter>();

            parms.Add(new SqlParameter { ParameterName = "@totalRowCount", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            foreach (var filter in view.Filters.Where(f => f.HasValidValue()))
            {
                parms.Add(new SqlParameter() { ParameterName = filter.ParameterName, Value = filter.Value ?? DBNull.Value });
            }

            if (pager != null)
            {
                if (pager.PageSize > 0)
                    parms.Add(new SqlParameter { ParameterName = "@pageSize", SqlDbType = SqlDbType.Int, Value = pager.PageSize });

                if (pager.Page > 0)
                    parms.Add(new SqlParameter { ParameterName = "@pageNumber", SqlDbType = SqlDbType.Int, Value = pager.Page });

                if (!pager.Sort.IsNullOrEmpty())
                {
                    parms.Add(new SqlParameter { ParameterName = "@sortColumnList", SqlDbType = SqlDbType.VarChar, Value = pager.Sort.Select(s => s.Field).ToCsv() });
                    parms.Add(new SqlParameter { ParameterName = "@sortDirectionList", SqlDbType = SqlDbType.VarChar, Value = pager.Sort.Select(s => s.Dir).ToCsv() });
                }
            }

            return parms.ToArray();
        }

        private static List<ColumnMetadata> GetReaderSchema(IDataReader dr)
        {
            return dr.GetSchemaTable().AsEnumerable().Select(row => new ColumnMetadata()
                                                            {
                                                                DataField = row["ColumnName"].ToString(), 
                                                                DataType = row["DataType"].ToString()
                                                            }).ToList();
        }

        private static object[] GetRow(IDataRecord dr, IReadOnlyList<ColumnMetadata> columns)
        {
            var row = new object[columns.Count];

            for (var i = 0; i < columns.Count; i++)
            {
                row[i] = (dr[columns[i].DataField] == DBNull.Value ? null : dr[columns[i].DataField]);
            }

            return row;
        }
    }
}
