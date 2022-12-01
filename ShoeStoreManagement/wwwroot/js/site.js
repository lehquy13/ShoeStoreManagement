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

        }
    })
}

function showContent2(url, title) {

    $.ajax({
        type: "GET",
        url: url,
        data: {},
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
            //$.notify("I'm over here !");
            //$.notify("Access granted", "success", { position: "right" });

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
        data: { filter: $("#searchFilter").val() },
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
        }
    });
    return false;

    //$.notify("I'm over here !");
    //$.notify("Access granted", "success", { position: "right" });
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
        type: "POST",
        url: url,
        data: { id: id },
        success: function (res) {
            if (res.isValid) {
                $('#hihi').html(res.html);
                $.notify("Added to your wishlist", "success", { position: "right" });
            }
            else {
                $.notify("Error", "warn", { position: "right" });
            }

        }
    })
}

function callWithId1(url, id) {
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id },
        success: function (res) {
            if (res.isValid) {
                $('#hihi').html(res.html);
                $.notify("Removed to your wishlist", "success", { position: "right" });
            }
            else {
                $.notify("Error", "warn", { position: "right" });
            }

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

editSth = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                    $('#hihi').html(res.html);
                }
                else {
                    $('#form-modal .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

createSth = form => {
    debugger;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {


                if (res.isValid) {
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                    $('#hihi').html(res.html);
                }
                else {
                    alert(res.html);
                    $('#form-modal .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function deleteSth(url, id) {
    alert("Delete this item?")
    alert(id)
    alert(url)
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id },
        success: function (res) {
            if (res.isValid) {
                $('#hihi').html(res.html);
                $('#form-modal .modal-body').html('');
                $('#form-modal .modal-title').html('');
                $('#form-modal').modal('hide');
            }
            else {
                alert("invalid");
                $('#form-modal .modal-body').html(res.html);
            }
        }
    })
}

function jQueryAjaxDelete(url) {
    $.ajax({
        type: "POST",
        url: url,
        success: function (res) {
            //$("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html();
            $("#form-modal").modal('hide');
            $("#hihi").html(res);

            $.notify("dung !");
        },
        error: function (xhr, status, error) {
            alert("sai");
            console.log(xhr);
            console.log(status);
            console.log(error);

        }
    })
    //prevent default form submit event
    return false;
}

function jQueryAjaxPost(form) {
    var obj = new FormData(form);
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: obj,
            contentType: false,
            processData: false,
            success: function (res) {
                $('#form-modal .modal-body').html('');
                $('#form-modal .modal-title').html('');
                $("#form-modal").modal('hide');
                $('#hihi').html(res);

            },
            error: function (err) {

                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
        return false;

    }
}

function jQueryAjaxSort(form) {
    var obj = new FormData(form);

    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: obj,
            processData: false,
            contentType: false,
            success: function (res) {
                $('#hihi').html(res);
            },
            error: function (err) {
                alert(err);

                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;

    } catch (ex) {

        alert(ex);
        return false;
    }
}

function jQueryAjaxPagination(url, p) {
    try {
        $.ajax({
            type: 'POST',
            url: url,
            data: { page: p },
            success: function (res) {
                $('#hihi').html(res);
            },
            error: function (err) {
                alert(err);

                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;

    } catch (ex) {

        alert(ex);
        return false;
    }
}

function jQueryAjaxTableSort(url, filter) {
    try {
        $.ajax({
            type: 'POST',
            url: url,
            data: { filter: filter },
            success: function (res) {
                $('#hihi').html(res);
            },
            error: function (err) {
                alert(err);

                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;

    } catch (ex) {

        alert(ex);
        return false;
    }
}

function jQueryAjaxSearch(form) {
    try {
        $.ajax({
            type: 'POST',
            url: url,
            data: { str: s },
            success: function (res) {
                $('#hihi').html(res);
            },
            error: function (err) {
                alert(err);

                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;

    } catch (ex) {

        alert(ex);
        return false;
    }
}

function checkVoucher(url) {
    if ($("#voucher-input").val() != null) {
        var id = $("#voucher-input").val();
        $.ajax({
            type: 'POST',
            url: url,
            data: { id: id },
            success: function (res) {
                if (res === "valid") {
                    $("#voucher-input").notify(
                        res, { position: "left", className: "success", showDuration: 400, showAnimation: 'slideDown' },

                    );
                }
                else {

                    $("#voucher-input").notify(
                        res, { position: "left", className: "warn", showDuration: 400, showAnimation: 'slideDown' },
                    );
                }

            },
            error: function (err) {
                alert('sai');
                alert(err);
                console.log(err)
            }
        })
    }
}

function createCategory(url) {
    if ($("#input-addCategories").val() != null) {
        var id = $("#input-addCategories").val();
        $.ajax({
            type: 'POST',
            url: url,
            data: { newC: id },
            success: function (res) {
                if (res.isValid === false) {
                    $("#input-addCategories").notify(
                        "EXISTED CATEGORY", { position: "left", className: "danger", showDuration: 400, showAnimation: 'slideDown' },
                    );
                }
                else {
                    $('#form-modal .modal-body').html(res.html);

                }

            },
            error: function (err) {
                alert('sai');
                alert(err);
                console.log(err)
            }
        })
    }
}

function EditCategory(url, oldC, id) {
    if ($("#input-" + id).val() !== null) {

        if ($("#input-" + id).val() !== oldC) {

            $.ajax({
                type: 'POST',
                url: url,
                data: { newC: $("#input-" + id).val(), oldC: oldC },
                success: function (res) {
                    if (res.isValid === false) {
                        $("#input-addCategories").notify(
                            "Error", { position: "left", className: "danger", showDuration: 400, showAnimation: 'slideDown' },
                        );
                    }
                    else {
                        $('#form-modal .modal-body').html(res.html);
                        $('#hihi').html(res.html1);
                    }
                },
                error: function (err) {
                    alert('error');
                    alert(err);
                    console.log(err)
                }
            })
        }


    }
}

/* BACKGROUND */

function removeBg() {
    $("#body").addClass("body-bg");
    return false;
}

function navigate(url, id) {
    alert(url);
    $.ajax({
        type: 'GET',
        url: url,
        data: { id: id },
        success: function (res) {
            res.html();
        },
        error: function (err) {
            alert('error');
            alert(err);
            console.log(err)
        }
    })
}