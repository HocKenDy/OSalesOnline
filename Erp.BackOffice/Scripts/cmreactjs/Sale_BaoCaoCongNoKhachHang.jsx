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
    //var BranchId = $('#branchId').val();
    //var SalerId = $('#salerId').val();


    ShowLoading();
    $.getJSON(url, {
        StartDate: StartDate,
        EndDate: EndDate,
        //BranchId: BranchId,
        //SalerId: SalerId,
    }).done(function (data) {
        ReactDOM.render(
            <TableReport data={data} />,
            document.getElementById('react_report')
        );
        HideLoading();
    });

    function table(data) {
        var sumDebt = 0;
       

        return (
            <table className="table table-bordered table-responsive" id="cTable">
                <thead>
                    <tr>
                        <th colSpan="5" className="cell-center"><span style={{ fontSize: 16 }}><b>{title} từ ngày {StartDate} đến ngày {EndDate}</b></span></th>
                    </tr>
                    <tr>
                        <th className="cell-center" style={{ width: 50 }}>STT</th>
                        <th className="cell-center" style={{ width: 100 }}>Mã khách hàng</th>
                        <th className="cell-center" style={{ width: 100 }}>Tên khách hàng</th>
                        <th className="cell-center" style={{ width: 100 }}>Số điện thoại</th>
                        <th className="cell-center" style={{ width: 100 }}>Công nợ</th>
                       
                    </tr>
                </thead>
                <tbody>
                    {

                        data.map(function (obj, i) {
                            sumDebt += obj.Debt;
                           
                            return (
                                <tr key={i} className={name}>
                                    <td className="cell-center" width={"40"}>{i + 1}</td>
                                    <td className="text-left">{obj.TargetCode}</td>
                                    <td className="text-left">{obj.TargetName}</td>
                                    <td className="text-right">{obj.Phone}</td> 
                                    <td className="text-right">{ConvertSoAm(obj.Debt)}</td>
                                                              
                                </tr>
                            );
                        })
                    }
                </tbody>
                <tfoot>
                    <tr>
                        <td colSpan="4" className="text-right text-uppercase" style={{ fontWeight: 'bold' }}>Tổng cộng</td>
                        <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(sumDebt)}</td>
                     
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