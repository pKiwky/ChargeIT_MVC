function openModal(controller, page, parameters) {
    console.log(parameters);

    $.ajax({
        url: `/${controller}/${page}`,
        type: "GET",
        data: parameters,
        success: function (data) {
            if(data.length > 0) {
                $("#partialViewContent").html(data);
                $('#myModal').modal('show');
            }
        }
    });
}

function sendRequest(controller, page) {
    var form = $('.modal-content form');
    var actionUrl = form.attr('action');
    var dataToSend = form.serialize();

    $.ajax({
        url: `/${controller}/${page}`,
        data: dataToSend,
        type: "POST",
        success: function (data) {
            if (data.success == true) {
                location.reload();
                return;
            }

            var modal = $('#myModal');
            modal.find('#partialViewContent').replaceWith(data);
        },
    });
}
