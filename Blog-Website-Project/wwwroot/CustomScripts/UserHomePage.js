$(document).ready(function () {
    GetFollowedArticles();
  
})

function GetFollowedArticles() {
    $.ajax({
        url: "/Article/GetFollowedArticlesPartial/",
        type: "GET",
        success: function (response) {
            $("#followed").html(response);
        }
    })
}
$("#popular-tab").click(function () {
    $.ajax({
        url: "/Article/GetPopularArticlesPartial/",
        type: "GET",
        success: function (response) {
            $("#popular").html(response);
        }
    })
})

$("#all-tab").click(function () {
    GetArticleByTopic();
})
