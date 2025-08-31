using ATB.Data.Extensions;
using ATB.Data.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Xml.Schema;

namespace AirportTicketBookingExercise.App.Utils
{
    public class CsvActionsHelper
    {
        public static List<T> GetAllRecords<T, TMap>(string csvPath)
            where TMap : ClassMap<T>
        {
            List<T> recordList = new List<T>();
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TMap>();
                var records = csv.GetRecords<T>();
                if (records.Count() > 0)
                    recordList = records.ToList();


            }
            return recordList;
        }

        public static void CreateRecord<T, TMap>(string csvPath, T record)
            where TMap : ClassMap<T>
        {
            using (var writer = new StreamWriter(csvPath, append: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TMap>();
                csv.WriteRecord<T>(record);
                csv.NextRecord();
            }
        }

        public static void CreateRecords<T, TMap>(string csvPath, List<T> records)
            where TMap : ClassMap<T>
        {
            using (var writer = new StreamWriter(csvPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TMap>();
                csv.WriteRecords(records);
            }
        }

        public static bool UpdateRecords<T, TMap>(string csvPath, List<T> records)
            where TMap : ClassMap<T>
        {
            using (var writer = new StreamWriter(csvPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TMap>();
                csv.WriteRecords(records);
                csv.NextRecord();
            }
            return true;


        }
    }
}
