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
    
    var loginURI = "http://oopcontactmanager.azurewebsites.net/ContactManagerService.svc/GetUser"
    
    document.getElementById("userName").innerHTML = "";
    
    
    //hash pword
    var passHash = hash(pWord);
    console.log(passHash);
    //How, never did this in JS
    //Hash on both sides in case someone hacks DB
    
    var jsonSendData = '{"UserName": "'+userName+'" , "PasswordHash": "'+passHash+'"}';
    //var url = //how do i access the c# api files?
    console.log(jsonSendData);
    var loginRequest = new XMLHttpRequest();
    loginRequest.open("POST", loginURI, false);
    loginRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
    try{
        loginRequest.send(jsonSendData);
        
        var jsonObject = JSON.parse(loginRequest.responseText);
        
        userID = jsonObject.UserId;
        console.log(userID);
        console.log(jsonObject);
        
        //single flag?
        if(userID < 0 || userID == null){
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
    hideOrShow("addDiv",false);
    hideOrShow("searchDiv",false);
    hideOrShow("loggedInDiv",false);
    hideOrShow("delete",false);
}

function createAccount(){
    
    userID = 0;
    userName = "";
    
    addUserAddr = "http://oopcontactmanager.azurewebsites.net/ContactManagerService.svc/AddUser";
    
    userName = document.getElementById("cUserName").value;
    var pWord = document.getElementById("cPassword").value;
    var checkPWord = document.getElementById("checkCPassword").value;
    
    if (pWord != checkPWord){
        //update user with an error
        document.getElementById("checkCPWordLabel").innerHTML = "Passwords do not match";
        
    }
    //do regex checks for field validity?
    
    //hash pword
    var passHash = hash(pWord);
    console.log(passHash);
    //exact same hash as in login
    
    var jsonSendData = '{"UserName": "'+userName+'", "PasswordHash": "' +passHash+ '"}';
    
    //check if username already exists
    
    //add new account/check for conflicts
    //xml request
    var cAccRequest = new XMLHttpRequest();
    cAccRequest.open("POST", addUserAddr, false);
    cAccRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
    try{
        cAccRequest.send(jsonSendData);
        
        var jsonObject = JSON.parse(cAccRequest.responseText);
        
        userID = jsonObject.UserId;
        console.log(userID);
        console.log(jsonObject);
        //single flag
        if(userID < 0 || userID == null){
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
    hideOrShow("displayDiv", true);
    
}

function addContact(){
    
    var addAPI = "http://oopcontactmanager.azurewebsites.net/ContactManagerService.svc/NewContact"
    
    var firstName = document.getElementById("firstName").value;
    var lastName = document.getElementById("lastName").value;
    var homePhone = document.getElementById("homePhone").value;
    var mobilePhone = document.getElementById("mobilePhone").value;
    var workPhone = document.getElementById("workPhone").value;
    var otherPhone = document.getElementById("otherPhone").value;
    var homeEmail = document.getElementById("personalEmail").value;
    var workEmail = document.getElementById("workEmail").value;
    var otherEmail = document.getElementById("otherEmail").value;
    //address info
    var homeAddress = [document.getElementById("homeAddressNum").value];
    homeAddress.push(document.getElementById("homeAddressStreet").value);
    homeAddress.push(document.getElementById("homeAddressCity").value);
    homeAddress.push(document.getElementById("homeAddressCountry").value);
    homeAddress.push(document.getElementById("homeAddressProvince").value);
    var workAddress = [document.getElementById("workAddressNum").value];
    workAddress.push(document.getElementById("workAddressStreet").value);
    workAddress.push(document.getElementById("workAddressCity").value);
    workAddress.push(document.getElementById("workAddressCountry").value);
    workAddress.push(document.getElementById("workAddressProvince").value);
    var otherAddress = [document.getElementById("otherAddressNum").value];
    otherAddress.push(document.getElementById("otherAddressStreet").value);
    otherAddress.push(document.getElementById("otherAddressCity").value);
    otherAddress.push(document.getElementById("otherAddressCountry").value);
    otherAddress.push(document.getElementById("otherAddressProvince").value);
    
    homeAddress.unshift("home");
    workAddress.unshift("work");
    otherAddress.unshift("other");
    
    //populate with contact info
    
    //var jsonSendData = {"UserId": userID, "FirstName": firstName, "LastName": lastName, "homePhone": homePhone, "workPhone": workPhone,  "otherPhone": otherPhone, "homeEmail": homeEmail};
    
    
     var jsonSendData = {"BusinessInfo": [userID, firstName,lastName], "PhoneInfo": [["work",workPhone],["home",homePhone],["mobile",mobilePhone],["other",otherPhone]], "AddressInfo": [homeAddress, workAddress, otherAddress], "EmailInfo": [["personal", homeEmail], ["work", workEmail], ["other", otherEmail]]};
    
    //or restructure formatting again to be JSON within arrays within JSON:
    
    /*
     
     = {...,"PhoneInfo": [{"Type": "work","PhoneNumber": workPhone},...], ...}
     
     */
     
    //wait until i know right order
    jsonSendData = JSON.stringify(jsonSendData);
    console.log(jsonSendData);
    //xml request
    var addRequest = new XMLHttpRequest();
    addRequest.open("POST", addAPI, true);
    addRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
    try{
        addRequest.onreadystatechange = function(){
            if (this.readyState == 4 && this.status == 200){
                //read back some JSON data and tell the user if the thing was input correctly?
                
                document.getElementById("addContactResult").innerHTML = firstName + " " + lastName + " added successfully.";
            }
        }
        addRequest.send(jsonSendData);
        
    }catch(err){
        
    }
    
}

//redundant?
/*
function displayContacts(){
    
    //get contact list and output table?
    
    //xml request
    var listRequest = new XMLHttpRequest();
    listRequest.open("POST", ???, true);
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
    
}*/

function searchContacts(){
    
    var srchAPI = "http://oopcontactmanager.azurewebsites.net/ContactManagerService.svc/GetAllContacts?uId=" + userID;
    
    var srchElement = document.getElementById("searchElement").value;
    
    contactTable = document.getElementById("contactTable");
    
    var jsonSendData = {"SearchElement": srchElement};
    jsonSendData = JSON.stringify(jsonSendData);
    
    //xml request
    var searchRequest = new XMLHttpRequest();
    searchRequest.open("POST", srchAPI, true);
    searchRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
    try{
        searchRequest.onreadystatechange = function(){
            //check the state before doing this
            if (this.readyState == 4 && this.status == 200){
                var jsonObject = JSON.parse(searchRequest.responseText);
                
                
                var tabl = document.getElementById("ContactTable");
                tabl.innerHTML = "";
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
                    
                    //var keys = Object.keys(jsonObject[i]);
                    console.log(keys);
                    for(var j = 0; j < jsonObject[i].BusinessInfo.length;j++){
                        var cell = document.createElement("td");
                        var cellData = document.createTextNode(jsonObject[i].BusinessInfo[j]);
                        console.log(jsonObject[i].BusinessInfo[j]);
                        cell.appendChild(cellData);
                        row.appendChild(cell);
                    }
                    for(var j = 0; j < jsonObject[i].PhoneInfo.length;j++){
                        var cell = document.createElement("td");
                        var cellData = document.createTextNode(jsonObject[i].PhoneInfo[j]);
                        console.log(jsonObject[i].PhoneInfo[j]);
                        cell.appendChild(cellData);
                        row.appendChild(cell);
                    }
                    for(var j = 0; j < jsonObject[i].EmailInfo.length;j++){
                        var cell = document.createElement("td");
                        var cellData = document.createTextNode(jsonObject[i].EmailInfo[j]);
                        console.log(jsonObject[i].EmailInfo[j]);
                        cell.appendChild(cellData);
                        row.appendChild(cell);
                    }
                    var cell = document.createElement("td");
                    var delButton = createElement("button");
                    /*delButton.innerHTML = X;
                    delButton.setAttribute("data-contactmanager-contactid", jsonObject[i].ContactId);
                    delButton.onClick = deleteContact(delButton.getAttribute("data-contactmanager-contactid"));
                     */
                    //or
                    
                    cell.innerHTML = "<button onClick = \"deleteContact(" + jsonObject[i].ContactId + ")\" data-contactmanager-contactid = \"" + jsonObject[i].ContactId + "\">X</button>";
                    
                    var rowID = "row" + jsonObject[i].ContactId;
                    
                    row.setAttribute("id", rowID);
                    
                    tablbdy.appendChild(row);
                }
                tabl.appendChild(tablbdy);
                //document.getElementById("tableSpan").appendChild(tabl);
            }
            
        }
        searchRequest.send(jsonSendData);
        
        
        
    }catch(err){
        
    }
    
}


function deleteContact(contactID){
    //figure out the correct order for JSON
    var jsonSendData = {"contactID": contactID, "userID": userID};
    
    var delAPI = "http://oopcontactmanager.azurewebsites.net/ContactManagerService.svc/DeleteContact?cId=" + contactID;
    
    console.log(this.getAttribute("data-contactmanager-contactid"));
    console.log(contactID);
    console.log(this.getAttribute("data-contactmanager-contactid") == contactID);
    
    //xml request
    var delRequest = new XMLHttpRequest();
    delRequest.open("POST", delAPI, true);
    delRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
    try{
        delRequest.onreadystatechange = function(){
            
            var response = JSON.parse(delRequest.responseText);
            
            if(response < 1){
                //error deleting
                console.log("Could not delete contact");
            }
            console.log("contact deleted successfully");
            //update the displayed table, but how?
            // search for children and remove from parent? (imma need to set some ID's?)
            var rowID = "row" + contactID;
            row = document.getElementById(rowID);
            
            //row.innerHTML = "";
            for(var i = 0; i < row.childNodes.length; i = 0){
                row.removeChild(row.childNodes[i]);
            }
            document.getElementById("ContactTable").firstChild.removeChild(row);
            
        }
        
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
    
    document.getElementById( elementID ).style.visibility = visibility;
    document.getElementById( elementID ).style.display = display;
}

function hash(pWord){
    return CryptoJS.MD5(pWord).toString();
}


//// add an update function if there is time
// updateAPI  : "http://oopcontactmanager.azurewebsites.net/ContactManagerService.svc/UpdateContact"
