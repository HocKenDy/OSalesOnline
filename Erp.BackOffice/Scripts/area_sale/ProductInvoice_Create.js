$(document).ready(function ()
{
    $("#CardCode").focus();
    if ($("#WarehouseSourceId").val() != '')
    {
        $('.listsearch').show();
        $('.WarehouseAleart').hide();
        AppendSearchProduct($("#WarehouseSourceId").val());
    }
    else
    {
        $('.WarehouseAleart').show();
        $("#Search_Product").html("");
    }

    $('#WarehouseSourceId').change(function ()
    {
        var $this = $(this);
        if ($this.val() != '')
        {
            $('.listsearch').show();
            $('.WarehouseAleart').hide();
            AppendSearchProduct($this.val());
        }
        else
        {
            $('.WarehouseAleart').show();
            $("#Search_Product").html("");
        }
        $('.detailList tr').each(function (index, elem)
        {
            $(elem).remove();
        });
        $('#ProductItemCount').val('');
        $('#TongSoLuong').text('');
        $('#TongThanhTien').text('');
        calcTotalAmount();
    });

    

    var $option = $("#CustomerId").find('#CustomerId_DisplayText');
    if ($option.val() != '')
    {
        $('.listsearch').show();
        $('.alw').hide();
    }
    else
    {
        alert("Vui lòng chọn khách hàng");
    }

    //Load thông tin cho trang Edit
    if ($("#CustomerId").val() != '')
    {
        loadContact($("#CustomerId").val());
    }

    if ($('#CheckUsePoint').is(":checked"))
    {
        $('#CheckUsePoint').val("True");
        $('#UsePoint').prop('readOnly', false);
    }
    else
    {
        $('#CheckUsePoint').val("False");
        $('#UsePoint').prop('readOnly', true);
    }
    $("#product_barcode").focus();

    calcTotalAmount();
    $('#TotalNoVAT').numberFormat();
    $('#TotalAmount').numberFormat();
    $('#InputDiscount').numberOnly();
    $('#UsePoint').numberOnly();
    $('#UsePointAmount').numberFormat();
    $('.detail_item_qty').numberOnly();
    $('.detail_item_price').numberFormat();
    $('.detail_item_discount').numberOnly();

    $('#InputDiscount').keypress(function (e)
    {
        if (e.which == 13)
        {
            e.preventDefault();
            if ($(this).val() != '')
            {
                $(".detail_item_discount").val($(this).val()).trigger("change");
            }
        }
    });


    $('#InputDiscount').focus(function ()
    {
        $(this).select();
    });
    $('#UsePoint').focus(function ()
    {
        $(this).select();
    });

    //// lấy địa chỉ theo khách hàng
    $('#CustomerId').change(function ()
    {
        loadContact($(this).val());
        var $this = $(this);
        var $option = $this.find('#CustomerId_DisplayText');
        if ($option.val() != '')
        {
            $('.listsearch').show();
            $('.alw').hide();
        }
        else
        {
            alert("Vui lòng chọn khác hàng");
        }
    });

    $('#ContactId').change(function ()
    {
        var $this = $(this);
        var $option = $this.find('option:selected');

        if ($option.val() != '')
        {

            $('#ShipName').val($option.text());
            $('#Phone').val($option.data('phone'));
            if ($option.data('address') != '/null/')
            {
                $('#ShipAddress').val($option.data('address').toString().replace(/\//g, ''));
            }
            else
            {
                $('#ShipAddress').val('');
            }
            city.val($option.data('city'));
            city.trigger("chosen:updated");
            city.trigger('change');

            setTimeout(function () { });

        } else
        {
            $('#ShipName, #Phone, #ShipAddress, #ShipCityId').val('').trigger('change');
            city.trigger("chosen:updated");
        }
    });

    // tính thành tiền và tổng cộng
    $('#listOrderDetail').on('change', '.detail_item_qty', function ()
    {
        var $this = $(this);
        var id = $this.closest('tr').data('id');
        if ($this.val() > $this.data("quantity-inventory") && $("#DetailList_" + id + "__ProductType").val() == "product")
        {
            $this.val($this.data("quantity-inventory"));
        }
        //tính tổng cộng
        calcAmountItem(id);
        calcTotalAmount();

    });

    $('#listOrderDetail').on('change', '.detail_item_price', function ()
    {
        var $this = $(this);
        var id = $this.closest('tr').data('id');
        calcAmountItem(id);
        calcTotalAmount();

    });

    $('#listOrderDetail').on('change', '.detail_item_discount', function ()
    {
        var $this = $(this);
        var id = $this.closest('tr').data('id');
        //tính tổng cộng
        calcAmountItem(id);
        calcTotalAmount();

    });
    $('#InputDiscount').change(function ()
    {
        if ($(this).val() != '')
        {
            $(".detail_item_discount").val($(this).val()).trigger("change");
        }
    });

    $('#listOrderDetail').on('change', '.detail_item_check', function ()
    {
        var $this = $(this);
        var id = $this.closest('tr').data('id');
        if ($('tr#product_item_' + id).find('.detail_item_check').is(':checked'))
        {
            $('tr#product_item_' + id).find('.detail_item_price').val(0);
            var q = $('tr#product_item_' + id).find('.detail_item_price').val();
            $('tr#product_item_' + id).find('.detail_item_check').val("True");
        }
        else
        {
            var q = $('tr#product_item_' + id).find('.pricetest').val().replace(/\./g, '');
            $('tr#product_item_' + id).find('.detail_item_price').val(q).trigger('change');
            $("#mask-DetailList_" + id + "__Price").val(currencyFormat(q));
            $('tr#product_item_' + id).find('.detail_item_check').val("False");
        }
        calcAmountItem(id);
        calcTotalAmount();

    });

    $('#listOrderDetail').on('focus', '.detail_item_discount', function ()
    {
        $(this).select();
    });
    $('#listOrderDetail').on('focus', '.detail_item_price', function ()
    {
        $(this).select();
    });

    $('#listOrderDetail').on('focus', '.detail_item_qty', function ()
    {
        $(this).select();
    });

    $('#listOrderDetail').on('keypress', '.detail_item_discount', function (e)
    {
        if (e.which == 13)
        {
            e.preventDefault();
            $("#product_barcode").focus();
        }
    });

    $('#listOrderDetail').on('keypress', '.detail_item_price', function (e)
    {
        if (e.which == 13)
        {
            e.preventDefault();
            $(this).parent().next().find("input:first").focus().select();
        }
    });

    $('#listOrderDetail').on('keypress', '.detail_item_qty', function (e)
    {
        if (e.which == 13)
        {
            e.preventDefault();
            $(this).parent().next().find("input:first").focus().select();
        }
    });
   

    $('#product_barcode').keypress(function (e)
    {
        if (e.which == 13)
        {
            e.preventDefault();
            $('#product_barcode').trigger('change');
        }
    });

    $(window).keydown(function (e)
    {
        if (e.which == 114)
        {
            e.preventDefault();
            $("#product_barcode").focus();
        } else if(e.which == 113)
        {
            e.preventDefault();
            $('#CardCode').select();
        }
    });
    $('#SaleOrder').on('keyup keypress', function (e)
    {
        var keyCode = e.keyCode || e.which;
        if (keyCode === 13)
        {
            e.preventDefault();
            return false;
        }
    });
    //khi nhập barcode
    $('#product_barcode').change(function ()
    {
        var $this = $(this);
        if ($this.val() != '')
        {

            var barcode = $this.val();
            //đặt lại giá trị rỗng
            $this.val('').focus();

            var valueSearch = searchProductByBarCodeContain(barcode);
            if (valueSearch == undefined)
            {
                alert('Không tìm thấy sản phẩm với mã code trên!');
                return;
            }

            $('#productSelectList').val(valueSearch).trigger("change");

            //kết thúc các lệnh của sự kiện
        }
    });

    // xóa sản phẩm
    $('#listOrderDetail').on('click', '.btn-delete-item', function ()
    {
        //$(this).closest('tr').next('tr.template_location').remove();
        $(this).closest('tr').remove();

        var countItem = $('.detailList tr').length;
        $('#ProductItemCount').val(countItem);

        if (countItem == 0)
        {
            $('#ProductItemCount').val('');
            $('#TongSoLuong').text('');
            $('#TongThanhTien').text('');
        }

        $('.detailList tr').each(function (index, tr)
        {
            $(tr).attr('role', index).attr("id", "product_item_" + index).data("id", index);
            $(tr).find('td:first-child').text(index + 1);
            $(tr).find('.pricetest').attr('name', 'DetailList[' + index + '].PriceTest').attr('id', 'DetailList_' + index + '__PriceTest');
            $(tr).find('.detail_item_id').attr('name', 'DetailList[' + index + '].ProductId').attr('id', 'DetailList_' + index + '__ProductId');
            $(tr).find('.detail_item_product_type').attr('name', 'DetailList[' + index + '].ProductType').attr('id', 'DetailList_' + index + '__ProductType');
            $(tr).find('.detail_item_qty').attr('name', 'DetailList[' + index + '].Quantity').attr('id', 'DetailList_' + index + '__Quantity');
            $(tr).find('.detail_item_price').last().attr('name', 'DetailList[' + index + '].Price').attr('id', 'DetailList_' + index + '__Price');
            $(tr).find('.detail_item_price').first().attr('id', 'mask-DetailList_' + index + '__Price');
            $(tr).find('.detail_item_check').attr('name', 'DetailList[' + index + '].CheckPromotion').attr('id', 'DetailList_' + index + '__CheckPromotion');
            $(tr).find('.detail_item_discount').attr('name', 'DetailList[' + index + '].DisCount').attr('id', 'DetailList_' + index + '__DisCount');
            $(tr).find('.detail_item_discount_amount').attr('name', 'DetailList[' + index + '].DisCountAmount1').attr('id', 'DetailList_' + index + '__DisCountAmount1');

            $(tr).find('.detail_item_point').last().attr('name', 'DetailList[' + index + '].Point').attr('id', 'DetailList_' + index + '__Point');
            $(tr).find('.detail_item_point').first().attr('id', 'mask-DetailList_' + index + '__Point');
            $(tr).find('.detail_item_p_point').attr('name', 'DetailList[' + index + '].ProductPoint').attr('id', 'DetailList_' + index + '__ProductPoint');
            $(tr).find('.detail_item_target_p_point').attr('name', 'DetailList[' + index + '].ProductTargetPoint').attr('id', 'DetailList_' + index + '__ProductTargetPoint');

        });
        calcTotalAmount();
    });

    //tính giảm giá, thuế
    $('#TaxFee, #Discount').change(function ()
    {
        calcTotalAmount();

    });
    //search thẻ
    $("#CardCode").on('keyup', function (e)
    {
        if (e.keyCode == 13)
        {
            appendSelectCustomerByCardCode($(this).val());
        }
    });
    $("#UsePoint").change(function ()
    {
        calcTotalPoint();
    });
}); // end document ready

function AppendSearchProduct(WarehouseSourceId)
{
    ShowLoading();
    $.get('/ProductInvoice/SearchProductInvoice/?WarehouseId=' + $("#WarehouseSourceId").val(), function (html)
    {
        $("#Search_Product").html(html);
    }).done(function ()
    {
        HideLoading();
    });
};
function checkUsePoint()
{
    var $this = $('#CheckUsePoint');
    if($this.is(":checked"))
    {
        $this.val("True");
        $('#UsePoint').prop('readOnly', false);
    }
    else
    {
        $this.val("False");
        $('#UsePoint').prop('readOnly', true);
    }
    calcTotalPoint();
}
function loadContact(customerId)
{

    $.getJSON('/CustomerDiscount/GetDiscountLast', {
        customerId: customerId
    }, function (res)
    {
        //console.log(res);
        var percent = res.ValuePercent == null ? 0 : parseInt(res.ValuePercent);

        $('#Discount').val(percent).trigger('change');
        $('#CustomerDiscountId').val(res.Id);
    });
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

function calcAmountItem(id)
{
    var input_price = $('#DetailList_' + id + '__Price');
    var price = input_price.val() != '' ? input_price.val() : 0;

    //Số lượng
    var input_qty = $('tr#product_item_' + id).find('.detail_item_qty');
    var qty = 1;
    if (input_qty.val() == '')
    {
        input_qty.val(1);
    } else
    {
        qty = parseInt(input_qty.val()) < 0 ? parseInt(input_qty.val()) * -1 : parseInt(input_qty.val());
    }

    //Chiết khấu
    var input_discount = $('tr#product_item_' + id).find('.detail_item_discount');
    var discount = 0;
    if (input_discount.val() == '')
    {
        input_discount.val(0);
    } else
    {
        discount = parseInt(input_discount.val());
    }

    //Thành tiền
    var total = price * qty;
    var discountAmount = discount * total / 100;
    var totalAmount = total - discountAmount;

    //Điểm tích lũy
    var point = 0;
    var p_point = parseFloat($('#DetailList_' + id + '__ProductPoint').val());
    var target_p_point = parseFloat($('#DetailList_' + id + '__ProductTargetPoint').val());
    if (target_p_point > 0)
    {
        point = Math.round(totalAmount * p_point / target_p_point);
    }
    $('#DetailList_' + id + '__Point').val(point);
    $('#mask-DetailList_' + id + '__Point').val(currencyFormat(point));

    //console.log(price, qty);
    $('tr#product_item_' + id).find('.detail_item_discount_amount').val(currencyFormat(discountAmount));
    $('tr#product_item_' + id).find('.detail_item_total').text(currencyFormat(totalAmount));
};

function calcTotalAmount()
{
    var total = 0;
    var total1 = 0;
    var point = 0;
    var selector = '.detailList tr';
    $(selector).each(function (index, elem)
    {
        if ($(elem).find('.detail_item_total').text() != '')
        { // la số thì mới tính
            total += parseInt($(elem).find('.detail_item_total').text().replace(/\./g, ''));
            $("#TongThanhTien").text(currencyFormat(total));
        }

        if ($(elem).find('.detail_item_qty').val() != '')
        { // la số thì mới tính
            total1 += parseInt($(elem).find('.detail_item_qty').val().replace(/\./g, ''));
            $("#TongSoLuong").text(currencyFormat(total1));
        }
        if($(elem).find('.detail_item_point').val() !='')
        {
            point += parseInt($(elem).find('.detail_item_point').val().replace(/\./g, ''));
            $('#TongDiem').text(currencyFormat(point));
        }

    });
    $('#mask-TotalNoVAT').val(currencyFormat(total));
    $('#TotalNoVAT').val(total);
    if ($('#TaxFee').val() != '')
    {
        if ($('#TaxFee').val() > 100)
        {
            $('#TaxFee').val(100);
        }

        var total = parseInt($("#TotalNoVAT").val());
        var vat = parseInt($('#TaxFee').val());
        var amount_Point = parseInt($('#UsePointAmount').val());
        $('#Amount').val(Math.round((total + (vat / 100) * total)));
        var TongTienSauVAT = (total + (vat / 100) * total) - amount_Point;
        if ($("#TotalNoVAT").val() != "")
        {
            $("#TotalAmount").val(Math.round(TongTienSauVAT));
            $('#mask-TotalAmount').val(currencyFormat(Math.round(TongTienSauVAT)));
            $("#ReceiptViewModel_Amount").val(Math.round(TongTienSauVAT));
            $('#mask-ReceiptViewModel_Amount').val(currencyFormat(Math.round(TongTienSauVAT)));
        } else
        {
            $("#TotalAmount").val("0");
            $('#mask-TotalAmount').val("0");
            $("#ReceiptViewModel_Amount").val(0);
            $('#mask-ReceiptViewModel_Amount').val(0);
        }
    }
};

function calcTotalPoint()
{
    var totalPoint = $('#AvailabilityPoint');
    var totalAmount = $('#Amount');
    var checkUsePoint = $('#CheckUsePoint');
    var usePoint = $('#UsePoint');
    var usePointAmount = $('#UsePointAmount');
    if(checkUsePoint.val() == "True" && parseInt(totalAmount.val()) >0)
    {
        if (usePoint.val() == "")
        {
            usePoint.val(0);
        }
        if (totalPoint.val() == "")
        {
            totalPoint.val(0);
        }
        if (parseInt(usePoint.val()) <= parseInt(totalPoint.val()))
        {
            var amount = parseInt(usePoint.val()) * parseInt(amountPoint);
            if (amount <= parseInt(totalAmount.val()))
            {
                usePointAmount.val(amount);
                $('#mask-UsePointAmount').val(currencyFormat(amount));
            }
            else
            {
                var point_check = parseInt(parseInt(totalAmount.val()) / parseInt(amountPoint));
                usePoint.val(point_check);
                calcTotalPoint();
            }
        }
        else
        {
            usePoint.val(totalPoint.val());
            calcTotalPoint();
        }
    }
    else
    {
        usePoint.val(0);
        usePointAmount.val(0);
        $('#mask-UsePointAmount').val(currencyFormat(0));
    }
    calcTotalAmount();
}

function findPromotion($detail_item_id)
{
    var categoryCode = $detail_item_id.closest('tr').find('.detail_item_category_type').val();
    var productId = $detail_item_id.val();
    var quantity = $detail_item_id.closest('tr').find('.detail_item_qty').val();
    quantity = parseInt(quantity);

    //1: ưu tiên cho sản phẩm
    var promotion_product = promotion.productList.filter(function (obj)
    {
        return obj.ProductId == productId && obj.QuantityFor >= quantity;
    });


    console.log('promotion_product', promotion_product);
    if (promotion_product.length > 0)
    {
        $detail_item_id.closest('tr').find('.detail_item_promotion .display-value').text(promotion_product[0].PercentValue + '%');
        $detail_item_id.closest('tr').find('.detail_item_promotion_id').val(promotion_product[0].PromotionId);
        $detail_item_id.closest('tr').find('.detail_item_promotion_detail_id').val(promotion_product[0].Id);
        $detail_item_id.closest('tr').find('.detail_item_promotion_value').val(promotion_product[0].PercentValue);

        var promotionItem = promotion.promotionList.find(function (obj)
        {
            return obj.Id == promotion_product_category.PromotionId;
        });
        $detail_item_id.closest('tr').find('.detail_item_promotion .display-value').attr('title', promotionItem != undefined ? promotionItem.Name : "");

        return;
    }

    //2: xét đến danh mục: tất cả sản phẩm (hàm find chỉ trả về phần tử đầu tiên tìm đc)
    var promotion_product_category = promotion.productCategoryList.find(function (obj)
    {
        return obj.ProductCategoryCode == categoryCode;
    });
    console.log('promotion_product_category', promotion_product_category);
    if (promotion_product_category != undefined)
    {
        $detail_item_id.closest('tr').find('.detail_item_promotion .display-value').text(promotion_product_category.PercentValue + '%');
        $detail_item_id.closest('tr').find('.detail_item_promotion_id').val(promotion_product_category.PromotionId);
        $detail_item_id.closest('tr').find('.detail_item_promotion_detail_id').val(promotion_product_category.Id);
        $detail_item_id.closest('tr').find('.detail_item_promotion_value').val(promotion_product_category.PercentValue);

        var promotionItem = promotion.promotionList.find(function (obj)
        {
            return obj.Id == promotion_product_category.PromotionId;
        });
        $detail_item_id.closest('tr').find('.detail_item_promotion .display-value').attr('title', promotionItem != undefined ? promotionItem.Name : "");

        return;
    }

    //3: xét đến cho tất cả sản phẩm
    var promotion_all = promotion.promotionList.find(function (obj)
    {
        return obj.IsAllProduct == true;
    });

    console.log('promotion_all', promotion_all);
    if (promotion_all != undefined)
    {
        $detail_item_id.closest('tr').find('.detail_item_promotion .display-value').text(promotion_all.PercentValue + '%');
        $detail_item_id.closest('tr').find('.detail_item_promotion .display-value').attr('title', promotion_all.Name);

        $detail_item_id.closest('tr').find('.detail_item_promotion_id').val(promotion_all.Id);
        $detail_item_id.closest('tr').find('.detail_item_promotion_value').val(promotion_all.PercentValue);
        return;
    }

    //nếu không có thì mặc định là 0
    $detail_item_id.closest('tr').find('.detail_item_promotion .display-value').text('0%');
    return;
};

function checkChosenProductOnTable()
{
    var flag = true;
    $('#detailList select.detail_item_id').each(function (index, elem)
    {
        if ($(elem).val() == '')
        {
            var message = $(elem).data('val-required') != undefined ? $(elem).data('val-required') : 'Chưa chọn sản phẩm!';
            $(elem).next('span').text(message);
            flag = false;
        }
    });
    return flag;
};

//hàm gọi lại từ form tạo mới khách hàng

function ClosePopupMemberCard()
{
    ClosePopup(false);
    appendSelectCustomer($("#CustomerId").val());
}
function ClosePopupAndDoSomethings(optionSelect)
{
    ClosePopup(false);
    $("#CustomerId").val($(optionSelect).val()).triggerHandler('change');
    appendSelectCustomer($(optionSelect).val());
}
 
function appendSelectCustomerByCardCode(Code)
{
    ShowLoading();
    var select = $('.js-data-CustomerId-ajax');
    $.ajax({
        type: 'GET',
        url: '/Customer/GetCustomerByCardCode?Code=' + Code
    }).then(function (data)
    {
        if (data != "failed")
        {
            // create the option and append to Select2
            var option = new Option(data.text, data.id, true, true);
            select.append(option).trigger('change');
            // manually trigger the `select2:select` event
            select.trigger({
                type: 'select2:select',
                params: {
                    data: data
                }
            });
            $('#infoCustomer').html(data.html);
            $('.availability_point').text(currencyFormat(data.point));
            $('#AvailabilityPoint').val(data.point);
        }
        else
        {
            alertPopup('Thông báo!', "Không tìm thấy khách hàng", 'error');
            $('#CardCode').val("");
            $('#CardCode').focus();
            $('.availability_point').text(0);
            $('#AvailabilityPoint').val(0);
            $('#infoCustomer').html("");
            var option = new Option("", "", true, true);
            select.append(option).trigger('change');
        }
    }).done(function ()
    {
        HideLoading();
    });
}
function appendSelectCustomer(Id)
{
    var select = $('.js-data-CustomerId-ajax');
    $.ajax({
        type: 'GET',
        url: '/Customer/GetCustomerById?Id=' + Id
    }).then(function (data)
    {
        if (data != null)
        {
            // create the option and append to Select2
            var option = new Option(data.text, data.id, true, true);
            select.append(option).trigger('change');
            // manually trigger the `select2:select` event
            select.trigger({
                type: 'select2:select',
                params: {
                    data: data
                }
            });
            $('#infoCustomer').html(data.html);
            $('.availability_point').text(currencyFormat(data.point));
            $('#AvailabilityPoint').val(data.point);

        }
        else
        {
            $('.availability_point').text(0);
            $('#AvailabilityPoint').val(0);
            $('#infoCustomer').html("");
            var option = new Option("", "", true, true);
            select.append(option).trigger('change');
        }
        

    });
}
// cập nhật quận theo tp, và phường theo quận
$(function ()
{
    
    var url = '/api/BackOfficeServiceAPI/FetchLocation';

    city.change(function ()
    {
        var id = $(this).val(); // Use $(this) so you don't traverse the DOM again
        $.getJSON(url, { parentId: id }, function (response)
        {
            districts.empty(); // remove any existing options
            ward.empty();
            $(document.createElement('option'))
                    .attr('value', '')
                    .text('- Rỗng -')
                    .appendTo(ward);
            $(response).each(function ()
            {
                $(document.createElement('option'))
                    .attr('value', this.Id)
                    .text(capitalizeFirstAllWords(this.Name.toLowerCase().replace('huyện', '').replace('quận', '')))
                    .appendTo(districts);
            });
            var $option = $('#ContactId').find('option:selected');
            districts.val($option.data('district'));
            districts.trigger("chosen:updated");
            districts.trigger('change');
        });
    });

    districts.change(function ()
    {
        var id = $(this).val(); // Use $(this) so you don't traverse the DOM again
        $.getJSON(url, { parentId: id }, function (response)
        {
            ward.empty(); // remove any existing options
            $(response).each(function ()
            {
                $(document.createElement('option'))
                    .attr('value', this.Id)
                    .text(capitalizeFirstAllWords(this.Name.toLowerCase()))
                    .appendTo(ward);
            });
            var $option = $('#ContactId').find('option:selected');
            ward.val($option.data('ward'));
            ward.trigger("chosen:updated");
        });
    });

    //Cho cái đếm tổng cộng nó readonly
    $("#ProductItemCount").attr("readonly", "true");



    $('#PaymentViewModel_PaymentMethod').change(function ()
    {
        if ($(this).val() == "Chuyển khoản")
            $('.control-group-payment-method').show();
        else
        {
            $('.control-group-payment-method').hide();
            $('#PaymentViewModel_BankAccountNo').val('');
            $('#PaymentViewModel_BankAccountName').val('');
            $('#PaymentViewModel_BankName').val('');
        }
    });
    $('#TongTienSauVAT').numberFormat();
    //Thu tiền
    $('#ReceiptViewModel_Amount').numberFormat();

    $('#AmountRemain').val('0');

    $('#mask-ReceiptViewModel_Amount').focus(function ()
    {
        $(this).select();
    });

    $('#mask-ReceiptViewModel_Amount').blur(function ()
    {
        var totalAmount = $("#TotalAmount").val();
        var amount = parseFloat($('#ReceiptViewModel_Amount').val());
        if (amount < totalAmount)
        {
            $('.NextPaymentDate-container').show();
            $('#AmountRemain').val(currencyFormat(totalAmount - amount));
        }
        else if (amount > totalAmount)
        {
            $('#ReceiptViewModel_Amount').val($("#TotalAmount").val());
            $('#mask-ReceiptViewModel_Amount').val(currencyFormat($("#TotalAmount").val()));
        } else if (amount == totalAmount)
        {
            $('.NextPaymentDate-container').hide();

        }
    });

    $("#btnShowPayment").click(function ()
    {
        $('#ReceiptViewModel_Amount').val($("#TotalAmount").val());
        $('#mask-ReceiptViewModel_Amount').val(currencyFormat($("#TotalAmount").val()));
        $('#mask-ReceiptViewModel_Amount').focus();
    });
    //VAT thay đổi
    $('#TaxFee').change(function ()
    {
        TinhVat();
    });


    function TinhVat()
    {
        if ($('#TaxFee').val() != '')
        {
            if ($('#TaxFee').val() > 100)
            {
                $('#TaxFee').val(100);
            }

            var total = parseInt($("#TotalNoVAT").val());
            var vat = parseInt($('#TaxFee').val());

            var TongTienSauVAT = (total + (vat / 100) * total);
            if ($("#TotalNoVAT").val() != "")
            {
                $("#TotalAmount").val(Math.round(TongTienSauVAT));
                $('#mask-TotalAmount').val(currencyFormat(Math.round(TongTienSauVAT)));
            } else
            {
                $("#TotalAmount").val("0");
                $('#mask-TotalAmount').val("0");
            }

        }
    };
    $('#SaleOrder').submit(function ()
    {
        if (!$(this).valid())
        {
            $('#popup_save').modal('toggle');
        }
    });
});