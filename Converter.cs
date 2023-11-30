using Microsoft.VisualBasic.FileIO;
using System.Text;

namespace ProgramHTML
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var inputPath = args[0];
            var outputPath = args[1];

            //Initializing csvParser
            TextFieldParser csvParser = new(inputPath);
            csvParser.CommentTokens = new string[] { "#" };
            csvParser.SetDelimiters(new string[] { "," });
            csvParser.HasFieldsEnclosedInQuotes = false;

            StringBuilder stringBuilder = new();
            StreamWriter streamWriter = new(outputPath);

            write_header(stringBuilder, streamWriter);
            write_column_names(csvParser, stringBuilder, streamWriter);
            write_rows(csvParser, stringBuilder, streamWriter);
            write_footer(stringBuilder, streamWriter);

            // End of writing table into string; disposing of the writer object
            streamWriter.Flush();
            streamWriter.Close();
            streamWriter.Dispose();
        }
        static void write_header(StringBuilder stringBuilder, StreamWriter streamWriter)
        {
            // Begin writing of table into string
            stringBuilder.Append("<center><table border=\"1\">");
            streamWriter.Write(stringBuilder);
            stringBuilder.Clear();
        }
        static void write_column_names(TextFieldParser csvParser, StringBuilder stringBuilder, StreamWriter streamWriter)
        {
            // Scan the first row for column names; set column number based on lenght of the first row.
            string[] columnNames = csvParser.ReadFields() ?? Array.Empty<string>();
            if (columnNames.Length == 0) return;
            // Write row with column names into string
            stringBuilder.Append("<tr>");
            for (int i = 0; i < columnNames.Length; i++)
            {
                stringBuilder.Append($"<th>{columnNames[i]}</th>");
            }
            stringBuilder.Append("</tr>");
            streamWriter.Write(stringBuilder);
            stringBuilder.Clear();
        }
        static void write_rows(TextFieldParser csvParser, StringBuilder stringBuilder, StreamWriter streamWriter)
        {
            // Loop for reading fields row by row
            int rowIndex = 0;
            List<string[]> rows = new();
            while (!csvParser.EndOfData)
            {
                string[] currentRow = csvParser.ReadFields() ?? Array.Empty<string>();
                rows.Add(currentRow);
                if (rowIndex % 2 == 0)
                    stringBuilder.Append("<tr bgcolor= #D3D3D3>");
                else
                    stringBuilder.Append("<tr>");
                // Loop for appending rows of table into string
                for (int i = 0; i < currentRow.Length; i++)
                {
                    stringBuilder.Append($"<td>{rows[rowIndex][i]}</td>");
                }
                stringBuilder.Append("</tr>");
                streamWriter.Write(stringBuilder);
                stringBuilder.Clear();
                rowIndex++;
            }
            streamWriter.Write(stringBuilder);
            stringBuilder.Clear();
        }
        static void write_footer(StringBuilder stringBuilder, StreamWriter streamWriter)
        {
            stringBuilder.Append("</table></center>");
            streamWriter.Write(stringBuilder);
            stringBuilder.Clear();
        }
    }
}