
//BC doanh thu chi tiết

function ChuyenNgay(input_date) {
    if (input_date == '' || input_date == null) return '';
    var _date = input_date.split("-");
    var convertdate = "";
    if (_date != null) {
        if (_date.length > 1) {
            convertdate = _date[2].split('T')[0] + "/" + _date[1] + "/" + _date[0];
        }
    }
    else if (_date.length == 1) {
        convertdate = input_date.split(" ")[0];
    }
    return convertdate;
}

function ConvertSoAm(input_date) {
    var convertdate = "";
    if (parseInt(input_date) < 0) {
        var aa = parseInt(input_date) * (-1);
        var bb = aa.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
        convertdate = "(" + bb + ")";
    }
    else {
        convertdate = parseInt(input_date).toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
    }
    return convertdate;
}


 function GetDatatable(url, title) {
        var startdate = $('#start').val();
        var enddate = $('#end').val();
        var PaymentMethod = $("#PaymentMethod :selected").val();
        var Name = $("#Name").val();
        var SalerId = $("#SalerId :selected").val();
        var Category = $("#Category :selected").val();
        var Code = $("#Code").val();
        var Phone = $("#Phone").val();
        var LaCN = $("#LaCN :selected").val();
        var CustomerId = $('#CustomerId').val();
		  ShowLoading();
        $.getJSON(url, {
            StartDate: startdate,
            EndDate: enddate,
            PaymentMethod: PaymentMethod,
            SalerId: SalerId,
            Category: Category,
            Code: Code,
        }).done(function (data) {
            ReactDOM.render(
            <TableReport datatable={data} />,
        document.getElementById('react_report')
        );
		    HideLoading();
    });


    //Create Table Report
    var TableReport = React.createClass({
        render: function () {
            //var datamodel = null;
			var FirstAmount = 0;
			var sumAmount_Receipt=0;
			var sumAmount_Payment=0;
            var tbody = this.props.datatable.map(function (comment_obj, i) {
                var convertdate = ChuyenNgay(comment_obj.VoucherDate);
				if(i == 0){
					FirstAmount = comment_obj.FirstAmount;
				}

				sumAmount_Receipt+=comment_obj.Amount_Receipt;
				sumAmount_Payment+=comment_obj.Amount_Payment;
                return (
				
                    <BodyTable stt={i+1}
                        VoucherDate={convertdate}
                        Code={comment_obj.Code}
                        Name={comment_obj.Name}
                        ReceiverUserName={comment_obj.ReceiverUserName}
                        Amount_Payment={comment_obj.Amount_Payment}
                        Amount_Receipt={comment_obj.Amount_Receipt}
                        FirstAmount={comment_obj.FirstAmount}
                        LastAmount={comment_obj.LastAmount}
                        ProductNamePurchase={comment_obj.ProductNamePurchase}
                        ProductNameInvoice={comment_obj.ProductNameInvoice}
                        MaChungTuGoc={comment_obj.MaChungTuGoc}
                        LoaiChungTuGoc={comment_obj.LoaiChungTuGoc}
                        Note={comment_obj.Note}
                        Category={comment_obj.Category}
                        key={i}>
                </BodyTable>
    );
    });
    return (
    <table className="table table-bordered table-responsive" id="cTable">
        <thead>
            <tr>
                <th colSpan="11" className="cell-center" ><h4>{title} từ {startdate} đến {enddate}</h4></th>
            </tr>
            <tr>
                <th>STT</th>
                <th className="cell-center">Ngày chứng từ</th>
                <th className="cell-center">Mã chứng từ</th>
                <th className="cell-center">Đối tượng</th>
                <th className="cell-center">Khoản mục</th>
                <th className="cell-center">Ghi chú</th>
                <th className="cell-center">Chứng từ gốc</th>
                <th className="cell-center">Tồn đầu kỳ</th>
                <th className="cell-center">Thu</th>
                <th className="cell-center">Chi</th>
                <th className="cell-center">Tồn cuối kỳ</th>
            </tr>
        </thead>
        <tbody>
            
            {tbody}
			<tr className="tr-bold">
                <td colSpan={7} className="cell-center text-right">  <b>TỔNG CỘNG</b></td>
                <td className="text-right">{FirstAmount.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
				<td className="text-right">{sumAmount_Receipt.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                <td className="text-right">{sumAmount_Payment.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                <td className="text-right">{ConvertSoAm(FirstAmount + sumAmount_Receipt - sumAmount_Payment)}</td>
            </tr>
        </tbody>
    </table>
    );
    }
    });

    //Create Table Report
    var BodyTable = React.createClass({
        btnClickMaChungTuGoc: function (MaChungTuGoc, LoaiChungTuGoc) {
            switch (LoaiChungTuGoc) {
                case "Payment":
                    OpenPopup('/Payment/Detail/?TransactionCode=' + MaChungTuGoc + '&IsPopup=true', 'Chi tiết phiếu chi', 0, 500);
                    break;
                case "Receipt":
                    OpenPopup('/Receipt/Detail/?TransactionCode=' + MaChungTuGoc + '&IsPopup=true', 'Chi tiết phiếu thu', 0, 500);
                    break;
                case "ProductInvoice":
                    OpenPopup('/ProductInvoice/Detail/?TransactionCode=' + MaChungTuGoc + '&IsPopup=true', 'Chi tiết đơn bán hàng', 0, 500);
                    break;
                case "PurchaseOrder":
                    OpenPopup('/PurchaseOrder/Detail/?TransactionCode=' + MaChungTuGoc + '&IsPopup=true', 'Chi tiết đơn mua hàng', 0, 500);
                        break;
                case "ServiceOrder":
                    OpenPopup('/PurchaseOrder/Detail/?TransactionCode=' + MaChungTuGoc + '&IsPopup=true', 'Chi tiết đơn làm sạch', 0, 500);
                        break;
            }
        },
        btnClickCode: function (Code, Category) {
            switch(Category) {
            case "payment":
            OpenPopup('/Payment/Detail/?TransactionCode=' + Code + '&IsPopup=true', 'Chi tiết phiếu chi', 0, 500);
                break;
            case "receipt":
            OpenPopup('/Receipt/Detail/?TransactionCode=' + Code + '&IsPopup=true', 'Chi tiết phiếu thu', 0, 500);
                break;
        }
},
        render: function () {
            var name = this.props.stt % 2 == 0 ? "alert- warning" : ""; 
            var color_FirstAmount = parseInt(this.props.FirstAmount) < 0 ? " text-right red" : "text-right green";
            var color_LastAmount = parseInt(this.props.LastAmount) < 0 ? "text-right red" : " text-right green";
            return (
                <tr className={name}>
                        <td>{this.props.stt}</td>
                        <td>{this.props.VoucherDate} </td>
                        <td><a onClick={() => this.btnClickCode(this.props.Code, this.props.Category)}>{this.props.Code}</a></td>
                        <td>{this.props.ReceiverUserName}</td>
                        <td>{this.props.Name}</td>
                        <td>{this.props.Note}</td>
                        <td><a onClick={() => this.btnClickMaChungTuGoc(this.props.MaChungTuGoc, this.props.LoaiChungTuGoc)}>{this.props.MaChungTuGoc}</a></td>
                        <td className={color_FirstAmount}>{ConvertSoAm(this.props.FirstAmount)}</td>
						<td className={"text-right"}>{this.props.Amount_Receipt}</td>
                        <td className={"text-right"}>{this.props.Amount_Payment}</td>
                        <td className={color_LastAmount}>{ConvertSoAm(this.props.LastAmount)}</td>
                    </tr>
                );
        }
    });
 };




