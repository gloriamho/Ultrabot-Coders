using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVTerrainGenerator : MonoBehaviour
{
    // Define a variable to store the CSV file path.
    public string latitudecsvFilePath = "Assets/FY23_ADC_Latitude_Haworth.csv";
    public string longitudecsvFilePath = "Assets/FY23_ADC_Longitude_Haworth.csv";

    // Define variable for terrain size and scaling
    public float unityMinX = -100f;
    public float unityMaxX = 100f;
    public float unityMinY = 0f;
    public float unityMaxY = 50f;

    //Terrain reference.
    Terrain terrain;
    private float minLongitude;
    private float maxLongitude;
    private float minLatitude;
    private float maxLatitude;

    void Start()
    {
        // Check if the CSV file exists.
        if (File.Exists(latitudecsvFilePath))
        {
            // Read latitude and longitude data from CSV files
            string[] latitudeLines = File.ReadAllLines(latitudecsvFilePath);
            string[] longitudeLines = File.ReadAllLines(longitudecsvFilePath);

            //Check if the number of rows in the CSV files matches.
            if (latitudeLines.Length != longitudeLines.Length)

            {
                Debug.LogError("Latitude and longitude data have different row counts.");
                return;
            }

            //Get a reference to the terrain component.
            terrain = GetComponent<Terrain>();

            //Iterate through the rows of data.
            for (int i = 0; i < latitudeLines.Length;i++)

            {
                // Parse latitude and longitude values.
                float latitude = float.Parse(latitudeLines[i]);
                float longitude = float.Parse(longitudeLines[i]);

                // Convert latitude and longitude to Unity coordinates.
                float unityX = Mathf.Lerp(unityMinX, unityMaxX, (longitude - minLongitude) / (maxLongitude - minLongitude));
                float unityY = Mathf.Lerp(unityMinY, unityMaxY, (longitude - minLatitude) / (maxLatitude - minLatitude));

                //TODO: Use unityX and unityY to generate your terrain mesh or perform other actions.
            }
        }
        else
        {
            Debug.LogError("Latitude or longitude CSV file not found.");
        }
    }
}
