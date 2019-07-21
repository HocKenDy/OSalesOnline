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
    var SalerId = $('#salerId').val();


    ShowLoading();
    $.getJSON(url, {
        StartDate: StartDate,
        EndDate: EndDate,
        BranchId: BranchId,
        SalerId: SalerId,
    }).done(function (data) {
        ReactDOM.render(
            <TableReport data={data} />,
            document.getElementById('react_report')
        );
        HideLoading();
    }); 

    function table(data) {
        var totalRevenue = 0;
        var totalCost = 0;
        var totalProfit = 0;

        return (
            <table className="table table-bordered table-responsive" id="cTable">
                <thead>
                    <tr>
                        <th colSpan="6" className="cell-center"><span style={{ fontSize: 16 }}><b>{title} từ ngày {StartDate} đến ngày {EndDate}</b></span></th>
                    </tr>
                    <tr>
                        <th className="cell-center" style={{ width: 30 }}>STT</th>
                        <th className="cell-center" style={{ width: 70 }}>Chi nhánh</th>
                        <th className="cell-center" style={{ width: 100 }}>Ngày</th>
                        <th className="cell-center" style={{ width: 100 }}>Tổng thu</th>
                        <th className="cell-center" style={{ width: 100 }}>Tổng chi</th>
                        <th className="cell-center" style={{ width: 100 }}>Lợi nhuận</th>
                       

                    </tr>
                </thead>
                <tbody>
                    {

                        data.map(function (obj, i) {
                            totalRevenue += obj.totalRevenue;
                            totalCost += obj.totalCost;
                            totalProfit += obj.Profit;
                            return (
                                <tr key={i} className={name}>
                                    <td className="cell-center" width={"40"}>{i + 1}</td>
                                    <td className="text-left">{obj.BranchName}</td>
                                    <td className="cell-center">{ChuyenNgay(obj.CreatedDate)}</td>
                                    <td className="text-right">{ConvertSoAm(obj.totalRevenue)}</td>
                                    <td className="text-right">{ConvertSoAm(obj.totalCost)}</td>
                                    <td className="text-right">{ConvertSoAm(obj.Profit)}</td>                           
                                </tr>
                            );
                        })
                    }
                </tbody>
                <tfoot>
                    <tr>
                        <td colSpan="3" className="text-right text-uppercase" style={{ fontWeight: 'bold' }}>Tổng cộng</td>
                        <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(totalRevenue)}</td>
                        <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(totalCost)}</td>
                        <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(totalProfit)}</td>
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