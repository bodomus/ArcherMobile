
var ed = document.getElementById('edit');
ed.addEventListener("click",
    function () {
        testnewLogin();
    });

var updateUserButton = document.getElementById('updateUser');
updateUserButton.addEventListener("click",
    function () {
        updateUser();
    });
var SetICRFlagButton = document.getElementById('SetICRFlag');
SetICRFlagButton.addEventListener("click",
    function () {
        SetICRFlag();
    });


function testUser(userId, userName, userAge) {

    //For test only
    //        var model = {
    //            'Email': 'bodomus@gmail.com',
    //            'Password': 'Unreal1970_',
    //            'IsEmployee': true
    //        }
    var model = {
        'Email': 'dvinichenko@archer-soft.com',
        'Password': '12345aA_',
        'IsEmployee': true
    }
    var d = { model: model };

    var url = "https://localhost:44361/Account/Login";
    var api = 'https://localhost:44361/api/Users/bbdb2d8b-0ac2-4603-8496-35b8766d615d';
    var apilogin = 'https://localhost:44361/api/Users/Login';
    //For test only
    $.ajax({
        type: "POST",
        url: 'api/users/login',
        data: d,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        //contentType: "application/x-www-form-urlencoded",
        success: function (response) {
            debugger;
            if (response != null) {

                //Refresh(response);
                getRooms(response);
                //updateUser(response);
                //SetICRFlag(response);
                //getUser('bbdb2d8b-0ac2-4603-8496-35b8766d615d', response);
                $.ajax({
                    type: "GET",
                    url: '/api/users/GetAnnouncements',
                    data: d,
                    headers: {
                        "Accept": "application/json",
                        "Authorization": "Bearer " + response.accessToken
                    },
                    //contentType: "application/json; charset=utf-8",
                    contentType: "application/x-www-form-urlencoded",
                    success: function (response) {
                        if (response != null) {

                        }
                    }
                });
            } else {
                alert("Something went wrong");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function getUser(id, response) {
    $.ajax({
        type: "GET",
        url: '/api/users/' + id,
        headers: {
            "Accept": "application/json",
            "Authorization": "Bearer " + response.accessToken
        },
        //contentType: "application/json; charset=utf-8",
        contentType: "application/x-www-form-urlencoded",
        success: function (response) {
            if (response != null) {

            }
        }
    });
}

function getRooms(response) {
    ;
    $.ajax({
        type: "GET",
        url: '/api/users/getrooms/',
        headers: {
            "Accept": "application/json",
            "Authorization": "Bearer " + response.accessToken
        },
        //contentType: "application/json; charset=utf-8",
        contentType: "application/x-www-form-urlencoded",
        success: function (response) {
            if (response != null) {
                debugger;
            }
        }
    });
}

function updateUser(response) {
    var modelForUpdate = {
        UserName: 'changeduser',
        Email: 'maxim.babarov@archer-soft.com',
        ConfirmICR: true,
        OldPassword: '12345aA_',
        NewPassword: '12345aA__',
        KeepLogged: true
    }

    $.ajax({
        type: "POST",
        url: '/api/users/update',
        data: JSON.stringify(modelForUpdate),
        dataType: 'json',
        headers: {
            "Accept": "application/json",
            "Authorization": "Bearer " + response.accessToken
        },
        contentType: "application/json; charset=utf-8",
        //contentType: "application/x-www-form-urlencoded",
        success: function (response) {
            if (response != null) {

            }
        }
    });
}

function SetICRFlag(response) {

    var model = {
        'Key': 'maxim.babarov@archer-soft.com',
        'Value': 'true'
    }
    debugger;
    $.ajax({
        type: "POST",
        url: '/api/users/setIcrFlag',
        data: JSON.stringify(model),
        dataType: 'json',
        headers: {
            "Accept": "application/json"
            ,"Authorization": "Bearer " + response.accessToken
        },
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response != null) {

            }
        }
    });
}

function Refresh(response) {

    var model = {
        'token': response.accessToken,
        'refreshToken': response.refreshToken
    }

    $.ajax({
        type: "POST",
        url: 'api/users/refresh',
        data: { 'token': response.accessToken, 'refreshToken': response.refreshToken },
        dataType: 'json',
        headers: {
            "Accept": "application/json",
            "Authorization": "Bearer " + response.accessToken
        },
        contentType: "application/x-www-form-urlencoded",
        success: function (response) {
            if (response != null) {

            }
        }
    });
}

function testnewLogin() {

    var model = {
        'Email': 'maxim.babarov@archer-soft.com',
        'Password': '12345aA__',
        'IsEmployee': true
    }

    $.ajax({
        type: "POST",
        url: '/api/users/Login',
        data: JSON.stringify(model),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        //contentType: "application/x-www-form-urlencoded",
        success: function (response) {
            debugger;
            if (response != null) {
                updateUser(response);
                //getRooms(response);
                //SetICRFlag(response);
                return;
                var RefreshRequest = {
                    'AccessToken': response.accessToken,
                    'RefreshToken': response.refreshToken
                }
                $.ajax({
                    type: "POST",
                    url: '/api/users/refresh',
                    dataType: 'json',
                    data: JSON.stringify(RefreshRequest),
                    headers: {
                        "Accept": "application/json",
                        "Authorization": "Bearer " + response.accessToken
                    },

                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        if (response != null) {
                            debugger;
                        }
                    }
                });
            } else {
                alert("Something went wrong");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
