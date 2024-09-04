using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Query_Parser
{
    public class QueryParser
    {
        private static readonly string ColumnPattern = @"\b\w+\b";
        private static readonly string WherePattern = @"WHERE\s+(\w+)\s*(=|<>|>|<|>=|<=)\s*(\'\d+\'|\'[^\']*\'|\d+)";

        public static string ValidateQuery(string query)
        {
            var trimmedQuery = query.Trim().TrimEnd(';');

          
            if (!trimmedQuery.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase) ||
                !trimmedQuery.Contains("FROM", StringComparison.OrdinalIgnoreCase))
            {
                return "Query must start with SELECT and include a FROM clause.";
            }

          
            var selectIndex = trimmedQuery.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) + "SELECT".Length;
            var fromIndex = trimmedQuery.IndexOf("FROM", StringComparison.OrdinalIgnoreCase);
            if (fromIndex == -1 || selectIndex >= fromIndex)
            {
                return "Query must include a FROM clause after the SELECT clause.";
            }
            var columnsPart = trimmedQuery.Substring(selectIndex, fromIndex - selectIndex).Trim();
            var columns = columnsPart.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (!ValidateColumns(columns))
            {
                return "Columns list is invalid.";
            }

          
            var whereIndex = trimmedQuery.IndexOf("WHERE", StringComparison.OrdinalIgnoreCase);
            if (whereIndex != -1)
            {
                var whereClause = trimmedQuery.Substring(whereIndex + "WHERE".Length).Trim();
                var error = ValidateWhereClause(whereClause, columns);
                if (error != null)
                {
                    return error;
                }
            }

            return "Query is valid.";
        }

        private static bool ValidateColumns(string[] columns)
        {
           
            foreach (var column in columns)
            {
                var trimmedColumn = column.Trim();
                if (string.IsNullOrEmpty(trimmedColumn) || !Regex.IsMatch(trimmedColumn, ColumnPattern))
                {
                    return false;
                }
            }
            return true;
        }

        private static string ValidateWhereClause(string whereClause, string[] columns)
        {
            var matches = Regex.Matches(whereClause, WherePattern, RegexOptions.IgnoreCase);
            if (matches.Count == 0)
            {
                return "Invalid WHERE clause syntax.";
            }

            foreach (Match match in matches)
            {
                var column = match.Groups[1].Value.Trim();
                var operatorValue = match.Groups[2].Value;
                var value = match.Groups[3].Value;

                if (Array.IndexOf(columns, column) == -1)
                {
                    return $"Invalid column name: {column}";
                }

                var colIndex = Array.IndexOf(columns, column);
                var colType = GetColumnType(colIndex);

                if (colType == "int" && value.StartsWith("'"))
                {
                    return $"Column {column} expects an integer, but got varchar.";
                }

                if (colType == "varchar" && !value.StartsWith("'"))
                {
                    return $"Column {column} expects varchar, but got integer.";
                }
            }
            return null;
        }

        private static string GetColumnType(int index)
        {
            return index % 2 == 0 ? "int" : "varchar";
        }
    }
}
