using System.Globalization;

internal class Program
{
    private static void Main(string[] args)
    {
        var file = new FileStream("", FileMode.Open);

        var records = new List<ValidRecord>();

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
                        values = line.Split('|');
                    } 
                    catch (Exception e)
                    {
                        throw new Exception("Reading file or splitting line failed.");
                    }

                    // if it's an empty row we don't want it
                    if (values.All(v => v == "")) { continue; }

                    // check we have the correct number of fields
                    if (values.Count() != 16)
                    {
                        Console.WriteLine("Line has incorrect number of fields. Line data: " + line);
                        continue;
                    }

                    // convert to date format, remove extra quotes and assume we're looking at data from GB
                    var dateToConvert = values[13].Replace("\"", "");
                    DateTime? date = null;
                    try
                    {                                               
                        date = DateTime.ParseExact(dateToConvert, "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfoByIetfLanguageTag("en-GB"));
                    } 
                    catch (Exception e) 
                    {
                        Console.WriteLine("Failed to convert date from field " + dateToConvert);
                    }                    

                    records.Add(new ValidRecord
                    {
                        A = values[0],

                        L = date
                    });
                }
            }

            Console.WriteLine("Done");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
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
