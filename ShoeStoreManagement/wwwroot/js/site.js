// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showContent(url, title, id) {

    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
        }
    })
}

function caiHamKhac() {
    alert($("#peMePls").val());
    $("#peMePls").html(199);

}


$(document).ready(function () {
    // GET BY ID
    $("#btn-get").on("click", function () {
        //var formData = new FormData();
        var id = $(this).attr("CCModal");
        var url = '@Url.Action("Edit", "Product")' + '/' + id;
        $.ajax({
            type: 'GET',
            url: url,
            contentType: false,
            processData: false,
            cache: false,
            data: formData,
            success: function (response) {
                if (response.responseCode == 0) {
                    var obj = JSON.parse(response.responseMessage);
                    $("#ProductId").val(obj.ProductId);

                }
                else {
                    bootbox.alert(response.ResponseMessage);
                }
            },
            error: errorCallback
        });
    });
    //SAVE
    $("#btn-insert-student").on("click", function () {
        var formData = new FormData();
        formData.append("name", $("#name").val());
        formData.append("email", $("#email").val());
        $.ajax({
            type: 'POST',
            url: '@Url.Action("InsertStudent", "Home")',
            contentType: false,
            processData: false,
            cache: false,
            data: formData,
            success: successCallback,
            error: errorCallback
        });
    });
    // UPDATE
    $("#btn-update-student").on("click", function () {
        var formData = new FormData();
        formData.append("id", $("#hdn-student-id").val());
        formData.append("name", $("#name").val());
        formData.append("email", $("#email").val());
        $.ajax({
            type: 'PUT',
            url: '@Url.Action("UpdateStudent", "Home")',
            contentType: false,
            processData: false,
            cache: false,
            data: formData,
            success: successCallback,
            error: errorCallback
        });
    });
    //DELETE
    $("#btn-delete-student").on("click", function () {
        var formData = new FormData();
        formData.append("id", $("#hdn-student-id").val());
        $.ajax({
            type: 'DELETE',
            url: '@Url.Action("DeleteStudent", "Home")',
            contentType: false,
            processData: false,
            cache: false,
            data: formData,
            success: successCallback,
            error: errorCallback
        });
    });
    function resetForm() {
        $("#hdn-student-id").val("");
        $("#name").val("");
        $("#email").val("");
    }
    function errorCallback() {
        bootbox.alert("Something went wrong please contact admin.");
    }
    function successCallback(response) {
        if (response.responseCode == 0) {
            resetForm();
            bootbox.alert(response.responseMessage, function () {

                //PERFORM REMAINING LOGIC
            });
        }
        else {
            bootbox.alert(response.ResponseMessage);
        }
    };
});