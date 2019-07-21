function loadSale() {
    ShowLoading();
    var code = $('#CodeSale');
    var name = $('#NameSale');
    var position = $('#Position');
    var branchId = $('#branchId');
    var departmentId = $('#DepartmentId');
    var SaleList = $('input[name="SaleListcancel"]').val();
    $.get('/InternalNotifications/Search', { CodeSale: code.val(), NameSale: name.val(), Position: position.val(), branchId: branchId.val(), DepartmentId: departmentId.val(), SaleList: SaleList }, function (res) {
        var $html_response = $('<div>' + res + '</div>');
        $('.area-detail').html($html_response.find('.box').html());
        $tr_template = $html_response.find('.box #SaleList tr:first-child');
        HideLoading();

        $('.table-fixed-column-1').tableFixedColumn({
            leftTableWidth: '0%',
            rightTableWidth: '100%',
            leftTableColumnWidth: [0],
            rightTableColumnWidth: [50, 280],
            columnHeight: 60,
            numberColumnFixedLeft: 0,
            contentHeight: '288px'
        });
    });
}

function CheckIsval() {
    var check = $('input[name="Sale-checkbox"]:checked').map(function () {
        return $(this).val();
    }).get().join(',');
    var checkSave = $('input[name="SaleListcancel"]');
    //console.log(checkSave);

    if (check != "") {
        if (checkSave.val() != "") {
            checkSave.val(checkSave.val() + ',' + check);
        }
        else {
            checkSave.val(check);
        }
    }
    if (checkSave.val() != "") {
        var formData = {
            check: checkSave.val()
        };
        $("#ListCheckSale").html('');
        ClickEventHandler(true, "/InternalNotifications/AddSale", "#ListCheckSale", formData, loadTableFixedColumn);
        loadSale();


    }
};

//$("#ListCheckSale").on('click', '.CheckIsvalReinit', function () {
//});

//function CheckIsvalReinit() {
//    var checkSave = $('input[name="SaleListcancel"]');
//    var formData = {
//        check: checkSave.val()
//    };
//    $("#ListCheckSale").html('');
//    ClickEventHandler(true, "/InternalNotifications/AddSale", "#ListCheckSale", formData, loadTableFixedColumn);
//    loadSale();
//};

function loadTableFixedColumn() {
    $('.table-fixed-column-2').tableFixedColumn({
        leftTableWidth: '0%',
        rightTableWidth: '100%',
        leftTableColumnWidth: [0],
        rightTableColumnWidth: [50, 280],
        columnHeight: 60,
        numberColumnFixedLeft: 0,
        contentHeight: '288px'
    });
};

//Fetch Department of University
var urDepartmentl = '/api/BackOfficeServiceAPI/FetchBranchDepartment';
var department = $("#DepartmentId"); // cache it

$("#branchId").change(function () {
    //console.log($(this).val());
    ShowLoading();
    department.empty(); // remove any existing options
    $(document.createElement('option'))
                .attr('value', '')
                .text('- Rỗng -')
                .appendTo(department).trigger('chosen:updated');
    var id = $(this).val(); // Use $(this) so you don't traverse the DOM again
    $.getJSON(urDepartmentl, { BranchId: id }, function (response) {
        department.empty(); // remove any existing options
        $(response).each(function () {
            $(document.createElement('option'))
                .attr('value', this.Id)
                .text(this.Sale_DepartmentId)
                .appendTo(department).trigger('chosen:updated');
            HideLoading();
        });
    });
});
function CheckAll() {
    if ($('input[name="checkAll"]').is(':checked')) {
        $('input[name="Sale-checkbox"]').prop('checked', true);
    } else {
        $('input[name="Sale-checkbox"]:checked').prop('checked', false);
    }
}
function ClearSearch() {
    $('#CodeSale').val('');
    $('#NameSale').val('');
    $('#Position').val('');
    $('#branchId').val('');
    $('#DepartmentId').val('');
}
$(document).ready(function () {
    CheckIsval();
    loadSale();
});