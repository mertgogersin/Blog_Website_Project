let topics = [];
$(document).ready(function () {
    var elements = document.getElementsByClassName('chkbtn');
  
    for (let i = 0; i < elements.length; i++) {
        if (elements[i].value === "true") {
            var id = elements[i].name;                    
            var topicDTO = {
                "topicID": id,
                "topicName": $("#topicName-" + id).text(),                
                "isSelected": "true"
            };
            topics.push(topicDTO);
        }
    }
    console.log(topics);
})


function GetTopic(id) {

    if ($("#topic-" + id).val() === "false") {

        $("#topic-" + id).val("true");
        var topicDTO = {
            "topicID": id,
            "topicName": $("#topicName-" + id).text(),
            "isSelected": $("#topic-" + id).val()
        };
        topics.push(topicDTO);
    }
    else {

        $("#topic-" + id).val("false");
        topics.map(item => {
            if (item.topicID == id && item.isSelected) {
                item.isSelected = $("#topic-" + id).val();
            }
        })
    }

    console.log(topics);
}
$("#btnPublish").click(function () {
    var articleDTO = {
        "title": $("#title").val(),
        "content": $("#content").val(),
        "subtitle": $("#subtitle").val()
    };
    var articleWithTopics = {
        "articleDTO": articleDTO,
        "topicDTOs": topics
    };
    console.log(articleWithTopics);
    $.ajax({
        url: "/Article/AddArticle/",
        type: "POST",
        data: {
            'articleWithTopics': articleWithTopics
        },
        success: function () {
            window.location.href = "/Article/Index/";
        },
        error: function () {
            console.log("error");
            window.location.href = "/Article/Index/";
        }
    });
})

$("#btnUpdate").click(function () {
    var id = $("#btnUpdate").attr("name");
    var articleDTO = {       
        "articleID": id,
        "title": $("#title").val(),
        "content": $("#content").val(),
        "subtitle": $("#subtitle").val()
    };
    var articleWithTopics = {
        "articleDTO": articleDTO,
        "topicDTOs": topics
    };
    console.log(articleWithTopics);
    $.ajax({
        url: "/Article/AddArticle/" + id,
        type: "POST",
        data: {
            'articleWithTopics': articleWithTopics
        },
        success: function () {
            window.location.href = "/Article/Index/";
        },
        error: function () {
            console.log("error");
            window.location.href = "/Article/Index/";
        }
    });
})

//I couldn't manage to work it, When i clicked to button, it always routed me Article/Index for no reason
//function AddOrUpdateArticle(id = null) {  
    
//    if (id == null) {

//        var articleDTO = {
//            "title": $("#title").val(),
//            "content": $("#content").val(),
//            "subtitle": $("#subtitle").val()
//        };
//    }
//    else {

//        var articleDTO = {
//            "articleID": id,
//            "title": $("#title").val(),
//            "content": $("#content").val(),
//            "subtitle": $("#subtitle").val()
//        };
//    }

//    var articleWithTopics = {
//        "articleDTO": articleDTO,
//        "topicDTOs": topics
//    };
//    console.log(articleWithTopics);
//    $.ajax({
//        url: "/Article/AddOrUpdateArticle/",
//        type: "POST",
//        data: {
//            'articleWithTopics': articleWithTopics
//        },
//        success: function (data) {
            
//        },

//    });
//}




