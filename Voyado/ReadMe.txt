Solved using .NET 6
Note I have not used .NET 6 previously and was only used because built-in IOC as im clueless how to write code without it.
The solution is build using seperate layers between a WEB project (VoyadoMain) and serverlogic projects VoyadoManagement and IVoyadomanagement
Ive split this up as diffrent projects to access re-usability and scaling(In theory it could be all made in-line javascript, but it would remove the reusability and scaling factor)
We can now create api's and let others access the serverlogic since its seperated (though I think these management projects should have a seperate ioc wireup)

ProgramFlow:
View(render textbox)
	->Controller(reroute only and handle exceptions)
		->Library(Run the serverlogic, one or many methods needed)
			->Management(The serverlogic)

Note that only GoogleSearch has an actual implementation(Bing only has a DummyImplementation), due to timeconstraints and it takes quite abit of time to lookup each apidocs and setup dev applications.
I dont see how implementing bing specifically would make it much clearer how I code since it would just be more or less the same implementationwise. However architectually it should scale nicely.
As you may notice the actual front-end has not recieved much love either. The goal i'd make an ajax-request and update a textbox or similar, or maybe using a partialview to render a more fancy tabel

To run the program , open it up in visual studio and run VoyadoMain (Nothing fancy has been made there)
To customize the searchendgine params needed modify appSettings.json(though there exists valid ones i used from testing)
	Note that this should in production obviously not be commited into a specific project directly, but for simplicity its hard-coded

