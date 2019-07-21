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
  var BranchId = $('#branchId  :selected').val();
  var SalerId = $('#salerId  :selected').val();
  var PaymentMethod = $('#paymentMethod  :selected').val();
  ShowLoading();
  $.getJSON(url, {
    StartDate: StartDate,
    EndDate: EndDate,
    BranchId: BranchId,
    SalerId: SalerId,
    PaymentMethod: PaymentMethod
  }).done(function (data) {
    ReactDOM.render(
      <TableReport data={data} />,
      document.getElementById('react_report')
    );
    HideLoading();
  });

  function table(data) {
    var sumAccumulatedPoint = 0;
    var sumUsePoint = 0;
    var sumUsePointAmount = 0;
    var sumTotalAmount = 0;
    var sumPaidAmount = 0;
      var sumRemainingAmount = 0;

    return (
      <table className="table table-bordered table-responsive" id="cTable">
        <thead>
          <tr>
            <th colSpan="13" className="cell-center"><span style={{ fontSize: 16 }}><b>{title} từ ngày {StartDate} đến ngày {EndDate}</b></span></th>
          </tr>
          <tr>
            <th className="cell-center" style={{ width: 30 }}>STT</th>
            <th className="cell-center" style={{ width: 100 }}>Ngày hoá đơn</th>
            <th className="cell-center" style={{ width: 80 }}>Chi nhánh</th>
            <th className="cell-center" style={{ width: 150 }}>Khách hàng</th>
            <th className="cell-center" style={{ width: 100 }}>Hoá đơn</th>
            <th className="cell-center" style={{ width: 70 }}>Điểm tích</th>
            <th className="cell-center" style={{ width: 100 }}>Điểm sử dụng</th>
            <th className="cell-center" style={{ width: 100 }}>Tiền sử dụng</th>
            <th className="cell-center" style={{ width: 100 }}>Tiền đơn hàng</th>
            <th className="cell-center" style={{ width: 100 }}>Đã thanh toán</th>
            <th className="cell-center" style={{ width: 100 }}>Còn lại</th>
            <th className="cell-center" style={{ width: 100 }}>Nhân viên</th>
            <th className="cell-center" style={{ width: 100 }}>Ghi chú</th>
          </tr>
        </thead>
        <tbody>
          {
            data.map(function (obj, i) {
              sumAccumulatedPoint += obj.AccumulatedPoint;
              sumUsePoint += obj.UsePoint;
              sumUsePointAmount += obj.UsePointAmount;
              sumTotalAmount += obj.TotalAmount;
              sumPaidAmount += obj.PaidAmount;
              sumRemainingAmount += obj.RemainingAmount;
              return (
                <tr key={i} className={name}>
                  <td className="cell-center" width={"40"}>{i + 1}</td>
                  <td className="">{ChuyenNgay(obj.CreatedDate)}</td>
                  <td className="">{obj.BranchName}</td>
                  <td className="">{obj.CustomerName}</td>
                  <td className="">{obj.Code}</td>
                  <td className="text-right">{ConvertSoAm(obj.AccumulatedPoint)}</td>
                  <td className="text-right">{ConvertSoAm(obj.UsePoint)}</td>
                  <td className="text-right">{ConvertSoAm(obj.UsePointAmount)}</td>
                  <td className="text-right">{ConvertSoAm(obj.TotalAmount)}</td>
                  <td className="text-right">{ConvertSoAm(obj.PaidAmount)}</td>
                  <td className="text-right">{ConvertSoAm(obj.RemainingAmount)}</td>
                  <td>{obj.SalerFullName}</td>
                  <td>{obj.Note}</td>
                </tr>
              );
            })
          }
        </tbody>
        <tfoot>
                <tr>
                    <td colSpan="5" className="text-right text-uppercase" style={{ fontWeight: 'bold' }}>Tổng cộng</td>
                    <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(sumAccumulatedPoint)}</td>
                    <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(sumUsePoint)}</td>
                    <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(sumUsePointAmount)}</td>
                    <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(sumTotalAmount)}</td>
                    <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(sumPaidAmount)}</td>
                    <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(sumRemainingAmount)}</td>
                    <td colSpan="2"></td>
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