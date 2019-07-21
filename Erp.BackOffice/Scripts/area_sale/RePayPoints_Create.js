$(document).ready(function ()
{
    $('.js-data-CustomerId-ajax').on('select2:select', function (e)
    {
        if ($('#WarehouseSourceId').val() != '' && $('#CustomerId').val() != '')
        {
            $('.listsearch').show();
            $('.WarehouseAleart').hide();
            AppendSearchProduct($('#WarehouseSourceId').val());
        }
        else
        {
            $('.WarehouseAleart').show();
            $('.listsearch').hide();
            $("#Search_Product").html("");
        }
        if ($("#Id").val() != '' && parseInt($("#Id").val()) <= 0)
        {
            $('.detailList tr').each(function (index, elem)
            {
                $(elem).remove();
            });
        }
    });
    $("#CardCode").focus();
    if ($("#WarehouseSourceId").val() != '' && $('#CustomerId').val() != '')
    {
        $('.listsearch').show();
        $('.WarehouseAleart').hide();
        AppendSearchProduct($("#WarehouseSourceId").val());
    }
    else
    {
        $('.WarehouseAleart').show();
        $('.listsearch').hide();
        $("#Search_Product").html("");
    }

    $('#WarehouseSourceId').change(function ()
    {
        var $this = $(this);
        if ($this.val() != '' && $('#CustomerId').val() != '')
        {
            $('.listsearch').show();
            $('.WarehouseAleart').hide();
            AppendSearchProduct($this.val());
        }
        else
        {
            $('.WarehouseAleart').show();
            $('.listsearch').hide();
            $("#Search_Product").html("");
        }
        if ($("#Id").val() != '' && parseInt($("#Id").val()) <= 0)
        {
            $('.detailList tr').each(function (index, elem)
            {
                $(elem).remove();
            });
        }
    });
    $('#CustomerId').change(function ()
    {
        var $this = $(this);
        if ($this.val() != '' && $('#WarehouseSourceId').val() != '')
        {
            $('.listsearch').show();
            $('.WarehouseAleart').hide();
            AppendSearchProduct($('#WarehouseSourceId').val());
        }
        else
        {
            $('.WarehouseAleart').show();
            $("#Search_Product").html("");
        }
        if ($("#Id").val() != '' && parseInt($("#Id").val()) <= 0)
        {
            $('.detailList tr').each(function (index, elem)
            {
                $(elem).remove();
            });
        }
 
    });

    $('.detail_item_qty').numberOnly();
    calcTotalPoint();

    // xóa sản phẩm
    $('#listOrderDetail').on('click', '.btn-delete-item', function ()
    {
        //$(this).closest('tr').next('tr.template_location').remove();
        $(this).closest('tr').remove();
        var countItem = $('.detailList tr').length;

        if (countItem == 0)
        {
            $('#TongSoLuong').text('');
            $('#TongDiem').text('');
        }

        $('#ProductItemCount').val(countItem);
        $('.detailList tr').each(function (index, tr)
        {
            $(tr).attr('role', index).attr("id", "product_item_" + index).data("id", index);
            $(tr).find('td:first-child').text(index + 1);
            $(tr).find('.detail_item_giftId').attr('name', 'DetailList[' + index + '].GiftId').attr('id', 'DetailList_' + index + '__GiftId');
            $(tr).find('.detail_item_qty').attr('name', 'DetailList[' + index + '].Quantity').attr('id', 'DetailList_' + index + '__Quantity');
            $(tr).find('.detail_item_points').attr('name', 'DetailList[' + index + '].Point').attr('id', 'DetailList_' + index + '__Point');
            $(tr).find('.detail_item_totalpoints').attr('name', 'DetailList[' + index + '].TotalPoint').attr('id', 'DetailList_' + index + '__TotalPoint');
        });
        calcTotalPoint();
    });

    // thay đổi tổng điểm
    $('#listOrderDetail').on('change', '.detail_item_qty', function ()
    {
        var $this = $(this);
        var id = $this.closest('tr').data('id');

        if ($(this).val() > $(this).data("quantity-inventory"))
        {
            $(this).val($(this).data("quantity-inventory"));
        }
        calcPointItem(id);
        calcTotalPoint();
        checkPointQuantityItem(id);
    });

}); // end document ready

function AppendSearchProduct(WarehouseSourceId)
{
    ShowLoading();
    $.get('/RePayPoints/SearchProductForGift', { WarehouseId: WarehouseSourceId }, function (res)
    {
        $("#Search_Product").html(res);
    }).done(function ()
    {
        HideLoading();
    });
};
//Tinh tong diem
function calcPointItem(id)
{
    var quantity = $("#DetailList_" + id + "__Quantity").val();
    var redemtionPoints = $("#DetailList_" + id + "__Point").val();
    var totalPoints = parseInt(quantity) * parseInt(redemtionPoints);
    $("#DetailList_" + id + "__TotalPoint").val(totalPoints);
    $('tr#product_item_' + id).find('.detail_item_total_point_text').text(currencyFormat(totalPoints));
}
function calcTotalPoint()
{
    var total = 0;
    var total1 = 0;
    var selector = '.detailList tr';
    $(selector).each(function (index, elem)
    {
        if ($('#DetailList_' + index + '__TotalPoint').val() != '')
        { // la số thì mới tính
            total += parseInt($('#DetailList_' + index + '__TotalPoint').val().replace(/\./g, ''));
            $("#TongDiem").text(currencyFormat(total));
        }
        if ($('#DetailList_' + index + '__Quantity').val() != '')
        { // la số thì mới tính
            total1 += parseInt($('#DetailList_' + index + '__Quantity').val().replace(/\./g, ''));
            $("#TongSoLuong").text(currencyFormat(total1));
        }
    });
    $("#TotalPoint").val(Math.round(total));
    $('.total_point').text(currencyFormat(Math.round(total)));
}
function checkPoint(point)
{
    var check = false;
    var totalPoint = $("#TotalPoint").val();
    var availabilityPoint = $("#AvailabilityPoint").val();
    if (parseInt(availabilityPoint) < parseInt(totalPoint) + parseInt(point))
    {
        check = true;
    }
    return check;
}
function checkPointQuantityItem(id)
{
    if(checkPoint(0))
    {
        $('#DetailList_' + id + '__Quantity').val(0);
        calcPointItem(id);
        calcTotalPoint();
    }
}
function searchProductByBarCodeContain(barcode)
{
    barcode = barcode.toLowerCase();
    //var $productSelect = $('.detail_item_id').first();

    var $optionList = $("#productSelectList").find('option');

    var arrResulft = [];
    for (var i = 0; i < $optionList.length; i++)
    {
        var data_code = $($optionList[i]).data('code') != undefined ? $($optionList[i]).data('code').toString().toLowerCase() : undefined;
        if (barcode == data_code)
            arrResulft.push($($optionList[i]).attr('value'));

        if (arrResulft.length == 1)
        {
            return arrResulft[0];
        }
    }

    return arrResulft[0];
};
function calcTotalAmount()
{
    var total1 = 0;
    var point = 0;
    var selector = '.detailList tr';
    $(selector).each(function (index, elem)
    {

        if ($(elem).find('.detail_item_qty').val() != '')
        { // la số thì mới tính
            total1 += parseInt($(elem).find('.detail_item_qty').val().replace(/\./g, ''));
            $("#TongSoLuong").text(currencyFormat(total1));
        }
        if ($(elem).find('.detail_item_redemption_points').val() != '')
        {
            point += parseInt($(elem).find('.detail_item_totalpoints').val().replace(/\./g, ''));
            $('#TongDiem').text(currencyFormat(point));
        }
    });

};

