let check = false;
var topicDTOs = [];
$(document).ready(function () {
    var elements = document.getElementsByClassName('chkbtn');
 
    for (let i = 0; i < elements.length; i++) {
        if (elements[i].value === "true") {
            var id = elements[i].name;
            console.log(id)
            var topicDTO = {
                "topicID": id,
                "topicName": $("#topicName-" + id).text(),
                "description": $("#topicDesc-" + id).text(),
                "isSelected": "true"
            };
            topicDTOs.push(topicDTO);
        }
    }

})
function AddTopic(id) {
    if ($("#input-" + id).val() === "false") {
        check = false;
    }
    else if ($("#input-" + id).val() === "true") {
        check = true;
    }

    if (!check) {

        $("#check-" + id).html(`<i class="fas fa-check-circle fa-4x" onclick="AddTopic(${id})"></i>
                                    <input value="true" id="input-${id}" style="display:none"/>`)
        var topicDTO = {
            "topicID": id,
            "topicName": $("#topicName-" + id).text(),
            "description": $("#topicDesc-" + id).text(),
            "isSelected": "true"
        };
        topicDTOs.push(topicDTO);

    }
    else {
        $("#check-" + id).html(`<i class="far fa-check-circle fa-4x" onclick="AddTopic(${id})"></i>
                                    <input value="false" id="input-${id}" style="display:none"/>`)
        topicDTOs.map(item => {
            if (item.topicID == id && item.isSelected) {
                item.isSelected = "false";
            }
        })
    }
}
$("#returnHome").click(function () {

    $.ajax({
        url: "/Topic/AddTopicsToUser",
        type: "POST",
        data: {
            "topicDTOs": topicDTOs
        },
        success: function () {
            window.location.href = "/User/UserHomePage/";
        }
    })
})