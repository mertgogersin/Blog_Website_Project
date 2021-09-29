$(document).ready(function () {
    $('button[data-toggle="modal"]').click(function () {
        $("#loginModal").modal("show");
    })
    $('button[data-dismiss="modal"]').click(function () {
        $(".modal").modal("hide");
    })
    var msg = '@ViewBag.Message';
    if (msg != "") {
        $("#alert").modal("show");
    }
    $("#getStarted").click(function () {
        $.ajax({
            url: "/Register/GetRegisterModalPartial/",
            data: "GET",
            success: function (response) {
                $("#modalPartial").html(response);
            }
        })
    })
})