# 3D Metabolism
## Project Description
A visualization of the Metabolic that leverages the third dimension and Unity animations to make the network easier to understand

## About this branch
this branch is to prototype the animation pipeline (IAT4U) pipeline in action.  Features:
- Prototyping temporary UI assets, like a 'play animation' button that will appear for each search result.
- Each node and edge will 'pulse' in color based on the order from the seearch.

## External Assets

### Included
(List of assets included in the project here. Include a link to the asset source.>

| Asset | Purpose |
| ------ | ------ |
| CleanNetwork.fbx | A static implementation of our network on Glycolysis and Gluconeogenesis |

## Prerequisites
1. Unity 2020.3.7f
2. Node npx (suggested)

## Getting Started
1. Clone the repository
2. Open the project.  The scene that is in active development is `development`.

## Testing WebGL builds

If you want to test features for the actual target (WebGL), there are multiple options after building the project (see below).

### Node
1. Go to the Builds folder, open up a terminal and do `npx http-server`.  
2. This will spin up a local server and you can copy/paste the URL to see your changes.
3. CTRL + c, to stop the service.

### Unity
In the project, go to File -> Build And Run.

## Building / Deploying

1. In Unity -> File -> Build Settings.  Include the scenes you want to build
2. Make sure the Platform is WebGL.  Default options are OK.
3. Click Build.
4. Log into AWS as an admin and go to S3 Buckets.
5. Drag and drop all the files inside the build folder into the bucket.
6. Save the changes - the app is now deployed and udpated on AWS.

## Contributing (Optional) 

1. Instructions for public contributions here
2. Create a wiki page if this is long

## Team

### Faculty:
Dr. Lindsay Rogers

### Current EML Team:

- Rayyan - Project Lead / Content Team
- Mohsen- Developer / Consultant
- Jiho- Designer
- Robert- Developer
- Ella - Content Team

### Spring 2021 Team:
- Dante - Lead
- Nikko - Developer
- Hai Lin - Developer
- Jenn - Designer
- Kim - Designer

### Fall 2020 Team:
- Courtney - Lead/Designer
- Kim - Designer
- Dante - Developer
- Nikko - Developer
- Hai Lin - Developer

## Documentation
For documentation, please visit the UBC Wiki for this repository (this wiki talks about a past Aframe version): 
https://wiki.ubc.ca/Documentation:Metabolism
(Create either an external wiki or gh pages site for the script documentation. A template doxyfile is included for generating doxygen based documentation.)
