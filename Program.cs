using System.ComponentModel.DataAnnotations;
using System.Globalization;

internal class Program
{
    private static void Main(string[] args)
    {
        var file = new FileStream("", FileMode.Open);

        var rows = new List<string>();
        var records = new List<ValidRecord>();

        var format = CultureInfo.GetCultureInfoByIetfLanguageTag("en-GB");

        try
        {
            using (FileStream stream = file)
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    string line = "";
                    string[] values = new string[0];
                    try
                    {
                        line = reader.ReadLine();
                        rows.Add(line);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Reading file or splitting line failed.");
                    }
                }

                rows = FilterRows(rows);

                foreach (var row in rows)
                {
                    var values = row.Split("|");
                    // convert to date format, remove extra quotes and assume we're looking at data from GB                    
                    ReadOnlySpan<char> dateSpan = values[13].AsSpan().Trim('"');
                    var dateConverted = DateTime.TryParseExact(dateSpan, "yyyy-MM-dd HH:mm:ss", format, DateTimeStyles.None, out DateTime date);

                    if (!dateConverted)
                    {
                        Console.WriteLine("Failed to convert date from field " + dateSpan.ToString());
                    }

                    records.Add(new ValidRecord
                    {
                        A = values[0],

                        L = dateConverted ? date : null
                    });
                }                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine("Done");
    }

    private List<string> FilterRows(List<string> rows)
    {
        List<string> filtered = new List<string>();        

        foreach (string row in rows)
        {
            var rowValid = true;

            var values = row.Split("|");

            // check we have the correct number of fields
            if (values.Length != 16)
            { 
                Console.WriteLine("Line has incorrect number of fields. Line data: " + row);
                rowValid = false;
            }

            // if it's an empty row we don't want it
            int emptyCount = 0;
            foreach (string value in values)
            {
                if (value == "")
                {
                    emptyCount++;
                }
            }
            if (emptyCount == values.Length)
            {
                rowValid = false;
            }

            if (rowValid) filtered.Add(row);
        }

        return filtered;
    }
}

public class ValidRecord
{
    public string A { get; set; }
    public string B { get; set; }
    public string C { get; set; }
    public string D { get; set; }
    public string E { get; set; }
    public string F { get; set; }
    public string G { get; set; }
    public string H { get; set; }
    public string I { get; set; }
    public string J { get; set; }        
    public string K { get; set; }    
    public DateTime? L { get; set; }    
    public DateTime? M { get; set; }        
    public DateTime? N { get; set; }
}
