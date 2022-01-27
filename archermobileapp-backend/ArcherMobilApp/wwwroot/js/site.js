// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function eventFire(el, etype) {
    if (el.fireEvent) {
        el.fireEvent('on' + etype);
    } else {
        var evObj = document.createEvent('Events');
        evObj.initEvent(etype, true, false);
        el.dispatchEvent(evObj);
    }
}

function select(selectId, optionToSelect, selectByValue = true) {
    var selectElement = document.getElementById(selectId);
    var selectOptions = selectElement.options;
    for (var opt, j = 0; opt = selectOptions[j]; j++) {
        if (selectByValue) {
            if (opt.value == optionToSelect) {
                selectElement.selectedIndex = j;
                break;
            }
        } else {
            if (opt.label == optionToSelect) {
                selectElement.selectedIndex = j;
                break;
            }   
        }
    }
}

function indexOfRole(value) {
    const len = document.getElementById("select").options.length;
    for (let i = 0; i < len; i++) {
        if (document.getElementById("select").options[i].label == value) {
            return i;
        }
    }
}

function lockoutById(id, table, flag) {
    if (table)
        table.buttons().disable();
    var model = {
        'Key': id,
        'Value': flag ? 'true' : 'false'
    }
    $.ajax({
        type: "POST",
        url: '/admin/lockout',
        data: JSON.stringify(model),
        dataType: 'json',
        headers: {
            "Accept": "application/json"
        },
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response != null && response.statusCode === 200) {
                if (table)
                    table.ajax.reload();
            }
        }
    }).always(function () {
        if (table)
            table.buttons().enable();
    });
}

function deleteById(id, table) {
    if (table)
        table.buttons().disable();
    var model = {
        'Key': id,
        'Value': 'true'
    }
    $.ajax({
        type: "POST",
        url: '/admin/DeleteUser',
        data: JSON.stringify(model),
        dataType: 'json',
        headers: {
            "Accept": "application/json"
        },
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response != null && response.statusCode === 200) {
                if (table)
                    table.ajax.reload();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Status: " + textStatus); alert("Error: " + errorThrown);
        }
    }).always(function () {
        if (table)
            table.buttons().enable();
    });
}

function clearUsersFields() {
    $('#Input_Name').val('');
    $('#Input_Email').val('');

    $('#Input_ConfirmPassword').val('');
    $('#Input_Password').val('');

    $('#Input_ConfirmICR').prop("checked", false);
}

function setMode(usersTableParams, mode) {
    usersTableParams.mode = mode;
    if (usersTableParams.mode === 'view')
        enableButtons($('#usersTable'), true);
    else
        enableButtons($('#usersTable'), false);
}

function enableButtons(table, status) {
    if (table) {
        if (status) {
            table.DataTable().buttons(0).enable();
            table.DataTable().buttons(1).enable();
            table.DataTable().buttons(2).enable();
            table.DataTable().buttons(3).enable();
            table.DataTable().buttons(4).enable();
        } else {
            table.DataTable().buttons(0).disable();
            table.DataTable().buttons(1).disable();
            table.DataTable().buttons(2).disable();
            table.DataTable().buttons(3).disable();
            table.DataTable().buttons(4).disable();
        }
    }
}

function saveUser(form, table, mode) {
    if (table)
        table.buttons().disable();
    var str = $('#' + form[0].id).serialize();
    str = str.split('Input.').join('');
    var url = mode == 'add' ? 'addUser' : "editUser";

    $.ajax({
        type: "POST",
        url: '/admin/' + url,
        data: str,
        success: function (response) {
            if (response != null && response.statusCode === 200) {
                if (table)
                    table.ajax.reload();
                if (mode == 'add')
                    clearUsersFields();
                enableButtons($('#usersTable'), true);
                if (mode === 'edit')
                    eventFire(document.getElementById('idCancelBtn'), 'click');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Status: " + textStatus); alert("Error: " + errorThrown);
        }
    }).always(function () {
        if (table) {
            table.buttons().enable();
        }
    });
}