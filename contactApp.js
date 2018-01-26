// Authors:
// COP-4331
//

/*
This file still needs testing and probably lots of bug fixes (and needs to set proper communication to/from API).
besides communication the rest of the functionality is built in a basic state that can be tested and edited for more sophistication upon perfecting the communication.
*///


var userID = 0;
var userName = "";

function login(){
    userID = 0;
    
    var userName = document.getElementById("loginName").value;
    var pWord = document.getElementById("loginPassword").value;
    
    document.getElementById("userName").innerHTML = "";
    
    
    //hash pword
    var passHash =/*???*/;
    //How, never did this in JS
    //Hash on both sides in case someone hacks DB
    
    var jsonSendData = '{"userName": "'+userName+'" , "passHash": "'+passHash+'"}';
    //var url = //how do i access the c# api files?
    
    var loginRequest = new XMLHttpRequest();
    loginRequest.open("POST", /*url*/, false);
    loginRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
    try{
        loginRequest.send(jsonSendData);
        
        var jsonObject = JSON.parse(loginRequest.responseText);
        
        userID = jsonObject.id;
        
        //single flag?
        if(userID < 1){
            document.getElementById("loginResult").innerHTML = "username and password do not match";
            return;
        }
        
        //do we want to set different flags for these cases or just do a single flag saying that the uname and pwd dont match?
        
        //check if user exists
        //if not tell user that the user doesnt exist
        
        //check if user and pwd match
        //if not tell the user
        
        document.getElementById("userName").innerHTML = userName;
        document.getElementById("loginResult").innerHTML = "Logged in as" + userName;
        
        console.log("successfully logged in as " + userName);
        
        document.getElementById("loginName").value = "";
        document.getElementById("loginPassword").value = "";
        
        // figure out the hiding see if there is a better way (do i really want to mess with display style or should i just touch visibility?
        
        //show other pages and important elements:
        
        hideOrShow("loginDiv", false);
        hideOrShow("loggedInDiv",true);
        hideOrShow("addDiv", true);
        hideOrShow("searchDiv", true);
        hideOrSHow("displayDiv", true);
        
    }catch(err){1
        document.getElementById("loginResult").innerHTML = err.message;
    }
}

//link this to on window close as well??

function logout(){
    userID = 0;
    fName = "";
    lName = "";
    
    //hide/show important divs
    hideOrShow("loggedInDiv",false);
    hideOrShow("loginDiv", true);
    hideOrShow("addDiv", false);
    hideOrShow("searchDiv", false);
    hideOrSHow("displayDiv", false);
}

function onCreateAccountTabClicked(){
    
    //clear buffers
    document.getElementById("loginName").value = "";
    document.getElementById("loginPassword").value = "";
    
    //hide current screen and show create account screen
    hideOrShow("loginDiv", false);
    hideOrShow("createAccDiv", true);
    
}

function createAccount(){
    
    userID = 0;
    userName = "";
    
    userName = document.getElementById("cUserName").value;
    var pWord = document.getElementById("cPWord").value;
    var checkPWord = document.getElementById("checkCPWord").value;
    
    if (pWord != checkCPWord){
        //update user with an error
        document.getElementById("checkCPWordLabel").innerHTML = "Passwords do not match";
        
    }
    //do regex checks for field validity?
    
    //hash pword
    var passHash = "";
    //exact same hash as in login
    
    var jsonSendData = '{"UserName": "'+userName+'", "passHash": " '+passHash+'"}';
    
    //check if username already exists
    
    //add new account/check for conflicts
    //xml request
    var cAccRequest = new XMLHttpRequest();
    cAccRequest.open("POST", /*???*/, true);
    cAccRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
    try{
        cAccRequest.send(jsonSendData);
        
        var jsonObject = JSON.parse(cAccRequest.responseText);
        
        userID = jsonObject.id;
        
        //single flag
        if(userID < 1){
            document.getElementById("cAccResponse").innerHTML = "username and password do not match";
            return;
        }
        
    }catch(err){
        document.getElementById("cAccResponse").innerHTML = err.message
        return;
    }
    
    //successful account creation exit processes
    document.getElementById("cAccResponse").innerHTML = "Created Account Successfully";
    document.getElementById("cUserName").value = "";
    document.getElementById("cPassword").value = "";
    document.getElementById("checkCPassword").value = "";
    
    hideOrShow("createAccDiv", false);
    hideOrShow("loggedInDiv", true);
    hideOrShow("addDiv", true);
    hideOrShow("searchDiv", true);
    hideOrSHow("displayDiv", true);
    
}

function addContact(){
    
    var firstName = document.getElementById("firstName").value;
    var lastName = document.getElementById("lastName").value;
    var homePhone = document.getElementById("homePhone").value;
    var mobilePhone = document.getElementById("mobilePhone").value;
    var workPhone = document.getElementById("workPhone").value;
    var otherPhone = document.getElementById("otherPhone").value;
    var homeEmail = document.getElementById("homeEmail").value;
    var workEmail = document.getElementById("workEmail").value;
    var otherEmail = document.getElementById("otherEmail").value;
    var homeAddress = document.getElementById("homeAddress").value;
    var workAddress = document.getElementById("workAddress").value;
    var otherAddress = document.getElementById("otherAddress").value;
    
    
    
    //populate with contact info
    
    var jsonSendData = {"UserId": userID, "FirstName": firstName, "LastName": lastName, "homePhone": homePhone, "workPhone": workPhone,  "otherPhone": otherPhone, "homeEmail": homeEmail};
    //wait until i know right order
    jsonSendData = JSON.stringify(jsonSendData);
    
    //xml request
    var addRequest = new XMLHttpRequest();
    addRequest.open("POST", /*???*/, true);
    addRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
    try{
        addRequest.onreadystatechange = function(){
            if (this.readyState == 4 && this.status == 200){
                //read back some JSON data and tell the user if the thing was input correctly?
                
                document.getElementById("addResult").innerHTML = firstName + " " + lastName + " added successfully.";
            }
        }
        addRequest.send(jsonSendData);
        
    }catch(err){
        
    }
    
}

//redundant?
function displayContacts(){
    
    //get contact list and output table?
    
    //xml request
    var listRequest = new XMLHttpRequest();
    listRequest.open("POST", /*???*/, true);
    listRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
    try{
        listRequest.onreadystatechange = function(){
            var jsonObject = JSON.parse(listRequest.responseText);
            
            //create table from JSON response
            for(){
                
                
                
            }
            
        }
        listRequest.send(jsonSendData);
        
        
    }catch(err){
        
    }
    
}

function searchContacts(){
    
    var srchElement = document.getElementById("searchElement").value;
    
    contactTable = document.getElementById("contactTable");
    
    var jsonSendData = {"SearchElement": srchElement};
    jsonSendData = JSON.stringify(jsonSendData);
    
    //xml request
    var searchRequest = new XMLHttpRequest();
    searchRequest.open("POST", /*???*/, true);
    searchRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
    try{
        searchRequest.onreadystatechange = function(){
            //check the state before doing this
            if (this.readyState == 4 && this.status == 200){
                var jsonObject = JSON.parse(searchRequest.responseText);
                
                
                var tabl = document.createElement("table");
                var tablbdy = document.createElement("tbody");
                
                //create table from JSON response
                //create a hidden table?
                //or separate into two functions?
                //search and retrieve?
                //selection box with 'onselect'?
                
                //onselect = "return showContact();"
                //for now create the table (which will be hidden and certain rows displayed?
                //or will we do a research?? (if so reevaluate how the code operates.
                //im using jquery for the next project
                for(var i = 0; i < jsonObject.length;i++){
                    var row = document.createElement("tr");
                    var keys = Object.keys(jsonObject[i]);
                    console.log(keys);
                    for(var j = 0; j < keys.length;j++){
                        var cell = document.createElement("td");
                        var cellData = document.createTextNode(jsonObject[i].keys[j]);
                        console.log(jsonObject[i].keys[j]);
                        cell.appendChild(cellData);
                        row.appendChild(cell);
                    }
                    var cell = document.createElement("td");
                    var delButton = createElement("button");
                    delButton.innerHTML = X;
                    delButton.setAttribute("data-contactmanager-contactid", jsonObject[i].ContactId);
                    delButton.onClick = deleteContact(delButton.getAttribute("data-contactmanager-contactid"));
                    //or
                    /
                    cell.innerHTML = "<button onClick = \"deleteContact(" + jsonObject[i].ContactId + ")\" data-contactmanager-contactid = \"" + jsonObject[i].ContactId + "\">X</button>";
                    
                    */
                    tablbdy.appendChild(row);
                }
                tabl.appendChild(tablbdy);
                document.getElementById("tableSpan").appendChild(tabl);
            }
            
        }
        searchRequest.send(jsonSendData);
        
        
        
    }catch(err){
        
    }
    
}


function deleteContact(contactID){
    //figure out the correct order for JSON
    var jsonSendData = {"contactID": contactID, "userID", userID};
    
    console.log(this.getAttribute("data-contactmanager-contactid"));
    console.log(contactID);
    console.log(this.getAttribute("data-contactmanager-contactid") == contactID);
    
    //xml request
    var delRequest = new XMLHttpRequest();
    delRequest.open("POST", /*???*/, true);
    delRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
    try{
        delRequest.send(jsonSendData); //cases for errors?
        
        var jsonObject = JSON.parse(delRequest.responseText);
        
    }catch(err){
        
    }
    
}

//no need for Jquery for this proj
//jQueryMobile for next porject?
//swap to jQuery?

function hideOrShow(elementID, newState){
    var visibility = "visible";
    var display = "block";
    if(!newState){
        var visibility = "hidden";
        var display = "none";
    }
    
    document.getElementById( elementId ).style.visibility = visibility;
    document.getElementById( elementId ).style.display = display;
}
