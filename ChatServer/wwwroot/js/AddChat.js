var userslist = document.getElementById("userslist");
var addUserButton = document.getElementById("addUserButton");
var emailInput = document.getElementById("receiverInput");
var users = new Array();
var nameInput = document.getElementById("chatNameInput")
var addChatBtn = document.getElementById("addChatBtn");
var usersDiv = document.getElementById("usersDiv");
usersDiv.style.display = "none";


addUserButton.onclick = function () {
	if (receiverInput.value != "") {
		//get user
		GetUser(emailInput.value)
		usersDiv.style.display = "block";
	}
}
addChatBtn.onclick = function () {
	if (users.length != 0) {
		AddChat(users, nameInput.value);
		users = [];
		nameInput.value = ""
		emailInput.value = ""
		userslist.innerHTML = "";
	}
}

function AddToList(user) {
	if (!users.includes(users.find(x => x.Id === user.Id)) && user.Id != loggedUser) {
		console.log("Trying to add");

		users.push(user)
		var li = document.createElement('li');
		li.setAttribute('class', 'list-group-item');
		userslist.appendChild(li);
		var btn = document.createElement('button');
		btn.setAttribute('class', 'btn btn-sm btn-danger  ')
		btn.innerHTML = "Delete";
		btn.style = "margin-left:10px;"
		btn.onclick = function () {
			users.splice(users.indexOf(user), 1);
			li.remove();
		}
		li.innerHTML = li.innerHTML + user.FirstName + " " + user.LastName;
		li.appendChild(btn);

	}
}