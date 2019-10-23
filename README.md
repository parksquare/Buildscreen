![alt tag](https://www.parksq.co.uk/images/logo-nav.png)

Park Square Consulting Azure DevOps Buildscreen
===============================================

View the latest builds from your Azure DevOps and on-premise Team Foundation Server projects.

This project is a modernized resurrection of the original open source project by (now defunct) Orbit One.

We've streamlined the service layer to use the latest Azure DevOps REST APIs for best performance, improved the styling, fixed some
bugs, and migrated the whole thing to .Net Core. Also supports display of Gravatar and Azure DevOps avatars.

Supported statuses
------------------

* Grey: queued
* Blue: in progress
* Green: succeeded
* Yellow: partially succeeded (usually due to failing tests)
* Red: failed
* Purple: stopped

Build tiles now have links to additional information within AzDO for swift assessment of the problem without losing valuable time.

How to Use
----------

You can build the app and run it locally or deploy the binaries in your favourite manner (Kestrel, IIS, etc.)

Or you can use the latest pre-built Docker image available on DockerHub

e.g. 

        docker run --name buildscreen -p 80:80 
        -e AzureDevOpsProvider:ServerUrl="https://yourazdo.visualstudio.com"
        -e AzureDevOpsProvider:AuthToken="yourauthtoken"
        -e AzureDevOpsProvider:ProjectCollection="projectcolletion"
        -e AzureDevOpsProvider:Projects="comma,delimited,list"
        -e AzureDevOpsProvider:Organization="your-organization"
        parksq/buildscreen:latest

Replace the ServerUrl, AuthToken, ProjectCollection and Projects (comma delimited list if more than one) values with those appropriate for your setup.

License
-------
![alt tag](https://www.gnu.org/graphics/gplv3-88x31.png)

GNU GENERAL PUBLIC LICENSE Version 3
 
This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

Check out our website and other free C# code. https://www.parksq.co.uk 
