var list = document.getElementById("userslistSelectedChat");

var openModalBtn = document.getElementById("openEditButton");

var addUserToSelectedChatBtn = document.getElementById("addUserToSelectedChatBtn");

var addUserToSelectedChatInput = document.getElementById("addUserToSelectedChat");

var saveChangesBtn = document.getElementById("saveChangesBtn");

var chatEditNameInput = document.getElementById("chatEditNameInput");

var deleteChatBtn = document.getElementById("deleteConversationBtn");


openModalBtn.onclick = function () {
	users = selectedChat.ChatUsers;
	chatEditNameInput.value = selectedChat.Title;
	list.innerHTML = "";
	selectedChat.ChatUsers.forEach(user => {
		var li = document.createElement('li');
		li.setAttribute('class', 'list-group-item');
		list.appendChild(li);
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
    })
	
}

addUserToSelectedChatBtn.onclick = function () {
	if (addUserToSelectedChatInput.value != "") {
		//get user
		GetUserSelectedChat(addUserToSelectedChatInput.value)
		
	}
}



function AddToSelectedChat(user) {
	if (!users.includes(users.find(x => x.Id === user.Id)) && user.Id != loggedUser) {


		users.push(user)
		var li = document.createElement('li');
		li.setAttribute('class', 'list-group-item');
		list.appendChild(li);
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
		console.log("user added");

		
	}
}

saveChangesBtn.onclick = function () {
	if (users != []) {
		EditChat(users, chatEditNameInput.value);
		
	}
	users = []
}
deleteChatBtn.onclick = function () {
	DeleteChat();
	selectedChat = null;
	selectedChatId = null;
	users = []
	chatDiv.style.display = "none";
	var hint = document.getElementById("hintH");
	hint.style.display = "block";
	LoadChats();
}