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
    var StartDate = $('#startDate').val();
    var EndDate = $('#endDate').val();
    var BranchId = $('#branchId').val();
    var WarehouseId = $('#warehouseId').val();


    ShowLoading();
    $.getJSON(url, {
        StartDate: StartDate,
        EndDate: EndDate,
        BranchId: BranchId,
         WarehouseId: WarehouseId,
    }).done(function (data) {
        ReactDOM.render(
            <TableReport data={data} />,
            document.getElementById('react_report')
        );
        HideLoading();
    });

    function table(data) {
        var sumFirstRemain = 0;
        var sumCenterInboundQuantity = 0;
        var sumCenterOutboundQuantity = 0;
        var sumLastRemain = 0;


        return (
            <table className="table table-bordered table-responsive" id="cTable">
                <thead>
                    <tr>
                        <th colSpan="8" className="cell-center"><span style={{ fontSize: 16 }}><b>{title} từ ngày {StartDate} đến ngày {EndDate}</b></span></th>
                    </tr>
                    <tr>
                        <th className="cell-center" style={{ width: 30 }}>STT</th>
                        <th className="cell-center" style={{ width: 100 }}>Mã quà tặng</th>
                        <th className="cell-center" style={{ width: 100 }}>Tên quà tặng</th>
                        <th className="cell-center" style={{ width: 50 }}>Đơn vị</th>
                        <th className="cell-center" style={{ width: 70 }}>Đầu kỳ</th>
                        <th className="cell-center" style={{ width: 70 }}>Nhập trong kỳ</th>
                        <th className="cell-center" style={{ width: 70 }}>Xuất trong kỳ</th>
                        <th className="cell-center" style={{ width: 70 }}>Cuối kỳ</th>

                    </tr>
                </thead>
                <tbody>
                    {

                        data.map(function (obj, i) {
                            sumFirstRemain += obj.First_Remain;
                            sumCenterInboundQuantity += obj.Center_InboundQuantity;
                            sumCenterOutboundQuantity += obj.Center_OutboundQuantity;
                            sumLastRemain += obj.Last_Remain;
                            return (
                                <tr key={i} className={name}>
                                    <td className="cell-center" width={"40"}>{i + 1}</td>
                                    <td className="text-left">{obj.ProductCode}</td>
                                    <td className="text-left">{obj.ProductName}</td>
                                    <td className="text-left">{obj.ProductUnit}</td>
                                    <td className="text-right">{ConvertSoAm(obj.First_Remain)}</td>
                                    <td className="text-right">{ConvertSoAm(obj.Center_InboundQuantity)}</td>
                                    <td className="text-right">{ConvertSoAm(obj.Center_OutboundQuantity)}</td>
                                    <td className="text-right">{ConvertSoAm(obj.Last_Remain)}</td>
                                </tr>
                            );
                        })
                    }
                </tbody>
                <tfoot>
                    <tr>
                        <td colSpan="4" className="text-right text-uppercase" style={{ fontWeight: 'bold' }}>Tổng cộng</td>
                        <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(sumFirstRemain)}</td>
                        <td className="text-right " style={{ fontWeight: 'bold' }} >{ConvertSoAm(sumCenterInboundQuantity)}</td>
                        <td className="text-right " style={{ fontWeight: 'bold' }} >{ConvertSoAm(sumCenterOutboundQuantity)}</td>
                        <td className="text-right" style={{ fontWeight: 'bold' }} >{ConvertSoAm(sumLastRemain)}</td>
                    </tr>
                </tfoot>
            </table>
        )
    };

    //Create Table Report
    var TableReport = React.createClass({
        render: function () {
            return (
                table(this.props.data)
            );
        }
    });
};