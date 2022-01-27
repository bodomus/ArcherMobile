gm.enterprise.enterpriseAnalyticGrid = $("#enterpriseAnalyticGrid").DataTable({
                "ajax": {
                    "url": $("#datasection").data("analyticgriddata"),
                    "type": "POST",
                    "data": function (d) {
                        var rangeValue = $("#AnalyticDateRange").dxRangeSelector().data().dxRangeSelector.getValue();
                        var startDate = rangeValue[0].toISOString();
                        var endDate = rangeValue[1].toISOString();
                        var join = $("#AnalyticDepartments").dxList().data().dxList._options.selectedItems.map(function (v) {
                            return v.Name;
                        }).join("', '");
                        var departments = join ? "'" + join + "'" : null;
                        var baselCategory = "'" + gm.enterprise.analyticGridType + "'";

                        var params = {
                            startDate: startDate,
                            endDate: endDate,
                            gridType: gm.enterprise.analyticGridType,
                            departments: departments,
                            isBasel: (basel),
                            baselCategory: baselCategory
                        };
                        var result = $.extend({},
                            d,
                            {
                                FilterParams: JSON.stringify(params)
                            });
                        return result;
                    }
                },
                "language": {
                    "searchPlaceholder": 'Enter Title or Reference ID to search',
                    "processing":
                        '<div class="loadingOverlay"><i class="fa fa-spinner fa-spin fa-3x fa-fw" ></i><span class="sr-only">Loading...</span></div>  ',
                    "sEmptyTable": "No Tasks Found"
                },
                "scrollInfinite": true,
                //"scrollY": 'calc(53vh)',
                'pageResize': true,
                'scrollX': true,
                'scrollCollapse': true,
                "columns": model.columns,
                "columnDefs": model.columnsDefs,
                "order": [[0, "asc"]],
                "serverSide": true,
                "pageLength": 10,
                "processing": true,
                "bPaginate": true,
                "destroy": true,
                "drawCallback": function (settings) {
                    var h = $('#enterpriseAnalyticGrid_wrapper').height() - 100 + 'px';

                    $('#enterpriseAnalyticGrid tbody td').each(function (k, cellObj) {
                        const onlyForOverflow = true;
                        var tooltip = $('.inner-tooltip');

                        if (this.innerText && (!onlyForOverflow || this.offsetWidth < this.scrollWidth)) {
                            var cellValue = settings && settings.aoColumns[k] ? $(settings.nTable).DataTable().cell(this).row().data()[settings.aoColumns[k].idx] : cellObj.innerText;
                            if (cellValue)
                                cellValue = cellValue.replace(new RegExp('\r\n', 'g'), '<br/>');

                            $(this).hover(function (e) {
                                var position = $(this).position();
                                tooltip.html('<div>' + cellValue + '</div>');
                                tooltip.css('left', position.left + 280);
                                tooltip.css('top', position.top + 60);
                                tooltip.css('display', 'block');
                            }, function () {
                                tooltip.css('display', 'none');
                            });
                        }
                    });


                    
                }

            });
        },
