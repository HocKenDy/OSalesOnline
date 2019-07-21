
function calcAmountItem($detail_item_id, priceFrom) {
    var $option = $detail_item_id.find('option:selected');

    var price = 0;
    var $priceElem = $detail_item_id.closest('tr').find('.detail_item_price');
    if (priceFrom == 'item select') {
        $priceElem.val($option.data('price')).trigger('change');
        price = parseFloat($option.data('price'));
    } else {
        price = parseFloat($priceElem.last().val().replace(/[^0-9\.]/g, ''));
    }

    var $qty = $detail_item_id.closest('tr').find('.detail_item_qty');
    var $discount = $detail_item_id.closest('tr').find('.detail_item_discount');
   
    var discount = 0;
    var qty = 1;
    if ($qty.val() == '') {
        $qty.val(1);
    } else {
        qty = parseInt($qty.val()) < 0 ? parseInt($qty.val()) * -1 : parseInt($qty.val());
    }
    var total = price * qty;
    var promotionPercent = $detail_item_id.closest('tr').find('.detail_item_promotion .display-value').length > 0 ? parseFloat($detail_item_id.closest('tr').find('.detail_item_promotion .display-value').text().replace(/\D/g, '')) : 0;
    var discountPercent = $discount.closest('tr').find('.detail_item_discount .display-value').length > 0 ? parseFloat($discount.closest('tr').find('.detail_item_discount .display-value').text().replace(/\D/g, '')) : 0;
   
    var discountamount = (discountPercent * total) / 100;
    console.log(discountamount);
 
    //nếu có khuyến mãi thì tính
    total = total - Math.round((promotionPercent * total) / 100);
    //nếu có chiết khấu từng sản phẩm.
    total = total - discountamount;
    //số tiền chiết khấu của từng sản phẩm.
    $detail_item_id.closest('tr').find('.detail_item_discount_amount').text(currencyFormat(discountamount));
    $detail_item_id.closest('tr').find('.detail_item_total').text(currencyFormat(total));
   
};

function calcTotalAmount($form) {
    var total = 0;
    $form.find('tbody.detailList tr').each(function (index, elem) {
        if ($(elem).find('.detail_item_total').text() != '') { // la số thì mới tính
            total += parseInt($(elem).find('.detail_item_total').text().replace(/\./g, ''));
        }
    });

    $form.find('#ProductListTotal').val(currencyFormat(total));

    if ($form.find('#Discount').val() != '') {
        var discount = parseInt($form.find('#Discount').val());
        $form.find('#DiscountMoney').val(currencyFormat(Math.round((discount * total) / 100)));
        total = total - Math.round((discount * total) / 100);
    }

    if ($form.find('#TaxFee').val() != '' && total > 0)
        total += Math.round((parseInt($('#TaxFee').val()) * total) / 100);


    $form.find('#mask-TotalAmount').val(currencyFormat(total));
    $form.find('#TotalAmount').val(total);

    $form.find('#mask-ReceiptViewModel_Amount').val(currencyFormat(total));
    $form.find('#ReceiptViewModel_Amount').val(total);

};