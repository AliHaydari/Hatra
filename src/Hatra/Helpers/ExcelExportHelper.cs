using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Hatra.Helpers
{
    public class ExcelExportHelper
    {
        public static string ExcelContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public static DataTable ToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                //table.Columns.Add(prop.Name, prop.PropertyType);
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType); // to avoid nullable types
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }

                table.Rows.Add(values);
            }

            return table;
        }

        public static byte[] ExportExcel(DataTable dt, string Heading = "", params string[] IgnoredColumns)
        {
            byte[] result = null;
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Exported Data");
                int StartFromRow = String.IsNullOrEmpty(Heading) ? 1 : 3;

                // add the content into the Excel file
                ws.Cells["A" + StartFromRow].LoadFromDataTable(dt, true);

                // autofit width of cells with small content
                //int colindex = 1;
                //foreach (DataColumn col in dt.Columns)
                //{
                //    ExcelRange columnCells = ws.Cells[ws.Dimension.Start.Row, colindex, ws.Dimension.End.Row, colindex];
                //    int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                //    if (maxLength < 150)
                //        ws.Column(colindex).AutoFit();

                //    colindex++;
                //}

                // format header - bold, yellow on black
                //using (ExcelRange r = ws.Cells[StartFromRow, 1, StartFromRow, dt.Columns.Count])
                //{
                //    r.Style.Font.Color.SetColor(System.Drawing.Color.Yellow);
                //    r.Style.Font.Bold = true;
                //    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                //}

                // format cells - add borders
                //using (ExcelRange r = ws.Cells[StartFromRow + 1, 1, StartFromRow + dt.Rows.Count, dt.Columns.Count])
                //{
                //    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                //    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                //    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                //    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                //    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                //}

                // removed ignored columns
                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (IgnoredColumns.Contains(dt.Columns[i].ColumnName))
                    {
                        ws.DeleteColumn(i + 1);
                    }
                }

                // add header and an additional column (left) and row (top)
                //if (!String.IsNullOrEmpty(Heading))
                //{
                //    ws.Cells["A1"].Value = Heading;
                //    ws.Cells["A1"].Style.Font.Size = 20;

                //    ws.InsertColumn(1, 1);
                //    ws.InsertRow(1, 1);
                //    ws.Column(1).Width = 5;
                //}

                result = pck.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcel<T>(List<T> data, string Heading = "", params string[] IgnoredColumns)
        {
            return ExportExcel(ToDataTable<T>(data), Heading, IgnoredColumns);
        }


        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    //var type = pro.PropertyType;

                    //object convertedValue;

                    //try
                    //{
                    //    convertedValue = Convert.ChangeType(dr[column.ColumnName], pro.PropertyType);
                    //}
                    //catch (Exception e)
                    //{
                    //    continue;
                    //}

                    //

                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            //var convertedValue = Convert.ChangeType(dr[column.ColumnName], pro.PropertyType);
                            var convertedValue = ChangeType(dr[column.ColumnName], pro.PropertyType);

                            pro.SetValue(obj, convertedValue, null);
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        private static T ChangeType<T>(object value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }

        private static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (string.IsNullOrEmpty(value?.ToString()))
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }
            else if (t.IsEnum)
            {
                var enumIntValue = Convert.ToInt32(value);
                return Enum.Parse(t, enumIntValue.ToString());
            }

            return Convert.ChangeType(value, t);
        }
    }
}
