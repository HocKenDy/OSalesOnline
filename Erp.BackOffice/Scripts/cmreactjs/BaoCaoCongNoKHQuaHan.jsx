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
      var StartDate = $('#StartDate').val();  
 var EndDate = $('#EndDate').val();  
 var BranchId = $('#BranchId').val();  
 var CustomerId = $('#CustomerId').data("id") == undefined ? "" : $('#CustomerId').data("id");  
     ShowLoading();
        $.getJSON(url, {
        StartDate: StartDate,  
EndDate: EndDate,  
BranchId: BranchId,  
CustomerId: CustomerId

        }).done(function (data) {
            ReactDOM.render(
            <TableReport datatable={data} />,
        document.getElementById('react_report')
          );
		       HideLoading();
            });


      var TableReport = React.createClass({
 render: function() {
 
 sumCustomerId=0;
 sumTotalAmount=0;
 sumDaThu=0;
 sumConLai=0;
 sumNgayTra=0;
 var tbody = this.props.datatable.map(function(comment_obj, i) {
 
 sumCustomerId += comment_obj.CustomerId;
 sumTotalAmount += comment_obj.TotalAmount;
 sumDaThu += comment_obj.DaThu;
 sumConLai += comment_obj.ConLai;
 sumNgayTra += comment_obj.NgayTra;
   return (
  <BodyTable stt ={i + 1}
 
CustomerId= { comment_obj.CustomerId }
MaChungTuGoc= { comment_obj.MaChungTuGoc }
TotalAmount= { comment_obj.TotalAmount }
DaThu= { comment_obj.DaThu }
ConLai= { comment_obj.ConLai }
CustomerCode= { comment_obj.CustomerCode }
CustomerName= { comment_obj.CustomerName }
NgayTra= { comment_obj.NgayTra }
   key ={i}>
    </BodyTable>
   );
    });
  return (
   <table className ="table table-bordered table-responsive" id = "cTable">
 <thead>
  <tr>
  <th colSpan = "7" className ="cell-center">< h4 >{ title} từ { StartDate} đến {EndDate}</h4></th>
  </tr>
  <tr>
 <th style={{width: 50}}> STT </th>
 
 <th className="text-center"  style={{width: 130}}>Mã chứng từ</th>
 <th className="text-center">Khách hàng</th>
 <th className="text-center"  style={{width: 130}}>Tổng tiền</th>
 <th className="text-center"  style={{width: 130}}>Đã thu</th>
 <th className="text-center"  style={{width: 130}}>Còn lại</th>

 <th className="text-center" style={{width: 130}}>Số ngày quá hạn</th>
  </tr>
   </thead>
  <tbody>
  {tbody}
  <tr className="tr-bold">
  <td></td>
  
 <td className ="cell-center text-right"></td> 
 <td className ="cell-center text-right"></td> 
   <td className="text-right">{ConvertSoAm(sumTotalAmount)}</td>
   <td className="text-right">{ConvertSoAm(sumDaThu)}</td>
   <td className="text-right">{ConvertSoAm(sumConLai)}</td>

  <td className ="cell-center text-right"></td> 
   </tr>
    </tbody>
    </table>
     );
     }
     });
     };

     
  var BodyTable = React.createClass({
     btnClickProductInvoice: function (TransactionCode) {
            OpenPopup('/ProductInvoice/Detail/?TransactionCode=' + TransactionCode + '&IsPopup=true', 'Chi tiết đơn hàng', 0, 500);
        },
    render: function() {
     var name = this.props.stt % 2 == 0 ? "alert-warning" : "";
    var color_CustomerId = parseInt(this.props.CustomerId) < 0 ? "text-right red":"text-right green"; 
  var color_TotalAmount = parseInt(this.props.TotalAmount) < 0 ? "text-right red":"text-right green"; 
  var color_DaThu = parseInt(this.props.DaThu) < 0 ? "text-right red":"text-right green"; 
  var color_ConLai = parseInt(this.props.ConLai) < 0 ? "text-right red":"text-right green"; 
  var color_NgayTra = parseInt(this.props.NgayTra) < 0 ? "text-right red":"text-right green"; 

  return (
   <tr className ={name}>
  <td>{this.props.stt}</td>
   
  <td><a onClick={() => this.btnClickProductInvoice(this.props.MaChungTuGoc)}>{this.props.MaChungTuGoc}</a></td>
  <td>{this.props.CustomerName}</td>
   <td className ={color_TotalAmount}>{ConvertSoAm(this.props.TotalAmount)}</td>
   <td className ={color_DaThu}>{ConvertSoAm(this.props.DaThu)}</td>
   <td className ={color_ConLai}>{ConvertSoAm(this.props.ConLai)}</td>

   <td className ={color_NgayTra}>{ConvertSoAm(this.props.NgayTra)}</td>
   </ tr >
   );
   }
   });