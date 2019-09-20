using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ElevationGraber
{
    public class TileMaker
    {
        private JsonSerializerSettings jsonConverterSettings { get; set; }

        public TileMaker()
        {
            jsonConverterSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
            };
        }
        private List<List<AltitudeResponse>> ReadDataFromJsonFile(string path = "AltitudeData.json")
        {
            string json = File.ReadAllText(path);
            List<List<AltitudeResponse>> data = JsonConvert.DeserializeObject<List<List<AltitudeResponse>>>(json, jsonConverterSettings);
        }
        public void MakeTileDataSet(int[] levelOfDetails)
        {
            
        }
        private void WriteTileSet(int levelOfDetail, List<List<AltitudeResponse>> altitudes)
        {

        }


    }
}
