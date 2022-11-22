// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $("#loaderbody").addClass('d-none');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('d-none');

    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('d-none');
    });
});

function modifyClass(className, isAdd) {

    if (isAdd) {
        $("btn-js").addClass(className);
    }
    else {
        $("btn-js").removeClass(className);
    }
}

function toCart(url, title, id) {

    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        success: function (res) {
            $("#size-dialog .modal-body").html(res);
            $("#size-dialog .modal-title").html(title);
            $("#size-dialog").modal('show');
            $.notify("I'm over here !");
        }
    })
}

function showContent(url, title, id) {

  $.ajax({
    type: "GET",
    url: url,
    data: { id: id },
    success: function (res) {
      $("#form-modal .modal-body").html(res);
      $("#form-modal .modal-title").html(title);
        $("#form-modal").modal('show');
        //$.notify("I'm over here !");
        //$.notify("Access granted", "success", { position: "right" });
        $("#form-modal .modal-title").notify(
            "I'm to the right of this box",
            { position: "top" }
        );
    }
  })
}

function deteleItem(url, id) {
    alert(url);
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id },
        success: function (res) {
          
            $("#_pickItem").html(res);
        }
    })
}

function showContentItem(url, title) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
            //$.notify("I'm over here !");
            //$.notify("Access granted", "success", { position: "right" });
        }
    })
}

function updateAmount(url, id, amount, sum) {
    //alert(url);
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id, amount: amount, sum: sum },
        success: function () {
        }
    })
}

function updateChecked(url, id, isChecked) {
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id, isChecked: isChecked },
        success: function () {
        }
    })
}

function deleteCartItem(url, id) {
    alert("Delete this item?")
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id },
        success: function () {
        }
    })
}

function call(url) {
    alert(url)
    $.ajax({
        type: "GET",
        url: url,
        success: function () {
        }
    })
}

function callWithId(url, id) {
    $.ajax({
        type: "GET",
        url: url,
        data: {id: id},
        success: function () {
        }
    })
}

function addToCart(url, amount, size) {
    $.ajax({
        type: "POST",
        url: url,
        data: { amount: amount, size: size },
        success: function () {
        }
    })
}

//function loadAmount(url, size) {
//    alert(url)
//    $.ajax({
//        type: "GET",
//        url: url,
//        data: { size: size }
//        success: function () {
//        }
//    })
//}