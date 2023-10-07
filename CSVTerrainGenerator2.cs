using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVTerrainGenerator : MonoBehaviour
{
    // Define a variable to store the CSV file paths for latitude, longitude, height, and slope data.
    public string latitudecsvFilePath = "Assets/FY23_ADC_Latitude_Haworth.csv";
    public string longitudecsvFilePath = "Assets/FY23_ADC_Longitude_Haworth.csv";
    public string heightcsvFilePath = "Assets/FY23_ADC_Height_Haworth.csv";
    public string slopecsvFilePath = "Assets/FY23_ADC_Slope_Haworth.csv";

    // Define variable for terrain size and scaling
    public float unityMinX = -100f;
    public float unityMaxX = 100f;
    public float unityMinY = 0f;
    public float unityMaxY = 50f;

    //Terrain reference.
    Terrain terrain;

    void Start()
    {
        // Check if the CSV files exist.
        if (File.Exists(latitudecsvFilePath)) && File.Exists(longitudecsvFilePath) && File.Exists(heightcsvFilePath) && File.Exists(slopecsvFilePath));
        {
            // Read latitude, longitude, height and slope data from CSV files
            string[] latitudeLines = File.ReadAllLines(latitudecsvFilePath);
            string[] longitudeLines = File.ReadAllLines(longitudecsvFilePath);
            string[] heightLines = File.ReadAllLines(heightcsvFilePath);
            string[] slopeLines = File.ReadAllLines(slopecsvFilePath);

            //Check if the number of rows in the CSV files matches.
            int rowCount = latitudeLines.Length;
            if (rowCount !=longitudeLines.Length || rowCount != heightLines.Length || rowCount !=slopeLines.Length)

            {
                Debug.LogError("CSV files have different row counts.");
                return;
            }

            //Get a reference to the terrain component.
            terrain = GetComponent<Terrain>();

            // Create a terrain data object to store terrain properties.
            TerrainData terrainData = new TerrainData();
            terrain.terrainData = terrainData;

            // Set terrain size.
            terrainData.size = new Vector3(unityMaxX - unityMinX, unityMaxY - unityMinY, 1f);

            //Define terrain heightmap resolution.
            int heightmapResolution = 513; // Adjust as needed.

            // Set terrain heightmap resolution.
            terrainData.heightmapResolution = heightmapResolution;

            // Create a heightmap array to store elevation data.
            float[,] heightmapData = new float[heightmapResolution, heightmapResolution];

            //Iterate through the rows of data.
            for (int i = 0; i < rowCount; i++)

            {
                // Parse latitude, longitude, height, and slope values.
                float latitude = float.Parse(latitudeLines[i]);
                float longitude = float.Parse(longitudeLines[i]);
                float height = float.Parse(heightLines[i]);
                float slope = float.Parse(slopeLines[i]);

                // Convert latitude and longitude to Unity coordinates.
                float unityX = Mathf.Lerp(unityMinX, unityMaxX, (longitude - minLongitude) / (maxLongitude - minLongitude));
                float unityY = Mathf.Lerp(unityMinY, unityMaxY, (longitude - minLatitude) / (maxLatitude - minLatitude));

                // Calculate normalized terrain coordinates.
                float normalizedX = (unityX - unityMinX) / (unityMaxX - unityMinX);
                float normalizedY = (unityY - unityMinY) / (unityMaxY - unityMinY);

                // Calculate heightmap position
                int xIndex = Mathf.FloorToInt(normalizedX * (heightmapResolution - 1));
                int yIndex = Mathf.FloorToInt(normalizedY * (heightmapResolution - 1));

                // Set height and slope values in the heightmap.
                heightmapData[xIndex, yIndex] = height;
                // You can also use the slope value to adjust terrain properties here.
            }

            // Apply the modified heightmap data to the terrain.
            terrainDdata.SetHeights(0, 0, heightmapData);

        }
        else
        {
            Debug.LogError("One or more CSV file not found.");
        }
    }
}
