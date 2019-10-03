$(document).ready(function () {

    var picture = Math.floor(Math.random() * 8)
    var back = Math.floor(Math.random() * 8)

    //document.getElementById('chatbody').style.backgroundImage = 'url(/images/chatbackground/bakery/' + back + '.jpg)';
    document.getElementById('chatbody').style.backgroundImage = 'url(/images/chatbackground/' + back + '.png)';
    document.getElementById('chat').style.backgroundImage = 'url(/images/chatbackground/chatback.png)';

    $("#message").emojioneArea({
        pickerPosition: "right",
        tonesStyle: "bullet",
        events: {
            keyup: function (editor, event) {
                $("#messajearea").val(this.getText());
                $("#message").val(this.getText());
                if (event.keyCode == 13) {
                    sendmessage.click();
                }
            },
            click: function (editor, event) {
                $("#messajearea").val(this.getText());
            },
            select: function (editor, event) {
                $("#messajearea").val(this.getText());
            }
        }
    });
});


$(function () {

    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message, room) {

        if (room == $('#room').val()) {
            if (name != $('#displayname').val()) {
                // mensaje de los demas
                $('#chat').append('<div class="incoming_msg"><div class= "received_msg"><div class="received_withd_msg"><p>' + htmlEncode(message) + '</p><div class="user-chat-name-other">' + name + '</div></div></div></div>');
            }
            else {
                // mensajes propios                
                $('#chat').append('<div class="outgoing_msg"><div class="sent_msg"><p>' + htmlEncode(message) + '</p><div class="user-chat-name">' + name + '</div></div></div>');
            }
        }
    };
    // Get the user name and store it to prepend to messages.
    $('#displayname').val(prompt('Enter your name:', ''));
    //$('#room').val(prompt('Enter your room:', ''));
    // Set initial focus to message input box.
    $('#message').focus();
    // Start the connection.
    $.connection.hub.start().done(function () {
        $('#sendmessage').click(function () {
            SendMessage();
        });
    });
});


function formatAMPM() {
    var date = new Date();
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}

function SendMessage() {
    var chat = $.connection.chatHub;

    chat.server.send($('#displayname').val(), $("#message").val(), $('#room').val());
    // Clear text box and reset focus for next comment.
    $('#message').val('').focus();
    setTimeout(function () {
        $(".emojionearea-editor").empty()
        $("#messajearea").val("");
        $("#chat").animate({ scrollTop: $('#chat').prop("scrollHeight") }, 1000);
    }, 25);
}


// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}