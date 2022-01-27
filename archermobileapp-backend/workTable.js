[HttpPost]
        public ActionResult GetUsers(DTParameters dtParameters)
        {
            var searchBy = dtParameters?.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            var model = (_userRepository.GetUsersAsync().Result);
            return Json(new
            {
                draw = 30,
                recordsTotal = 30,
                recordsFiltered = 30,
                data = model.ToList()
            });
        }


        $(document).ready(function () {
        let model = getColumns();

        $('#usersTable').DataTable({
                ajax: {
                    url: '@Url.Action("GetUsers", "Admin")',
                    type: "POST",
                    dataType: "json",
                    data: function (d) {
                        
                        return d;
                    }
                },
                

                "columns": [
                { "data": "userId" },
                    { "data": "roleId" },
                    { "data": "roleName" },
                    { "data": "userName" },
                    { "data": "email" },
                    { "data": "emailConfirmed" },
                    { "data": "lockoutEnabled" },

                { "data": "confirmICR" },
                { "data": "lastLogin" }
                    ],
                "columnDefs": 
                [
                {
                    "targets": [0],
                    "width": "130px"
                }],

                stateSave: true,
                autoWidth: true,
                // ServerSide Setups
                processing: true,
                serverSide: true,
                // Paging Setups
                paging: true,
                // Searching Setups
                searching: { regex: true }
                // Ajax Filter

            });
    });