$(document).ready(function () {
    setInterval(function () {
        UpdateNotifications();
    }, 3000);

    function UpdateNotifications() {
        $.ajax({
            type: "GET",
            url: "/Home/GetNotifications",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log(response);
                if (response != null) {

                    var j = ""; var p = 0; var Q = "";
                    $.each(response, function (k, v) {
                        p++;
                        if (p < 6) {
                            j += "<li><a><img src='/img/nty.png' style='height:20px;width:20px'/> <strong>  " + v.Notification.toString() + " </strong> <br /> <div style='text-align:right;font-size:smaller;font-style:italic'> <i style='font-size:smaller'> updated : " + v.LastUpdated.toString() + "</i></div></a></li>";
                        }
                        Q += "<a><img src='/img/nty.png' style='height:20px;width:20px'/> <strong>  " + v.Notification.toString() + " </strong> <br /> <div style='text-align:right;font-size:smaller;font-style:italic'> <i style='font-size:smaller'> updated : " + v.LastUpdated.toString() + "</i></div></a>";
                    });
                    $('#myNotifyList').html("");
                    j += "<li><a data-toggle='modal' data-target='#myModal'><div style='text-align:center'><strong> Show more </strong></div></li>";
                    $('#myNotifyList').append(j);
                    $('#countNotify').html("<span>" + p + "</span>");
                    $('#myAllNotifications').html("");
                    $('#myAllNotifications').append(Q);
                }
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    }
});

