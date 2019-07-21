
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


 function GetDatatable(url, title) {
        var startdate = $('#start').val();
        var enddate = $('#end').val();
        var ProductCode = $("#ProductCode").val();
        var BranchId = $("#BranchId :selected").val();
		 var WarehouseId = $("#WarehouseId :selected").val();
		 ShowLoading();
        $.getJSON(url, {
            StartDate: startdate,
            EndDate: enddate,
            ProductCode: ProductCode,
            BranchId: BranchId,
			WarehouseId:WarehouseId
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
           var sumFirst_Remain = 0;
           var sumCenter_InboundQuantity = 0;
           var sumCenter_OutboundQuantity = 0;
           var sumLast_Remain = 0;

            var tbody = this.props.datatable.map(function (comment_obj, i) {
                sumFirst_Remain += comment_obj.First_Remain;

                sumCenter_InboundQuantity += comment_obj.Center_InboundQuantity;
                sumCenter_OutboundQuantity += comment_obj.Center_OutboundQuantity;
                sumLast_Remain += comment_obj.Last_Remain;
                return (
                    <BodyTable stt={i+1}
                        ProductCode={comment_obj.ProductCode}
                        ProductName={comment_obj.ProductName}
                        ProductUnit={comment_obj.ProductUnit}
                        First_Remain={comment_obj.First_Remain}
                        Center_InboundQuantity={comment_obj.Center_InboundQuantity}
                        Center_OutboundQuantity={comment_obj.Center_OutboundQuantity}
                        Last_Remain={comment_obj.Last_Remain}
                        key={i}>
                </BodyTable>
    );
    });
    return (
    <table className="table table-bordered table-responsive" id="cTable">
        <thead>
            <tr>
                <th colSpan="8" className="cell-center" ><h4>{title} từ {startdate} đến {enddate}</h4></th>
            </tr>
            <tr>
			 <th>STT</th>
                <th className="cell-center">Mã sản phẩm</th>
                <th className="cell-center">Tên sản phẩm</th>
                <th>Đơn vị</th>
                    <th className="cell-center">Đầu kỳ</th>
                    <th  className="cell-center">Nhập hàng</th>
                    <th  className="cell-center">Xuất hàng</th>
                    <th  className="cell-center">Cuối kỳ</th>
            </tr>
        </thead>
        <tbody>

            {tbody}

            <tr className="tr-bold">
                <td colSpan={4} className="cell-center text-right">  <b>TỔNG CỘNG</b></td>
                <td className={"text-right red"}>{sumFirst_Remain.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                <td className={"text-right red"}>{sumCenter_InboundQuantity.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                <td className={"text-right red"}>{sumCenter_OutboundQuantity.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                <td className={"text-right red"}>{sumLast_Remain.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
            </tr>

        </tbody>
    </table>
    );
    }
    });

    //Create Table Report
    var BodyTable = React.createClass({
        render: function () {
            var name = this.props.stt % 2 == 0 ? "alert- warning":"";
            return (
                <tr className={name}>
                    <td>{this.props.stt}</td>
                        <td>{this.props.ProductCode} </td>
                        <td>{this.props.ProductName}</td>
                        <td>{this.props.ProductUnit}</td>
                        <td className={"text-right red"}>{this.props.First_Remain.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                        <td className={"text-right red"}>{this.props.Center_InboundQuantity.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                        <td className={"text-right red"}>{this.props.Center_OutboundQuantity.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                        <td className={"text-right red"}>{this.props.Last_Remain.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                    </tr>
                );
        }
    });
 };
