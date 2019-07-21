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
 var CustomerCode = $('#CustomerCode').val();  
 var CustomerName = $('#CustomerName').val();  
 		  ShowLoading();
        $.getJSON(url, {
        StartDate: StartDate,  
EndDate: EndDate,  
CustomerCode: CustomerCode,  
CustomerName: CustomerName

        }).done(function (data) {
            ReactDOM.render(
            <TableReport datatable={data} />,
        document.getElementById('react_report')
          );
		    HideLoading();
            });


      var TableReport = React.createClass({
 render: function() {
 
 sumTotalAmount=0;
 sumPaidAmount=0;
 sumRemainingAmount=0;
 var tbody = this.props.datatable.map(function(comment_obj, i) {
 
 sumTotalAmount += comment_obj.TotalAmount;
 sumPaidAmount += comment_obj.PaidAmount;
 sumRemainingAmount += comment_obj.RemainingAmount;
   return (
  <BodyTable stt ={i + 1}
 
CustomerCode= { comment_obj.CustomerCode }
CustomerName= { comment_obj.CustomerName }
Code= { comment_obj.Code }
TotalAmount= { comment_obj.TotalAmount }
PaidAmount= { comment_obj.PaidAmount }
RemainingAmount= { comment_obj.RemainingAmount }
   key ={i}>
    </BodyTable>
   );
    });
  return (
   <table className ="table table-bordered table-responsive" id = "cTable">
 <thead>
  <tr>
  <th colSpan = "8" className ="cell-center">< h4 >{ title} từ { StartDate} đến {EndDate}</h4></th>
  </tr>
  <tr>
 <th> STT </th>
 
 <th className="text-center">Mã khách hàng</th>
 <th className="text-center">Tên khách hàng</th>
 <th className="text-center">Mã hóa đơn</th>
 <th className="text-center">Tổng tiền</th>
 <th className="text-center">Đã thanh toán</th>
 <th className="text-center">Còn lại</th>
  </tr>
   </thead>
  <tbody>
  {tbody}
  <tr className="tr-bold">
  <td></td>
  
 <td className ="cell-center text-right"></td> 
 <td className ="cell-center text-right"></td> 
 <td className ="cell-center text-right"></td> 
   <td className="text-right">{ConvertSoAm(sumTotalAmount)}</td>
   <td className="text-right">{ConvertSoAm(sumPaidAmount)}</td>
   <td className="text-right">{ConvertSoAm(sumRemainingAmount)}</td>
   </tr>
    </tbody>
    </table>
     );
     }
     });
     };

     
  var BodyTable = React.createClass({
    render: function() {
     var name = this.props.stt % 2 == 0 ? "alert-warning" : "";
    var color_TotalAmount = parseInt(this.props.TotalAmount) < 0 ? "text-right red":"text-right green"; 
  var color_PaidAmount = parseInt(this.props.PaidAmount) < 0 ? "text-right red":"text-right green"; 
  var color_RemainingAmount = parseInt(this.props.RemainingAmount) < 0 ? "text-right red":"text-right green"; 

  return (
   <tr className ={name}>
  <td>{this.props.stt}</td>
   
  <td>{this.props.CustomerCode}</td>
  <td>{this.props.CustomerName}</td>
  <td>{this.props.Code}</td>
   <td className ={color_TotalAmount}>{ConvertSoAm(this.props.TotalAmount)}</td>
   <td className ={color_PaidAmount}>{ConvertSoAm(this.props.PaidAmount)}</td>
   <td className ={color_RemainingAmount}>{ConvertSoAm(this.props.RemainingAmount)}</td>
   </ tr >
   );
   }
   });