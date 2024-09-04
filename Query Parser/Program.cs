using System;
using System.Collections.Generic;
using Query_Parser;

internal class Program
{
    private static void Main(string[] args)
    {
        var queries = new List<string>
{
    "SELECT Col1, Col2, Col3, Col4, Col5 FROM TABLE1 WHERE Col1 = '100';",
    "SELECT Col1, Col2, Col3, Col4, Col5 FROM TABLE1 WHERE Col1 >< '100';",
    "SELECT Col1, Col2 FROM TABLE1 WHERE Col1 >< '100';",
    "SELECT Col1, Col 2, Col3, Col4, Col5 FROM TABLE1 WEHERE Col2 = '100';",
    "SELECT Col1, Col2, Col3 WHERE Col1 = '100';",
    "SELECT Col1, Col2 FROM TABLE1 WHERE Col1 = '100;",
    "SELECT Col1, Col2, Col3, Col4, Col’5 FROM TABLE1 WEHERE Col1 >< '100';"
};


        foreach (var query in queries)
        {
            Console.WriteLine($"Query: {query}");
            var result = QueryParser.ValidateQuery(query);
            Console.WriteLine(result);
            Console.WriteLine();
        }
    }
}
