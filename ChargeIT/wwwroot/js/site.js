// Setare globala.
toastr.options.positionClass = 'toast-bottom-right';

$(document).ready(function () {
    var message = localStorage.getItem("message");
    if (message != null) {
        toastr.success(message);
        localStorage.clear();
    }

});

function openModal(controller, page, parameters) {
    console.log(parameters);

    $.ajax({
        url: `/${controller}/${page}`,
        type: "GET",
        data: parameters,
        success: function (data) {
            if (data.length > 0) {
                $("#partialViewContent").html(data);
                $('#myModal').modal('show');
            }
        }
    });
}

function sendPostRequest(controller, page) {
    var form = $('form');
    var actionUrl = form.attr('action');
    var dataToSend = form.serialize();

    $.ajax({
        url: `/${controller}/${page}`,
        data: dataToSend,
        type: "POST",
        success: function (data) {
            error = data["error"];
            message = data["message"];

            if (error != null) {
                toastr.error(error);
            }
            // Presupunand ca orice request executat cu succes contine 'message'.
            else if (message != null) {
                localStorage.setItem("message", message);
                toastr.success(message);

                location.reload();
                return;
            }

            var modal = $('#myModal');

            if (modal.length == 0) {
                var reg = /\<body[^>]*\>([^]*)\<\/body/m;
                var body = data.match(reg)[1];

                $("body").html(body);
            } else {
                modal.find('#partialViewContent').html(data);
            }
        },
    });
}

function sendDeleteRequest(controller, page, parameters) {
    $.ajax({
        url: `/${controller}/${page}`,
        data: parameters,
        type: "DELETE",
        success: function (data) {
            error = data["error"];
            message = data["message"];

            if (error != null) {
                toastr.error(error);
            } else {
                if (message != null) {
                    localStorage.setItem("message", message);
                    toastr.success(message);
                }
                location.reload();
            }
        },
    });
}

function showStationMapInfoWindow(station) {
    var infowIndow = new google.maps.InfoWindow({
        content:
            "<div><h5>" + station.Code + "</h5>" +
            "<p> <b>City: </b>" + station.City + "</p>" +
            "<p> <b>Street: </b>" + station.Street + "</p>" +
            "<p> <b>Number: </b>" + station.Number + "</p>" +
            "</div>"
    });

    return infowIndow;
}