########################################################################################################
# Description:
# - this script will convert the raw data (point cloud) from ADC challenge into ONE single CSV file
# - the converted point cloud will also be in cartesian format (rather than spherical)
# - this CSV file can be downsampled and converted to a mesh (.obj) with the CloudCompare tool
# - once converted to a mesh, it can be imported and viewed inside Unity
# - CloudCompare - Convert to mesh steps:
#       1.) Open haworth-xyz.csv and "Yes to all"
#       2.) Edit > Edit global shift and scale
#       3.) Change X, Y, and Z Shift to 0 - Then Click on Yes
#       4.) Edit > Subsample
#       5.) Change "Min Space Between Points" to 8~18 (larger value = higher compression) - Click OK
#       6.) Select the new downsampled mesh
#       7.) Edit > Mesh > Delaunay 2.5 (best fitting plane) - Click OK
#       8.) Select newly created mesh and Save it as an .obj file
########################################################################################################
import csv
import pdb
import math

# Formula from ADC 2024 - Appendix C - Spherical to Cartesian Conversion
def convert_vertices_to_cartesian(longitude, latitude, height):
    L_RADIUS = 1737400
    radius = L_RADIUS + height
    x = radius * math.cos(math.radians(latitude)) * math.cos(math.radians(longitude))
    y = radius * math.cos(math.radians(latitude)) * math.sin(math.radians(longitude))
    z = radius * math.sin(math.radians(abs(latitude)))
    return x, y, z

def main():

    # Create a file to store point cloud in xyz format in one single CSV file
    file = open("haworth-xyz.csv", "w")

    print("Importing CSVs!\n")
    # NOTE: If you are using this script, make sure the path below to the raw data csv files are correct
    # Read raw data and put into 2D array
    with open("./FY23_ADC_Longitude_Haworth.csv", 'r') as file_longitude:
        data_longitude = list(csv.reader(file_longitude))

    with open("./FY23_ADC_Latitude_Haworth.csv", 'r') as file_latitude:
        data_latitude = list(csv.reader(file_latitude))

    with open("./FY23_ADC_Height_Haworth.csv", 'r') as file_height:
        data_height = list(csv.reader(file_height))

    i = 0
    j = 0

    print("Processing Data!\n")
    # Iterate through all "points" in the data
    # "i" and "j" will be my incrementing variables to traverse through the 2D array of raw data
    for row in data_longitude:
        for values in row:
            x, y, z = convert_vertices_to_cartesian(float(data_longitude[i][j]), float(data_latitude[i][j]), float(data_height[i][j]))

            # Create the string (will be used for each line) of how we want to write it to the CSV file
            # Format: <x coordinate>,<y coordinate>,<z coordinate>
            cartesian_str = str(x) + "," + str(y) + "," + str(z) + "\n"
            file.write(cartesian_str)

            j += 1
        j = 0
        i += 1

    # Close the file since we are done writing to it
    file.close()
    print("Processing Complete!\n")

if __name__ == "__main__":
    main()
