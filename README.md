## Ilc.SampleHarvester

A sample project to illustrate how to build your own harvester for Ilc.

### In Step 4

The AdventureWorks harvester is extended to use custom InformaitonObjects. 
The Custom Types are BikeProduct and ProductPhoto.
The harvester can now collect informations of products and ads DetailLinks for each product.

With the DetailLink a client can request detailed information which only will be available when
an information is viewed in the details view. 
In this sample the Product when a bike product is viewed an details are requested
the harvester reads the image bytes from the database and returns them.
