$(document).ready(function () {
    GetPopularArticles();
})

function GetPopularArticles() {
    $.ajax({
        url: "/Article/GetPopularArticlesPartial/",
        type: "GET",
        success: function (response) {
            $("#popular").html(response);
        }
    })
}
$("#articles-tab").click(function () {
    GetArticleByTopic();
})