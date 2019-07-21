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
  var SalerId = $('#salerId  :selected').val();

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
      var sumAccumulatedQuantity = 0;
      var sumPrice = 0;
      var sumTotalAmount = 0;
      var sumTotalpoint = 0;
      
    return (
      <table className="table table-bordered table-responsive" id="cTable">
        <thead>
          <tr>
            <th colSpan="12" className="cell-center"><span style={{ fontSize: 16 }}><b>{title} từ ngày {StartDate} đến ngày {EndDate}</b></span></th>
          </tr>
          <tr>
                    <th className="cell-center" style={{ width: 50 }}>STT</th>
                    <th className="cell-center" style={{ width: 100 }}>Ngày hoá đơn</th>
                    <th className="cell-center" style={{ width: 100 }}>Chi nhánh</th>
                    <th className="cell-center" style={{ width: 180 }}>Khách hàng</th>
                    <th className="cell-center" style={{ width: 100 }}>Hoá đơn</th>
                    <th className="cell-center" style={{ width: 150 }}>Dịch vụ</th>
                    <th className="cell-center" style={{ width: 50 }}>Đơn vị</th>
                    <th className="cell-center" style={{ width: 70 }}>Số lượng</th>
                    <th className="cell-center" style={{ width: 100 }}>Giá</th>
                    <th className="cell-center" style={{ width: 70 }}>Thành tiền</th>
                    <th className="cell-center" style={{ width: 50 }}>Điểm</th>
                    <th className="cell-center" style={{ width: 100 }}>Nhân viên</th>
           
          </tr>
        </thead>
        <tbody>
                {
                   
                    data.map(function (obj, i) {
                      
                        var len = obj.GroupItem.length;
                        if (len == 1) {
                            sumAccumulatedQuantity += obj.Quantity;
                            sumPrice += obj.Price;
                            sumTotalAmount += obj.Amount;
                            sumTotalpoint += obj.Point;
                            return (
                                <tr key={i} className={name}>
                                    <td className="cell-center" width={"40"}>{i + 1}</td>
                                    <td className="">{ChuyenNgay(obj.CreatedDate)}</td>
                                    <td className="text-left">{obj.BranchName}</td>
                                    <td className="text-left">{obj.CustomerName}</td>
                                    <td className="text-left">{obj.Code}</td>
                                    <td className="text-left">{obj.ProductName}</td>
                                    <td className="text-left">{obj.Unit}</td>
                                    <td className="text-right">{ConvertSoAm(obj.Quantity)}</td>
                                    <td className="text-right">{ConvertSoAm(obj.Price)}</td>
                                    <td className="text-right">{ConvertSoAm(obj.Amount)}</td>
                                    <td className="text-right">{obj.Point}</td>
                                    <td className="text-left">{obj.SalerName}</td>
                                </tr>
                            );
                        }
                        else {
                            for (let j = 0; j < obj.GroupItem.length; ++j) {
                                sumAccumulatedQuantity += obj.GroupItem[j].Quantity;
                                sumPrice += obj.GroupItem[j].Price;
                                sumTotalAmount += obj.GroupItem[j].Amount;
                                sumTotalpoint += obj.GroupItem[j].Point;
                            }
                            return (
                                randerTableSub(obj.GroupItem, i + 1)
                            );
                        }
                    })
                }
            </tbody>
            <tfoot>
                <tr>
                    <td colSpan="7" className="text-right text-uppercase" style={{ fontWeight: 'bold' }}>Tổng cộng</td>
                    <td className="text-right" style={{ fontWeight: 'bold' }}>{ConvertSoAm(sumAccumulatedQuantity)}</td>
                    <td className="text-right " style={{ fontWeight: 'bold' }} >{ConvertSoAm(sumPrice)}</td>
                    <td className="text-right " style={{ fontWeight: 'bold' }} >{ConvertSoAm(sumTotalAmount)}</td>
                    <td className="text-right" style={{ fontWeight: 'bold' }} >{ConvertSoAm(sumTotalpoint)}</td>
                    <td className=""></td>
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
function randerTableSub(data2, len) {
    var len2 = data2.length;
   
    return (
        data2.map(function (obj2, i) {
            var sumAmount = 0;
            for (let i = 0; i < data2.length; ++i) {
                sumAmount += data2[i].Amount;
            }

            if (i == 0) {
                return (
                    <tr key={i} className={name}>
                        <td className="cell-center" style={{ verticalAlign: 'middle' }} rowSpan={len2} width={"40"}>{len}</td>
                        <td className="" style={{ verticalAlign: 'middle' }} rowSpan={len2}>{ChuyenNgay(obj2.CreatedDate)}</td>
                        <td className="text-left" style={{ verticalAlign: 'middle' }} rowSpan={len2}>{obj2.BranchName}</td>
                        <td className="text-left" style={{ verticalAlign: 'middle' }} rowSpan={len2}>{obj2.CustomerName}</td>
                        <td className="text-left" style={{ verticalAlign: 'middle' }} rowSpan={len2}>{obj2.Code}</td>
                        <td className="text-left">{obj2.ProductName}</td>
                        <td className="text-left">{obj2.Unit}</td>
                        <td className="text-right">{ConvertSoAm(obj2.Quantity)}</td>
                        <td className="text-right">{ConvertSoAm(obj2.Price)}</td>
                        <td className="text-right" style={{ verticalAlign: 'middle' }} rowSpan={len2}>{ConvertSoAm(sumAmount)}</td>
                        <td className="text-right">{obj2.Point}</td>
                        <td className="text-left" style={{ verticalAlign: 'middle' }} rowSpan={len2}>{obj2.SalerName}</td>
                    </tr>
                );
            }
            else {
                return (
                    <tr key={i} className={name}>

                        <td className="text-left">{obj2.ProductName}</td>
                        <td className="text-left">{obj2.Unit}</td>
                        <td className="text-right">{ConvertSoAm(obj2.Quantity)}</td>
                        <td className="text-right">{ConvertSoAm(obj2.Price)}</td>
                        <td className="text-right">{obj2.Point}</td>

                    </tr>
                );
            }

        })
    );
}