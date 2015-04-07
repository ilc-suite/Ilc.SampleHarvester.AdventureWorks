# Ilc.SampleHarvester.AdventureWorks
Sample Harvester for Ilc based on the AdventureWorks MSSQL Server Database.
In this Repository you get a sample on how to write harvester for Ilc.
The different steps for creating harvester with different capabilities are seperated in the branches for this repository.

If you are new, start from the beginning.

###[In Step 1] (https://github.com/ilc-suite/Ilc.SampleHarvester.AdventureWorks/tree/1-InformationPoint)

The project implements the necessary interfaces to work as an InformationPoint Harvester for an Ilc System.

###[In Step 2] (https://github.com/ilc-suite/Ilc.SampleHarvester.AdventureWorks/tree/2-CollectInformations)

The project from Step 1 is extended to implements the interfaces to work as an InformationCollecter for a selected InformationPoint.

### [In Step 3] (https://github.com/ilc-suite/Ilc.SampleHarvester.AdventureWorks/tree/3-ExpandInformations)

A new harvester project is created Ilc.SampleHarvester.ExpandContact.sln.
This harvester takes Informations of type Person from the previous harvester of Step 2 and 
extends the Persons data with an url of an image.

### [In Step 4] (https://github.com/ilc-suite/Ilc.SampleHarvester.AdventureWorks/tree/4-CollectDetails)

The AdventureWorks harvester is extended to use custom InformaitonObjects. 
The Custom Types are BikeProduct and ProductPhoto.
The harvester can now collect informations of products and ads DetailLinks for each product.

With the DetailLink a client can request detailed information which only will be available when
an information is viewed in the details view. 
In this sample the Product when a bike product is viewed an details are requested
the harvester reads the image bytes from the database and returns them.

### [In Step 5] (https://github.com/ilc-suite/Ilc.SampleHarvester.AdventureWorks/tree/5-CredentialAuth)

The Ilc.SampleHarvester.ExpandContact harvester is extended to use credential authentication. 
This sample illustrates how to implement authentication for 3rd party applications. 
The harvester is modified to now only serve pictures to authenticated users.
