Vertical Farming Development Journal:

Downloaded and installed Visual Studio, Git and Docker.

Tried running docker-compose up, however had some issues with that, an error occurred. 
After doing some research into this, found that I had not complete the docker installation properly.
Finished installing docker, installed Ubuntu 18.0, not sure if I need this?
Could run the docker-compose inside of Ubuntu shell? But I've been doing it in powershell instead.
After making these changes, ran docker-compose up and can now access localhost pages.

Took JSON recipe data from the page and pasted it into it's own JSON file on Visual Studio Code.
Restructured so I can see and understand the recipe process.
NOTE: There seems to be an error in the JSON? For strawberries, there are two lighting phases
named "Phase 3", seems like one should be named "Phase 1".

Can also get access to the structured JSON on the swagger page.
Not really sure how Docker works. Am I hosting the recipe localhost on a virtual
Linux machine or is it done on Windows? If Docker is used to host virtual environments
within the same PC environment, is it hosting it on a different Operating System?
How can I see this? I downloaded Ubuntu so maybe it's hosting it on there. Not really sure.

Created repository for code: https://github.com/Ehoward12/IGSTechTest
Cloned onto local machine.

Looking at setting up Visual Studio project in repository.
Not sure which project to create, could be a console solution given that the purpose
is to create a JSON file, which doesn't require any sort of interface.
In the bonus points section, it mentions that we get bonus points for creating a 
docker solution. Not sure how to do this or what this means, but I assume that the
recipe application is created in a docker solution given that it's run by docker-compose.
Reading this to learn about C# dockerised solutions: https://docs.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=windows

Following the page above to create a C# docker application.
Created a Hello World application, pushing to repository...

Opened project in Visual Studio, ran docker solution, Hello World works.

Created a function to connect the recipe API using HTTP and pull the JSON, printing it to the 
console. This seems to work okay. Will abstract this to a seperate file, so it
is contained within it's own "layer" which will only be used as a communication layer
for the API.

Committed these changes. 
Starting to plan a structure for the schedule JSON.

Planned structure for the schedule:

{
    "schedule":
    [
        {"actions":[
            {"crop":"basil", "time": "00:00:00:00", "type":"light", "data":{"intensity":0}},
            {"crop":"basil", "time": "00:01:00:00", "type":"water", "data":{"amount":55}}
        ]}
    ]
}

Trying to now take the output from the API and convert it into some sort of JSON object.
Looks like I can use Newtonsoft.json for this. Installed using the VS installer.
For this, I can serialize and deserialize data. I think I need to create a class of type
JsonSerializerSettings, which can then be converted to and from a JSON string? 
Creating JSON schedule class now. Will also need an action class?
Created action class.

Creating a class for parsing the json recipe format. Will create functions that will
return specific areas of the recipe JSON. Can then use this to put them into 
schedule and action objects.

Created a whole bunch of classes for storing a full recipe as an easily readable object.
This works. Pushed changes to Git. 
The next step will be to start thinking about how to create the schedule from this...

So I think here, some assumptions should be made. Considering on the GitHub page, the
example schedule shows the tray number, this made me realize that the recipes
are abitrary and are only what should happen given we have some trays of those
crops. So we need to define what we actually have in the farm, so that we can
create a schedule for those crops given the recipes we have.

For this, I will say we have 10 trays, of which 6 are Basil, and 4 are strawberries.
They shall be named "Tray 1", "Tray 2" etc.

We will therefore need to update our schedule structure for the actions:

{
    "schedule":
    [
        {"actions":[
            {"tray":1, "crop":"basil", "time": "00:00:00:00", "type":"light", "data":{"intensity":0}},
            {"tray":2, "crop":"basil", "time": "00:01:00:00", "type":"water", "data":{"amount":55}}
        ]}
    ]
}

We should also define for how long our schedule should last. 
To make sure we're including a range of actions, we'll make the schedule last for a month.
However, this doesn't make sense. We have a set amount of repetitions on every phase,
therefore, we can play the schedule out until completion. For example,
the lighting phase of Basil lasts 24 hours and has 5 repetitions. We only have this 
one lighting phase, so we can assume that after 5 24 hour phases, we will have completed
the recipe? This seems like the most appropriate assumption for how long the schedule 
will last.

Using a notepad and pen to jot out what the algorithm would look like for creating the actions.
Are we assuming that the phases are in JSON order? Since we have the names set to "Phase 1",
"Phase 2" and so on, it would be thought that phases follow in order, so we should
specifically look for the Phase 1 string and continue on from there, however,
due to the fact that for the strawberries, there are two phase 3's, and this is what
we're pulling from the API, we can't do this. We will have to assume phases are in order in
JSON and run through them that way.

Nevermind, there is an "order" attribute which we can use to designate and order in which
we run the phases.

Creating a class called "CropTray". We will also have a "Farm" class, which will contain all 
the trays within our farm. The trays will have a crop and name attribute. The name
of the crop will link to the name of the crop from the recipe object.

Created first draft of algorithm to make schedule actions for lighting phases.
Will now need to test this to make sure it works.
To test this, I will run through the correct actions on a notepad, as note what
they should be, then run through the code in VS debugger to verify that we are
getting the correct actions.

Spent a while testing the lighting algorithm until it worked, then implemented the same
code for the watering phases. All actions are now getting added to a list.
This list will now need to be ordered. Currently, there are 522 actions in 
this list, so not sure how to order them properly as sorting could take a long time.
Let's have a look...

Okay the sort function does it nice and easy, can sort on object properties.
In this case, sorted on the DateTime, which puts the schedule into time order.

Now need to convert this list of actions into JSON. Think there might be an easy way
to do this using the serialize tools I saw before. Going to push again here.

JSON serialization doesn't seem to work, maybe because of the DateTime value
of Action? Shouldn't be too difficult to manually do it so just going to create
a function to manually create the JSON string.

Done that, committed everything, will take a break...

Come back. Have commented all code. JSON is outputted to a file, but I wonder if I can
get it to be sent to a URL, and run the app like the IGS recipeAPI is run. Not sure how to do
this. 

Just looked at RecipeAPI code, looks like there's a better way to create JSON objects 
than doing it manually like I did. I haven't got much time to make the change here,
so will leave this be, however it could definitely be something I improve for next time.

Looks like to create the web app, it uses some WebApplication class. Not sure how
difficult this is going to be to implement now given I've already finalized everything.
Maybe because I've not got much code in program.cs, it won't be too difficult to integrate.

No C# recognition of WebApplication, is this a publically available library or something
IGS has developed?

Ah, so it is a Microsoft framework, but found that I'm using .NET v4.0 for some reason,
when WebApplication exists on version 6.0... Not sure how this happened when I installed
.NET from new. Will see about updating. Okay this is odd because my application 
in Visual Studio has the target set to 6.0. Why then can I not see the WebApplication package?

There might be a disconnect between what is available on VS 2022 and my computer SDK install.
Trying now to install .net 6.0 SDK as I realized that I didn't actually install a new
SDK when I did my installations. I just installed VS 2022, maybe this doesn't install
the SDK if you already have a older one on your PC?

Installed 6.0, restarted PC, still getting the same issue. Maybe I can check the .sln file of the IGS
project and see if there's an installed package there which is eluding me?

Okay, so I compared the .proj files for both my project and the IGS project and found that the SDK in use
is different. Needed to use a web SDK, changed now.

Does this mean I've created the wrong type of application? Possibly.

Okay cool, added a web application and created a URL for the schedule JSON, which after running, seems to work.

Had loads of trouble first with the docker-compose solution. Spent a good few hours trying to get it to
run just so that I could access a home page returning "Hello World". After a while and Following
different tutorials, I managed to get the Hello World to work, however I had issues connecting to
the recipe API app from within my application. I eventually found that this was due to not being
able to access a docker app from a docker app on localhost, and changed it to
http://host.docker.internal:8080/recipe which works.

Seems like everything is working now! Unfortunately I haven't had time to work on the unit tests,
however if I were to do these, I would have tested a few elements of the application. 
Firstly, the API access to the recipe would have been tested. Secondly, the ability for the
JsonRecipeParser to parse the JSON data and convert it to objects would have been tested,
by having a bunch of dummy JSON data of different variance and sending it through, which
I could then test the results of. I would have also tested the ability to construct a schedule
from this data, by passing in recipe and tray objects and verifying that we see the correct
list of Actions in turn. These would have all been runnable from within a single unit test
operation, which would tally up passes, failures and keep track of these in a log file.
Ideally, a Jenkins system would be setup whereby these tests are run after every new commit.

To further develop my application for a production environment, apart from having the addition of 
unit tests, it would also be good to use a JSON framework with more purpose, for future maintenance.
Something else which would also be good in a production environment, is some method of static analysis
which is conducted automatically on a commit, to make sure we adhere to a specific coding standard.
I also think my comments could have been more consistent, and adhere to a commenting standard.
The application could be set to run on a timer, so that everyday a schedule for that day is generated
using the information we have at that time (recipes, trays etc.). This would run every morning, 
generate a shcedule for the day, which would then be sent to some other software which would be
able to interpret it and send commands to the hardware controlling the farm. The developers
would be able to commit changes to branches, using some form of continuous development, then when
changes are pushed to the master branch, the software would be updated automatically from an online
repository and continue to run. Ultimately, the desire would be to have one seamless process, which 
would make it easy for developers to add clean, tested and working code.

Something else that I wish I did would be to move all source files into contained folders, instead
of them all being in the root directory of the repo.