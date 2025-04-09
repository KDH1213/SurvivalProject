using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;

public abstract class DataTable
{
    // 경로를 아이디로 하여 리소스를 로드하여 실행,
    // 정보를 가지고 있는 테이블을 통해 로딩
    public static readonly string FormatPath = "Tables/{0}";
    public abstract void Load(string path);
    public static List<T> LoadCSV<T>(string csv)
    {
        using (var reader = new StringReader(csv))
        {
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csvReader.GetRecords<T>().ToList<T>();
            }
        }

    }
}
