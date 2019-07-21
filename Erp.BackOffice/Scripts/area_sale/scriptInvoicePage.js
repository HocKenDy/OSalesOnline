

function initPageInvoice($form) {
    $("#ProductItemCount").attr("readonly", "true");

    var $tbody = $form.find('tbody.detailList');

    //khởi tạo bảng danh sách chọn sản phẩm
    $('#productSelectList').radComboBox({
        colTitle: 'ID, Hình, Tên sản phẩm',
        colValue: 1,
        colImage: 2,
        colHide: '1',
        colSize: '0px,50px,',
        colClass: ',,',
        height: 300,
        boxSearch: true,
        colSearch: 2,
        customFunction: function () {
            //$('#sidebar-collapse').click(function () {
            //    $tbody.find('[name="DetailList[0].ProductId"]').trigger('rcb:reinit'); // init lại bảng rad combo box
            //});
        }
    });

    $form.find('#Discount, #TaxFee').numberOnly();
    //$('#TaxFee').numberOnly();
    $form.find('#TotalAmount').numberFormat();
    $form.find('.detail_item_price').numberFormat('before');
    //$form.find('.detail_item_discount').numberFormat('before');
    //$form.find('.detail_item_discount_amount').numberFormat('before');
    $form.find('#ReceiptViewModel_Amount').val('0').numberFormat();

    //chọn payment method mặc đỉnh để validate
    $form.find('#ReceiptViewModel_PaymentMethod').val($form.find('#ReceiptViewModel_PaymentMethod option:last-child').attr('value'));

    // lấy địa chỉ theo khách hàng
    $form.find('#CustomerId').change(function () {
        var $this = $(this);
        $form.find('#ContactId').html('');
        $.getJSON('/Contact/GetContactListByCustomerId', { customerId: $this.val() }, function (res) {
            for (var i in res) {
                var option = '<option value="' + res[i].Id + '" data-city="' + res[i].CityId + '" data-district="' + res[i].DistrictId + '" data-ward="' + res[i].WardId + '" data-address="/' + res[i].Address + '/" data-phone="' + res[i].Phone + '">' + res[i].LastName + ' ' + res[i].FirstName + '</option>';
                $form.find('#ContactId').append($(option));
            }

            if (res.length == 0)
                $form.find('#ContactId').html('<option value="">KH này không có liên hệ</option>');

            $form.find('#ContactId').trigger("chosen:updated");
            $form.find('#ContactId').trigger('change');
        });

        $.getJSON('/CustomerDiscount/GetDiscountLast', { customerId: $this.val() }, function (res) {
            console.log(res);
            var percent = res.ValuePercent == null ? 0 : parseInt(res.ValuePercent);

            $('#Discount').val(percent).trigger('change');
            $('#CustomerDiscountId').val(res.Id);
        });
    });

    $form.find('#ContactId').change(function () {
        var $this = $(this);
        var $option = $this.find('option:selected');

        if ($option.val() != '') {
            $form.find('#ShipName').val($option.text());
            $form.find('#Phone').val($option.data('phone'));
            $form.find('#ShipAddress').val($option.data('address').toString().replace(/\//g, ''));
            $form.find('#ShipCityId').val($option.data('city'));
            $form.find('#ShipCityId').trigger("chosen:updated");
            $form.find('#ShipCityId').trigger('change');
        } else {
            $form.find('#ShipName, #Phone, #ShipAddress, #ShipCityId').val('').trigger('change');
            $form.find('#ShipCityId').trigger("chosen:updated");
        }
    });

    $form.find('#IsPayment').change(function () {
        $form.find('.control-group-payment').toggle();
        $form.find('#ReceiptViewModel_PaymentMethod').val('').trigger('change');
        if ($(this).is(':checked') == true) {
            $form.find('#ReceiptViewModel_Amount, #mask-ReceiptViewModel_Amount').val($form.find('#TotalAmount').val()).trigger('change');
            $form.find('#NextPaymentDate').val(modelNextDayPayment);
            $form.find('#AmountRemain').val('0');

            $form.find('.content-scroll').animate({ scrollTop: $(this).offset().top - 50 }, 100);
        } else {
            $form.find('.content-scroll').animate({ scrollTop: 0 }, 500);
        }
    });

    $form.find('#ReceiptViewModel_PaymentMethod').change(function () {
        if ($(this).val() == "Chuyển khoản")
            $form.find('.control-group-payment-method').show();
        else {
            $form.find('.control-group-payment-method').hide();
            $form.find('#ReceiptViewModel_BankAccountNo').val('');
            $form.find('#ReceiptViewModel_BankAccountName').val('');
            $form.find('#ReceiptViewModel_BankName').val('');
        }
    });

    $form.find('#mask-ReceiptViewModel_Amount').blur(function () {
        var totalAmount = parseFloat($form.find('#TotalAmount').val());
        var amount = parseFloat($form.find('#ReceiptViewModel_Amount').val());
        if (amount < totalAmount) {
            $form.find('.NextPaymentDate-container').show();
            $form.find('#AmountRemain').val(currencyFormat(totalAmount - amount));
        }
        else
            $form.find('.NextPaymentDate-container').hide();
    });

    //$tbody.on('change', '.detail_item_category_type', function () {
    //    var $this = $(this);
    //    var $item = $this.closest('tr').find('.detail_item_id');
    //    var $option = $item.find('option:not([value=""])');
    //    $option.css('display', 'block');
    //    $item.val('').trigger('change');
    //    if ($this.val() != '') {
    //        $.each($option, function (index, elem) {
    //            if ($(elem).data('product-type') != $this.val()) {
    //                $(elem).css('display', 'none');
    //            }
    //        });
    //    }
    //    updateRadComboBox($tbody.find('select.detail_item_id'), $form);
    //});

    $form.find('.product-display').click(function () {
        var productId = $(this).attr('role');

        var row_has_select = $tbody.find('.detail_item_id').filter(function (obj) {
            return this.value == productId;
        });

        if (row_has_select.length != 0) {
            var qty = $(row_has_select).closest('tr').find('.detail_item_qty').val() != '' ? parseInt($(row_has_select).closest('tr').find('.detail_item_qty').val()) : 1;
            $(row_has_select).closest('tr').find('.detail_item_qty').val((qty+1)).trigger('change');
            return;
        }

        var row_empty = $tbody.find('.detail_item_id').filter(function (obj) {
            return this.value == '';
        });
        if (row_empty[0] != undefined) {
            $(row_empty[0]).val(productId).trigger('change');
        } else {
            $form.find('.btn-add-detail-item[role="product"]').trigger('click');
            setTimeout(function () {
                console.log($tbody.find('tr:last-child .detail_item_id'));
                $tbody.find('tr:last-child .detail_item_id').val(productId).trigger('change');
            }, 500);
        }
    });

    // hiển thị giá và tính thành tiền khi chọn sản phẩm
    //$tbody.on('change', '.detail_item_id', function () {
    //    var $this = $(this);
    //    $this.closest('tr').find('.detail_item_promotion .display-value').tooltip('destroy');
    //    //$this.closest('tr').find('.detail_item_discount .display-value').tooltip('destroy');
    //    $this.closest('form-group').removeClass('has-error');
    //    $this.next('span').text('');

    //    //disabled các sản phẩm đã chọn rồi
    //    disabledProductSelected($tbody);

    //    if ($this.val() == '') {
    //        $this.closest('tr').find('.detail_item_promotion .display-value').attr('title', '');
    //        $this.closest('tr').find('.detail_item_promotion .display-value').val('0%');
    //        $this.closest('tr').find('.detail_item_discount .display-value').attr('title', '');
    //        $this.closest('tr').find('.detail_item_discount .display-value').val('0');
    //        $this.closest('tr').find('.detail_item_qty').val(1);
    //        $this.closest('tr').find('.detail_item_price').val(0).trigger('change');
    //        $this.closest('tr').find('.detail_item_discount_amount').val(0).trigger('change');
    //        $this.closest('tr').find('.detail_item_total').text(0);
    //    } else {

    //        var categoryCode = $this.find('option:selected').data('product-type');
    //        if (categoryCode != undefined && categoryCode != '') {
    //            $(this).closest('tr').find('.detail_item_category_type').val(categoryCode);
    //        }

    //        var unit = $this.find('option:selected').data('unit');
    //        $this.closest('tr').find('.detail_item_unit').val(unit);

    //        //tìm khuyến mãi cho sản phẩm
    //        findPromotion($this);

    //        calcAmountItem($this, 'item select');

    //        $this.closest('tr').find('.detail_item_promotion .display-value').tooltip('show');
    //        setTimeout(function () {
    //            $this.closest('tr').find('.detail_item_promotion .display-value').tooltip('hide');
    //            $('.tooltip.in').remove();
    //        }, 2000);
    //    }

    //    updateRadComboBox($('select.detail_item_id'), $form);

    //    calcTotalAmount($form);
    //});

    $('#productSelectList').on('change', function () {
        var $this = $(this);
        var selected = $this.find("option:selected");

        if (selected.val() == '' || $('#product_item_' + selected.val()).length > 0)
            return;

        var OrderNo = $('.box-detail .detailList > tr:not(.template_location)').length;
        var ProductId = selected.val();
        var ProductName = selected.text();
        var Unit = selected.data("unit");
        var Quantity = 1;
        var Price = selected.data("price");
        var ProductType = selected.data("product-type");
        var ProductCode = selected.data("code");

        var formdata = {
            OrderNo: OrderNo,
            ProductId: ProductId,
            ProductName: ProductName,
            Unit: Unit,
            Quantity: Quantity,
            Price: Price,
            ProductType: ProductType,
            ProductCode: ProductCode
        };

        //Thêm dòng mới
        ClickEventHandler(true, "/ProductInbound/LoadProductItem", ".detailList", formdata, function () {
            $('#ProductItemCount').val($('.detailList tr').length);
            $('.detail_item_price').numberFormat();
            calcTotalAmount($form);
        });
    });

    // tính thành tiền và tổng cộng
    $tbody.on('change', '.detail_item_qty', function () {
        var $this = $(this);
        var $detail_item_id = $this.closest('tr').find('.detail_item_id');
        calcAmountItem($detail_item_id, 'price');
        calcTotalAmount($form);
    });

    $tbody.on('change', '.detail-product-price .detail_item_price:last-of-type', function () {
        var $this = $(this);
        var $detail_item_id = $this.closest('tr').find('.detail_item_id');
        calcAmountItem($detail_item_id, 'price');
        calcTotalAmount($form);
    });
    $tbody.on('change', '.detail_item_discount', function () {
        var $this = $(this);
        var $detail_item_id = $this.closest('tr').find('.detail_item_id');
        calcAmountItem($detail_item_id, 'price');
        calcTotalAmount($form);
    });
    //khi nhập barcode
    $form.find('#product_barcode').change(function () {
        var $this = $(this);
        if ($this.val() != '') {
            var barcode = $(this).val();

            $(this).val('');

            var valueSearch = searchProductByBarCodeContain(barcode);
            if (valueSearch == undefined) {
                $form.find('[data-valmsg-for="numOfdetailItem"]').text('Không tìm thấy sản phẩm với mã code trên!');
                return;
            }

            console.log(valueSearch);
            var $hasSelect = $tbody.find('.detail_item_id').filter(function () {
                return $.trim($(this).val()).indexOf(valueSearch) != -1; //find('option:selected').attr('value')
            });
            console.log($hasSelect);
            if ($hasSelect.length != 0) {
                $form.find('[data-valmsg-for="numOfdetailItem"]').text('Đã chọn sản phẩm với mã code trên!');
                return;
            }

            var $emptySelect = $tbody.find('.detail_item_id').filter(function () {
                return $.trim($(this).val()) == '';
            });
            var $item = $emptySelect.first();

            if ($item.length == 0) {
                // nếu không có dòng chưa chọn thì thêm dòng mới
                $form.find('.btn-add-detail-item[role="product"]').trigger('click');
                $item = $tbody.find('tr:last-child .detail_item_id');

            } else { // các dòng đã chọn sản phẩm
                var $option = $item.find('option[value="' + valueSearch + '"]');
                if ($option.length != 0) {
                    $item.val($option.attr('value')).trigger('change');
                    $form.find('.btn-add-detail-item[role="product"]').trigger('click');
                }
            }
        }
    });

    //thêm mới sản phẩm
    $form.find('.btn-add-detail-item').click(function () {
        //var reg = new RegExp('DetailList[0].Quantity', 'g');
        
        var len = $tbody.find('tr').length;
        var tr_new = $tr_template.clone()[0].outerHTML;
        tr_new = tr_new.replace(/\[0\]/g, "[" + len + "]").replace(/_0_/g, "_" + len + "_");
        var $tr_new = $(tr_new);
        $tr_new.attr('role', len);
        $tr_new.find('td:first-child').text(len + 1);
        $tr_new.find('.detail_item_price').val(0);
        $tr_new.find('.detail_item_promotion .display-value').text('0%');
        $tr_new.find('.detail_item_discount .display-value').text('0');
        $tr_new.find('td:last-child input').val('');
        $tr_new.find('.detail_item_total').text('0');
        $tr_new.find('.detail_item_discount_amount').text('0');
        //đưa về tùy chọn cho sản phẩm
        $tr_new.find('select').val('');

        $tbody.append($tr_new);
        var $tr_after_append = $tbody.find('tr[role="' + len + '"]');
        $tr_after_append.find('.detail_item_price').numberFormat('before');
        $tr_after_append.find('.detail_item_discount').numberFormat('before');
        //disabled các sản phẩm đã chọn rồi
        disabledProductSelected($tbody);

        //nếu là nút bấm thêm sản phẩm
        if ($(this).attr('role') == 'product') {
            $tr_new.find('select.type_service').remove();
            //khởi tạo bảng danh sách chọn sản phẩm
            $tbody.find('[name="DetailList[' + len + '].ProductId"]').radComboBox({
                colTitle: 'ID,Hình,Tên SP,Tồn kho',
                colValue: 1,
                colHide: '1',
                colSize: '0px,50px,400px,100px',
                colClass: ',,,text-right',
                colImage: '2',
                width: 550,
                boxSearch: true,
                customFunction: function () {
                    $('#sidebar-collapse').click(function () {
                        $tbody.find('[name="DetailList[' + len + '].ProductId"]').trigger('rcb:reinit'); // init lại bảng rad combo box
                    });
                }
            });

        } else {// nếu là nút bấm thêm dịch vụ
            $tr_new.find('select.type_product').remove();

            //khởi tạo bảng danh sách chọn sản phẩm
            $tbody.find('[name="DetailList[' + len + '].ProductId"]').radComboBox({
                colTitle: 'ID,Hình,Tên Gói/DV, DV đi kèm',
                colValue: 1,
                colHide: '1',
                colSize: '0px,50px,200px,200px',
                colClass: ',,,text-left',
                colImage: '2',
                width: 450,
                boxSearch: true,
                customFunction: function () {
                    $('#sidebar-collapse').click(function () {
                        $tbody.find('[name="DetailList[' + len + '].ProductId"]').trigger('rcb:reinit'); // init lại bảng rad combo box
                    });
                }
            });
        }

        $form.find('#numOfdetailItem').next('span').text('');
        $form.find('#numOfdetailItem').val(len);

        calcTotalAmount($form);
    });

    // xóa sản phẩm
    $tbody.on('click', '.btn-delete-item', function () {
        $(this).closest('tr').remove();
        if ($tbody.find('tr').length == 0) {
            $('#ProductItemCount').val(0);
        }
        else
        {
            $('#ProductItemCount').val($('.detailList tr').length);
        }
        calcTotalAmount($form);

        $tbody.find('tr').each(function (index, tr) {
            $(tr).attr('role', index);
            $(tr).find('td:first-child').text(index + 1);

            $(tr).find('.detail_item_id').attr('name', 'DetailList[' + index + '].ProductId').attr('id', 'DetailList_' + index + '_ProductId');
            $(tr).find('.detail_item_qty').attr('name', 'DetailList[' + index + '].Quantity').attr('id', 'DetailList_' + index + '_Quantity');
            $(tr).find('.detail_item_price').filter(':not(.mask-format-currency)').attr('name', 'DetailList[' + index + '].Price').attr('id', 'DetailList_' + index + '_Price');
            $(tr).find('.detail_item_unit').attr('name', 'DetailList[' + index + '].Unit');
            $(tr).find('.detail_item_promotion_id').attr('name', 'DetailList[' + index + '].PromotionId');
            $(tr).find('.detail_item_promotion_detail_id').attr('name', 'DetailList[' + index + '].PromotionDetailId');
            $(tr).find('.detail_item_promotion_value').attr('name', 'DetailList[' + index + '].PromotionValue');
        });

    });

    //tính giảm giá, thuế
    $form.find('#TaxFee, #Discount').change(function () {
        calcTotalAmount($form);
    });

    //resetLableRequired($form);
    //$form.submit(function () {
    //    ShowLoading();
    //    if ($(this).valid()) {
    //        if (checkFieldLiveRequired($(this)) == true) {
    //            if (checkChosenProductOnTable($tbody) == true)
    //                return true;
    //            else
    //                console.log('validate checkChosenProductOnTable');
    //        } else {
    //            console.log('validate checkFieldLiveRequired');
    //        }
    //    } else {
    //        console.log('validate mvc');
    //    }

    //    HideLoading(); return false;
    //});


    //$('body').on('click', 'input, textarea, select, span.lbl, .chosen-single', function () {
    //    var scrollTop = (window.pageYOffset !== undefined) ? window.pageYOffset : (document.documentElement || document.body.parentNode || document.body).scrollTop;
    //    scrollToTopPosition($(this).offset().top - 50);
    //});
};

function disabledProductSelected($tbody) {
    //disabled sản phẩm này ở các danh sách chọn khác
    var optionsSeletedValue = ',';
    $tbody.find('select.detail_item_id').each(function (index, elem) {
        if ($(elem).val() != '')
            optionsSeletedValue += $(elem).val() + ",";
    });
    $tbody.find('select.detail_item_id option').removeAttr('disabled');
    $tbody.find('select.detail_item_id').each(function (index, elem) {
        $(elem).find('option:not([value="' + $(elem).val() + '"])').each(function (index2, option) {
            var value = ',' + $(option).attr('value') + ',';
            if (optionsSeletedValue.indexOf(value) != -1)
                $(option).attr('disabled', 'disabled');
        });
    });
}

function checkChosenProductOnTable($tbody) {
    var flag = true;
    $tbody.find('select.detail_item_id').each(function (index, elem) {
        if ($(elem).val() == '') {
            var message = $(elem).data('val-required') != undefined ? $(elem).data('val-required') : 'Chưa chọn sản phẩm!';
            $(elem).next('span').text(message);
            flag = false;
        }
    });
    return flag;
};

//hàm gọi lại từ form tạo mới khách hàng
function ClosePopupAndDoSomethings($form, optionSelect) {
    ClosePopup(false);
    $form.find('#CustomerId').append($(optionSelect)).trigger("chosen:updated");
}

function updateRadComboBox(selectors, $form) {
    for (var i = 0; i < selectors.length; i++) {
        $form.find('#' + $(selectors[i]).attr('id')).trigger('rcb:updated'); // cập nhật bảng rad combo box
    }
};