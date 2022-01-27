var getColumns = function () {
    let columns = [];
    let columnsDef = [];
    let model = { columns, columnsDef };
    columns.push({ "data": "UserId", "title": "Id" });
    //            columns.push({ "data": "ConfirmICR", "title": "ConfirmICR" });
    //            columns.push({ "data": "LastLogin", "title": "LastLogin" });
    //            columns.push({ "data": "KeepLogged", "title": "KeepLogged" });
    //            columns.push({ "data": "ICRConfirmationDate", "title": "ICRConfirmationDate" });
    //            columns.push({ "data": "UserName", "title": "UserName" });
    //            columns.push({ "data": "Email", "title": "Email" });
    return model;
}

$(document).ready(function () {
    let model = getColumns();
    let usersTableParams = {}
    let preventIsAdminCheckClick = false;

    $('#select').off('change').on('change',
        function () {
            preventIsAdminCheckClick = true;
            if ($(this)[0].options[$(this)[0].selectedIndex].text == 'Admins') {
                $('#idPassword').css('display', "block");
                $('#idConfirmPassword').css('display', "block");
                $('#Input_ConfirmPassword').attr('disabled', false);
                $('#Input_Password').attr('disabled', false);
                document.getElementById("Input_IsAdmin").checked = true;
                //$('#Input_IsAdmin').attr('checked', true);
                //eventFire(document.getElementById('Input_IsAdmin'), 'click');
            } else {
                $('#idPassword').css('display', "none");
                $('#idConfirmPassword').css('display', "none");
                $('#Input_ConfirmPassword').attr('disabled', true);
                $('#Input_Password').attr('disabled', true);
                //$('#Input_IsAdmin').attr('checked', false);
                document.getElementById("Input_IsAdmin").checked = false;
            }
            preventIsAdminCheckClick = false;
        });


    $('#Input_IsAdmin').off('click').on('click', function () {

        if ($('#Input_IsAdmin').is(":checked")) {
            $('#idPassword').css('display', "block");
            $('#idConfirmPassword').css('display', "block");
            $('#Input_ConfirmPassword').attr('disabled', false);
            $('#Input_Password').attr('disabled', false);
            let index = 0;
            const len = document.getElementById("select").options.length;
            for (let i = 0; i < len; i++) {
                if (document.getElementById("select").options[i].label == 'Admins') {
                    index = i;
                    break;
                }
            }
            document.getElementById("select").selectedIndex = index;
        } else {
            $('#idPassword').css('display', "none");
            $('#idConfirmPassword').css('display', "none");
            $('#Input_ConfirmPassword').attr('disabled', true);
            $('#Input_Password').attr('disabled', true);
            document.getElementById("select").selectedIndex = 0;
        }
    });
    usersTableParams.mode = 'view';
    var table = $('#usersTable').DataTable({
        ajax: {
            url: $("#datasection").data("getusers"),
            type: "POST",
            dataType: "json",
            data: function (d) {

                return d;
            }
        },
        "cursor": "pointer",
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        dom: 'Blfrtip',
        select: 'single',
        "columns": [
            { "data": "userId" },
            { "data": "roleId" },
            {
                "data": "userName",
                "render": function (data, type, full) {
                    return "<div style='text-align:left;'><span style='white-space: nowrap;'>" +
                        data +
                        "</span></div>";
                }

            },
            {
                "data": "roleName",
                "render": function (data, type, full) {
                    return "<div style='text-align:left;'><span style='white-space: nowrap;'>" +
                        data +
                        "</span></div>";
                }

            },

            { "data": "email" },
            {
                "data": "lockoutEnabled",
                "tooltip": "Lockout",
                "render": function (data, type, row) {
                    if (type === 'display') {
                        return '<input type="checkbox" ' + ((data == 1) ? 'checked' : '') + ' id="input' + row.id + '" class="filter-ck" />';
                    }
                    return data;
                }
            },
            {
                "data": "lockoutEnd",
                "render": function (data, type, row) {

                    if (!data)
                        return '';
                    // If display or filter data is requested, format the date
                    if (type === "display" || type === "filter") {
                        return moment(data).format("ddd DD/MM/YYYY HH:mm:ss");
                    }
                    return data;
                }
            },
            {
                "data": "confirmICR",
                "tooltip": "Confirm inner rule document",
                "render": function (data, type, row) {
                    if (type === 'display') {
                        return '<input type="checkbox" ' + ((data == 1) ? 'checked' : '') + ' id="input' + row.id + '" class="filter-ck" />';
                    }
                    return data;
                }
            },
            {
                "data": "lastLogin",
                "render": function (data, type, row) {

                    if (!data)
                        return '';
                    // If display or filter data is requested, format the date
                    if (type === "display" || type === "filter") {
                        return moment(data).format("ddd DD/MM/YYYY HH:mm:ss");
                    }
                    return data;
                }
            }
        ],

        "buttons": [
            {
                text: 'Add user',
                sButtonClass: "btn-edit",
                className: 'btn btn-primary mb40',
                action: function (e, dt, node, config) {
                    setMode(usersTableParams, 'add');
                    $('#edit').css('display', 'block');
                    $('#idConfirmICR').css('display', 'none');
                    $('#userGrid').removeClass('col-md-12').addClass('col-md-9');
                    
                    
                    $('#captionId').text('New account');
                    $('#Input_ConfirmPassword').val('123456');
                    $('#Input_ConfirmPassword').attr('disabled', true);
                    $('#Input_Password').val('123456');
                    $('#Input_Password').attr('disabled', true);
                    $('#idPassword').css('display', "none");
                    $('#idConfirmPassword').css('display', "none");

                    $('#Input_Name').val('');
                    $('#Input_Email').val('');


                    document.getElementById("Input_IsAdmin").checked = false;
                    //$('#Input_IsAdmin').attr('checked', false);
                    $("#usersTable").addClass("disabled");
                    select("select", 'Employees', false);
                }
            }, {
                text: 'Edit user',
                sButtonClass: "btn-edit",
                className: 'btn btn-primary mb40',
                action: function (e, dt, node, config) {
                    var sel = table.rows('.selected').data();

                    if (sel && sel.length == 0)
                        return;
                    this.disable();
                    setMode(usersTableParams, 'edit');
                    $('#captionId').text('Edit account');
                    $('#edit').css('display', 'block');
                    $('#idConfirmICR').css('display', 'block');
                    $('#userGrid').removeClass('col-md-12').addClass('col-md-9');

                    var user = table.rows('.selected').data()[0];
                    $('#Input_Name').val(user['userName']);
                    $('#Input_Email').val(user['email']);
                    $('#Input_OldEmail').val(user['email']);
                    $('#Input_RoleId').val(user['roleId']);
                    $('#Input_OldRoleId').val(user['roleId']);
                    $('#idPassword').css('display', 'none');
                    $('#idConfirmPassword').css('display', 'none');
                    $('#Input_ConfirmPassword').val('');
                    $('#Input_Password').val('');

                    var isAdmin = user['isAdmin'];
                    document.getElementById("Input_IsAdmin").checked = isAdmin;
                    //$('#Input_IsAdmin').attr('checked', isAdmin);
                    var check = user["confirmICR"];
                    
                    select("select", user['roleId']);
                    $('#Input_ConfirmICR').prop("checked", check);
                }
            }, {
                text: 'Delete user',
                sButtonClass: "btn-edit",
                className: 'btn btn-danger mb40',
                action: function (e, dt, node, config) {
                    var sel = table.rows('.selected').data();

                    if (sel && sel.length == 0)
                        return;
                    var id = table.rows('.selected').data()[0]['userId'];

                    deleteById(id, table);
                }
            }, {
                text: 'Lockout user',
                sButtonClass: "btn-edit",
                className: 'btn btn-warning ml40 mb40',
                action: function (e, dt, node, config) {
                    var sel = table.rows('.selected').data();

                    if (sel && sel.length == 0)
                        return;
                    var id = table.rows('.selected').data()[0]['userId'];

                    lockoutById(id, table, true);
                }
            }, {
                text: 'UnLock user',
                sButtonClass: "btn-edit",
                className: 'btn btn-warning mb40',
                action: function (e, dt, node, config) {
                    var sel = table.rows('.selected').data();

                    if (sel && sel.length == 0)
                        return;
                    var id = table.rows('.selected').data()[0]['userId'];

                    lockoutById(id, table, false);
                }
            }
        ],
        "columnDefs":
            [
                {
                    "targets": [0],
                    "visible": false,
                    "searchable": false
                }, {
                    "targets": [1],
                    "visible": false,
                    "searchable": false
                }
            ],
        "createdRow": function (row, data, dataIndex) {

            if (data.confirmICR) {
                $(row).css('color', 'lightseagreen');
            }
            if (data.lockoutEnabled && data.lockoutEnd) {
                $(row).css('color', 'lightgray');
            }
        },
        scrollY: '50vh',
        scrollCollapse: true,
        stateSave: true,
        autoWidth: false,
        processing: true,
        serverSide: true,
        paging: true,
        searching: { regex: true }
    });
    //        table.on("createdRow",
    //            function(row, data, dataIndex) {
    //
    //                if (data.confirmICR) {
    //                    $(row).addClass('lockout-enabled');
    //                }
    //            });

    table.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {
            var data = dt.data();
            
            if (usersTableParams.mode === 'edit' || usersTableParams.mode === 'add')
                return;
            if (usersTableParams.mode === 'edit') {
                $('#Input_Name').val(data['userName']);
                $('#Input_Email').val(data['email']);
                $('#Input_InputRole').val(data['roleId']);
                $('#Input_ConfirmPassword').val('');
                $('#Input_Password').val('');
                var check = data['confirmICR'];
                $('#Input_ConfirmICR').prop("checked", check);
            } else if (usersTableParams.mode === 'add') {
                $('#Input_Name').val('');
                $('#Input_Email').val('');

                $('#Input_ConfirmPassword').val('');
                $('#Input_Password').val('');

                $('#Input_ConfirmICR').prop("checked", false);
            }
        }
    });

    $('#idAddBtn').off('click').on('click', function () {
        saveUser($('#formUser'), table, usersTableParams.mode);
    });

    $('#idCancelBtn').off('click').on('click',
        function () {
            $('#edit').css('display', 'none');
            $('#userGrid').removeClass('col-md-9').addClass('col-md-12');

            if (table)
                table.buttons().enable();

            setMode(usersTableParams, 'view');
        });
});