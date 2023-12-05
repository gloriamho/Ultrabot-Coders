#!/usr/bin/python

import csv
import pdb
import math

decimateFactor = 4

with open("./FY23_ADC_Latitude_Haworth.csv", 'r') as latFile:
        latData = list(csv.reader(latFile))

with open("./FY23_ADC_Longitude_Haworth.csv", 'r') as longFile:
        longData = list(csv.reader(longFile))

with open("./FY23_ADC_Height_Haworth.csv", 'r') as heightFile:
        heightData = list(csv.reader(heightFile))

rowCount = len(latData)
colCount = len(latData[0])

centroidX = 0
centroidY = 0
centroidZ = 0
count = 0;
points = []
for j in range(colCount):
        actualJ = j / decimateFactor
        for i in range(rowCount):
                actualI = i / decimateFactor
                if j % decimateFactor == 0 and i % decimateFactor == 0:
                        height = float(heightData[j][i])
                        latitude = float(latData[j][i])
                        longitude = float(longData[j][i])
                        radius = 1737400 + height
                        x = radius * math.cos(math.radians(latitude)) * math.cos(math.radians(longitude))

                        # Swapped y and z because Unity is Y up by default
                        z = radius * math.cos(math.radians(latitude)) * math.sin(math.radians(longitude))
                        y = radius * math.sin(math.radians(latitude))

                        centroidX += x
                        centroidY += y
                        centroidZ += z

                        points.append([x,y,z])

centroidX /= len(points)
centroidY /= len(points)
centroidZ /= len(points)

outFile = open("haworth.obj", "w")
for pt in points:
        print("v %f %f %f" % (pt[0] - centroidX, pt[1] - centroidY, pt[2] - centroidZ), file=outFile)

deciColCount = colCount // decimateFactor
deciRowCount = rowCount // decimateFactor

for j in range(deciColCount - 1):
        for i in range(deciRowCount - 1):

            # Offset of point with 1 based indexing
            pt = j * deciRowCount + i + 1;

            # First triangle
            print("f %d %d %d" % (pt, pt + deciRowCount, pt + 1), file=outFile)

            # Second triangle
            print("f %d %d %d" % (pt + deciRowCount, pt + deciRowCount + 1, pt + 1), file=outFile)

print("Centroid is %f %f %f" % (centroidX, centroidY, centroidZ))