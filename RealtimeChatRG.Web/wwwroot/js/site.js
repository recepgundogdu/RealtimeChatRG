var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();
var selectedRoomId = 0;
var nickName = "";


$(document).ready(() => {
    connection.start();
    connection.on("receiveMessage", (roomId, user, message) => {
        if (selectedRoomId == roomId) {
            $("#txtMessages").append(`${user}: ${message}\n`);
            var textarea = document.getElementById('txtMessages');
            textarea.scrollTop = textarea.scrollHeight;
        }
    });
});


$("#btnLogin").click(() => {
    nickName = $("#txtNickname").val();
    $("#LoginPanel").hide();
    $("#ChatPanel").show();
    $("#menuBar").show();
    $("#lblUsername").html(nickName);
    Cookie.Set("username", nickName, 365);
});

$("#btnJoin").click(() => {
    $("#txtMessage, #btnSend").removeAttr("disabled");
    $("#ChatPanel .card-header b").html($("#lbRooms .active").html() + ": " + nickName);
    selectedRoomId = $("#lbRooms .active").attr("href").replace("#", "");
    GetMessages(selectedRoomId);
});

$("#btnSend").click(() => {
    let message = $("#txtMessage").val();
    if (selectedRoomId != 0 && nickName != "" && message != "") {
        connection.invoke("SendMessage", selectedRoomId, nickName, message)
            .catch(error => console.log("Mesaj gönderilirken hata alınmıştır."));

        $("#txtMessage").val("");
    }
});

$("#lblLogout").click(() => {
    Cookie.Remove("username");
    nickName = "";
    $("#txtNickname").val(nickName);
    $("#LoginPanel").show();
    $("#ChatPanel").hide();
    $("#menuBar").hide();
    $("#lblUsername").html(nickName);
});



$("#txtMessage").keydown(function (e) {
    if (e.keyCode == 13) {
        $("#btnSend").click();
    }
});
$("#txtNickname").keydown(function (e) {
    if (e.keyCode == 13) {
        $("#btnLogin").click();
    }
});

var Cookie = {
    Set: function (cname, cvalue, exdays) {
        const d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        let expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    },
    Get: function (cname) {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    },
    Remove: function (cname) {
        this.Set(cname, "", "0");
        document.cookie = name + '=; Max-Age=0'
    }
}


$(document).ready(() => {
    let user = Cookie.Get("username");
    if (user != "") {
        $("#txtNickname").val(user);
        $("#btnLogin").click();
    }
});

function GetMessages(roomId) {
     $.ajax({
        url: '/Home/GetMessages',
        type: 'POST',
        dataType: 'JSON',
        contentType: "application/json",
        data: roomId.toString(),
        success: function (data) {
            var chatHistory = "";
            for (var i = 0; i < data.length; i++) {
                chatHistory += data[i].username + ": " + data[i].text + "\n";
            }
            $("#txtMessages").html(chatHistory);
            var textarea = document.getElementById('txtMessages');
            textarea.scrollTop = textarea.scrollHeight;
        }
    });
}