

function GetArticleByTopic(id = null) {
    var myUrl;
    if (id == null) {
        myUrl = "/Article/GetArticlesPartial"
    }
    else {
        myUrl = "/Article/GetArticlesPartial/" + id;
    }
    $.ajax({
        url: myUrl,
        type: "GET",
        success: function (response) {
            $("#allArticles").html(response);
        }
    });
}


function GetUser(id, url) {
    $.ajax({
        url: "/Member/GetUser/" + id,
        type: "GET",
        success: function () {
            window.location.href = "/Member/" + url;
        }
    })
}