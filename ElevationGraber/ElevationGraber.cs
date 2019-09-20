using ElevationGraber.Extensions;
using ElevationGraber.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ElevationGraber
{
    /// <summary>
    /// Утилита сбора высот точек посредством сервиса jawg.io
    /// https://www.jawg.io/docs/apidocs/elevation/#request-elevations-for-a-set-of-locations
    /// </summary>
    public class ElevationGraber
    {
        //https://www.jawg.io/lab/access-tokens
        private readonly string accessToken = "uVTXIlrjB0B6a0FJnlFIPODzaIm9t2bTSdISI1hUU3KiXklhh5nUeF3JAbNSmEGm";

        private string URI => "https://api.jawg.io/elevations/path";

        private string RequestUri => $"{URI}?access-token={accessToken}";

        private GeoPoint TopLeft { get; set; }
        private GeoPoint BottomRight { get; set; }

        private int SamplesCount { get; set; }

        private double GeoPointStep => 0.001;
        private JsonSerializerSettings jsonConverterSettings { get; set; }

        public ElevationGraber(GeoPoint topLeft, GeoPoint bottomRight, int stepCount = 10000, int samples = 1024)
        {
            SamplesCount = samples;
            TopLeft = topLeft;
            BottomRight = bottomRight;

            jsonConverterSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
            };
        }

        public async Task StartGrab()
        {
            List<List<AltitudeResponse>> data = await GrabAltitude();
            await WriteAltitudeData(data);
        }

        private async Task<List<AltitudeResponse>> SendRequest(AltitudeRequest request)
        {
            HttpClient httpClient = new HttpClient();

            string json = JsonConvert.SerializeObject(request, jsonConverterSettings);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage htppResponse = await httpClient.PostAsync(RequestUri, httpContent);

            List<AltitudeResponse> altitudeResponse = null;
            if (htppResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawData = await htppResponse.Content.ReadAsStringAsync();
                altitudeResponse = JsonConvert.DeserializeObject<List<AltitudeResponse>>(rawData);
            }

            return altitudeResponse;
        }

        private AltitudeRequest MakeRequest(GeoPoint startPoint, GeoPoint endPoint)
        {
            return new AltitudeRequest
            {
                Path = string.Join(" | ", startPoint, endPoint),
                Samples = SamplesCount
            };
        }

        private async Task<List<List<AltitudeResponse>>> GrabAltitude()
        {
            List<List<AltitudeResponse>> data = new List<List<AltitudeResponse>>();

            int stepsCount = (int)Math.Ceiling(Math.Abs(TopLeft.Lng - BottomRight.Lng) / GeoPointStep);
            List<Task> tasks = new List<Task>();

            for (var i = 0; i < stepsCount; i++)
            {
                GeoPoint startPoint = new GeoPoint { Lat = TopLeft.Lat, Lng = TopLeft.Lng + i * GeoPointStep };
                GeoPoint endPoint = new GeoPoint { Lat = BottomRight.Lat, Lng = TopLeft.Lng + i * GeoPointStep };

                AltitudeRequest request = MakeRequest(startPoint, endPoint);
                List<AltitudeResponse> response = await SendRequest(request);
                data.Add(response);
            }
            return data;
        }

        public void JsonToHeatMap(string path = "AltitudeData.json")
        {
            string json = File.ReadAllText(path);
            List<List<AltitudeResponse>> data = JsonConvert.DeserializeObject<List<List<AltitudeResponse>>>(json, jsonConverterSettings);
            MapVisualizer mapVisualizer = new MapVisualizer();

            mapVisualizer.MakeImage(data.Where(l => l != null).ToList());
        }

        private async Task WriteAltitudeData(List<List<AltitudeResponse>> data, string path = "AltitudeData.json")
        {
            string json = JsonConvert.SerializeObject(data, jsonConverterSettings);
            await File.WriteAllTextAsync(path, json);
        }
    }
}
